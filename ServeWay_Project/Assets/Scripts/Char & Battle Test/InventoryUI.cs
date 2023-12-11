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
        int i = 0;
        foreach(IngredientList.IngredientsName item in inventory.inventory.Keys)
        {
            inventoryButtonList_0[i].transform.GetChild(0).gameObject.SetActive(true);
            inventoryButtonList_0[i].GetComponent<Image>().sprite = itemList.ingredientList[itemList.FindIndex(item)].sprite;
            inventoryButtonList_0[i].transform.GetChild(0).GetComponent<Text>().text = inventory.inventory[item].ToString();

            i++;
        }

        if(i < inventoryButtonList_0.Count - 1)
        {
            for(; i < inventoryButtonList_0.Count; i++)
            {
                inventoryButtonList_0[i].transform.GetChild(0).gameObject.SetActive(false);
                inventoryButtonList_0[i].GetComponent<Image>().sprite = defaultSprite;
            }
        }
    }

    public void InitIngred_1()
    {
        for (int i = 0; i < inventoryButtonList_1.Count; i++)
        {
            inventoryButtonList_1[i].transform.GetChild(0).gameObject.SetActive(false);
            inventoryButtonList_1[i].GetComponent<Image>().sprite = defaultSprite;
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
            if(this.page < 1)
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
