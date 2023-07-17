using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public List<GameObject> buttonList;
    public List<string> itemNameList;
    public List<Sprite> itemSpriteList;
    public GameObject panel;

    private Dictionary<string, Sprite> itemList;
    private InventoryManager inventory;
    private Sprite defaultSprite;
    void Start()
    {
        inventory = FindObjectOfType<InventoryManager>();
        defaultSprite = buttonList[0].GetComponent<Image>().sprite;

        itemList = new Dictionary<string, Sprite>();
        for(int i = 0; i < itemNameList.Count; i++)
        {
            itemList.Add(itemNameList[i], itemSpriteList[i]);
        }

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
        foreach(string item in inventory.inventory.Keys)
        {
            buttonList[i].transform.GetChild(0).gameObject.SetActive(true);
            buttonList[i].GetComponent<Image>().sprite = itemList[item];
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
        string name;

        foreach(string itemName in itemList.Keys)
        {
            if(itemList[itemName] == buttonList[num].GetComponent<Image>().sprite)
            {
                name = itemName;
                inventory.DeleteItem(name, 1);
                UpdateUI();
                break;
            }
        }
    }
}
