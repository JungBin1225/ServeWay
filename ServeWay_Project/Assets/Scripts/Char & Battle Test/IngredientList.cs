using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient List", menuName = "Scriptable Object/Ingredient List", order = int.MinValue)]


[System.Serializable]
public class IngredientList : ScriptableObject
{
    public enum IngredientsName{Pumpkin, Strawberrie};

    [SerializeField]
    public IngredientInfo ingredientInfo;
}

[System.Serializable]
public class IngredientInfo : SerializableDictionary<IngredientList.IngredientsName, Sprite> { }

