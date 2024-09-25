using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    private PlayerController player;
    private DataController dataController;

    public NameAmount inventory;

    void Start()
    {
        inventory = new NameAmount();
        player = FindObjectOfType<PlayerController>();
        dataController = FindObjectOfType<DataController>();

        if (inventory.Count == 0 && GameManager.gameManager.charData.saveFile.inventory != null)
        {
            if(!SceneManager.GetActiveScene().name.Contains("Tutorial"))
            {
                LoadInventory();
            }
        }
    }

    
    void Update()
    {
        
    }

    public void GetItem(Ingred_Name itemName, int amount)
    {
        if(inventory.ContainsKey(itemName))
        {
            inventory[itemName] += amount;
        }
        else
        {
            inventory.Add(itemName, amount);
            GetPassive(itemName, true);

            dataController.FoodIngredDex.UpdateIngredDex(itemName);
        }

        /*foreach(IngredientList.IngredientsName name in inventory.Keys)
        {
            Debug.Log(name + ", " + inventory[name]);
        }*/
    }

    public void DeleteItem(Ingred_Name itemName, int amount)
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
        Dictionary<Ingred_Name, int> saveInven = GameManager.gameManager.charData.saveFile.inventory;

        inventory.Clear();
        foreach (Ingred_Name name in saveInven.Keys)
        {
            GetItem(name, saveInven[name]);
        }
    }

    public void GetPassive(Ingred_Name itemName, bool increase)
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
            case Ingred_Name.Pumpkin:
                player.speed += amount * 1;
                break;
            case Ingred_Name.Strawberrie:
                player.chargeLength += amount * 0.1f;
                break;
        }
    }

    public int GetInventoryAmount()
    {
        int amount = 0;

        foreach(Ingred_Name ingred in inventory.Keys)
        {
            amount += inventory[ingred];
        }

        return amount;
    }

    public List<Sprite> GetTopIngred()
    {
        List<Sprite> topList = new List<Sprite>();

        if(inventory.Count < 5)
        {
            foreach(Ingred_Name ingred in inventory.Keys)
            {
                topList.Add(dataController.FindIngredient(ingred).sprite);
            }
        }
        else
        {
            foreach (Ingred_Name ingred in inventory.Keys)
            {
                if(topList.Count < 5 && dataController.FindIngredient(ingred).grade == Ingred_Grade.STAR_4)
                {
                    topList.Add(dataController.FindIngredient(ingred).sprite);
                }
            }

            if(topList.Count < 5)
            {
                foreach (Ingred_Name ingred in inventory.Keys)
                {
                    if (topList.Count < 5 && dataController.FindIngredient(ingred).grade == Ingred_Grade.STAR_3)
                    {
                        topList.Add(dataController.FindIngredient(ingred).sprite);
                    }
                }
            }

            if (topList.Count < 5)
            {
                foreach (Ingred_Name ingred in inventory.Keys)
                {
                    if (topList.Count < 5 && dataController.FindIngredient(ingred).grade == Ingred_Grade.STAR_2)
                    {
                        topList.Add(dataController.FindIngredient(ingred).sprite);
                    }
                }
            }

            if (topList.Count < 5)
            {
                foreach (Ingred_Name ingred in inventory.Keys)
                {
                    if (topList.Count < 5 && dataController.FindIngredient(ingred).grade == Ingred_Grade.STAR_1)
                    {
                        topList.Add(dataController.FindIngredient(ingred).sprite);
                    }
                }
            }
        }
        return topList;
    }
}
