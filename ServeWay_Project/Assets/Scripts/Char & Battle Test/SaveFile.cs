using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Save File", menuName = "Scriptable Object/Save Flie", order = int.MinValue + 1)]
public class SaveFile : ScriptableObject
{
    public NameAmount inventory;
    public List<string> weaponList;
    public List<Create_Success> weaponSuccess;
    public float playerHp;
    public float playerSpeed;
    public float playerChargeSpeed;
    public float playerChargeLength;
    public float playerChargeCooltime;

    public int stage;
    public List<Boss_Nation> bossNations;

    public bool isMapSave;
    public List<Room> roomList;
    public int startX;
    public int startY;

    public void Reset()
    {
        inventory = new NameAmount();
        weaponList = new List<string>();
        weaponSuccess = new List<Create_Success>();
        playerHp = 10;
        playerSpeed = 0;
        playerChargeSpeed = 0;
        playerChargeLength = 0;
        playerChargeCooltime = 0;

        stage = 0;
        bossNations = new List<Boss_Nation>();

        roomList = new List<Room>();
        startX = 0;
        startY = 0;
    }
}
