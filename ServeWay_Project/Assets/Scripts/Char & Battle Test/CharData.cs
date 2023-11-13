using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharData : MonoBehaviour
{

    public SaveFile saveFile;

    /*public List<string> weaponList;
    public float playerHp;
    public float playerSpeed;
    public float playerChargeSpeed;
    public float playerChargeLength;
    public float playerChargeCooltime;

    public int stage;*/

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

        for (int i = 0; i < plInfo.weaponSlot.gameObject.transform.childCount; i++)
        {
            string name = plInfo.weaponSlot.gameObject.transform.GetChild(i).GetChild(0).GetComponent<WeaponController>().weaponName;
            Create_Success success = plInfo.weaponSlot.gameObject.transform.GetChild(i).GetChild(0).GetComponent<WeaponController>().success;

            saveFile.weaponList.Add(name);
            saveFile.weaponSuccess.Add(success);
        }

        saveFile.playerSpeed = plInfo.speed;
        saveFile.playerChargeSpeed = plInfo.chargeSpeed;
        saveFile.playerChargeLength = plInfo.chargeLength;
        saveFile.playerChargeCooltime = plInfo.chargeCooltime;
        saveFile.playerHp = plInfo.GetnowHp();

        saveFile.inventory = new NameAmount();
        foreach(IngredientList.IngredientsName name in inventory.inventory.Keys)
        {
            saveFile.inventory.Add(name, inventory.inventory[name]);
        }

        saveFile.stage = GameManager.gameManager.stage;
        saveFile.bossNations = GameManager.gameManager.bossNations;

        UnityEditor.EditorUtility.SetDirty(saveFile);
    }

    public void SaveMapData(Room[ , ] roomList, int startX, int startY)
    {
        saveFile.roomList = new List<Room>();

        foreach(Room room in roomList)
        {
            room.triggerBox = null;

            saveFile.roomList.Add(room);
        }
        saveFile.startX = startX;
        saveFile.startY = startY;

        saveFile.isMapSave = true;

        UnityEditor.EditorUtility.SetDirty(saveFile);
    }

    public void DeleteMapData()
    {
        saveFile.isMapSave = false;

        UnityEditor.EditorUtility.SetDirty(saveFile);
    }

}
