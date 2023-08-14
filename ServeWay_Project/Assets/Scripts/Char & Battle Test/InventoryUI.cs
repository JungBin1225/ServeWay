using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public List<GameObject> buttonList;
    //public List<string> itemNameList;
    //public List<Sprite> itemSpriteList;
    public GameObject panel;

    //private Dictionary<string, Sprite> itemList;
    private InventoryManager inventory;
    private Sprite defaultSprite;
    private IngredientList itemList;

    void Start()
    {
        inventory = FindObjectOfType<InventoryManager>();
        defaultSprite = buttonList[0].GetComponent<Image>().sprite;
        itemList = FindObjectOfType<DataController>().IngredientList;

        //UpdateUI();
    }

    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            panel.SetActive(!panel.activeSelf);
            if(panel.activeSelf)
            {
                Time.timeScale = 0;
                UpdateUI();
            }
            else
            {
                Time.timeScale = 1;
            }
        }
    }

    public void UpdateUI()
    {
        int i = 0;
        foreach(IngredientList.IngredientsName item in inventory.inventory.Keys)
        {
            buttonList[i].transform.GetChild(0).gameObject.SetActive(true);
            buttonList[i].GetComponent<Image>().sprite = itemList.ingredientInfo[item];
            buttonList[i].transform.GetChild(0).GetComponent<Text>().text = inventory.inventory[item].ToString();

            i++;
        }

        if(i < buttonList.Count - 1)
        {
            for(; i < buttonList.Count; i++)
            {
                buttonList[i].transform.GetChild(0).gameObject.SetActive(false);
                buttonList[i].GetComponent<Image>().sprite = defaultSprite;
            }
        }
    }

    public void OnButtonClicked(int num)
    {
        IngredientList.IngredientsName name;

        foreach(IngredientList.IngredientsName itemName in itemList.ingredientInfo.Keys)
        {
            if(itemList.ingredientInfo[itemName] == buttonList[num].GetComponent<Image>().sprite)
            {
                name = itemName;
                inventory.DeleteItem(name, 1);
                UpdateUI();
                break;
            }
        }
    }
}
