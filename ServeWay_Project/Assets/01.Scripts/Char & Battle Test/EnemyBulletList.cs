using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Bullet List", menuName = "Scriptable Object/Enemy Bullet List", order = int.MinValue + 4)]
public class EnemyBulletList : ScriptableObject
{
    [SerializeField] public List<GameObject> meatBullet;
    [SerializeField] public List<GameObject> soupBullet;
    [SerializeField] public List<GameObject> noodleBullet;
    [SerializeField] public List<GameObject> riceBullet;
    [SerializeField] public List<GameObject> breadBullet;
}
