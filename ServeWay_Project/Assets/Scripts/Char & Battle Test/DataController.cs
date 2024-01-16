using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataController : MonoBehaviour
{
    public FoodDataSet foodData;
    public StartFoodDataSet startFoodData;
    public IngredientList IngredientList;
    public EnemyList enemyList;
    public FoodIngredDex FoodIngredDex;

    public FoodData FindFood(string name)
    {
        foreach(FoodData food in foodData.FoodDatas)
        {
            if(name == food.foodName)
            {
                return food;
            }
        }

        foreach(FoodData food in startFoodData.StartFoodDatas)
        {
            if(name == food.foodName)
            {
                return food;
            }
        }

        return null;
    }

    public FoodData FindFood(Sprite sprite)
    {
        foreach (FoodData food in foodData.FoodDatas)
        {
            if (sprite == food.foodSprite)
            {
                return food;
            }
        }

        foreach (FoodData food in startFoodData.StartFoodDatas)
        {
            if (sprite == food.foodSprite)
            {
                return food;
            }
        }

        return null;
    }
}
