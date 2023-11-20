using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private PlayerController player;

    public NameAmount inventory;

    void Start()
    {
        inventory = new NameAmount();
        player = FindObjectOfType<PlayerController>();

        if (inventory.Count == 0 && GameManager.gameManager.charData.saveFile.inventory != null)
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
            GetPassive(itemName, true);
        }

        /*foreach(IngredientList.IngredientsName name in inventory.Keys)
        {
            Debug.Log(name + ", " + inventory[name]);
        }*/
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
                GetPassive(itemName, false);
            }
        }

        /*foreach (IngredientList.IngredientsName name in inventory.Keys)
        {
            Debug.Log(name + ", " + inventory[name]);
        }*/
    }

    public void LoadInventory()
    {
        Dictionary<IngredientList.IngredientsName, int> saveInven = GameManager.gameManager.charData.saveFile.inventory;

        inventory.Clear();
        foreach (IngredientList.IngredientsName name in saveInven.Keys)
        {
            inventory.Add(name, saveInven[name]);
        }
    }

    public void GetPassive(IngredientList.IngredientsName itemName, bool increase)
    {
        int amount = 0;

        if(increase)
        {
            amount = 1;
        }
        else
        {
            amount = -1;
        }

        switch(itemName)
        {
            case IngredientList.IngredientsName.Pumpkin:
                player.speed += amount * 10;
                break;
            case IngredientList.IngredientsName.Strawberrie:
                player.chargeLength += amount * 0.1f;
                break;
        }
    }
}
