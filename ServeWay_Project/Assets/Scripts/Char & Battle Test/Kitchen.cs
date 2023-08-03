using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kitchen : MonoBehaviour
{
    private FoodInfoList foodInfo;
    private InventoryManager Inventory;
    private string list;
    private bool isTouch;

    void Start()
    {
        list = "";
        isTouch = false;
        Inventory = FindObjectOfType<InventoryManager>();
        foodInfo = FindObjectOfType<DataController>().FoodInfoList;
    }

    
    void Update()
    {
        if(isTouch)
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                CreatableList();
            }
        }
    }

    private void CreatableList()
    {
        foreach(FoodInfo food in foodInfo.foodInfo)
        {
            if(CheckIngredient(food.needIngredient, Inventory.inventory))
            {
                Debug.Log(food.foodName);
            }
        }
    }

    private bool CheckIngredient(NameAmount food, Dictionary<IngredientList.IngredientsName, int> inven)
    {
        foreach(IngredientList.IngredientsName ingred in food.Keys)
        {
            if(!inven.ContainsKey(ingred) || food[ingred] > inven[ingred])
            {
                return false;
            }
        }

        return true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            isTouch = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            isTouch = false;
        }
    }
}
