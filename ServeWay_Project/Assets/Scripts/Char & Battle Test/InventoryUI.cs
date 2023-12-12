using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Sprite defaultSprite;

    private List<GameObject> inventoryButtonList_0;
    private List<GameObject> inventoryButtonList_1;
    private List<GameObject> foodImageList;
    private InventoryManager inventory;
    private IngredientList itemList;
    private FoodInfoList foodInfoList;
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
        foodInfoList = FindObjectOfType<DataController>().FoodInfoList;
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
            foodImageList[i].GetComponent<Image>().sprite = foodInfoList.FindFood(name).foodSprite;

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

    public void OnButtonClicked(int num)
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
    }
}
