using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharData : MonoBehaviour
{
    public SaveFile saveFile;

    void Start()
    {
        
    }

    void Update()
    {
        
    }


    public void SaveData()
    {
        PlayerController plInfo = FindObjectOfType<PlayerController>();
        InventoryManager inventory = GameManager.gameManager.inventory;

        saveFile.weaponList.Clear();
        saveFile.weaponSuccess.Clear();

        for (int i = 0; i < plInfo.weaponSlot.gameObject.transform.childCount; i++)
        {
            string name = plInfo.weaponSlot.gameObject.transform.GetChild(i).GetChild(0).GetComponent<WeaponController>().weaponName;
            Create_Success success = plInfo.weaponSlot.gameObject.transform.GetChild(i).GetChild(0).GetComponent<WeaponController>().success;

            saveFile.weaponList.Add(name);
            saveFile.weaponSuccess.Add(success);
        }
        saveFile.weaponIndex = plInfo.weaponSlot.index;

        saveFile.playerSpeed = plInfo.speed;
        saveFile.playerChargeSpeed = plInfo.chargeSpeed;
        saveFile.playerChargeLength = plInfo.chargeLength;
        saveFile.playerChargeCooltime = plInfo.chargeCooltime;
        saveFile.playerHp = plInfo.GetnowHp();

        saveFile.inventory = new NameAmount();
        foreach(Ingred_Name name in inventory.inventory.Keys)
        {
            saveFile.inventory.Add(name, inventory.inventory[name]);
        }

        saveFile.stage = GameManager.gameManager.stage;
        saveFile.bossNations = GameManager.gameManager.bossNations;
        saveFile.bossJobs = GameManager.gameManager.bossJobList;
        saveFile.themes = GameManager.gameManager.stageThemes;

        UnityEditor.EditorUtility.SetDirty(saveFile);
        PlayerPrefs.Save();
    }

    public void SaveMapData(Room[ , ] roomList, int startX, int startY)
    {
        saveFile.roomList = new List<Room>();

        foreach(Room room in roomList)
        {
            room.enemyGenerator = null;

            saveFile.roomList.Add(room);
        }
        saveFile.startX = startX;
        saveFile.startY = startY;

        saveFile.isMapSave = true;

        UnityEditor.EditorUtility.SetDirty(saveFile);
        PlayerPrefs.Save();
    }

    public void DeleteMapData()
    {
        saveFile.isMapSave = false;

        UnityEditor.EditorUtility.SetDirty(saveFile);
    }

    public List<string> FindFoodIngredInSave(List<string> foodList)
    {
        List<string> result = new List<string>();

        foreach(string food in foodList)
        {
            if(PlayerPrefs.HasKey(food))
            {
                result.Add(food);
            }
        }

        return result;
    }

    public NameAmount FindFoodIngredInSave(List<Ingred_Name> ingredList)
    {
        NameAmount result = new NameAmount();

        foreach(Ingred_Name name in ingredList)
        {
            if(PlayerPrefs.HasKey(name.ToString()))
            {
                result.Add(name, PlayerPrefs.GetInt(name.ToString()));
            }
        }

        return result;
    }
}
