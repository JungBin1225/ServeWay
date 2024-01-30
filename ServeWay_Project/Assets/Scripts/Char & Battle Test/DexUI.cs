using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DexUI : MonoBehaviour
{
    private DataController dataController;
    private int dexMod;
    private List<string> foodList;
    private List<Ingred_Name> ingredientList;
    private List<GameObject> buttonList;
    private int page;

    public Sprite lockSprite;
    public GameObject buttonGruop;
    public GameObject infoWindow;
    public Material defultMaterial;
    public Material grayScale;

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
        ingredientList = new List<Ingred_Name>();
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
                    foodList.Add(food);
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

                switch (dataController.FoodIngredDex.foodDex[foodList[i]])
                {
                    case FoodDex_Status.CREATED:
                        buttonList[i % buttonList.Count].GetComponent<Image>().sprite = dataController.FindFood(foodList[i]).foodSprite;
                        buttonList[i % buttonList.Count].GetComponent<Image>().material = defultMaterial;
                        break;
                    case FoodDex_Status.RECIPE:
                        buttonList[i % buttonList.Count].GetComponent<Image>().sprite = dataController.FindFood(foodList[i]).foodSprite;
                        buttonList[i % buttonList.Count].GetComponent<Image>().material = grayScale;
                        break;
                    case FoodDex_Status.LOCKED:
                        buttonList[i % buttonList.Count].GetComponent<Image>().sprite = lockSprite;
                        buttonList[i % buttonList.Count].GetComponent<Image>().material = defultMaterial;

                        break;
                }

                
            }
        }
        else
        {
            ingredientList = new List<Ingred_Name>();

            foreach (Ingred_Name ingred in dataController.FoodIngredDex.ingredDex.Keys)
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
                buttonList[i % buttonList.Count].GetComponent<Image>().sprite = dataController.FindIngredient(ingredientList[i]).sprite;
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
                if (buttonList.Count * (this.page + 1) < foodList.Count)
                {
                    this.page += 1;
                }
            }
            else
            {
                Debug.Log(ingredientList.Count);
                if (buttonList.Count * (this.page + 1) < ingredientList.Count)
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
            if (dataController.FindFood(image.sprite) != null)
            {
                FoodData food = dataController.FindFood(image.sprite);

                if(dataController.FoodIngredDex.foodDex[food.foodName] == FoodDex_Status.CREATED)
                {
                    infoWindow.transform.GetChild(3).GetChild(2).gameObject.SetActive(true);
                    infoWindow.transform.GetChild(3).GetChild(3).gameObject.SetActive(true);
                    infoWindow.transform.GetChild(3).GetChild(5).gameObject.SetActive(true);
                    infoWindow.transform.GetChild(3).GetChild(6).gameObject.SetActive(true);

                    infoWindow.transform.GetChild(2).GetComponent<Image>().sprite = food.foodSprite;
                    infoWindow.transform.GetChild(3).GetChild(0).GetComponent<TMP_Text>().text = food.foodName;
                    infoWindow.transform.GetChild(3).GetChild(1).GetComponent<TMP_Text>().text = food.EunmToString(food.grade);
                    infoWindow.transform.GetChild(3).GetChild(2).GetComponent<TMP_Text>().text = food.EunmToString(food.mainIngred);
                    infoWindow.transform.GetChild(3).GetChild(3).GetComponent<TMP_Text>().text = food.EunmToString(food.nation);
                    infoWindow.transform.GetChild(3).GetChild(4).GetComponent<TMP_Text>().text = string.Format("만족도: {0}", food.damage);
                    infoWindow.transform.GetChild(3).GetChild(5).GetComponent<TMP_Text>().text = string.Format("서빙 속도: {0}", food.speed);
                    infoWindow.transform.GetChild(3).GetChild(6).GetComponent<TMP_Text>().text = string.Format("조리 속도: {0}", food.coolTime);
                }
                else
                {
                    infoWindow.SetActive(false);
                }
            }
            else
            {
                Ingredient ingred = dataController.FindIngredient(image.sprite);

                infoWindow.transform.GetChild(2).GetComponent<Image>().sprite = ingred.sprite;
                infoWindow.transform.GetChild(3).GetChild(0).GetComponent<TMP_Text>().text = ingred.EnumToString(ingred.name);
                infoWindow.transform.GetChild(3).GetChild(1).GetComponent<TMP_Text>().text = ingred.EunmToString(ingred.grade);
                infoWindow.transform.GetChild(3).GetChild(2).gameObject.SetActive(false);
                infoWindow.transform.GetChild(3).GetChild(3).gameObject.SetActive(false);
                infoWindow.transform.GetChild(3).GetChild(4).GetComponent<TMP_Text>().text = ingred.passive;
                infoWindow.transform.GetChild(3).GetChild(5).gameObject.SetActive(false);
                infoWindow.transform.GetChild(3).GetChild(6).gameObject.SetActive(false);
            }
        }
    }

    public void OnInfoCloseClicked()
    {
        infoWindow.SetActive(false);
    }
}
