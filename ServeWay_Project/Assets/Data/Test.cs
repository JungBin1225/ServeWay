using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public FoodData foodData;
    public Sprite sprite;

    void Start()
    {
        foodData = FindObjectOfType<DataController>().foodData.FoodDatas[0];

        sprite = foodData.foodSprite;
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
