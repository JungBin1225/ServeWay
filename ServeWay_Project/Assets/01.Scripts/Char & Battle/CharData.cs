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

        if (PlayerPrefs.HasKey("food_0"))
        {
            SetData();
        }
        SetDex();
        SetTuto();
        SetEnding();
    }

    void Update()
    {
        
    }


    public void SaveData()
    {
        PlayerController plInfo = FindObjectOfType<PlayerController>();
        InventoryManager inventory = GameManager.gameManager.inventory;

        bool bgmMute = false;
        bool sfxMute = false;
        float bgmValue = 1;
        float sfxValue = 1;

        if (PlayerPrefs.HasKey("BGM_Sound"))
        {
            bgmMute = bool.Parse(PlayerPrefs.GetString("BGM_Mute"));
            sfxMute = bool.Parse(PlayerPrefs.GetString("SFX_Mute"));

            bgmValue = PlayerPrefs.GetFloat("BGM_Sound");
            sfxValue = PlayerPrefs.GetFloat("SFX_Sound");
        }

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

            PlayerPrefs.SetString(string.Format("food_{0}", i.ToString()), name);
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

        List<string> s = FoodDexToString(saveFile.foodDex);
        PlayerPrefs.SetString("foodDex_Created", s[0]);
        PlayerPrefs.SetString("foodDex_Recipe", s[1]);
        PlayerPrefs.SetString("foodDex_Locked", s[2]);

        PlayerPrefs.SetInt("stage", GameManager.gameManager.stage);
        for(int i = 0; i < 7; i++)
        {
            PlayerPrefs.SetString(string.Format("boss_nation_{0}", i.ToString()), GameManager.gameManager.bossNations[i].ToString());
            PlayerPrefs.SetString(string.Format("boss_job_{0}", i.ToString()), GameManager.gameManager.bossJobList[i].ToString());
            PlayerPrefs.SetString(string.Format("stage_theme_{0}", i.ToString()), GameManager.gameManager.stageThemes[i].ToString());
        }

        PlayerPrefs.SetString("isTuto", saveFile.isTuto.ToString());
        PlayerPrefs.SetString("isEnding", saveFile.isEnding.ToString());

        PlayerPrefs.SetString("BGM_Mute", bgmMute.ToString());
        PlayerPrefs.SetString("SFX_Mute", sfxMute.ToString());
        PlayerPrefs.SetFloat("BGM_Sound", bgmValue);
        PlayerPrefs.SetFloat("SFX_Sound", sfxValue);

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

        saveFile.stage = 0;
        saveFile.stage = PlayerPrefs.GetInt("stage");
        //saveFile.stage = 1;//test

        for (int i = 0; i < 7; i++)
        {
            saveFile.bossNations.Add(GameManager.gameManager.StringToNation(PlayerPrefs.GetString(string.Format("boss_nation_{0}", i.ToString()))));
            saveFile.bossJobs.Add(GameManager.gameManager.StringToJob(PlayerPrefs.GetString(string.Format("boss_job_{0}", i.ToString()))));
            saveFile.themes.Add(GameManager.gameManager.StringToTheme(PlayerPrefs.GetString(string.Format("stage_theme_{0}", i.ToString()))));
        }

        saveFile.isTuto = bool.Parse(PlayerPrefs.GetString("isTuto"));

        if(PlayerPrefs.HasKey("isEnding"))
        {
            saveFile.isEnding = bool.Parse(PlayerPrefs.GetString("isEnding"));
        }
        

        //맵 데이터 저장
        saveFile.roomList = new List<Room>();
        for (int i = 0; i < 25; i++)
        {
            if(PlayerPrefs.HasKey(string.Format("room_{0}_noderect_x", i.ToString())))
            {
                Rect nodeRect = new Rect(PlayerPrefs.GetFloat(string.Format("room_{0}_noderect_x", i.ToString())), PlayerPrefs.GetFloat(string.Format("room_{0}_noderect_y", i.ToString())), PlayerPrefs.GetFloat(string.Format("room_{0}_noderect_w", i.ToString())), PlayerPrefs.GetFloat(string.Format("room_{0}_noderect_h", i.ToString())));
                Rect roomRect = new Rect(PlayerPrefs.GetFloat(string.Format("room_{0}_roomrect_x", i.ToString())), PlayerPrefs.GetFloat(string.Format("room_{0}_roomrect_y", i.ToString())), PlayerPrefs.GetFloat(string.Format("room_{0}_roomrect_w", i.ToString())), PlayerPrefs.GetFloat(string.Format("room_{0}_roomrect_h", i.ToString())));
                int isCreated = PlayerPrefs.GetInt(string.Format("room_{0}_iscreated", i.ToString()));
                RoomType roomType = StringToRoomType(PlayerPrefs.GetString(string.Format("room_{0}_type", i.ToString())));

                float rightY = PlayerPrefs.GetFloat(string.Format("room_{0}_right_y", i.ToString()));
                float leftY = PlayerPrefs.GetFloat(string.Format("room_{0}_left_y", i.ToString()));
                float upX = PlayerPrefs.GetFloat(string.Format("room_{0}_up_x", i.ToString()));
                float downX = PlayerPrefs.GetFloat(string.Format("room_{0}_down_x", i.ToString()));

                Room room = new Room(nodeRect, roomRect, isCreated, roomType, rightY, leftY, upX, downX);
                saveFile.roomList.Add(room);
            }
        }
        saveFile.startX = PlayerPrefs.GetInt("startX");
        saveFile.startY = PlayerPrefs.GetInt("startY");

        if(PlayerPrefs.GetString("map_save") == "true")
        {
            saveFile.isMapSave = true;
        }
        else
        {
            saveFile.isMapSave = false;
        }
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

    public void DeleteAllWithoutTutoSound()
    {
        bool tuto = saveFile.isTuto;
        bool ending = saveFile.isEnding;
        bool bgmMute = false;
        bool sfxMute = false;
        float bgmValue = 1;
        float sfxValue = 1;
        FoodDex foodDex = saveFile.foodDex;
        IngredDex ingredDex = saveFile.ingredDex;

        if (PlayerPrefs.HasKey("BGM_Sound"))
        {
            bgmMute = bool.Parse(PlayerPrefs.GetString("BGM_Mute"));
            sfxMute = bool.Parse(PlayerPrefs.GetString("SFX_Mute"));

            bgmValue = PlayerPrefs.GetFloat("BGM_Sound");
            sfxValue = PlayerPrefs.GetFloat("SFX_Sound");
        }

        saveFile = new SaveFile();
        
        PlayerPrefs.DeleteAll();
        
        PlayerPrefs.SetString("isTuto", tuto.ToString());
        PlayerPrefs.SetString("isEnding", ending.ToString());
        PlayerPrefs.SetString("BGM_Mute", bgmMute.ToString());
        PlayerPrefs.SetString("SFX_Mute", sfxMute.ToString());
        PlayerPrefs.SetFloat("BGM_Sound", bgmValue);
        PlayerPrefs.SetFloat("SFX_Sound", sfxValue);

        List<string> s = FoodDexToString(foodDex);
        PlayerPrefs.SetString("foodDex_Created", s[0]);
        PlayerPrefs.SetString("foodDex_Recipe", s[1]);
        PlayerPrefs.SetString("foodDex_Locked", s[2]);

        saveFile.isTuto = tuto;
        saveFile.isEnding = ending;

        GameManager.gameManager.stage = 0;
        GameManager.gameManager.charData.saveFile.foodDex = foodDex;
        GameManager.gameManager.charData.saveFile.ingredDex = ingredDex;
        GameManager.gameManager.InitList();

        PlayerPrefs.Save();
    }

    public void TutorialClear()
    {
        saveFile.isTuto = true;
        PlayerPrefs.SetString("isTuto", true.ToString());

        PlayerPrefs.Save();
    }

    public void SetTuto()
    {
        saveFile.isTuto = bool.Parse(PlayerPrefs.GetString("isTuto"));
    }

    public List<string> FoodDexToString()
    {
        List<string> result = new List<string>();
        result.Add("");//created
        result.Add("");//recipe
        result.Add("");//locked

        DataController data = FindObjectOfType<DataController>();

        int index = 0;
        foreach(string name in data.FoodIngredDex.foodDex.Keys)
        {
            string temp = string.Format("{0:D2}", index);
            switch(data.FoodIngredDex.foodDex[name])
            {
                case FoodDex_Status.CREATED:
                    result[0] += temp;
                    break;

                case FoodDex_Status.RECIPE:
                    result[1] += temp;
                    break;

                case FoodDex_Status.LOCKED:
                    result[2] += temp;
                    break;
            }

            index++;
        }

        return result;
    }

    public List<string> FoodDexToString(FoodDex dex)
    {
        List<string> result = new List<string>();
        result.Add("");//created
        result.Add("");//recipe
        result.Add("");//locked

        int index = 0;
        foreach (string name in dex.Keys)
        {
            string temp = string.Format("{0:D2}", index);
            switch (dex[name])
            {
                case FoodDex_Status.CREATED:
                    result[0] += temp;
                    break;

                case FoodDex_Status.RECIPE:
                    result[1] += temp;
                    break;

                case FoodDex_Status.LOCKED:
                    result[2] += temp;
                    break;
            }

            index++;
        }

        return result;
    }

    public FoodDex StringToFoodDex()
    {
        FoodDex result = new FoodDex();
        DataController data = FindObjectOfType<DataController>();

        if (!PlayerPrefs.HasKey("foodDex_Created"))
        {
            foreach(string name in data.FoodIngredDex.foodDex.Keys)
            {
                result.Add(name, data.FoodIngredDex.foodDex[name]);
            }
            Debug.Log("aaaa");
        }
        else
        {
            List<int> created = IndexToList(PlayerPrefs.GetString("foodDex_Created"));
            List<int> recipe = IndexToList(PlayerPrefs.GetString("foodDex_Recipe"));
            List<int> locked = IndexToList(PlayerPrefs.GetString("foodDex_Locked"));

            int i = 0;
            foreach(string name in data.FoodIngredDex.foodDex.Keys)
            {
                if (created.Contains(i))
                {
                    result.Add(name, FoodDex_Status.CREATED);
                }
                else if (recipe.Contains(i))
                {
                    result.Add(name, FoodDex_Status.RECIPE);
                }
                else if (locked.Contains(i))
                {
                    result.Add(name, FoodDex_Status.LOCKED);
                }
                else
                {
                    result.Add(name, data.FoodIngredDex.foodDex[name]);
                }

                i++;
            }
        }

        return result;
    }

    private List<int> IndexToList(string s)
    {
        List<int> result = new List<int>();
        int num = s.Length;
        for(int i = 0; i < s.Length; i += 2)
        {
            int temp = int.Parse(s.Substring(i, 2));
            result.Add(temp);
        }

        return result;
    }

    public void SetDex()
    {
        saveFile.foodDex = StringToFoodDex();
    }

    public void SetEnding()
    {
        if (PlayerPrefs.HasKey("isEnding"))
        {
            saveFile.isEnding = bool.Parse(PlayerPrefs.GetString("isEnding"));
        }
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

    public RoomType StringToRoomType(string name)
    {
        switch(name)
        {
            case "ROOM_NORMAL":
                return RoomType.ROOM_NORMAL;
            case "ROOM_KITCHEN":
                return RoomType.ROOM_KITCHEN;
            case "ROOM_BOSS":
                return RoomType.ROOM_BOSS;
            case "ROOM_START":
                return RoomType.ROOM_START;
            default:
                return RoomType.ROOM_NORMAL;
        }
    }
}
