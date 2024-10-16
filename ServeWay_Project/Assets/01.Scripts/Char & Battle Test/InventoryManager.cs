using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    private PlayerController player;
    private DataController dataController;

    public NameAmount inventory;

    public float increase_Speed;
    public float increase_ChargeSpeed;
    public float increase_ChargeCoolTime;
    public float increase_Damage;
    public float increase_BulletSpeed;
    public float increase_BulletCoolTime;
    public float decrease_EnemySpeed;
    public float decrease_EnemyAttackRange;
    public float decrease_EnemyAttackTime;

    void Start()
    {
        inventory = new NameAmount();
        player = FindObjectOfType<PlayerController>();
        dataController = FindObjectOfType<DataController>();

        InitIncrease();

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
            case Ingred_Name.Meat:
                decrease_EnemyAttackTime -= 0.03f * amount;
                break;
            case Ingred_Name.Spice:
                decrease_EnemySpeed -= 0.03f * amount;
                break;
            case Ingred_Name.Onion:
                increase_ChargeSpeed += 0.03f * amount;
                break;
            case Ingred_Name.Water:
                increase_Speed += 0.03f * amount;
                break;
            case Ingred_Name.Sugar:
                increase_BulletSpeed += 0.03f * amount;
                break;
            case Ingred_Name.Egg:
                increase_Damage += 0.03f * amount;
                break;
            case Ingred_Name.Rice:
                increase_Damage += 0.05f * amount;
                break;
            case Ingred_Name.Fish:
                increase_Damage += 0.05f * amount;
                break;
            case Ingred_Name.Oil:
                increase_ChargeSpeed += 0.05f * amount;
                break;
            case Ingred_Name.Flour:
                decrease_EnemyAttackRange -= 0.05f * amount;
                break;
            case Ingred_Name.Salt:
                decrease_EnemySpeed -= 0.05f * amount;
                break;
            case Ingred_Name.Tomato:
                increase_ChargeCoolTime -= 0.05f * amount;
                break;
            case Ingred_Name.Sauce:
                decrease_EnemySpeed -= 0.05f * amount;
                break;
            case Ingred_Name.Noodle:
                increase_BulletSpeed += 0.05f * amount;
                break;
            case Ingred_Name.Mushroom:
                increase_BulletCoolTime -= 0.05f * amount;
                break;
            case Ingred_Name.Milk:
                increase_Speed += 0.05f * amount;
                break;
            case Ingred_Name.Garlic:
                decrease_EnemyAttackTime -= 0.05f * amount;
                break;
            case Ingred_Name.Butter:
                ;
                break;
            case Ingred_Name.Cheese:
                ;
                break;
            case Ingred_Name.Fruit:
                ;
                break;
            case Ingred_Name.Greenonion:
                ;
                break;
            case Ingred_Name.Carrot:
                ;
                break;
            case Ingred_Name.Beansprout:
                ;
                break;
            case Ingred_Name.Seaweed:
                ;
                break;
            case Ingred_Name.Vanilla:
                ;
                break;
            case Ingred_Name.Coriander:
                ;
                break;
            case Ingred_Name.Kimchi:
                ;
                break;
            case Ingred_Name.Vinegar:
                ;
                break;
            case Ingred_Name.Tofu:
                ;
                break;
            case Ingred_Name.Wine:
                ;
                break;
            case Ingred_Name.Salary:
                ;
                break;
            case Ingred_Name.Honey:
                ;
                break;
            case Ingred_Name.Ginger:
                ;
                break;
            case Ingred_Name.Radish:
                ;
                break;
            case Ingred_Name.Redbean:
                ;
                break;
            case Ingred_Name.Pumpkin:
                ;
                break;
            case Ingred_Name.Eggplant:
                ;
                break;
            case Ingred_Name.Olive:
                ;
                break;
            case Ingred_Name.Lettuce:
                ;
                break;
            case Ingred_Name.Cream:
                ;
                break;
            case Ingred_Name.Nuts:
                ;
                break;
            case Ingred_Name.Cucumber:
                ;
                break;
            case Ingred_Name.Corn:
                ;
                break;
            case Ingred_Name.Coconut:
                ;
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

    public List<Ingredient> GetTopIngred()
    {
        List<Ingredient> topList = new List<Ingredient>();

        foreach (Ingred_Name ingred in inventory.Keys)
        {
            Ingredient nowIngred = dataController.FindIngredient(ingred);
            if (topList.Count < 5)
            {
                topList.Add(nowIngred);
            }
            else
            {
                foreach (Ingredient ingredient in topList)
                {
                    if (ingredient.grade < nowIngred.grade)
                    {
                        topList[topList.IndexOf(ingredient)] = nowIngred;
                        break;
                    }
                }
            }
        }

        return topList;
    }

    private void InitIncrease()
    {
        increase_Speed = 1.0f;
        increase_ChargeSpeed = 1.0f;
        increase_ChargeCoolTime = 1.0f;
        increase_Damage = 1.0f;
        increase_BulletSpeed = 1.0f;
        increase_BulletCoolTime = 1.0f;
        decrease_EnemySpeed = 1.0f;
        decrease_EnemyAttackRange = 1.0f;
        decrease_EnemyAttackTime = 1.0f;
}
}
