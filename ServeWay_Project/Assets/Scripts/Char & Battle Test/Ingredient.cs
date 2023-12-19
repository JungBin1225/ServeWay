using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Ingred_Grade
{
    STAR_1,
    STAR_2,
    STAR_3,
    STAR_4
};

[Serializable]
public class Ingredient
{
    public int index;
    public IngredientList.IngredientsName name;
    public Ingred_Grade grade;
    public Sprite sprite;
    public GameObject prefab;

    public string passive;

    public string EnumToString(IngredientList.IngredientsName name)
    {
        switch(name)
        {
            case IngredientList.IngredientsName.Pumpkin:
                return "호박";
            case IngredientList.IngredientsName.Strawberrie:
                return "딸기";
            case IngredientList.IngredientsName.Water:
                return "물";
            case IngredientList.IngredientsName.Meat:
                return "고기";
            case IngredientList.IngredientsName.Bread:
                return "빵";
            case IngredientList.IngredientsName.Noodle:
                return "면";
            case IngredientList.IngredientsName.Onion:
                return "양파";
            case IngredientList.IngredientsName.Lettuce:
                return "양배추";
            case IngredientList.IngredientsName.Garlic:
                return "마늘";
            case IngredientList.IngredientsName.Pepper:
                return "피망";
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
