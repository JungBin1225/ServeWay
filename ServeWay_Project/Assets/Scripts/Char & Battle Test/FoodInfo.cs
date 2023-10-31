using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Food_Grade
{
    STAR_1,
    STAR_2,
    STAR_3,
    STAR_4
};

public enum Food_MainIngred
{
    MEAT,
    RICE,
    SOUP,
    NOODLE,
    BREAD
};

public enum Food_Nation
{
    KOREA,
    JAPAN,
    CHINA,
    USA,
    FRANCE
};

[Serializable]
public class NameAmount : SerializableDictionary<IngredientList.IngredientsName, int> { }

[Serializable]
public class FoodInfo
{
    public string foodName;
    public Sprite foodSprite;

    public Food_Grade grade;
    public Food_MainIngred mainIngred;
    public Food_Nation nation;

    public float damage;
    public float speed;
    public float coolTime;

    public float successDamage;
    public float successSpeed;
    public float successCoolTime;

    public GameObject foodPrefab;
    public NameAmount needIngredient;

    public string EunmToString(Food_Grade grade)
    {
        switch(grade)
        {
            case Food_Grade.STAR_1:
                return "1성";
            case Food_Grade.STAR_2:
                return "2성";
            case Food_Grade.STAR_3:
                return "3성";
            case Food_Grade.STAR_4:
                return "4성";
            default:
                return "";
        }
    }

    public string EunmToString(Food_MainIngred ingred)
    {
        switch (ingred)
        {
            case Food_MainIngred.MEAT:
                return "고기";
            case Food_MainIngred.RICE:
                return "쌀";
            case Food_MainIngred.SOUP:
                return "국물";
            case Food_MainIngred.NOODLE:
                return "면";
            case Food_MainIngred.BREAD:
                return "빵";
            default:
                return "";
        }
    }

    public string EunmToString(Food_Nation nation)
    {
        switch (nation)
        {
            case Food_Nation.KOREA:
                return "한국";
            case Food_Nation.JAPAN:
                return "일본";
            case Food_Nation.CHINA:
                return "중국";
            case Food_Nation.USA:
                return "미국";
            case Food_Nation.FRANCE:
                return "프랑스";
            default:
                return "";
        }
    }
}
