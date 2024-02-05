using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FoodDex_Status
{
    CREATED, RECIPE, LOCKED
};

[System.Serializable]
public class FoodDex : SerializableDictionary<string, FoodDex_Status> { }

[System.Serializable]
public class IngredDex : SerializableDictionary<Ingred_Name, bool> { }

[CreateAssetMenu(fileName = "FoodIngred Dex", menuName = "Scriptable Object/FoodIngred Dex", order = int.MinValue + 3)]
public class FoodIngredDex : ScriptableObject
{
    public FoodDex foodDex;
    public IngredDex ingredDex;

    public void UpdateFoodDex(string foodName, FoodDex_Status status)
    {
        if(foodDex[foodName] == FoodDex_Status.LOCKED)
        {
            foodDex[foodName] = FoodDex_Status.RECIPE;
        }

        switch(status)
        {
            case FoodDex_Status.LOCKED:
                foodDex[foodName] = FoodDex_Status.LOCKED;
                break;
            case FoodDex_Status.RECIPE:
                if(foodDex[foodName] == FoodDex_Status.LOCKED)
                {
                    foodDex[foodName] = FoodDex_Status.RECIPE;
                }
                break;
            case FoodDex_Status.CREATED:
                foodDex[foodName] = FoodDex_Status.CREATED;
                break;
        }

        UnityEditor.EditorUtility.SetDirty(this);
    }

    public void UpdateIngredDex(Ingred_Name name)
    {
        ingredDex[name] = true;

        UnityEditor.EditorUtility.SetDirty(this);
    }
}
