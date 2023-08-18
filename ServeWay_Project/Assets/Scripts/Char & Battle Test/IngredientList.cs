using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient List", menuName = "Scriptable Object/Ingredient List", order = int.MinValue)]


[System.Serializable]
public class IngredientList : ScriptableObject
{
    public enum IngredientsName{Pumpkin, Strawberrie};

    [SerializeField]
    public List<Ingredient> ingredientList;

    public int FindIndex(IngredientsName name)
    {
        foreach(Ingredient ingredient in ingredientList)
        {
            if(ingredient.name == name)
            {
                return ingredient.index;
            }
        }

        return 1000;
    }
}

