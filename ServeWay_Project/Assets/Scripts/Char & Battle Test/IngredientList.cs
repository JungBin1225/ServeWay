using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient List", menuName = "Scriptable Object/Ingredient List", order = int.MinValue)]


[System.Serializable]
public class IngredientList : ScriptableObject
{
    public enum IngredientsName
    {
        Pumpkin, Strawberrie, Water, Meat, Bread, Noodle, Onion, Lettuce, Garlic, Pepper
    };

    [SerializeField]
    public List<Ingredient> ingredientList;

    public Ingredient FindIngredient(IngredientsName name)
    {
        foreach(Ingredient ingredient in ingredientList)
        {
            if(ingredient.name == name)
            {
                return ingredient;
            }
        }

        return null;
    }
}

