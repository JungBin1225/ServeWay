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
                return "ȣ��";
            case IngredientList.IngredientsName.Strawberrie:
                return "����";
            case IngredientList.IngredientsName.Water:
                return "��";
            case IngredientList.IngredientsName.Meat:
                return "���";
            case IngredientList.IngredientsName.Bread:
                return "��";
            case IngredientList.IngredientsName.Noodle:
                return "��";
            case IngredientList.IngredientsName.Onion:
                return "����";
            case IngredientList.IngredientsName.Lettuce:
                return "�����";
            case IngredientList.IngredientsName.Garlic:
                return "����";
            case IngredientList.IngredientsName.Pepper:
                return "�Ǹ�";
            case IngredientList.IngredientsName.Rice:
                return "��";
            case IngredientList.IngredientsName.Salt:
                return "�ұ�";
            case IngredientList.IngredientsName.Sugar:
                return "����";
            default:
                return "";
        }
    }

    public string EunmToString(Ingred_Grade grade)
    {
        switch (grade)
        {
            case Ingred_Grade.STAR_1:
                return "1��";
            case Ingred_Grade.STAR_2:
                return "2��";
            case Ingred_Grade.STAR_3:
                return "3��";
            case Ingred_Grade.STAR_4:
                return "4��";
            default:
                return "";
        }
    }
}
