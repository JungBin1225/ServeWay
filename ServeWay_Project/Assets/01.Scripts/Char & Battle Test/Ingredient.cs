using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public enum Ingred_Grade
{
    STAR_1,
    STAR_2,
    STAR_3,
    STAR_4
};

public enum Ingred_Name
{
    Pumpkin, Strawberrie, Water, Meat, Bread, Noodle, Onion, Lettuce, Garlic, Pepper, Rice, Salt, Sugar
};

[Serializable]
public class Ingredient
{
    [SerializeField] private int _Index;
    [SerializeField] private Ingred_Name _Name;
    [SerializeField] private Ingred_Grade _Grade;
    [SerializeField] private string _SpritePath;
    [SerializeField] private string _PrefabPath;
    [SerializeField] private string _Passive;

    public int index { get { return _Index; } }
    public Ingred_Name name { get { return _Name; } }
    public Ingred_Grade grade { get { return _Grade; } }
    public Sprite sprite
    {
        get
        {
            Sprite getSprite = null;
            UnityEngine.Object[] sprites = AssetDatabase.LoadAllAssetsAtPath("Assets/Prefabs/MapPrefabs/Backyard - Free/backyard.png");
            foreach (UnityEngine.Object sprite in sprites)
            {
                if (sprite.name.Contains(_SpritePath))
                {
                    getSprite = (Sprite)sprite;
                }
            }
            return getSprite;
        }
    }
    public GameObject prefab
    {
        get
        {
            GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(_PrefabPath, typeof(GameObject));

            return prefab;
        }
    }

    public string passive { get { return _Passive; } }

    public string EnumToString(Ingred_Name name)
    {
        switch(name)
        {
            case Ingred_Name.Pumpkin:
                return "È£¹Ú";
            case Ingred_Name.Strawberrie:
                return "µþ±â";
            case Ingred_Name.Water:
                return "¹°";
            case Ingred_Name.Meat:
                return "°í±â";
            case Ingred_Name.Bread:
                return "»§";
            case Ingred_Name.Noodle:
                return "¸é";
            case Ingred_Name.Onion:
                return "¾çÆÄ";
            case Ingred_Name.Lettuce:
                return "¾ç¹èÃß";
            case Ingred_Name.Garlic:
                return "¸¶´Ã";
            case Ingred_Name.Pepper:
                return "ÇÇ¸Á";
            case Ingred_Name.Rice:
                return "½Ò";
            case Ingred_Name.Salt:
                return "¼Ò±Ý";
            case Ingred_Name.Sugar:
                return "¼³ÅÁ";
            default:
                return "";
        }
    }

    public string EunmToString(Ingred_Grade grade)
    {
        switch (grade)
        {
            case Ingred_Grade.STAR_1:
                return "1¼º";
            case Ingred_Grade.STAR_2:
                return "2¼º";
            case Ingred_Grade.STAR_3:
                return "3¼º";
            case Ingred_Grade.STAR_4:
                return "4¼º";
            default:
                return "";
        }
    }
}
