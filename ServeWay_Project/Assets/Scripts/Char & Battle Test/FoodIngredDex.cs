using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FoodDex : SerializableDictionary<string, bool> { }

[System.Serializable]
public class IngredDex : SerializableDictionary<Ingred_Name, bool> { }

[CreateAssetMenu(fileName = "FoodIngred Dex", menuName = "Scriptable Object/FoodIngred Dex", order = int.MinValue + 3)]
public class FoodIngredDex : ScriptableObject
{
    public FoodDex foodDex;
    public IngredDex ingredDex;
}
