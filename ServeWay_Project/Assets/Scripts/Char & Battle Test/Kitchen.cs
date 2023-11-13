using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Kitchen : MonoBehaviour
{
    private FoodInfoList foodInfo;
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
        foodInfo = FindObjectOfType<DataController>().FoodInfoList;
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
            foreach(FoodInfo food in foodInfo.start_foodInfo)
            {
                list.Add(food.foodName);
            }
        }
        else
        {
            foreach (FoodInfo food in foodInfo.foodInfo)
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
                // 미니맵
                if (!isVisited)
                {
                    isVisited = true;
                    // miniMapMeshGroup 게임 오브젝트의 자식 오브젝트로 방의 메시 프리팹 생성
                    Instantiate(miniRoomMesh, transform).transform.SetParent(GameObject.Find("miniMapMeshGroup").transform);
                }

                GameObject.Find("miniPlayer").transform.position = gameObject.transform.position;
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
