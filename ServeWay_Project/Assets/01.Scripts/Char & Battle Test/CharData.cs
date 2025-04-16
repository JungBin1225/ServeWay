using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharData : MonoBehaviour
{
    public SaveFile saveFile;

    void Start()
    {
        saveFile = new SaveFile();
        saveFile.Reset();

        if (PlayerPrefs.HasKey("food_1"))
        {
            SetData();
        }
    }

    void Update()
    {
        
    }


    public void SaveData()
    {
        PlayerController plInfo = FindObjectOfType<PlayerController>();
        InventoryManager inventory = GameManager.gameManager.inventory;

        PlayerPrefs.DeleteAll();

        for (int i = 0; i < plInfo.weaponSlot.gameObject.transform.childCount; i++)
        {
            string name = plInfo.weaponSlot.gameObject.transform.GetChild(i).GetChild(0).GetComponent<WeaponController>().weaponName;
            Create_Success success = plInfo.weaponSlot.gameObject.transform.GetChild(i).GetChild(0).GetComponent<WeaponController>().success;
            int success_int = 0;
            switch(success)
            {
                case Create_Success.FAIL:
                    success_int = -1;
                    break;
                case Create_Success.SUCCESS:
                    success_int = 0;
                    break;
                case Create_Success.GREAT:
                    success_int = 1;
                    break;
            }

            PlayerPrefs.SetString(string.Format("food_{0}", i.ToString()), "name");
            PlayerPrefs.SetInt(string.Format("food_success_{0}", i.ToString()), success_int);
        }
        PlayerPrefs.SetInt("food_index", plInfo.weaponSlot.index);

        PlayerPrefs.SetFloat("player_speed", plInfo.speed);
        PlayerPrefs.SetFloat("player_charge_speed", plInfo.chargeSpeed);
        PlayerPrefs.SetFloat("player_charge_length", plInfo.chargeLength);
        PlayerPrefs.SetFloat("player_charge_cooltime", plInfo.chargeCooltime);
        PlayerPrefs.SetFloat("player_hp", plInfo.GetnowHp());

        foreach(Ingred_Name name in inventory.inventory.Keys)
        {
            PlayerPrefs.SetInt(name.ToString(), inventory.inventory[name]);
        }

        PlayerPrefs.SetInt("stage", GameManager.gameManager.stage);
        for(int i = 0; i < 7; i++)
        {
            PlayerPrefs.SetString(string.Format("boss_nation_{0}", i.ToString()), GameManager.gameManager.bossNations[i].ToString());
            PlayerPrefs.SetString(string.Format("boss_job_{0}", i.ToString()), GameManager.gameManager.bossJobList[i].ToString());
            PlayerPrefs.SetString(string.Format("stage_theme_{0}", i.ToString()), GameManager.gameManager.stageThemes[i].ToString());
        }

        PlayerPrefs.Save();
    }

    public void SetData()
    {
        saveFile = new SaveFile();
        saveFile.Reset();

        saveFile.inventory = FindIngredInSave();
        for (int i = 0; i < 3; i++)
        {
            if (PlayerPrefs.HasKey(string.Format("food_{0}", i.ToString())))
            {
                saveFile.weaponList.Add(PlayerPrefs.GetString(string.Format("food_{0}", i.ToString())));
                switch (PlayerPrefs.GetInt(string.Format("food_{0}", i.ToString())))
                {
                    case -1:
                        saveFile.weaponSuccess.Add(Create_Success.FAIL);
                        break;
                    case 0:
                        saveFile.weaponSuccess.Add(Create_Success.FAIL);
                        break;
                    case 1:
                        saveFile.weaponSuccess.Add(Create_Success.FAIL);
                        break;
                }
            }
        }
        saveFile.weaponIndex = PlayerPrefs.GetInt("food_index");


        saveFile.playerSpeed = PlayerPrefs.GetFloat("player_speed");
        saveFile.playerChargeSpeed = PlayerPrefs.GetFloat("player_charge_speed");
        saveFile.playerChargeLength = PlayerPrefs.GetFloat("player_charge_length");
        saveFile.playerChargeCooltime = PlayerPrefs.GetFloat("player_charge_cooltime");
        saveFile.playerHp = PlayerPrefs.GetFloat("player_hp");

        saveFile.inventory = FindIngredInSave();

        saveFile.stage = PlayerPrefs.GetInt("stage");
        for (int i = 0; i < 7; i++)
        {
            saveFile.bossNations.Add(GameManager.gameManager.StringToNation(PlayerPrefs.GetString(string.Format("boss_nation_{0}", i.ToString()))));
            saveFile.bossJobs.Add(GameManager.gameManager.StringToJob(PlayerPrefs.GetString(string.Format("boss_job_{0}", i.ToString()))));
            saveFile.themes.Add(GameManager.gameManager.StringToTheme(PlayerPrefs.GetString(string.Format("stage_theme_{0}", i.ToString()))));
        }

        //맵 데이터 저장
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

        int i = 0;
        foreach(Room room in saveFile.roomList)
        {
            PlayerPrefs.SetFloat(string.Format("room_{0}_noderect_x", i.ToString()), room.nodeRect.x);
            PlayerPrefs.SetFloat(string.Format("room_{0}_noderect_y", i.ToString()), room.nodeRect.y);
            PlayerPrefs.SetFloat(string.Format("room_{0}_noderect_w", i.ToString()), room.nodeRect.width);
            PlayerPrefs.SetFloat(string.Format("room_{0}_noderect_h", i.ToString()), room.nodeRect.height);

            PlayerPrefs.SetFloat(string.Format("room_{0}_roomrect_x", i.ToString()), room.roomRect.x);
            PlayerPrefs.SetFloat(string.Format("room_{0}_roomrect_y", i.ToString()), room.roomRect.y);
            PlayerPrefs.SetFloat(string.Format("room_{0}_roomrect_w", i.ToString()), room.roomRect.width);
            PlayerPrefs.SetFloat(string.Format("room_{0}_roomrect_h", i.ToString()), room.roomRect.height);

            PlayerPrefs.SetInt(string.Format("room_{0}_iscreated", i.ToString()), room.isCreated);
            PlayerPrefs.SetString(string.Format("room_{0}_type", i.ToString()), room.roomType.ToString());

            PlayerPrefs.SetFloat(string.Format("room_{0}_right_y", i.ToString()), room.rightYPoint);
            PlayerPrefs.SetFloat(string.Format("room_{0}_left_y", i.ToString()), room.leftYPoint);
            PlayerPrefs.SetFloat(string.Format("room_{0}_up_x", i.ToString()), room.upXPoint);
            PlayerPrefs.SetFloat(string.Format("room_{0}_down_x", i.ToString()), room.downXPoint);
            i++;
        }
        PlayerPrefs.SetInt("startX", startX);
        PlayerPrefs.SetInt("startY", startY);
        PlayerPrefs.SetString("map_save", "true");


        PlayerPrefs.Save();
    }

    public void DeleteMapData()
    {
        saveFile.isMapSave = false;
        PlayerPrefs.SetString("map_save", "false");

        for(int i = 0; i < 25; i++)
        {
            PlayerPrefs.DeleteKey(string.Format("room_{0}_noderect_x", i.ToString()));
            PlayerPrefs.DeleteKey(string.Format("room_{0}_noderect_y", i.ToString()));
            PlayerPrefs.DeleteKey(string.Format("room_{0}_noderect_w", i.ToString()));
            PlayerPrefs.DeleteKey(string.Format("room_{0}_noderect_h", i.ToString()));

            PlayerPrefs.DeleteKey(string.Format("room_{0}_roomrect_x", i.ToString()));
            PlayerPrefs.DeleteKey(string.Format("room_{0}_roomrect_y", i.ToString()));
            PlayerPrefs.DeleteKey(string.Format("room_{0}_roomrect_w", i.ToString()));
            PlayerPrefs.DeleteKey(string.Format("room_{0}_roomrect_h", i.ToString()));

            PlayerPrefs.DeleteKey(string.Format("room_{0}_iscreated", i.ToString()));
            PlayerPrefs.DeleteKey(string.Format("room_{0}_type", i.ToString()));

            PlayerPrefs.DeleteKey(string.Format("room_{0}_right_y", i.ToString()));
            PlayerPrefs.DeleteKey(string.Format("room_{0}_left_y", i.ToString()));
            PlayerPrefs.DeleteKey(string.Format("room_{0}_up_x", i.ToString()));
            PlayerPrefs.DeleteKey(string.Format("room_{0}_down_x", i.ToString()));

            PlayerPrefs.DeleteKey(string.Format("startX", i.ToString()));
            PlayerPrefs.DeleteKey(string.Format("startY", i.ToString()));
        }

        PlayerPrefs.Save();
    }

    public NameAmount FindIngredInSave()
    {
        NameAmount result = new NameAmount();
        DataController data = FindObjectOfType<DataController>();

        foreach (Ingred_Name name in data.FoodIngredDex.ingredDex.Keys)
        {
            if (PlayerPrefs.HasKey(name.ToString()))
            {
                result.Add(name, PlayerPrefs.GetInt(name.ToString()));
            }
        }

        return result;
    }
}
