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

    public bool isKimchi;
    public bool isTofu;
    public bool isGinger;
    public bool isCream;
    public bool isNuts;

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
                decrease_EnemyAttackTime += 0.03f * amount;
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
                decrease_EnemyAttackTime += 0.05f * amount;
                break;
            case Ingred_Name.Butter:
                decrease_EnemyAttackTime += 0.08f * amount;
                break;
            case Ingred_Name.Cheese:
                decrease_EnemyAttackRange -= 0.08f * amount;
                break;
            case Ingred_Name.Fruit:
                increase_Damage += 0.08f * amount;
                break;
            case Ingred_Name.Greenonion:
                increase_ChargeCoolTime -= 0.08f * amount;
                break;
            case Ingred_Name.Carrot:
                increase_ChargeSpeed += 0.08f * amount;
                break;
            case Ingred_Name.Beansprout:
                increase_BulletCoolTime -= 0.08f * amount;
                break;
            case Ingred_Name.Seaweed:
                increase_BulletSpeed += 0.08f * amount;
                break;
            case Ingred_Name.Vanilla:
                increase_Speed += 0.08f * amount;
                break;
            case Ingred_Name.Coriander:
                decrease_EnemySpeed -= 0.08f * amount;
                break;
            case Ingred_Name.Kimchi:
                isKimchi = increase;
                break;
            case Ingred_Name.Vinegar:
                increase_Speed += 0.07f * amount;
                increase_ChargeSpeed += 0.07f * amount;
                break;
            case Ingred_Name.Tofu:
                isTofu = increase;
                break;
            case Ingred_Name.Wine:
                increase_Speed += 0.08f * amount;
                decrease_EnemySpeed += 0.08f * amount;
                break;
            case Ingred_Name.Salary:
                increase_ChargeSpeed += 0.12f * amount;
                break;
            case Ingred_Name.Honey:
                increase_Speed -= 0.1f * amount;
                decrease_EnemySpeed -= 0.1f * amount;
                break;
            case Ingred_Name.Ginger:
                isGinger = increase;
                break;
            case Ingred_Name.Radish:
                increase_BulletCoolTime += 0.1f * amount;
                increase_Damage += 0.05f * amount;
                break;
            case Ingred_Name.Redbean:
                decrease_EnemySpeed -= 0.12f * amount;
                break;
            case Ingred_Name.Pumpkin:
                increase_Damage += 0.12f * amount;
                break;
            case Ingred_Name.Eggplant:
                decrease_EnemyAttackRange -= 0.12f * amount;
                break;
            case Ingred_Name.Olive:
                increase_ChargeCoolTime -= 0.12f * amount;
                break;
            case Ingred_Name.Lettuce:
                increase_BulletCoolTime -= 0.12f;
                break;
            case Ingred_Name.Cream:
                isCream = increase;
                increase_Speed -= 0.15f * amount;
                increase_ChargeCoolTime += 0.1f * amount;
                break;
            case Ingred_Name.Nuts:
                isNuts = increase;
                increase_ChargeCoolTime += 0.1f * amount;
                break;
            case Ingred_Name.Cucumber:
                increase_Speed += 0.12f * amount;
                break;
            case Ingred_Name.Corn:
                increase_BulletSpeed += 0.12f * amount;
                break;
            case Ingred_Name.Coconut:
                increase_Damage -= 0.15f * amount;
                decrease_EnemyAttackTime += 0.1f * amount;
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
            if (topList.Count < 3)
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

        isKimchi = false;
        isTofu = false;
        isGinger = false;
        isCream = false;
        isNuts = false;
    }
}
