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
}
