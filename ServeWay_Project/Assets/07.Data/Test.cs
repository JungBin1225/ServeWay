using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private FoodIngredDex dex;
    private FoodDataSet foodData;
    private StartFoodDataSet startFoodData;
    private IngredientDataSet ingredientData;

    public bool refresh;

    void Start()
    {
        dex = FindObjectOfType<DataController>().FoodIngredDex;
        foodData = FindObjectOfType<DataController>().foodData;
        startFoodData = FindObjectOfType<DataController>().startFoodData;
        ingredientData = FindObjectOfType<DataController>().IngredientList;

        refresh = false;
    }

    
    void Update()
    {
        if(refresh)
        {
            RefreshDex();
            refresh = false;
        }
    }

    private void RefreshDex()
    {
        foreach(FoodData food in foodData.FoodDatas)
        {
            if(!dex.foodDex.ContainsKey(food.foodName))
            {
                dex.AddFoodDex(food.foodName, FoodDex_Status.RECIPE);
            }
        }
        foreach (FoodData food in startFoodData.StartFoodDatas)
        {
            if (!dex.foodDex.ContainsKey(food.foodName))
            {
                dex.AddFoodDex(food.foodName, FoodDex_Status.RECIPE);
            }
        }

        foreach(Ingredient ingred in ingredientData.IngredientList)
        {
            if(!dex.ingredDex.ContainsKey(ingred.name))
            {
                dex.AddIngredDex(ingred.name);
            }
        }

        Debug.Log("Refresh Success");
    }
}
