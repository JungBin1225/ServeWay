using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Map Object List", menuName = "Scriptable Object/Map Object List", order = int.MinValue + 6)]
public class MapObjectList : ScriptableObject
{
    public List<GameObject> testList;
    public List<GameObject> campingList;
    public List<GameObject> schoolList;
}
