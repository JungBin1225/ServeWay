using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private FoodIngredDex dex;
    private FoodDataSet foodData;
    private StartFoodDataSet startFoodData;
    private IngredientDataSet ingredientData;

    private int star1;
    private int star2;
    private int star3;
    private int star4;

    public bool dexRefresh;
    public bool addInventory;
    public bool checkPercent;

    void Start()
    {
        dex = FindObjectOfType<DataController>().FoodIngredDex;
        foodData = FindObjectOfType<DataController>().foodData;
        startFoodData = FindObjectOfType<DataController>().startFoodData;
        ingredientData = FindObjectOfType<DataController>().IngredientList;

        star1 = 0;
        star2 = 0;
        star3 = 0;
        star4 = 0;

        dexRefresh = false;
        addInventory = false;
        checkPercent = false;
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

        if(checkPercent)
        {
            CheckPercent();
            checkPercent = false;
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

    private void CheckPercent()
    {
        star1 = 0;
        star2 = 0;
        star3 = 0;
        star4 = 0;

        EnemyGenerator generator = FindObjectOfType<EnemyGenerator>();

        for(int i = 0; i < 1000; i++)
        {
            Ingredient ingred = generator.RandomIngredient();

            switch(ingred.grade)
            {
                case Ingred_Grade.STAR_1:
                    star1++;
                    break;
                case Ingred_Grade.STAR_2:
                    star2++;
                    break;
                case Ingred_Grade.STAR_3:
                    star3++;
                    break;
                case Ingred_Grade.STAR_4:
                    star4++;
                    break;
            }
        }

        Debug.Log("1성: " + star1 + " 2성: " + star2 + " 3성: " + star3 + " 4성: " + star4);
    }
}
