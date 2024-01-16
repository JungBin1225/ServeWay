using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public FoodData foodData;
    public GameObject prefab;

    void Start()
    {
        foodData = FindObjectOfType<DataController>().foodData.FoodDatas[0];

        prefab = foodData.foodPrefab;
        foreach(IngredientList.IngredientsName name in foodData.needIngredient.Keys)
        {
            Debug.Log(name + ", " +foodData.needIngredient[name].ToString());
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
