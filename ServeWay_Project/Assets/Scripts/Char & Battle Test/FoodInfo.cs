using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class NameAmount : SerializableDictionary<IngredientList.IngredientsName, int> { }

[Serializable]
public class FoodInfo
{
    public string foodName;
    public Sprite foodSprite;
    public GameObject foodPrefab;
    public NameAmount needIngredient;
}
