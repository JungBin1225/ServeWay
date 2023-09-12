using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kitchen : MonoBehaviour
{
    private FoodInfoList foodInfo;
    private InventoryManager Inventory;
    private List<string> list;
    private bool isTouch;
    private CreateUI createUI;

    void Start()
    {
        list = new List<string>();
        isTouch = false;
        Inventory = FindObjectOfType<InventoryManager>();
        foodInfo = FindObjectOfType<DataController>().FoodInfoList;
        createUI = FindObjectOfType<CreateUI>();

        createUI.gameObject.SetActive(false);
    }

    
    void Update()
    {
        if(isTouch)
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                if(Time.timeScale == 1)
                {
                    CreatableList();
                    createUI.gameObject.SetActive(true);
                    createUI.SetList(list);
                    list.Clear();
                    Time.timeScale = 0;
                }
            }
        }
    }

    private void CreatableList()
    {
        foreach(FoodInfo food in foodInfo.foodInfo)
        {
            if(CheckIngredient(food.needIngredient, Inventory.inventory))
            {
                list.Add(food.foodName);
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
