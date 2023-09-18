using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Save File", menuName = "Scriptable Object/Save Flie", order = int.MinValue + 1)]
public class SaveFile : ScriptableObject
{
    public Dictionary<IngredientList.IngredientsName, int> inventory;
    public List<string> weaponList;
    public float playerHp;
    public float playerSpeed;
    public float playerChargeSpeed;
    public float playerChargeLength;
    public float playerChargeCooltime;

    public int stage;

    public bool isMapSave;
    public List<Room> roomList;
    public int startX;
    public int startY;
}
