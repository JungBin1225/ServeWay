using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Sprite defaultSprite;

    private List<GameObject> inventoryButtonList;
    private List<GameObject> foodImageList;
    private InventoryManager inventory;
    private IngredientList itemList;
    private FoodInfoList foodInfoList;
    private List<string> weaponName;
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
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            foodImageList.Add(transform.GetChild(0).GetChild(i).gameObject);
        }

        inventoryButtonList = new List<GameObject>();
        for (int i = 0; i < transform.GetChild(1).childCount; i++)
        {
            inventoryButtonList.Add(transform.GetChild(1).GetChild(i).gameObject);
        }

        InitFood();
        InitIngred();
    }

    void Update()
    {
        
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

    public void InitIngred()
    {
        int i = 0;
        foreach(IngredientList.IngredientsName item in inventory.inventory.Keys)
        {
            inventoryButtonList[i].transform.GetChild(0).gameObject.SetActive(true);
            inventoryButtonList[i].GetComponent<Image>().sprite = itemList.ingredientList[itemList.FindIndex(item)].sprite;
            inventoryButtonList[i].transform.GetChild(0).GetComponent<Text>().text = inventory.inventory[item].ToString();

            i++;
        }

        if(i < inventoryButtonList.Count - 1)
        {
            for(; i < inventoryButtonList.Count; i++)
            {
                inventoryButtonList[i].transform.GetChild(0).gameObject.SetActive(false);
                inventoryButtonList[i].GetComponent<Image>().sprite = defaultSprite;
            }
        }
    }
    

    public void OnButtonClicked(int num)
    {
        IngredientList.IngredientsName name;

        foreach(Ingredient item in itemList.ingredientList)
        {
            if(item.sprite == inventoryButtonList[num].GetComponent<Image>().sprite)
            {
                name = item.name;
                inventory.DeleteItem(name, 1);
                InitIngred();
                break;
            }
        }
    }
}
