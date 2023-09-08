using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Dictionary<IngredientList.IngredientsName, int> inventory;
    //public InventoryUI InventoryUI;

    void Start()
    {
        inventory = new Dictionary<IngredientList.IngredientsName, int>();
        //InventoryUI = FindObjectOfType<InventoryUI>();

        if(inventory.Count == 0 && GameManager.gameManager.charData.saveFile.inventory != null)
        {
            LoadInventory();
        }
    }

    
    void Update()
    {
        
    }

    public void GetItem(IngredientList.IngredientsName itemName, int amount)
    {
        if(inventory.ContainsKey(itemName))
        {
            inventory[itemName] += amount;
        }
        else
        {
            inventory.Add(itemName, amount);
        }

        foreach(IngredientList.IngredientsName name in inventory.Keys)
        {
            Debug.Log(name + ", " + inventory[name]);
        }
    }

    public void DeleteItem(IngredientList.IngredientsName itemName, int amount)
    {
        if(inventory.ContainsKey(itemName))
        {
            if(inventory[itemName] > amount)
            {
                inventory[itemName] -= amount;
            }
            else
            {
                inventory.Remove(itemName);
            }
        }

        foreach (IngredientList.IngredientsName name in inventory.Keys)
        {
            Debug.Log(name + ", " + inventory[name]);
        }
    }

    public void LoadInventory()
    {
        Dictionary<IngredientList.IngredientsName, int> saveInven = GameManager.gameManager.charData.saveFile.inventory;

        inventory.Clear();
        foreach(IngredientList.IngredientsName name in saveInven.Keys)
        {
            inventory.Add(name, saveInven[name]);
        }
    }
}
