using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialEnemy : MonoBehaviour
{
    public float maxHp;
    public GameObject bulletPrefab;
    public float bulletDamage;
    public float bulletSpeed;
    public bool attackAble;
    public ChargingTutorial tutorial;
    public Image hpImage;
    public GameObject damageEffect;

    private float hp;
    private GameObject target;
    private bool moveAble;
    private Rigidbody2D rigidBody;

    private void Start()
    {
        hp = maxHp;
        moveAble = true;
        attackAble = false;
        target = GameObject.FindGameObjectWithTag("Player");
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(attackAble)
        {
            StartCoroutine(Fire());
            attackAble = false;
        }

        if (hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private IEnumerator Fire()
    {
        for(int i = 0; i < 6; i++)
        {
            FireSoupBullet(bulletSpeed, bulletDamage, 6, 3);
            tutorial.AddMissonAmount();
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void FireSoupBullet(float speed, float damage, float radius, float bulletAmount)
    {
        float startAngle = (radius * 10) / 2;
        float differAngle = (radius * 10) / (bulletAmount - 1);
        Vector3 fromAngle = Quaternion.FromToRotation(Vector3.up, target.transform.position - transform.position).eulerAngles;

        for (int i = 0; i < bulletAmount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(fromAngle + new Vector3(0, 0, startAngle - (differAngle * i))));
            bullet.GetComponent<EnemyBullet>().SetTarget(-bullet.transform.up);
            bullet.GetComponent<EnemyBullet>().SetSpeed(speed);
            bullet.GetComponent<EnemyBullet>().SetDamage(damage);
        }
    }

    private IEnumerator Knockback(GameObject player)
    {
        moveAble = false;
        rigidBody.velocity = Vector2.zero;
        rigidBody.AddForce((transform.position - player.transform.position).normalized * 20, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.2f);

        rigidBody.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.1f);

        moveAble = true;
    }

    public void GetDamage(float damage, Vector3 effectPos)
    {
        GameObject effect = Instantiate(damageEffect, transform.position, transform.rotation);

        hp -= damage;
        if (hp <= 0)
        {
            hpImage.fillAmount = 0;
        }
        else
        {
            hpImage.fillAmount = hp / maxHp;
        }
    }

    public float GetNowHp()
    {
        return hp;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.gameObject.GetComponent<PlayerController>().isCharge)
        {
            StartCoroutine(Knockback(collision.gameObject));
        }
        else if (!moveAble && collision.gameObject.tag == "Wall")
        {
            rigidBody.velocity = Vector2.zero;
        }
    }
}
