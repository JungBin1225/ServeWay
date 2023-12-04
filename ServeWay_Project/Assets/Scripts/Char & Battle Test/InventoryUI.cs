using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public List<GameObject> inventoryButtonList;
    public GameObject buttonGroup;
    public List<GameObject> panel;

    private InventoryManager inventory;
    private Sprite defaultSprite;
    private IngredientList itemList;
    private bool isOpen;
    private int index;
    void Start()
    {
        inventory = FindObjectOfType<InventoryManager>();
        defaultSprite = inventoryButtonList[0].GetComponent<Image>().sprite;
        itemList = FindObjectOfType<DataController>().IngredientList;
        isOpen = false;
        index = 0;
    }

    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if(!isOpen)
            {
                if(Time.timeScale == 1)
                {
                    Time.timeScale = 0;
                    GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
                    buttonGroup.SetActive(true);

                    isOpen = true;
                    index = 0;
                    OpenPanel(index);
                }
            }
            else
            {
                isOpen = false;
                buttonGroup.SetActive(false);
                foreach (GameObject menu in panel)
                {
                    menu.SetActive(false);
                }
                GetComponent<Image>().color = new Color(0, 0, 0, 0);
                Time.timeScale = 1;
            }
        }
    }

    public void OpenPanel(int index)
    {
        foreach (GameObject menu in panel)
        {
            menu.SetActive(false);
        }

        switch(index)
        {
            case 0:
                //mini map
                panel[index].SetActive(true);
                OpenMap();
                break;
            case 1:
                //inventory
                panel[index].SetActive(true);
                OpenInventory();
                break;
            case 2:
                //food&ingred info
                panel[index].SetActive(true);
                OpenInfoList();
                break;
        }
    }

    public void OpenMap()
    {

    }

    public void OpenInventory()
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

    public void OpenInfoList()
    {

    }

    public void OnIndexButtonClicked(int num)
    {
        index = num;
        OpenPanel(num);
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
                OpenInventory();
                break;
            }
        }
    }
}
