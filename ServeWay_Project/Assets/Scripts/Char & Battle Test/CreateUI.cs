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
    private FoodInfoList foodInfo;
    private FoodInfo selectedFood;
    private IngredientList ingredientInfo;
    private InventoryManager inventory;

    public GameObject explaneUI;
    public GameObject ingredientUI;

    void Start()
    {
        foodInfo = FindObjectOfType<DataController>().FoodInfoList;
        ingredientInfo = FindObjectOfType<DataController>().IngredientList;
        inventory = FindObjectOfType<InventoryManager>();

        buttonList = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).gameObject.name.Contains("Button"))
            {
                buttonList.Add(transform.GetChild(i).gameObject);
            }
        }

        ingredientList = new List<GameObject>();
        for(int i = 0; i < ingredientUI.transform.childCount; i++)
        {
            ingredientList.Add(ingredientUI.transform.GetChild(i).gameObject);
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            CloseUI();
        }
    }

    public void SetList(List<string> nameList)
    {
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
        List<FoodInfo> list;

        explaneUI.SetActive(true);

        if(SceneManager.GetActiveScene().name.Contains("Start"))
        {
            list = foodInfo.start_foodInfo;
        }
        else
        {
            list = foodInfo.foodInfo;
        }
        

        foreach(FoodInfo food in list)
        {
            if(food.foodName == text.text)
            {
                selectedFood = food;

                explaneUI.transform.GetChild(0).GetComponent<Image>().sprite = food.foodSprite;
                explaneUI.transform.GetChild(1).GetComponent<TMP_Text>().text = food.foodName;

                for(int i = 0; i < ingredientList.Count; i++)
                {
                    ingredientList[i].SetActive(false);
                }

                int index = 0;

                foreach (IngredientList.IngredientsName name in food.needIngredient.Keys)
                {
                    ingredientList[index].SetActive(true);
                    ingredientList[index].GetComponent<Image>().sprite = ingredientInfo.ingredientList[ingredientInfo.FindIndex(name)].sprite;
                    ingredientList[index].GetComponentInChildren<TMP_Text>().text = string.Format("X {0}", food.needIngredient[name].ToString());

                    index++;
                }
            }
        }
    }

    public void OnCreateClicked()
    {
        if (!SceneManager.GetActiveScene().name.Contains("Start"))
        {
            foreach (IngredientList.IngredientsName name in selectedFood.needIngredient.Keys)
            {
                inventory.DeleteItem(name, selectedFood.needIngredient[name]);
            }
        }

        Instantiate(selectedFood.foodPrefab, GameObject.FindGameObjectWithTag("Player").transform.position + new Vector3(-2f, 0, 0), Quaternion.Euler(0, 0, 0));

        CloseUI();
    }

    public void CloseUI()
    {
        explaneUI.SetActive(false);
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
