using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EnemyAttackType
{
    MEAT,
    RICE,
    SOUP,
    NOODLE,
    BREAD
};

public class EnemyController : MonoBehaviour
{
    private float hp;
    private GameObject target;
    private Vector2 dir;
    private Rigidbody2D rigidBody;
    private Animator anim;
    private float coolTime;

    public float maxHp;
    public float speed;
    public EnemyAttackType attackType;
    public float range;
    public float attackCoolTime;
    public float damage;
    public float bulletSpeed;
    public List<float> alphaStat;
    public GameObject bulletPrefab;
    public bool moveAble;

    private Vector2 minPos;
    public Vector2 maxPos;
    private GameObject generator;

    public Image hpImage;

    public GameObject damageEffect;

    void Start()
    {
        hp = maxHp;
        moveAble = true;
        coolTime = attackCoolTime;

        anim = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine(EnemyMove());
    }

    
    void Update()
    {
        if(hp <= 0)
        {
            //generator.GetComponent<EnemyGenerator>().enemyAmount--;
            Destroy(this.gameObject);
        }

        EnemyAttack();
    }

    public void GetDamage(float damage, Vector3 effectPos)
    {
        //GameObject effect = Instantiate(damageEffect, effectPos, transform.rotation);

        hp -= damage;
        if(hp <= 0)
        {
            hpImage.fillAmount = 0;
        }
        else
        {
            hpImage.fillAmount = hp / maxHp;
        }
    }

    private IEnumerator EnemyMove()
    {
        while(hp != 0)
        {
            dir = target.transform.position - transform.position;
            //anim.SetBool("move", true);

            if(!moveAble)
            {
                yield return null;
            }

            if (dir.magnitude > range)
            {
                //chase target
                rigidBody.velocity = dir.normalized * speed;
                yield return null;
            }
            else
            {
                //move & attack
                float posX = Random.Range(minPos.x, maxPos.x);
                float posY = Random.Range(minPos.y, maxPos.y);
                rigidBody.velocity = new Vector2(posX - transform.position.x, posY - transform.position.y).normalized * speed;
                yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
            }
        }
    }

    private void EnemyAttack()
    {
        if(target.transform.position.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            hpImage.gameObject.transform.parent.parent.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            hpImage.gameObject.transform.parent.parent.localRotation = Quaternion.Euler(0, 180, 0);
        }

        if(coolTime > 0)
        {
            coolTime -= Time.deltaTime;
        }
        else
        {
            coolTime = 0;
        }

        if(coolTime == 0 && dir.magnitude <= range && moveAble)
        {
            if(attackType != EnemyAttackType.NOODLE)
            {
                EnemyFire();
            }
            else
            {
                StartCoroutine(EnemyLaser());
            }
            
            coolTime = attackCoolTime;
        }
    }

    private void EnemyFire()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);

        switch(attackType)
        {
            case EnemyAttackType.BREAD:
                var breadBullet = bullet.GetComponent<EnemyExplosionBullet>();
                breadBullet.SetTarget(transform.position - target.transform.position);
                breadBullet.SetSpeed(bulletSpeed);
                breadBullet.SetDamage(damage);
                breadBullet.SetRadius(alphaStat[0]);
                break;
            case EnemyAttackType.MEAT:
                var meatBullet = bullet.GetComponent<EnemyBullet>();
                meatBullet.SetTarget(transform.position - target.transform.position);
                meatBullet.SetSpeed(bulletSpeed);
                meatBullet.SetDamage(damage);
                break;
            case EnemyAttackType.RICE:
                var riceBullet = bullet.GetComponent<EnemyBullet>();
                riceBullet.SetTarget(transform.position - target.transform.position);
                riceBullet.SetSpeed(bulletSpeed);
                riceBullet.SetDamage(damage);
                break;
            case EnemyAttackType.SOUP:
                Destroy(bullet);
                FireSoupBullet(bulletSpeed, damage, alphaStat[0], alphaStat[1]);
                break;
        }
    }

    private IEnumerator EnemyLaser()
    {
        yield return null;
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


    public void SetVector(Vector2 min, Vector2 max)
    {
        minPos = min;
        maxPos = max;
    }

    public void SetGenerator(GameObject generator)
    {
        this.generator = generator;
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
        else if(moveAble && collision.gameObject.tag == "Wall")
        {
            rigidBody.velocity *= -1;
        }
    }
}
