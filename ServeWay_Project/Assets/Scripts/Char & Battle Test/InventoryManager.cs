using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Dictionary<string, int> inventory;
    public InventoryUI InventoryUI;

    void Start()
    {
        inventory = new Dictionary<string, int>();
        InventoryUI = FindObjectOfType<InventoryUI>();
    }

    
    void Update()
    {
        
    }

    public void GetItem(string itemName, int amount)
    {
        if(inventory.ContainsKey(itemName))
        {
            inventory[itemName] += amount;
        }
        else
        {
            inventory.Add(itemName, amount);
        }

        foreach(string name in inventory.Keys)
        {
            Debug.Log(name + ", " + inventory[name]);
        }
    }

    public void DeleteItem(string itemName, int amount)
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

        foreach (string name in inventory.Keys)
        {
            Debug.Log(name + ", " + inventory[name]);
        }
    }
}
