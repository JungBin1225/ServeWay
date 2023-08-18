using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreateUI : MonoBehaviour
{
    private List<GameObject> buttonList;
    private FoodInfoList foodInfo;
    private FoodInfo selectedFood;
    private InventoryManager inventory;

    public GameObject explaneUI;

    void Start()
    {
        foodInfo = FindObjectOfType<DataController>().FoodInfoList;
        inventory = FindObjectOfType<InventoryManager>();

        buttonList = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).gameObject.name.Contains("Button"))
            {
                buttonList.Add(transform.GetChild(i).gameObject);
            }
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
        explaneUI.SetActive(true);

        foreach(FoodInfo food in foodInfo.foodInfo)
        {
            if(food.foodName == text.text)
            {
                selectedFood = food;

                explaneUI.transform.GetChild(0).GetComponent<Image>().sprite = food.foodSprite;
                explaneUI.transform.GetChild(1).GetComponent<TMP_Text>().text = food.foodName;
            }
        }
    }

    public void OnCreateClicked()
    {
        foreach(IngredientList.IngredientsName name in selectedFood.needIngredient.Keys)
        {
            inventory.DeleteItem(name, selectedFood.needIngredient[name]);
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
