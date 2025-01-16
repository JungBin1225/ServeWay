using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy List", menuName = "Scriptable Object/Enemy List", order = int.MinValue + 2)]
public class EnemyList : ScriptableObject
{
    [SerializeField] public GameObject meatEnemy;
    [SerializeField] public GameObject riceEnemy;
    [SerializeField] public GameObject soupEnemy;
    [SerializeField] public GameObject noodleEnemy;
    [SerializeField] public GameObject breadEnemy;

    public GameObject RandomEnemy()
    {
        return meatEnemy;
    }
}
