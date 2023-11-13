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

    // �̴ϸ�
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
                // �̴ϸ�
                if (!isVisited)
                {
                    isVisited = true;
                    // miniMapMeshGroup ���� ������Ʈ�� �ڽ� ������Ʈ�� ���� �޽� ������ ����
                    Instantiate(miniRoomMesh, transform).transform.SetParent(GameObject.Find("miniMapMeshGroup").transform);
                }

                GameObject.Find("miniPlayer").transform.position = gameObject.transform.position;
            }

            // �ֹ� �۵�
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
