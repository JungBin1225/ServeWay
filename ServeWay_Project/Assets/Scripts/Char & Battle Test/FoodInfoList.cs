using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Food Info List", menuName = "Scriptable Object/Food Info List", order = int.MaxValue)]
public class FoodInfoList : ScriptableObject
{
    [SerializeField]
    public List<FoodInfo> foodInfo;
}
