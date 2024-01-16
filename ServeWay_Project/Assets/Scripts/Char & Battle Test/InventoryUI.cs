using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public Sprite defaultSprite;
    public GameObject infoWindow;

    private List<GameObject> inventoryButtonList_0;
    private List<GameObject> inventoryButtonList_1;
    private List<GameObject> foodImageList;
    private InventoryManager inventory;
    private IngredientList itemList;
    private DataController foodInfo;
    private List<string> weaponName;
    private List<IngredientList.IngredientsName> ingredients;
    private int page;

    void Start()
    {

    }

    private void OnEnable()
    {
        inventory = FindObjectOfType<InventoryManager>();
        itemList = FindObjectOfType<DataController>().IngredientList;
        foodInfo = FindObjectOfType<DataController>();
        weaponName = FindObjectOfType<WeaponSlot>().ReturnWeaponList();

        InitIngredList();

        foodImageList = new List<GameObject>();
        for (int i = 0; i < transform.GetChild(0).GetChild(0).childCount; i++)
        {
            foodImageList.Add(transform.GetChild(0).GetChild(0).GetChild(i).gameObject);
        }

        inventoryButtonList_0 = new List<GameObject>();
        for (int i = 0; i < transform.GetChild(0).GetChild(1).childCount; i++)
        {
            inventoryButtonList_0.Add(transform.GetChild(0).GetChild(1).GetChild(i).gameObject);
        }

        inventoryButtonList_1 = new List<GameObject>();
        for (int i = 0; i < transform.GetChild(1).childCount; i++)
        {
            inventoryButtonList_1.Add(transform.GetChild(1).GetChild(i).gameObject);
        }

        page = 0;

        InitPage(page);
    }

    private void OnDisable()
    {
        infoWindow.SetActive(false);
    }

    void Update()
    {
        
    }

    public void InitIngredList()
    {
        ingredients = new List<IngredientList.IngredientsName>();
        foreach (IngredientList.IngredientsName item in inventory.inventory.Keys)
        {
            ingredients.Add(item);
        }
    }

    public void InitPage(int page)
    {
        if(page == 0)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);

            InitFood();
            InitIngred_0();
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);

            InitIngred_1();
        }
    }

    public void InitFood()
    {
        int i = 0;
        foreach(string name in weaponName)
        {
            foodImageList[i].GetComponent<Image>().sprite = foodInfo.FindFood(name).foodSprite;

            i++;
        }

        if(i < foodImageList.Count - 1)
        {
            for(; i < foodImageList.Count; i++)
            {
                foodImageList[i].GetComponent<Image>().sprite = defaultSprite;
            }
        }
    }

    public void InitIngred_0()
    {
        InitIngredList();

        for (int j = 0; j < inventoryButtonList_0.Count; j++)
        {
            inventoryButtonList_0[j].transform.GetChild(0).gameObject.SetActive(false);
            inventoryButtonList_0[j].GetComponent<Image>().sprite = defaultSprite;
        }

        int i = 0;
        for(; i < inventoryButtonList_0.Count; i++)
        {
            if (ingredients.Count <= i)
            {
                break;
            }
            inventoryButtonList_0[i].transform.GetChild(0).gameObject.SetActive(true);
            inventoryButtonList_0[i].GetComponent<Image>().sprite = itemList.FindIngredient(ingredients[i]).sprite;
            inventoryButtonList_0[i].transform.GetChild(0).GetComponent<Text>().text = inventory.inventory[ingredients[i]].ToString();
        }
    }

    public void InitIngred_1()
    {
        InitIngredList();

        for (int j = 0; j < inventoryButtonList_1.Count; j++)
        {
            inventoryButtonList_1[j].transform.GetChild(0).gameObject.SetActive(false);
            inventoryButtonList_1[j].GetComponent<Image>().sprite = defaultSprite;
        }

        int i = 0;
        if (ingredients.Count > ((page - 1) * 6) + inventoryButtonList_0.Count)
        {
            for(; i < inventoryButtonList_1.Count; i++)
            {
                if (ingredients.Count <= ((page - 1) * 6) + inventoryButtonList_0.Count + i)
                {
                    break;
                }

                inventoryButtonList_1[i].transform.GetChild(0).gameObject.SetActive(true);
                inventoryButtonList_1[i].GetComponent<Image>().sprite = itemList.FindIngredient(ingredients[((page - 1) * 6) + inventoryButtonList_0.Count + i]).sprite;
                inventoryButtonList_1[i].transform.GetChild(0).GetComponent<Text>().text = inventory.inventory[ingredients[((page - 1) * 6) + inventoryButtonList_0.Count + i]].ToString();
            }
        }
    }
    
    public void OnPageClicked(int page)
    {
        if(page < 0)
        {
            if(this.page > 0)
            {
                this.page -= 1;
            }
        }
        else
        {
            if (this.page < ((ingredients.Count - 4) / 6 + 1) && ingredients.Count > 3)
            {
                this.page += 1;
            }
        }

        InitPage(this.page);
    }

    public void OnInfoOpenClicked(Image image)
    {
        if(image.sprite != defaultSprite)
        {
            infoWindow.SetActive(true);

            if(foodInfo.FindFood(image.sprite) != null)
            {
                FoodData food = foodInfo.FindFood(image.sprite);
                Create_Success success = FindObjectOfType<WeaponSlot>().GetWeaponInfo(food.foodName).success;

                string success_D = "";
                string success_S = "";
                string success_C = "";
                Color color = new Color(0, 0, 0);

                switch (success)
                {
                    case Create_Success.FAIL:
                        success_D = "- " + food.successDamage.ToString();
                        success_S = "- " + food.successSpeed.ToString();
                        success_C = "+ " + food.successCoolTime.ToString();
                        color = new Color(1, 0, 0);
                        break;
                    case Create_Success.SUCCESS:
                        success_D = "+ 0";
                        success_S = "+ 0";
                        success_C = "- 0";
                        color = new Color(1, 1, 1);
                        break;
                    case Create_Success.GREAT:
                        success_D = "+ " + food.successDamage.ToString();
                        success_S = "+ " + food.successSpeed.ToString();
                        success_C = "- " + food.successCoolTime.ToString();
                        color = new Color(0, 1, 0);
                        break;
                }

                infoWindow.transform.GetChild(3).GetChild(2).gameObject.SetActive(true);
                infoWindow.transform.GetChild(3).GetChild(3).gameObject.SetActive(true);
                infoWindow.transform.GetChild(3).GetChild(5).gameObject.SetActive(true);
                infoWindow.transform.GetChild(3).GetChild(6).gameObject.SetActive(true);
                infoWindow.transform.GetChild(3).GetChild(7).gameObject.SetActive(true);
                infoWindow.transform.GetChild(3).GetChild(8).gameObject.SetActive(true);
                infoWindow.transform.GetChild(3).GetChild(9).gameObject.SetActive(true);

                infoWindow.transform.GetChild(3).GetChild(7).GetComponent<TMP_Text>().color = color;
                infoWindow.transform.GetChild(3).GetChild(8).GetComponent<TMP_Text>().color = color;
                infoWindow.transform.GetChild(3).GetChild(9).GetComponent<TMP_Text>().color = color;

                infoWindow.transform.GetChild(2).GetComponent<Image>().sprite = food.foodSprite;
                infoWindow.transform.GetChild(3).GetChild(0).GetComponent<TMP_Text>().text = food.foodName;
                infoWindow.transform.GetChild(3).GetChild(1).GetComponent<TMP_Text>().text = food.EunmToString(food.grade);
                infoWindow.transform.GetChild(3).GetChild(2).GetComponent<TMP_Text>().text = food.EunmToString(food.mainIngred);
                infoWindow.transform.GetChild(3).GetChild(3).GetComponent<TMP_Text>().text = food.EunmToString(food.nation);
                infoWindow.transform.GetChild(3).GetChild(4).GetComponent<TMP_Text>().text = string.Format("만족도: {0}", food.damage);
                infoWindow.transform.GetChild(3).GetChild(5).GetComponent<TMP_Text>().text = string.Format("서빙 속도: {0}", food.speed);
                infoWindow.transform.GetChild(3).GetChild(6).GetComponent<TMP_Text>().text = string.Format("조리 속도: {0}", food.coolTime);
                infoWindow.transform.GetChild(3).GetChild(7).GetComponent<TMP_Text>().text = string.Format("({0})", success_D);
                infoWindow.transform.GetChild(3).GetChild(8).GetComponent<TMP_Text>().text = string.Format("({0})", success_S);
                infoWindow.transform.GetChild(3).GetChild(9).GetComponent<TMP_Text>().text = string.Format("({0})", success_C);
            }
            else
            {
                Ingredient ingred = itemList.FindIngredient(image.sprite);

                infoWindow.transform.GetChild(2).GetComponent<Image>().sprite = ingred.sprite;
                infoWindow.transform.GetChild(3).GetChild(0).GetComponent<TMP_Text>().text = ingred.EnumToString(ingred.name);
                infoWindow.transform.GetChild(3).GetChild(1).GetComponent<TMP_Text>().text = ingred.EunmToString(ingred.grade);
                infoWindow.transform.GetChild(3).GetChild(2).gameObject.SetActive(false);
                infoWindow.transform.GetChild(3).GetChild(3).gameObject.SetActive(false);
                infoWindow.transform.GetChild(3).GetChild(4).GetComponent<TMP_Text>().text = ingred.passive;
                infoWindow.transform.GetChild(3).GetChild(5).gameObject.SetActive(false);
                infoWindow.transform.GetChild(3).GetChild(6).gameObject.SetActive(false);
                infoWindow.transform.GetChild(3).GetChild(7).gameObject.SetActive(false);
                infoWindow.transform.GetChild(3).GetChild(8).gameObject.SetActive(false);
                infoWindow.transform.GetChild(3).GetChild(9).gameObject.SetActive(false);
            }
        }
    }

    public void OnInfoCloseClicked()
    {
        infoWindow.SetActive(false);
    }

    /*public void OnButtonClicked(int num)
    {
        IngredientList.IngredientsName name;

        foreach(Ingredient item in itemList.ingredientList)
        {
            Image image = inventoryButtonList_0[num].GetComponent<Image>();

            if(page == 0)
            {
                image = inventoryButtonList_0[num].GetComponent<Image>();
            }
            else
            {
                image = inventoryButtonList_1[num].GetComponent<Image>();
            }

            if(item.sprite == image.sprite)
            {
                name = item.name;
                inventory.DeleteItem(name, 1);
                InitPage(this.page);
                break;
            }
        }
    }*/
}
