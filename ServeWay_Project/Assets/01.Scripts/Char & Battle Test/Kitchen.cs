using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Kitchen : MonoBehaviour
{
    private FoodDataSet foodInfo;
    private StartFoodDataSet startFoodInfo;
    private FoodIngredDex dex;
    private InventoryManager Inventory;
    private List<string> list;
    private bool isTouch;
    private CreateUI createUI;
    private InteractionWindow interaction;

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
        dex = FindObjectOfType<DataController>().FoodIngredDex;
        createUI = FindObjectOfType<CreateUI>();
        interaction = FindObjectOfType<InteractionWindow>();

        createUI.gameObject.SetActive(false);

        isVisited = false;
    }

    
    void Update()
    {
        if(isTouch && interaction.cookInteraction.activeSelf)
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                if(Time.timeScale == 1)
                {
                    CreatableList();
                    createUI.gameObject.SetActive(true);
                    createUI.deltaTime = Time.deltaTime;
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
                if(dex.foodDex[food.foodName] != FoodDex_Status.LOCKED)
                {
                    list.Add(food.foodName);
                }
            }
        }
        else
        {
            foreach (FoodData food in foodInfo.FoodDatas)
            {
                if (CheckIngredient(food.needIngredient, Inventory.inventory) && dex.foodDex[food.foodName] != FoodDex_Status.LOCKED)
                {
                    list.Add(food.foodName);
                }
            }
        }
        
    }

    private bool CheckIngredient(NameAmount food, Dictionary<Ingred_Name, int> inven)
    {
        foreach(Ingred_Name ingred in food.Keys)
        {
            if(!inven.ContainsKey(ingred) || food[ingred] > inven[ingred])
            {
                return false;
            }
        }

        return true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            /*if (!SceneManager.GetActiveScene().name.Contains("Start"))
            {

            }*/

            // 주방 작동
            isTouch = true;
            interaction.SetCookAble(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            isTouch = false;
            interaction.SetCookAble(false);
        }
    }
}
