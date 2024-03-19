using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy List", menuName = "Scriptable Object/Enemy List", order = int.MinValue + 2)]
public class EnemyList : ScriptableObject
{
    [SerializeField]
    public List<GameObject> enemyPrefab;

    public GameObject RandomEnemy()
    {
        int index = Random.Range(0, enemyPrefab.Count);

        return enemyPrefab[index];
    }
}
