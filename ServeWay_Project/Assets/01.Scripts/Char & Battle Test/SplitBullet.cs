using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitBullet : EnemyBullet
{
    public GameObject bulletPrefab;

    private float bigDamage;
    private float splitSpeed;
    private float splitDamage;

    public void SetBigDamage(float damage)
    {
        this.bigDamage = damage;
    }

    public void SetSplitSpeed(float speed)
    {
        this.splitSpeed = speed;
    }

    public void SetSplitDamage(float damage)
    {
        this.splitDamage = damage;
    }

    private void BulletSplit()
    {
        float radius = 0.1f;
        for (int i = 0; i < 20; i++)
        {
            float angle = i * Mathf.PI * 2 / 20;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            Vector3 pos = transform.position + new Vector3(x, y, 0);
            float angleDegrees = -angle * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(0, 0, angleDegrees);
            GameObject bullet = Instantiate(bulletPrefab, pos, rot);
            bullet.GetComponent<EnemyBullet>().SetTarget(new Vector3(-x, -y, 0));
            bullet.GetComponent<EnemyBullet>().SetSpeed(this.splitSpeed);
            bullet.GetComponent<EnemyBullet>().SetDamage(this.splitDamage);
            bullet.GetComponent<EnemyBullet>().destroyEffect = this.destroyEffect;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealth>().PlayerDamaged(bigDamage);
        }

        if (collision.tag == "Player" || collision.tag == "Wall")
        {
            BulletSplit();
            Instantiate(destroyEffect, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
