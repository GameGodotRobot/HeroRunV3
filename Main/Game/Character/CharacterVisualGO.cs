using Godot;
using System;
using System.Collections.Generic;

public partial class CharacterVisualGO : RefCounted
{ 
    public enum RACE
    {
        HUMAN = 1,
        ORK = 2
    }
    private RACE _race;
    private void RaceSet(RACE value) {
        _race = value;
        HairstyleSetByTypeId(0);
        HairColorSetByTypeId(0);
        EyeColorSetByTypeId(0);
        BodyColorSetByTypeId(0);
        DressColorSetByTypeId(0);
        PantsColorSetByTypeId(0);
        DetailsSetByTypeId(0);
    }
    public RACE RaceGet() {
        return _race;
    }
    
    private readonly static Dictionary<RACE, Vector2I[]> HAIRSTYLE_TYPE = new Dictionary<RACE, Vector2I[]>{
        [RACE.HUMAN] = new Vector2I[] {
            Vector2I.Zero, 
            new Vector2I(32,0)},
        [RACE.ORK] = new Vector2I[] {
            new Vector2I(32,0),
            Vector2I.Zero}
    };
    private Vector2I _hairstyle;
    private void HairstyleSet(Vector2I value){
        _hairstyle = value;
    }
    public Vector2I HairstyleGet() {
        return _hairstyle;
    }
    public void HairstyleSetByTypeId(int value) {
        HairstyleSet(HAIRSTYLE_TYPE[RaceGet()][value]);
    }
    public int HairstyleTypesSize() {
        return HAIRSTYLE_TYPE[RaceGet()].Length;
    }

    private readonly static Dictionary<RACE, Color[]> HAIR_COLOR_TYPE = new Dictionary<RACE, Color[]>{
        [RACE.HUMAN] = new Color[] {
			new Color("#d65631"), //рыжие
			new Color("#fff616"), //блондин
			new Color("#df1b1b"), //красные
			new Color("#e3f0ee"), //седые
			new Color("#58a0f3"), //голубые
			new Color("#7af358") //зеленые
        },
        [RACE.ORK] = new Color[] {
			new Color("#d65631"), //рыжие
			new Color("#fff616"), //блондин
			new Color("#df1b1b"), //красные
			new Color("#e3f0ee"), //седые
			new Color("#58a0f3"), //голубые
			new Color("#7af358") //зеленые
        }
    };
    private Color _hair_color;
    private void HairColorSet(Color value){
        _hair_color = value;
    }
    public Color HairColorGet() {
        return _hair_color;
    }
    public void HairColorSetByTypeId(int value) {
        HairColorSet(HAIR_COLOR_TYPE[RaceGet()][value]);
    }
    public int HairColorTypesSize() {
        return HAIR_COLOR_TYPE[RaceGet()].Length;
    }


    private readonly static Dictionary<RACE, Color[]> EYE_COLOR_TYPE = new Dictionary<RACE, Color[]>{
        [RACE.HUMAN] = new Color[] {
			new Color("#2976ba"), //голубые
			new Color("#56ae44"), //зеленые
			new Color("#e82948"), //красные
			new Color("#1d1d1d"), //черные
			new Color("#a03415") //каричневые
        },
        [RACE.ORK] = new Color[] {
			new Color("#000000"), //черные
			new Color("#b21616"), //красные
			new Color("#baa843"), //желтые 
			new Color("#6a1c6b") //фиолетовые
        }
    };
    private Color _eye_color;
    private void EyeColorSet(Color value){
        _eye_color = value;
    }
    public Color EyeColorGet() {
        return _eye_color;
    }
    public void EyeColorSetByTypeId(int value) {
        EyeColorSet(EYE_COLOR_TYPE[RaceGet()][value]);
    }
    public int EyeColorTypesSize() {
        return EYE_COLOR_TYPE[RaceGet()].Length;
    }

    private readonly static Dictionary<RACE, Color[]> BODY_COLOR_TYPE = new Dictionary<RACE, Color[]>{
        [RACE.HUMAN] = new Color[] {
			new Color("#fccba3"), //обычное
			new Color("#c8966d"), //смуглый
			new Color("#b6904b") //каричневый
        },
        [RACE.ORK] = new Color[] {
			new Color("#3d734f"), //зеленый
			new Color("#a18c42"), //каричневый
			new Color("#968f78"), //серый
			new Color("#ac3f44") //красный
        }
    };
    private Color _body_color;
    private void BodyColorSet(Color value){
        _body_color = value;
    }
    public Color BodyColorGet() {
        return _body_color;
    }
    public void BodyColorSetByTypeId(int value) {
        BodyColorSet(BODY_COLOR_TYPE[RaceGet()][value]);
    }
    public int BodyColorTypesSize() {
        return BODY_COLOR_TYPE[RaceGet()].Length;
    }

    private readonly static Dictionary<RACE, Color[]> DRESS_COLOR_TYPE = new Dictionary<RACE, Color[]>{
        [RACE.HUMAN] = new Color[] {
			new Color("#fccba3"), //обычное
			new Color("#5a86bf"), //голубая
			new Color("#60bb36"), //зеленая
			new Color("#bb8136"), //каричневая
			new Color("#e8634d"), //караловая
			new Color("#b04de8") //фиолетовая
        },
        [RACE.ORK] = new Color[] {
			new Color("#3d734f"), //зеленый
			new Color("#a18c42"), //каричневый
			new Color("#968f78"), //серый
			new Color("#ac3f44") //красный
        }
    };
    private Color _dress_color;
    private void DressColorSet(Color value){
        _dress_color = value;
    }
    public Color DressColorGet() {
        return _dress_color;
    }
    public void DressColorSetByTypeId(int value) {
        DressColorSet(DRESS_COLOR_TYPE[RaceGet()][value]);
    }
    public int DressColorTypesSize() {
        return DRESS_COLOR_TYPE[RaceGet()].Length;
    }
    
    private readonly static Dictionary<RACE, Color[]> PANTS_COLOR_TYPE = new Dictionary<RACE, Color[]>{
        [RACE.HUMAN] = new Color[] {
			new Color("#fccba3"), //обычное
			new Color("#5a86bf"), //голубая
			new Color("#60bb36"), //зеленая
			new Color("#bb8136"), //каричневая
			new Color("#e8634d"), //караловая
			new Color("#b04de8") //фиолетовая
        },
        [RACE.ORK] = new Color[] {
			new Color("#29527e"), //Синий
			new Color("#65297e"), //Фиолетовый
			new Color("#297e34"), //зеленый
			new Color("#5f2f15"), //каричневый
			new Color("#1e1e24") //черный
        }
    };
    private Color _pants_color;
    private void PantsColorSet(Color value){
        _pants_color = value;
    }
    public Color PantsColorGet() {
        return _pants_color;
    }
    public void PantsColorSetByTypeId(int value) {
        PantsColorSet(PANTS_COLOR_TYPE[RaceGet()][value]);
    }
    public int PantsColorTypesSize() {
        return PANTS_COLOR_TYPE[RaceGet()].Length;
    }

    private readonly static Dictionary<RACE, Vector2I[]> DETAILS_TYPE = new Dictionary<RACE, Vector2I[]>{
        [RACE.HUMAN] = new Vector2I[] {
			new Vector2I(16,80)
        },
        [RACE.ORK] = new Vector2I[] {
			new Vector2I(0,80),
			new Vector2I(16,80)
        }
    };
    private Vector2I _details;
    private void DetailsSet(Vector2I value){
        _details = value;
    }
    public Vector2I DetailsGet() {
        return _details;
    }
    public void DetailsSetByTypeId(int value) {
        DetailsSet(DETAILS_TYPE[RaceGet()][value]);
    }
    public int DetailsTypesSize() {
        return DETAILS_TYPE[RaceGet()].Length;
    }

    public void Shuffle() {
        HairstyleSetByTypeId(0);
        HairColorSetByTypeId(0);
        EyeColorSetByTypeId(0);
        BodyColorSetByTypeId(0);
        DressColorSetByTypeId(0);
        PantsColorSetByTypeId(0);
        DetailsSetByTypeId(0);
    }

    public CharacterVisualGO() {
        RaceSet(RACE.HUMAN);
    }
}
