using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Kitchen : MonoBehaviour
{
    private DataController data;
    private FoodDataSet foodInfo;
    private StartFoodDataSet startFoodInfo;
    private FoodIngredDex dex;
    private InventoryManager Inventory;
    private PlayerController player;
    private List<string> list;
    private bool isTouch;
    private bool startMaked;
    private CreateUI createUI;
    private InteractionWindow interaction;

    // 미니맵
    [SerializeField] private GameObject miniRoomMesh;
    [SerializeField] private SpriteRenderer foodImage;
    private bool isVisited = false;


    void Start()
    {
        list = new List<string>();
        isTouch = false;
        startMaked = false;
        Inventory = FindObjectOfType<InventoryManager>();
        data = FindObjectOfType<DataController>();
        player = FindObjectOfType<PlayerController>();
        foodInfo = data.foodData;
        startFoodInfo = data.startFoodData;
        dex = data.FoodIngredDex;
        createUI = FindObjectOfType<CreateUI>();
        createUI.kitchen = this;
        interaction = FindObjectOfType<InteractionWindow>();

        createUI.gameObject.SetActive(false);

        isVisited = false;

        if(!SceneManager.GetActiveScene().name.Contains("Start") && !SceneManager.GetActiveScene().name.Contains("Tutorial"))
        {
            SetNationFood();
        }
    }

    
    void Update()
    {
        if(isTouch && interaction.cookInteraction.activeSelf)
        {
            if(Input.GetKeyDown(KeyCode.F) && !startMaked)
            {
                if (Time.timeScale == 1 && player.controllAble)
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

    private void SetNationFood()
    {
        List<FoodData> foodList = data.GetNationFoodList(GameManager.gameManager.bossNations[GameManager.gameManager.stage - 1]);

        foodImage.enabled = true;
        foodImage.sprite = foodList[Random.Range(0, foodList.Count)].foodSprite;
    }

    public void StartMaked()
    {
        startMaked = true;
        isTouch = false;
        interaction.SetCookAble(false);
        interaction.AlreadyMaked(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !startMaked)
        {
            /*if (!SceneManager.GetActiveScene().name.Contains("Start"))
            {

            }*/

            // 주방 작동
            isTouch = true;
            interaction.SetCookAble(true);
        }
        else if(collision.gameObject.tag == "Player" && startMaked)
        {
            interaction.AlreadyMaked(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isTouch = false;
            interaction.SetCookAble(false);
            interaction.AlreadyMaked(false);
        }
    }
}
