using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss List", menuName = "Scriptable Object/Boss List", order = int.MinValue + 3)]
public class BossList : ScriptableObject
{
    public List<GameObject> bossPrefab;
}
