using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Parser : RefCounted
{
    private String _error;
    private void ErrorSet(String value) {
        _error = value;
    }
    public String ErrorGet() {
        return _error;
    }

    private int _position;
    private void PositionSet(int value) {
        _position = value;
    }
    public int PositionGet() {
        return _position;
    }

    List<Token> _tokens;
    private void TokensSet(List<Token> tokens) {
        _tokens = tokens;
    }
    private List<Token> TokensGet() {
        return _tokens;
    }
    private Token TokenPopBack() {
        if (_tokens.Count == 0){
            return null;
        }
        Token token = _tokens[_tokens.Count - 1];
        _tokens.RemoveAt(_tokens.Count - 1);
        PositionSet(token.PositionGet());
        return token;
    }
    private bool TokenSkip(Token.TOKEN_TYPE tt) {
        if (_tokens.Count == 0){
            return false;
        }
        Token token = _tokens[_tokens.Count - 1];
        if (token.TokenTypeGet() == tt) {
            _tokens.RemoveAt(_tokens.Count - 1);
            PositionSet(token.PositionGet());
            return true;
        }
        return false;
    }
    private int TokenCountGet(Token.TOKEN_TYPE tt) {
        int token_count = 0;
        while(_tokens.Count - token_count > 0) {
           // GD.Print("============");
            //GD.Print(_tokens[_tokens.Count-token_count-1].TokenTypeGet());
            if (_tokens[_tokens.Count-token_count-1].TokenTypeGet() == tt){
                token_count +=1;
                continue;
            }
            break;
        }
        return token_count;
    }
    private bool IsToken(Token.TOKEN_TYPE tt, int ln = 1){
        if (_tokens.Count-ln >= 0) {
            return _tokens[_tokens.Count-ln].TokenTypeGet() == tt;    
        }
        return false;
        
    }
    private Token.TOKEN_TYPE TokenTypeGet(int ln = 1) {
        if (_tokens.Count-ln >= 0) {
            return _tokens[_tokens.Count-ln].TokenTypeGet();    
        }
        return Token.TOKEN_TYPE.NONE;
    }


    private CodeFunctionSet _function;
    private void FunctionCurrentSet(CodeFunctionSet value) {
        _function = value;
    }
    private CodeFunctionSet FunctionCurrentGet() {
        return _function;
    }

    private static String TypeNameGet(CodeCommand.RETURN_TYPE type) {
        switch (type) {
            case CodeCommand.RETURN_TYPE.NUMBER:
                return "num";
            case CodeCommand.RETURN_TYPE.TEXT:
                return "text";
            case CodeCommand.RETURN_TYPE.BOOL:
                return "bool";
            case CodeCommand.RETURN_TYPE.VECTOR:
                return "vec";
            case CodeCommand.RETURN_TYPE.ANY:
                return "any";
        }
        return "void";
    }

    CodeScope<CodeCommand> _scope;
    private void ScopeSet(CodeScope<CodeCommand> value) {
        _scope = value;
    }
    private CodeScope<CodeCommand> ScopeGet() {
        return _scope;
    }

    static Dictionary<String, CodeFunctionSet> _native_functions_table = new Dictionary<String, CodeFunctionSet> {
        ["say"] = new CodeFunctionSet(0,"say", 
                         new Dictionary<String, CodeCommand.RETURN_TYPE> {["text"] = CodeCommand.RETURN_TYPE.ANY},
                         new Say(),
                         CodeCommand.RETURN_TYPE.VOID),
        ["print"] = new CodeFunctionSet(0,"print", 
                         new Dictionary<String, CodeCommand.RETURN_TYPE> {["text"] = CodeCommand.RETURN_TYPE.ANY},
                         new Print(),
                         CodeCommand.RETURN_TYPE.VOID),
        ["move"] = new CodeFunctionSet(0,"move", 
                         new Dictionary<String, CodeCommand.RETURN_TYPE> {["position"] = CodeCommand.RETURN_TYPE.VECTOR},
                         new Move(),
                         CodeCommand.RETURN_TYPE.BOOL),
        ["vec"] = new CodeFunctionSet(0,"vec",
                         new Dictionary<String, CodeCommand.RETURN_TYPE> {["x"] = CodeCommand.RETURN_TYPE.NUMBER, ["y"] = CodeCommand.RETURN_TYPE.NUMBER},
                         new Vec(),
                         CodeCommand.RETURN_TYPE.VECTOR),
        ["pos"] = new CodeFunctionSet(0,"pos",
                         new Dictionary<String, CodeCommand.RETURN_TYPE>(),
                         new Pos(),
                         CodeCommand.RETURN_TYPE.VECTOR)
    };
    Dictionary<String, CodeFunctionSet> _functions_table = new Dictionary<String, CodeFunctionSet>();
    private void FunctionAdd(CodeFunctionSet value) {
        _functions_table[value.NameGet()] = value;
    }
    private CodeFunctionSet FunctionGet(String name) {
        CodeFunctionSet func = _native_functions_table.GetValueOrDefault(name, null);
        if (func != null) {
            return func;
        }
        return _functions_table.GetValueOrDefault(name, null);
    }

    public Dictionary<String, CodeState> Parse(String text) {
        ErrorSet("");
        Lexer lexer = new Lexer();
        TokensSet(lexer.Analyze(text));
        if (lexer.ErrorGet() != "") {
            ErrorSet(lexer.ErrorGet());
            PositionSet(lexer.ErrorPositionGet());
            return new Dictionary<String, CodeState>();
        }
        foreach(Token token in _tokens) {
            GD.Print(token.TokenTypeGet());
        }
        
        _tokens.Reverse();
        ScopeSet(new CodeScope<CodeCommand>());
        HeadersParse();
        if (ErrorGet() != ""){
            return new Dictionary<String, CodeState>();
        }
        List<CodeState> states = MainParse();
        if (ErrorGet() != ""){
            return new Dictionary<String, CodeState>();
        }
        Dictionary<String, CodeState> states_list = new Dictionary<String, CodeState>();
        foreach (CodeState state in states) {
            states_list[state.NameGet()] = state;
        }
        return states_list;
    }

    private List<CodeState> MainParse() {
        List<CodeState> states = new List<CodeState>();

        while(TokensGet().Count > 0) {
            Token token = TokenPopBack();

            switch (token.TokenTypeGet()) {
                case Token.TOKEN_TYPE.STATE:
                    CodeState state = StateParse();
                    if (ErrorGet() != "") {
                        return null;
                    }
                    states.Add(state);
                    break;
                case Token.TOKEN_TYPE.FUNC:
                    FunctionSetParse();
                    if (ErrorGet() != "") {
                        return null;
                    }
                    break;
            }
        }
        return states;
    }

    private void HeadersParse() {
        List<Token> tokens = new List<Token>(TokensGet());
        while(TokensGet().Count > 0) {
            if (TokenPopBack().TokenTypeGet() != Token.TOKEN_TYPE.FUNC) {
                continue;
            }
            ScopeGet().ScopeStart();
            CodeFunctionSet func = FunctionHeaderParse();
            ScopeGet().ScopeEnd();
            if (ErrorGet() != "") {
                return;
            }
            if (FunctionGet(func.NameGet()) != null) {
                ErrorSet("PARSE FUNCTION: FUNCTION ALREADY DECLARED");
                return;
            }
            FunctionAdd(func);
        }
        TokensSet(tokens);
    }

    private void FunctionSetParse() {
        ScopeGet().ScopeStart();
        CodeFunctionSet header = FunctionHeaderParse();
        if (ErrorGet() != "") {
            return;
        }

        FunctionCurrentSet(header);
        CodeBody code_body = BodyParse(1, BODY_CYCLE.FUNCTION);
        FunctionCurrentSet(null);
        if (ErrorGet() != "") {
            return;
        }

        if (AlwaysReturns(code_body) == false) {
            PositionSet(header.PositionGet());
            ErrorSet($"Функция {header.NameGet()} должна вернуть {TypeNameGet(header.ReturnTypeGet())}: нет return в конце");
            return;
        }

        ScopeGet().ScopeEnd();
        _functions_table[header.NameGet()].CodeBodySet(code_body);
    }

    private bool AlwaysReturns(CodeBody body) {
        return body.CommandsGet().Count > 0 && body.CommandsGet().Last() is Return;
    }

    private CodeFunctionSet FunctionHeaderParse() {
        Token func_name = TokenPopBack();
        if (func_name == null || func_name.TokenTypeGet() != Token.TOKEN_TYPE.NAME) {
            ErrorSet("PARSE FUNCTION: FUNCTION NAME INVALID");
            return null;
        }

        if (TokenSkip(Token.TOKEN_TYPE.LPAR) == false) {
            ErrorSet("PARSE FUNCTION: LPAR INVALID");
            return null;
        }

        Dictionary<String, CodeCommand.RETURN_TYPE> parameters = FunctionSetParametersParse();
        if (ErrorGet() != "") {
            return null;
        }

        if (TokenSkip(Token.TOKEN_TYPE.RPAR) == false) {
            ErrorSet("PARSE FUNCTION: RPAR INVALID");
            return null;
        }

        if (TokenSkip(Token.TOKEN_TYPE.NEXT) == false) {
            ErrorSet("PARSE FUNCTION: NEXT INVALID");
            return null;
        }

        Token ret = TokenPopBack();
        if (ret == null) {
            ErrorSet("PARSE FUNCTION: RETURN TYPE INVALID");
            return null;
        }

        if (TokenSkip(Token.TOKEN_TYPE.COLON) == false) {
            ErrorSet("PARSE FUNCTION: COLON INVALID");
            return null;
        }

        switch(ret.TokenTypeGet()) {
            case Token.TOKEN_TYPE.TYPE_TEXT:
                return new CodeFunctionSet(func_name.PositionGet(), func_name.TextGet(), parameters, CodeCommand.RETURN_TYPE.TEXT);
            case Token.TOKEN_TYPE.TYPE_BOOL:
                return new CodeFunctionSet(func_name.PositionGet(), func_name.TextGet(), parameters, CodeCommand.RETURN_TYPE.BOOL);
            case Token.TOKEN_TYPE.TYPE_NUMBER:
                return new CodeFunctionSet(func_name.PositionGet(), func_name.TextGet(), parameters, CodeCommand.RETURN_TYPE.NUMBER);
            case Token.TOKEN_TYPE.TYPE_VECTOR:
                return new CodeFunctionSet(func_name.PositionGet(), func_name.TextGet(), parameters, CodeCommand.RETURN_TYPE.VECTOR);
        }

        ErrorSet("PARSE FUNCTION: RETURN TYPE INVALID");
        return null;
    }

    private Dictionary<String, CodeCommand.RETURN_TYPE> FunctionSetParametersParse() {
        Dictionary<String, CodeCommand.RETURN_TYPE> parameters = new Dictionary<String, CodeCommand.RETURN_TYPE>();
       
        if (IsToken(Token.TOKEN_TYPE.RPAR)) {
            return parameters;
        }

        do {
            Token token = TokenPopBack();
            if (token == null){
                ErrorSet("PARSE FUNCTION PARAMETERS: TOKEN TYPE INVALID");
                return null;
            }
            switch (token.TokenTypeGet()) {
                case Token.TOKEN_TYPE.TYPE_TEXT:
                    Token vname1 = TokenPopBack();
                    if(vname1 == null) {
                        ErrorSet("PARSE FUNCTION PARAMETERS: VARIABLE NAME INVALID");
                        return null;
                    }
                    ScopeGet().VariableSet(vname1.TextGet(), new Text("", vname1.PositionGet()));
                    parameters[vname1.TextGet()] = CodeCommand.RETURN_TYPE.TEXT;
                    break;
                case Token.TOKEN_TYPE.TYPE_NUMBER:
                    Token vname2 = TokenPopBack();
                    if(vname2 == null) {
                        ErrorSet("PARSE FUNCTION PARAMETERS: VARIABLE NAME INVALID");
                        return null;
                    }
                    ScopeGet().VariableSet(vname2.TextGet(), new Number(0, vname2.PositionGet()));
                    parameters[vname2.TextGet()] = CodeCommand.RETURN_TYPE.NUMBER;
                    break;
                case Token.TOKEN_TYPE.TYPE_BOOL:
                    Token vname3 = TokenPopBack();
                    if(vname3 == null) {
                        ErrorSet("PARSE FUNCTION PARAMETERS: VARIABLE NAME INVALID");
                        return null;
                    }
                    ScopeGet().VariableSet(vname3.TextGet(), new Boolean(false, vname3.PositionGet()));
                    parameters[vname3.TextGet()] = CodeCommand.RETURN_TYPE.BOOL;
                    break;
                case Token.TOKEN_TYPE.TYPE_VECTOR:
                    Token vname4 = TokenPopBack();
                    if(vname4 == null) {
                        ErrorSet("PARSE FUNCTION PARAMETERS: VARIABLE NAME INVALID");
                        return null;
                    }
                    ScopeGet().VariableSet(vname4.TextGet(), new Vector(Vector2.Zero, vname4.PositionGet()));
                    parameters[vname4.TextGet()] = CodeCommand.RETURN_TYPE.VECTOR;
                    break;
                default:
                    ErrorSet("PARSE FUNCTION PARAMETERS: VARIABLE NOT FOUND");
                    return null;
            }
        }  while(TokenSkip(Token.TOKEN_TYPE.COMMA) == true);

        return parameters;
    }

    private CodeState StateParse() {
        ScopeGet().ScopeStart();
        Token state_name = TokenPopBack();
        if (state_name == null || state_name.TokenTypeGet() != Token.TOKEN_TYPE.NAME) {
            ErrorSet("PARSE STATE: STATE NAME INVALID");
            return null;
        }
        if (TokenSkip(Token.TOKEN_TYPE.COLON) == false) {
            ErrorSet("PARSE STATE: COLON INVALID");
            return null;
        }
        ICodeBody code_body = BodyParse(1, BODY_CYCLE.STATE);
        ScopeGet().ScopeEnd();
        if (ErrorGet() != "") {
            return null;
        }
        return new CodeState(state_name.TextGet(), code_body);
    }

    private enum BODY_CYCLE {
        STATE = 1,
        FUNCTION = 2
    }
    private CodeBody BodyParse(int max_tab_count, BODY_CYCLE cycle) {
        ScopeGet().ScopeStart();
        List<CodeCommand> commands = new List<CodeCommand>();
        int tab_count = TokenCountGet(Token.TOKEN_TYPE.TAB);
        while (tab_count == max_tab_count) {
            _tokens.RemoveRange(_tokens.Count-tab_count,tab_count);

            if (IsToken(Token.TOKEN_TYPE.ASSIGN, 2)) {
                CodeCommand variable_set = VariableSetParse();
                if (ErrorGet() != "") {
                    return null;
                }
                commands.Add(variable_set);
            }else if (IsToken(Token.TOKEN_TYPE.LPAR, 2)) {
                CodeCommand function = FunctionGetFormula();
                if (ErrorGet() != "") {
                    return null;
                }
                commands.Add(function);
            } else {
                Token token = TokenPopBack();
                switch (token.TokenTypeGet()) {
                    case Token.TOKEN_TYPE.IF:
                        CodeCommand condition = ConditionParse();
                        if (ErrorGet() != "") {
                            return null;
                        }
                        commands.Add(condition);
                        break;
                    case Token.TOKEN_TYPE.RETURN:
                        if (cycle == BODY_CYCLE.STATE) {
                            ErrorSet("Команда return не используется в state");
                            return null;
                        }
                        CodeCommand ret = ReturnParse();
                        if (ErrorGet() != "") {
                            return null;
                        }
                        commands.Add(ret);
                        int next_tab_count = TokenCountGet(Token.TOKEN_TYPE.TAB);
                        if (next_tab_count == max_tab_count && _tokens.Count > next_tab_count) {
                            PositionSet(_tokens[_tokens.Count - next_tab_count - 1].PositionGet());
                            ErrorSet("Недостижимый код после return");
                            return null;
                        }
                        break;
                    default:
                        ErrorSet($"Неожиданная команда в теле: {token.TextGet()}");
                        return null;
                }
            }
            tab_count = TokenCountGet(Token.TOKEN_TYPE.TAB);
        }
        if (commands.Count == 0) {
            ErrorSet("Отсутствуют команды после объявления оператора");
            return null;
        }
        ScopeGet().ScopeEnd();
        return new CodeBody(commands, PositionGet());
    }


    private CodeCommand ReturnParse() {
        int position = PositionGet();
        CodeCommand var = BitFormulaParse();
        if (ErrorGet() != "") {
            return null;
        }
        CodeFunctionSet function = FunctionCurrentGet();
        if (function != null && var.ReturnTypeGet() != function.ReturnTypeGet()) {
            PositionSet(position);
            ErrorSet($"return возвращает {TypeNameGet(var.ReturnTypeGet())}, а функция {function.NameGet()} объявлена как {TypeNameGet(function.ReturnTypeGet())}");
            return null;
        }
        return new Return(var, var.PositionGet());
    }

    private CodeCommand VariableSetParse() {
        Token variable_name = TokenPopBack();
        if (variable_name == null || variable_name.TokenTypeGet() != Token.TOKEN_TYPE.NAME) {
            ErrorSet("PARSE COMMAND: VARIABLE NAME INVALID");
            return null;
        }
        if (TokenSkip(Token.TOKEN_TYPE.ASSIGN) == false) {
            ErrorSet("PARSE COMMAND: VARIABLE ASSIGN INVALID");
            return null;
        }
        CodeCommand command = BitFormulaParse();
        if (ErrorGet() != "") {
            return null;
        }
        CodeCommand variable = ScopeGet().VariableGet(variable_name.TextGet());
        if (variable != null) {
            if (command.ReturnTypeGet() != variable.ReturnTypeGet()){
                ErrorSet("PARSE COMMAND: VARIABLE TYPE INVALID");
                return null;
            }
        }
        ScopeGet().VariableSet(variable_name.TextGet(), command);
        return new VariableSet(variable_name.TextGet(), command, variable_name.PositionGet());
    }

    private CodeCommand ConditionParse() {
        if (TokenSkip(Token.TOKEN_TYPE.COLON) == false) {
            ErrorSet("PARSE CONDITION: COLON INVALID");
            return null;
        }
        return null;
    }
    
    private CodeCommand BitFormulaParse(){
        CodeCommand left = BitOperatorParse();
        if (ErrorGet() != "") {
            return null;
        }
        while (Token.IsBitOperator(TokenTypeGet())) {
            left = BitOperatorParse(left);
            if (ErrorGet() != "") {
                return null;
            }
        }
        return left;
    }

    private CodeCommand BitOperatorParse(CodeCommand left = null){
        if (left == null) {
            if (IsToken(Token.TOKEN_TYPE.NOT)){ //переписать на match
                Token token = TokenPopBack();
                CodeCommand formula = BitOperatorParse();
                if (ErrorGet() != "") {
                   return null;
                }
                return new BitFormulaNot(formula, token.PositionGet());
            } else if (IsToken(Token.TOKEN_TYPE.LPAR)) {
                TokenPopBack();
                CodeCommand formula = BitFormulaParse();
                if (ErrorGet() != "") {
                     return null;
                }
                if(TokenSkip(Token.TOKEN_TYPE.RPAR) == false) {
                    ErrorSet("PARSE BIT OPERATOR: FORMULA PARENTHESES INVALID");
                    return null;
                }
                return formula;
            }
            CodeCommand variable = ComparsionFormulaParse();
            if (ErrorGet() != "") {
                return null;
            }
            return variable;
        } else {
            
            Token toperator = TokenPopBack();
            if (toperator == null) {
                ErrorSet("PARSE BIT OPERATOR: BIT OPERATOR INVALID");
                return null;
            }
                
            CodeCommand right = BitFormulaParse();
            if (ErrorGet() != "") {
                return null;
            }
   
            if (right.ReturnTypeGet() == CodeCommand.RETURN_TYPE.BOOL && left.ReturnTypeGet() == CodeCommand.RETURN_TYPE.BOOL) {
                switch (toperator.TokenTypeGet()) {
                    case Token.TOKEN_TYPE.AND:
                        return new BitFormulaAnd(left, right, toperator.PositionGet());
                    case Token.TOKEN_TYPE.OR:
                        return new BitFormulaOr(left, right, toperator.PositionGet());
                }
            } else {
                ErrorSet("PARSE BIT OPERATOR: FORMULA CONVERT INVALID");
                return null;
            }
        }
        ErrorSet("PARSE BIT OPERATOR: FORMULA INVALID");
        return null;
    }

    private CodeCommand ComparsionFormulaParse() {
        CodeCommand left = ComparsionOperatorParse();
        if (ErrorGet() != "") {
            return null;
        }
        while (Token.IsComparsionOperator(TokenTypeGet())) {
            left = ComparsionOperatorParse(left);
            if (ErrorGet() != "") {
                return null;
            }
        }
        return left;

    }
    private CodeCommand ComparsionOperatorParse(CodeCommand left = null) {
        if (left == null){
            CodeCommand variable = SummatorFormulaParse(); // математическая, битовая или строковая
            if (ErrorGet() != "") {
                return null;
            }
            return variable;
        } else {
            Token toperator = TokenPopBack();
            if (toperator == null) {
                ErrorSet("PARSE COMPARSION OPERATOR: COMPARSION OPERATOR INVALID");
                return null;
            }
                
            CodeCommand right = SummatorFormulaParse(); // математическая, битовая или строковая
            if (ErrorGet() != "") {
                return null;
            }
            if (right.ReturnTypeGet() == CodeCommand.RETURN_TYPE.NUMBER && left.ReturnTypeGet() == CodeCommand.RETURN_TYPE.NUMBER) {
                switch (toperator.TokenTypeGet()) {
                    case Token.TOKEN_TYPE.MORE:
                        return new MathFormulaMore(left, right, toperator.PositionGet());
                    case Token.TOKEN_TYPE.LESS:
                        return new MathFormulaLess(left, right, toperator.PositionGet());
                    case Token.TOKEN_TYPE.MORE_EQUAL:
                        return new MathFormulaMoreEqual(left, right, toperator.PositionGet());
                    case Token.TOKEN_TYPE.LESS_EQUAL:
                        return new MathFormulaLessEqual(left, right, toperator.PositionGet());
                    case Token.TOKEN_TYPE.NOT_EQUAL:
                        return new MathFormulaNotEqual(left, right, toperator.PositionGet());
                    case Token.TOKEN_TYPE.EQUAL:
                        return new MathFormulaEqual(left, right, toperator.PositionGet());
                }
            } else if (right.ReturnTypeGet() == CodeCommand.RETURN_TYPE.BOOL && left.ReturnTypeGet() == CodeCommand.RETURN_TYPE.BOOL) {
                switch (toperator.TokenTypeGet()) {
                    case Token.TOKEN_TYPE.NOT_EQUAL:
                        return new BitFormulaNotEqual(left, right, toperator.PositionGet());
                    case Token.TOKEN_TYPE.EQUAL:
                        return new BitFormulaEqual(left, right, toperator.PositionGet());
                }
            } else if (right.ReturnTypeGet() == CodeCommand.RETURN_TYPE.TEXT && left.ReturnTypeGet() == CodeCommand.RETURN_TYPE.TEXT) {
                switch (toperator.TokenTypeGet()) {
                    case Token.TOKEN_TYPE.EQUAL:
                        return new TextFormulaEqual(left, right, toperator.PositionGet());
                    case Token.TOKEN_TYPE.NOT_EQUAL:
                        return new TextFormulaNotEqual(left, right, toperator.PositionGet());
                }
            }
        }
        
        ErrorSet("PARSE COMPARSION OPERATOR: FORMULA INVALID");
        return null;
    }


    private CodeCommand SummatorFormulaParse() {
       CodeCommand left = SummatorOperatorParse();
       if (ErrorGet() != "") {
            return null;
        }
        while (Token.IsMathOperator(TokenTypeGet())) {
            left = SummatorOperatorParse(left);
            if (ErrorGet() != "") {
                return null;
            }
        }
        return left;
    }

    private CodeCommand SummatorOperatorParse(CodeCommand left = null) {
        if (left == null) {
            if (IsToken(Token.TOKEN_TYPE.MINUS)){ //переписать на match
                Token token = TokenPopBack();
                CodeCommand formula = SummatorOperatorParse();
                if (ErrorGet() != "") {
                   return null;
                }
                return new MathFormulaNot(formula, token.PositionGet());
            } else if (IsToken(Token.TOKEN_TYPE.LPAR)) {
                TokenPopBack();
                CodeCommand formula = SummatorFormulaParse();
                if (ErrorGet() != "") {
                     return null;
                }
                if(TokenSkip(Token.TOKEN_TYPE.RPAR) == false) {
                    ErrorSet("PARSE FORMULA: FORMULA PARENTHESES INVALID");
                    return null;
                }
                return formula;
            }
            CodeCommand variable = VariableParse();
            if (ErrorGet() != "") {
                return null;
            }
            return variable;
        }
        else {
            Token toperator = TokenPopBack();
            if (toperator == null) {
                ErrorSet("PARSE FORMULA: SUMMATOR OPERATOR INVALID");
                return null;
            }
            
            CodeCommand right;
            if (IsToken(Token.TOKEN_TYPE.MULTIPLE, 2) == true){
                CodeCommand variable = VariableParse();
                if (ErrorGet() != "") {
                     return null;
                }
                right = SummatorOperatorParse(variable);
                if (ErrorGet() != "") {
                    return null;
                }
            } else if (IsToken(Token.TOKEN_TYPE.DIVISION, 2) == true){
                CodeCommand variable = VariableParse();
                if (ErrorGet() != "") {
                     return null;
                }
                right = SummatorOperatorParse(variable);
                if (ErrorGet() != "") {
                    return null;
                }
            }
            else {
                right = SummatorOperatorParse();
                if (ErrorGet() != "") {
                    return null;
                }
            }

            if (right.ReturnTypeGet() == CodeCommand.RETURN_TYPE.NUMBER && left.ReturnTypeGet() == CodeCommand.RETURN_TYPE.NUMBER) {
                switch (toperator.TokenTypeGet()) {
                    case Token.TOKEN_TYPE.PLUS:
                        return new MathFormulaPlus(left, right, toperator.PositionGet());
                    case Token.TOKEN_TYPE.MINUS:
                        return new MathFormulaMinus(left, right, toperator.PositionGet());
                    case Token.TOKEN_TYPE.MULTIPLE:
                        return new MathFormulaMultiple(left, right, toperator.PositionGet());
                    case Token.TOKEN_TYPE.DIVISION:
                        return new MathFormulaDivision(left, right, toperator.PositionGet());
                }
            } else if (right.ReturnTypeGet() == CodeCommand.RETURN_TYPE.TEXT && left.ReturnTypeGet() == CodeCommand.RETURN_TYPE.TEXT) {
                switch (toperator.TokenTypeGet()) {
                    case Token.TOKEN_TYPE.PLUS:
                        return new TextFormulaPlus(left, right, toperator.PositionGet());
                }
            }
        }
        ErrorSet("PARSE FORMULA: FORMULA INVALID");
        return null;
    }

    private CodeCommand VariableParse() {
        CodeCommand value = ValueParse();
        while (value != null && IsToken(Token.TOKEN_TYPE.DOT) && IsToken(Token.TOKEN_TYPE.LPAR, 3) == false) {
            value = FieldParse(value);
        }
        return value;
    }

    private CodeCommand FieldParse(CodeCommand target) {
        TokenSkip(Token.TOKEN_TYPE.DOT);
        Token field = TokenPopBack();
        if (field == null || field.TokenTypeGet() != Token.TOKEN_TYPE.NAME) {
            ErrorSet("Ожидалось имя поля после точки");
            return null;
        }
        if (target.ReturnTypeGet() != CodeCommand.RETURN_TYPE.VECTOR) {
            ErrorSet($"Поле .{field.TextGet()} есть только у vec, а не у {TypeNameGet(target.ReturnTypeGet())}");
            return null;
        }
        if (field.TextGet() != "x" && field.TextGet() != "y") {
            ErrorSet($"У vec нет поля .{field.TextGet()}, есть только .x и .y");
            return null;
        }
        return new VectorField(target, field.TextGet(), field.PositionGet());
    }

    private CodeCommand ValueParse() {
        if(IsToken(Token.TOKEN_TYPE.LPAR, 2)) {
            CodeCommand func = FunctionGetFormula();
             if (ErrorGet() != "") {
                return null;
            }
            return func;
        }

        Token token = TokenPopBack();
        if (token == null) {
            ErrorSet("Объявление переменной не найдено");
            return null;
        }
        switch (token.TokenTypeGet())
        {
            case Token.TOKEN_TYPE.NUMBER:
                return new Number(Convert.ToDouble(token.TextGet(), System.Globalization.CultureInfo.InvariantCulture), token.PositionGet());
            case Token.TOKEN_TYPE.TEXT:
                return new Text(token.TextGet().TrimStart('"').TrimEnd('"'), token.PositionGet());
            case Token.TOKEN_TYPE.TRUE:
                return new Boolean(true, token.PositionGet());
            case Token.TOKEN_TYPE.FALSE:
                return new Boolean(false, token.PositionGet());
            case Token.TOKEN_TYPE.NAME:
                CodeCommand variable = ScopeGet().VariableGet(token.TextGet());
                if (variable == null) {
                    ErrorSet("Переменная не объявлена");
                    return null;                    
                }
                switch(variable.ReturnTypeGet()) {
                    case CodeCommand.RETURN_TYPE.TEXT:
                        return new VariableTextGet(token.TextGet(), token.PositionGet());
                    case CodeCommand.RETURN_TYPE.NUMBER:
                        return new VariableNumberGet(token.TextGet(), token.PositionGet());
                    case CodeCommand.RETURN_TYPE.BOOL:
                        return new VariableBoolGet(token.TextGet(), token.PositionGet());
                    case CodeCommand.RETURN_TYPE.VECTOR:
                        return new VariableVectorGet(token.TextGet(), token.PositionGet());
                }
                break;
        }
        ErrorSet("Разбор переменной не удался");
        return null;
    }


    private CodeCommand FunctionGetFormula() {
        CodeCommand parent = FunctionGetOperator();
        while(IsToken(Token.TOKEN_TYPE.DOT) && IsToken(Token.TOKEN_TYPE.LPAR, 3)) {
            TokenSkip(Token.TOKEN_TYPE.DOT);
            // если объект может иметь функцию
            parent = FunctionGetOperator(parent);
        }
        return parent;
    }

 // вначале обязует написать функцию
 // 
    private CodeCommand FunctionGetOperator(CodeCommand parent = null) {
        Token func_name = TokenPopBack();
        if (func_name == null) {
            ErrorSet("Разбор функции не удался");
            return null;
        }
        CodeFunctionSet function = FunctionGet(func_name.TextGet());
        if (function == null) {
            ErrorSet("Определение функции по названию не найдено");
            return null;
        }
        if (TokenSkip(Token.TOKEN_TYPE.LPAR) == false) {
            ErrorSet("Ожидалась открывающая скобка");
            return null;
        }
        List<CodeCommand> param_values = FunctionGetParametersParse();
        if (ErrorGet() != "") {
            return null;
        }
        if (TokenSkip(Token.TOKEN_TYPE.RPAR) == false) {
            ErrorSet("Ожидалась закрывающая скобка");
            return null;
        }

        List<String> param_names = function.ParametersGet().Keys.ToList();
        if( param_names.Count != param_values.Count) {
            ErrorSet("Количество переданных аргументов функции не равно количеству объявленных");
            return null;
        }
        Dictionary<String, CodeCommand> parameters = new Dictionary<String, CodeCommand>();
        for(int i = 0; i < param_names.Count; i++) {
            if (function.ParametersGet()[param_names[i]] == CodeCommand.RETURN_TYPE.VOID) {
                ErrorSet("Переданный тип аргумента функции равен void");
                return null;
            }
            if (function.ParametersGet()[param_names[i]] != CodeCommand.RETURN_TYPE.ANY && function.ParametersGet()[param_names[i]] != param_values[i].ReturnTypeGet()) {
                ErrorSet("Переданный тип аргумента функции не равен объявленному типу аргумента");
                return null;
            }
            parameters[param_names[i]] = param_values[i];
        }

        return new CodeFunctionGet(func_name.PositionGet(), parameters, function, parent);
    }

    private List<CodeCommand> FunctionGetParametersParse() {
       List<CodeCommand> parameters = new List<CodeCommand>();
       
        if (IsToken(Token.TOKEN_TYPE.RPAR)) {
            return parameters;
        }
        
        do {
            CodeCommand variable = BitFormulaParse();
            if (ErrorGet() != "") {
                return null;
            }
            parameters.Add(variable);
        }  while(TokenSkip(Token.TOKEN_TYPE.COMMA) == true);

        return parameters;
    }

    public Parser() {

    }
}
