using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Food Info List", menuName = "Scriptable Object/Food Info List", order = int.MaxValue)]
public class FoodInfoList : ScriptableObject
{
    [SerializeField]
    public List<FoodInfo> foodInfo;
    public List<FoodInfo> start_foodInfo;

    public string FindNameToSprite(Sprite sprite)
    {
        foreach(FoodInfo food in foodInfo)
        {
            if(sprite == food.foodSprite)
            {
                return food.foodName;
            }
        }
        
        return null;
    }

    public GameObject FindPrefabToName(string name)
    {
        foreach (FoodInfo food in foodInfo)
        {
            if (name == food.foodName)
            {
                return food.foodPrefab;
            }
        }

        return null;
    }
}
