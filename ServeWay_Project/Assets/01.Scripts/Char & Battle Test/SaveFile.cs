using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveFile
{
    public NameAmount inventory;
    public List<string> weaponList;
    public int weaponIndex;
    public List<Create_Success> weaponSuccess;
    public float playerHp;
    public float playerSpeed;
    public float playerChargeSpeed;
    public float playerChargeLength;
    public float playerChargeCooltime;

    public int stage;
    public List<Food_Nation> bossNations;
    public List<Boss_Job> bossJobs;
    public List<Stage_Theme> themes;

    public bool isMapSave;
    public List<Room> roomList;
    public int startX;
    public int startY;

    public bool isTuto;

    public void Reset()
    {
        inventory = new NameAmount();
        weaponList = new List<string>();
        weaponIndex = 0;
        weaponSuccess = new List<Create_Success>();
        playerHp = 10;
        playerSpeed = 0;
        playerChargeSpeed = 0;
        playerChargeLength = 0;
        playerChargeCooltime = 0;

        stage = 0;
        bossNations = new List<Food_Nation>();
        bossJobs = new List<Boss_Job>();
        themes = new List<Stage_Theme>();

        roomList = new List<Room>();
        startX = 0;
        startY = 0;
    }
}
