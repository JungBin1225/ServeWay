using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DexUI : MonoBehaviour
{
    private DataController dataController;
    private int dexMod;
    private List<string> foodList;
    private List<IngredientList.IngredientsName> ingredientList;
    private List<GameObject> buttonList;
    private int page;

    public Sprite lockSprite;
    public GameObject buttonGruop;
    public GameObject infoWindow;

    void Start()
    {
        /*dataController = FindObjectOfType<DataController>();
        foodDex = transform.GetChild(0).gameObject;
        ingredDex = transform.GetChild(1).gameObject;*/
    }

    private void OnEnable()
    {
        dataController = FindObjectOfType<DataController>();
        buttonList = new List<GameObject>();
        for(int i = 0; i < buttonGruop.transform.childCount; i++)
        {
            buttonList.Add(buttonGruop.transform.GetChild(i).gameObject);
        }

        foodList = new List<string>();
        ingredientList = new List<IngredientList.IngredientsName>();
        dexMod = 0;
        page = 0;

        InitList(page);
    }

    private void InitList(int page)
    {
        if(dexMod == 0)
        {
            foodList = new List<string>();

            foreach (string food in dataController.FoodIngredDex.foodDex.Keys)
            {
                if(dataController.FoodIngredDex.foodDex[food])
                {
                    foodList.Add(food);
                }
            }

            foreach(GameObject button in buttonList)
            {
                button.GetComponent<Image>().sprite = lockSprite;
            }

            
            for(int i = buttonList.Count * page; i < foodList.Count; i++)
            {
                if(i != buttonList.Count * page && i % buttonList.Count == 0)
                {
                    break;
                }
                buttonList[i % buttonList.Count].GetComponent<Image>().sprite = dataController.FoodInfoList.FindFood(foodList[i]).foodSprite;
            }
        }
        else
        {
            ingredientList = new List<IngredientList.IngredientsName>();

            foreach (IngredientList.IngredientsName ingred in dataController.FoodIngredDex.ingredDex.Keys)
            {
                if(dataController.FoodIngredDex.ingredDex[ingred])
                {
                    ingredientList.Add(ingred);
                }
            }

            foreach (GameObject button in buttonList)
            {
                button.GetComponent<Image>().sprite = lockSprite;
            }

            for (int i = buttonList.Count * page; i < ingredientList.Count; i++)
            {
                if (i != buttonList.Count * page && i % buttonList.Count == 0)
                {
                    break;
                }
                buttonList[i % buttonList.Count].GetComponent<Image>().sprite = dataController.IngredientList.FindIngredient(ingredientList[i]).sprite;
            }
        }
    }

    public void OnModChangeClicked(int index)
    {
        dexMod = index;
        page = 0;

        InitList(page);
    }

    public void OnPageClicked(int page)
    {
        if (page < 0)
        {
            if (this.page > 0)
            {
                this.page -= 1;
            }
        }
        else
        {
            if (dexMod == 0)
            {
                if (buttonList.Count * (page + 1) < foodList.Count)
                {
                    this.page += 1;
                }
            }
            else
            {
                if (buttonList.Count * (page + 1) < ingredientList.Count)
                {
                    this.page += 1;
                }
            }
        }

        InitList(this.page);
    }

    public void OnInfoOpenClicked(Image image)
    {
        if (image.sprite != lockSprite)
        {
            infoWindow.SetActive(true);
            infoWindow.transform.GetChild(2).GetComponent<Image>().sprite = image.sprite;
        }
    }

    public void OnInfoCloseClicked()
    {
        infoWindow.SetActive(false);
    }
}
