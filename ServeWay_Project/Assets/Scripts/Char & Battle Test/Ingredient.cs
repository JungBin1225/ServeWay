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
                return "È£¹Ú";
            case IngredientList.IngredientsName.Strawberrie:
                return "µþ±â";
            case IngredientList.IngredientsName.Water:
                return "¹°";
            case IngredientList.IngredientsName.Meat:
                return "°í±â";
            case IngredientList.IngredientsName.Bread:
                return "»§";
            case IngredientList.IngredientsName.Noodle:
                return "¸é";
            case IngredientList.IngredientsName.Onion:
                return "¾çÆÄ";
            case IngredientList.IngredientsName.Lettuce:
                return "¾ç¹èÃß";
            case IngredientList.IngredientsName.Garlic:
                return "¸¶´Ã";
            case IngredientList.IngredientsName.Pepper:
                return "ÇÇ¸Á";
            case IngredientList.IngredientsName.Rice:
                return "½Ò";
            case IngredientList.IngredientsName.Salt:
                return "¼Ò±Ý";
            case IngredientList.IngredientsName.Sugar:
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
