using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CreateUI : MonoBehaviour
{
    private List<GameObject> buttonList;
    private List<GameObject> ingredientList;
    private DataController dataController;
    private FoodDataSet foodInfo;
    private StartFoodDataSet startFoodInfo;
    private FoodData selectedFood;
    private IngredientDataSet ingredientInfo;
    private InventoryManager inventory;
    private WeaponSlot weaponSlot;
    private bool isFirst;
    public bool isOpen;

    public float deltaTime;
    public Create_Success success;
    public GameObject buttonGroup;
    public GameObject explaneUI;
    public GameObject ingredientUI;
    public List<GameObject> minigameUI;
    public GameObject weaponPrefab;
    public AudioSource menuOpen;
    public AudioSource menuClick;

    void Start()
    {
        dataController = FindObjectOfType<DataController>();
        foodInfo = dataController.foodData;
        startFoodInfo = dataController.startFoodData;
        ingredientInfo = dataController.IngredientList;
        inventory = FindObjectOfType<InventoryManager>();
        weaponSlot = FindObjectOfType<WeaponSlot>();

        buttonList = new List<GameObject>();
        for (int i = 0; i < buttonGroup.transform.childCount; i++)
        {
            buttonList.Add(buttonGroup.transform.GetChild(i).gameObject);
        }

        ingredientList = new List<GameObject>();
        for(int i = 0; i < ingredientUI.transform.childCount; i++)
        {
            ingredientList.Add(ingredientUI.transform.GetChild(i).gameObject);
        }

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && isOpen)
        {
            CloseUI();
        }
    }

    private void OnEnable()
    {
        isOpen = true;
        if(!isFirst)
        {
            isFirst = true;
        }
        else
        {
            menuOpen.Play();
        }
    }

    private void OnDisable()
    {
        isOpen = false;
    }

    public void SetList(List<string> nameList)
    {
        Debug.Log(buttonList.Count);

        for(int i = 0; i < buttonList.Count; i++)
        {
            if(i < nameList.Count)
            {
                buttonList[i].SetActive(true);
                buttonList[i].GetComponentInChildren<TMP_Text>().text = nameList[i];
            }
            else
            {
                buttonList[i].SetActive(false);
            }
        }
    }

    public void OnSelectClicked(TMP_Text text)
    {
        List<FoodData> list;

        explaneUI.SetActive(true);

        if(SceneManager.GetActiveScene().name.Contains("Start"))
        {
            list = startFoodInfo.StartFoodDatas;
        }
        else
        {
            list = foodInfo.FoodDatas;
        }

        menuClick.Play();

        foreach(FoodData food in list)
        {
            if(food.foodName == text.text)
            {
                selectedFood = food;

                explaneUI.transform.GetChild(0).GetComponent<Image>().sprite = food.foodSprite;
                explaneUI.transform.GetChild(1).GetComponent<TMP_Text>().text = food.foodName;
                explaneUI.transform.GetChild(2).GetComponent<TMP_Text>().text = food.EunmToString(food.grade);
                explaneUI.transform.GetChild(3).GetComponent<TMP_Text>().text = food.EunmToString(food.mainIngred);
                explaneUI.transform.GetChild(4).GetComponent<TMP_Text>().text = food.EunmToString(food.nation);

                int miniGameIndex = 0;
                switch(food.mainIngred)
                {
                    case Food_MainIngred.RICE:
                        miniGameIndex = 0;
                        break;
                    case Food_MainIngred.SOUP:
                        miniGameIndex = 1;
                        break;
                    case Food_MainIngred.NOODLE:
                        miniGameIndex = 2;
                        break;
                    case Food_MainIngred.BREAD:
                        miniGameIndex = 3;
                        break;
                    case Food_MainIngred.MEAT:
                        miniGameIndex = 4;
                        break;
                }
                explaneUI.transform.GetChild(5).GetComponent<Button>().onClick.RemoveAllListeners();
                explaneUI.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(delegate () { this.OnCreateClicked(miniGameIndex); });

                for (int i = 0; i < ingredientList.Count; i++)
                {
                    ingredientList[i].SetActive(false);
                }

                int index = 0;

                foreach (Ingred_Name name in food.needIngredient.Keys)
                {
                    ingredientList[index].SetActive(true);
                    ingredientList[index].GetComponent<Image>().sprite = dataController.FindIngredient(name).sprite;
                    ingredientList[index].GetComponentInChildren<TMP_Text>().text = string.Format("X {0}", food.needIngredient[name].ToString());

                    index++;
                }
            }
        }
    }

    public void OnCreateClicked(int index)
    {
        minigameUI[index].SetActive(true);
        menuClick.Play();
    }

    public void OnGameCleared(GameObject gameUI)
    {
        gameUI.SetActive(false);

        if (!SceneManager.GetActiveScene().name.Contains("Start"))
        {
            foreach (Ingred_Name name in selectedFood.needIngredient.Keys)
            {
                inventory.DeleteItem(name, selectedFood.needIngredient[name]);
            }
        }

        if(weaponSlot.WeaponCount() != 3)
        {
            weaponSlot.GetWeapon(weaponPrefab, success, selectedFood.foodName);
        }
        else
        {
            GameObject food = Instantiate(selectedFood.foodPrefab, GameObject.FindGameObjectWithTag("Player").transform.position + new Vector3(-2f, 0, 0), Quaternion.Euler(0, 0, 0));
            food.GetComponent<GetItem>().name = selectedFood.foodName;
            food.GetComponent<GetItem>().SetSprite();
            food.GetComponent<GetItem>().success = success;
        }

        CloseUI();
    }

    public void CloseUI()
    {
        explaneUI.SetActive(false);
        foreach(GameObject minigame in minigameUI)
        {
            minigame.SetActive(false);
        }
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
