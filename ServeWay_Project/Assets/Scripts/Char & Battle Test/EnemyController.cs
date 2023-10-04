using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float range;
    public float attackCoolTime;
    public float damage;
    public float bulletSpeed;
    public GameObject bulletPrefab;
    public bool moveAble;

    private Vector2 minPos;
    public Vector2 maxPos;
    private GameObject generator;

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
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
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
            EnemyFire();
            coolTime = attackCoolTime;
        }
    }

    private void EnemyFire()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bullet.GetComponent<EnemyBullet>().SetTarget(transform.position - target.transform.position);
        bullet.GetComponent<EnemyBullet>().SetDamage(damage);
        bullet.GetComponent<EnemyBullet>().SetSpeed(bulletSpeed);
    }

    private IEnumerator Knockback(GameObject player)
    {
        Debug.Log("aa");
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
