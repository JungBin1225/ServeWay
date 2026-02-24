using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherSplit : MonoBehaviour
{
    private GameObject bulletParent;
    private GameObject effectParent;

    public GameObject split;
    public GameObject sound;

    void Start()
    {
        bulletParent = GameObject.Find("BulletList");
        effectParent = GameObject.Find("EffectList");
    }

    void Update()
    {
        
    }

    private void OnDestroy()
    {
        int bulletAmount = 10;
        float startAngle = 0;
        float differAngle = 360 / (bulletAmount);

        for (int i = 0; i < bulletAmount; i++)
        {
            GameObject bullet = Instantiate(split, transform.position, Quaternion.Euler(new Vector3(0, 0, startAngle - (differAngle * i))), bulletParent.transform);
            bullet.GetComponent<EnemyBullet>().SetTarget(-bullet.transform.up);
            bullet.GetComponent<EnemyBullet>().SetSpeed(5);
            bullet.GetComponent<EnemyBullet>().SetDamage(1);
            bullet.GetComponent<EnemyBullet>().SetSprite(GetComponent<EnemyExplosionBullet>().GetSprite());
            bullet.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
        }

        Instantiate(sound, transform.position, transform.rotation, effectParent.transform);
    }
}
