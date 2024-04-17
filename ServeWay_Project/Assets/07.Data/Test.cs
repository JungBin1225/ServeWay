using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private FoodIngredDex dex;
    private FoodDataSet foodData;
    private StartFoodDataSet startFoodData;
    private IngredientDataSet ingredientData;

    public bool dexRefresh;
    public bool addInventory;

    void Start()
    {
        dex = FindObjectOfType<DataController>().FoodIngredDex;
        foodData = FindObjectOfType<DataController>().foodData;
        startFoodData = FindObjectOfType<DataController>().startFoodData;
        ingredientData = FindObjectOfType<DataController>().IngredientList;

        dexRefresh = false;
        addInventory = false;
    }

    
    void Update()
    {
        if(dexRefresh)
        {
            RefreshDex();
            dexRefresh = false;
        }

        if(addInventory)
        {
            AddAllInven();
            addInventory = false;
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

    private void AddAllInven()
    {
        foreach(Ingredient ingred in ingredientData.IngredientList)
        {
            GameManager.gameManager.inventory.GetItem(ingred.name, 5);
        }
    }
}
