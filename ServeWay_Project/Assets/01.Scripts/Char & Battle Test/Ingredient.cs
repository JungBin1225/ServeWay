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
    Pumpkin, Strawberrie, Water, Meat, Flour, Noodle, Onion,
    Lettuce, Garlic, Pepper, Rice, Salt, Sugar, Spice, Egg,
    Fish, Oil, Tomato, Sauce, Mushroom, Milk, Butter, Cheese,
    Fruit, Greenonion, Carrot, Beansprout, Seaweed, Vanilla,
    Coriander, Kimchi, Vinegar, Tofu, Wine, Salary, Radish,
    Redbean, Eggplant, Olive, Cream, Nuts, Cucumber, Honey,
    Ginger, Corn, Coconut
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
    private Sprite _IngredSprite;

    public int index { get { return _Index; } }
    public Ingred_Name name { get { return _Name; } }
    public Ingred_Grade grade { get { return _Grade; } }
    public Sprite sprite
    {
        get
        {
            Sprite[] sprites = Resources.LoadAll<Sprite>("Pixel_House_Icons_1.2.1-16x");
            _IngredSprite = sprites[int.Parse(_SpritePath)] as Sprite;

            return _IngredSprite;
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
                return "호박";
            case Ingred_Name.Strawberrie:
                return "딸기";
            case Ingred_Name.Water:
                return "물";
            case Ingred_Name.Meat:
                return "고기";
            case Ingred_Name.Flour:
                return "밀가루";
            case Ingred_Name.Noodle:
                return "면";
            case Ingred_Name.Onion:
                return "양파";
            case Ingred_Name.Lettuce:
                return "양배추";
            case Ingred_Name.Garlic:
                return "마늘";
            case Ingred_Name.Pepper:
                return "피망";
            case Ingred_Name.Rice:
                return "쌀";
            case Ingred_Name.Salt:
                return "소금";
            case Ingred_Name.Sugar:
                return "설탕";
            case Ingred_Name.Spice:
                return "향신료";
            case Ingred_Name.Egg:
                return "달걀";
            case Ingred_Name.Fish:
                return "해산물";
            case Ingred_Name.Oil:
                return "기름";
            case Ingred_Name.Tomato:
                return "토마토";
            case Ingred_Name.Sauce:
                return "장";
            case Ingred_Name.Mushroom:
                return "버섯";
            case Ingred_Name.Milk:
                return "우유";
            case Ingred_Name.Butter:
                return "버터";
            case Ingred_Name.Cheese:
                return "치즈";
            case Ingred_Name.Fruit:
                return "과일";
            case Ingred_Name.Greenonion:
                return "파";
            case Ingred_Name.Carrot:
                return "당근";
            case Ingred_Name.Beansprout:
                return "숙주";
            case Ingred_Name.Seaweed:
                return "김";
            case Ingred_Name.Vanilla:
                return "바닐라";
            case Ingred_Name.Coriander:
                return "고수";
            case Ingred_Name.Kimchi:
                return "김치";
            case Ingred_Name.Vinegar:
                return "식초";
            case Ingred_Name.Tofu:
                return "두부";
            case Ingred_Name.Wine:
                return "와인";
            case Ingred_Name.Salary:
                return "샐러리";
            case Ingred_Name.Radish:
                return "무";
            case Ingred_Name.Redbean:
                return "팥";
            case Ingred_Name.Eggplant:
                return "가지";
            case Ingred_Name.Olive:
                return "올리브";
            case Ingred_Name.Cream:
                return "크림";
            case Ingred_Name.Nuts:
                return "견과류";
            case Ingred_Name.Cucumber:
                return "오이";
            case Ingred_Name.Honey:
                return "꿀";
            case Ingred_Name.Ginger:
                return "생강";
            case Ingred_Name.Corn:
                return "옥수수";
            case Ingred_Name.Coconut:
                return "코코넛";
            default:
                return "";
        }
    }

    public string EunmToString(Ingred_Grade grade)
    {
        switch (grade)
        {
            case Ingred_Grade.STAR_1:
                return "1성";
            case Ingred_Grade.STAR_2:
                return "2성";
            case Ingred_Grade.STAR_3:
                return "3성";
            case Ingred_Grade.STAR_4:
                return "4성";
            default:
                return "";
        }
    }
}
