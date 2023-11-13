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
}
