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

        for (int i = 0; i < plInfo.weaponSlot.gameObject.transform.childCount; i++)
        {
            saveFile.weaponList.Add(plInfo.weaponSlot.gameObject.transform.GetChild(i).GetChild(0).GetComponent<WeaponController>().weaponName);
        }

        saveFile.playerSpeed = plInfo.speed;
        saveFile.playerChargeSpeed = plInfo.chargeSpeed;
        saveFile.playerChargeLength = plInfo.chargeLength;
        saveFile.playerChargeCooltime = plInfo.chargeCooltime;
        saveFile.playerHp = plInfo.GetnowHp();

        saveFile.inventory = new Dictionary<IngredientList.IngredientsName, int>();
        foreach(IngredientList.IngredientsName name in inventory.inventory.Keys)
        {
            saveFile.inventory.Add(name, inventory.inventory[name]);
        }

        UnityEditor.EditorUtility.SetDirty(saveFile);
    }
}
