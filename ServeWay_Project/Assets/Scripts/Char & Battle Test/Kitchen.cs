using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Kitchen : MonoBehaviour
{
    private FoodDataSet foodInfo;
    private StartFoodDataSet startFoodInfo;
    private InventoryManager Inventory;
    private List<string> list;
    private bool isTouch;
    private CreateUI createUI;

    // 미니맵
    [SerializeField] GameObject miniRoomMesh;
    private bool isVisited = false;

    void Start()
    {
        list = new List<string>();
        isTouch = false;
        Inventory = FindObjectOfType<InventoryManager>();
        foodInfo = FindObjectOfType<DataController>().foodData;
        startFoodInfo = FindObjectOfType<DataController>().startFoodData;
        createUI = FindObjectOfType<CreateUI>();

        createUI.gameObject.SetActive(false);

        isVisited = false;
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
        if (SceneManager.GetActiveScene().name.Contains("Start"))
        {
            foreach(FoodData food in startFoodInfo.StartFoodDatas)
            {
                list.Add(food.foodName);
            }
        }
        else
        {
            foreach (FoodData food in foodInfo.FoodDatas)
            {
                if (CheckIngredient(food.needIngredient, Inventory.inventory))
                {
                    list.Add(food.foodName);
                }
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
        if(collision.gameObject.tag == "Player")
        {
            if (!SceneManager.GetActiveScene().name.Contains("Start"))
            {

            }

            // 주방 작동
            isTouch = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            isTouch = false;
        }
    }
}
