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
    private LineRenderer lineRenderer;
    private float coolTime;
    private GameObject laser;

    public float maxHp;
    public float speed;
    public EnemyAttackType attackType;
    public float range;
    public float attackCoolTime;
    public float damage;
    public float bulletSpeed;
    public List<float> alphaStat;
    public GameObject bulletPrefab;
    public GameObject laserPrefab;
    public bool moveAble;

    private Vector2 minPos;
    public Vector2 maxPos;
    private GameObject generator;

    public Image hpImage;

    public GameObject damageEffect;
    public GameObject eatSound;

    void Start()
    {
        hp = maxHp;
        moveAble = true;
        coolTime = attackCoolTime;

        anim = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.enabled = false;

        StartCoroutine(EnemyMove());
    }

    
    void Update()
    {
        if(hp <= 0)
        {
            //generator.GetComponent<EnemyGenerator>().enemyAmount--;
            if(lineRenderer.enabled)
            {
                lineRenderer.SetPosition(1, transform.position);
                lineRenderer.enabled = false;
                if (laser != null)
                {
                    Destroy(laser);
                }
            }
            Destroy(this.gameObject);
        }

        EnemyAttack();
    }

    public void GetDamage(float damage, Vector3 effectPos)
    {
        GameObject effect = Instantiate(damageEffect, transform.position, transform.rotation);
        GameObject sound = Instantiate(eatSound, transform.position, transform.rotation);

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
                rigidBody.velocity = Vector2.zero;
                yield return null;
            }
            else
            {
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
                coolTime = attackCoolTime;
            }
            else
            {
                StartCoroutine(EnemyLaser());
            }
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
                breadBullet.SetSprite(gameObject.GetComponent<SpriteRenderer>().sprite);
                break;
            case EnemyAttackType.MEAT:
                var meatBullet = bullet.GetComponent<EnemyBullet>();
                meatBullet.SetTarget(transform.position - target.transform.position);
                meatBullet.SetSpeed(bulletSpeed);
                meatBullet.SetDamage(damage);
                meatBullet.SetSprite(gameObject.GetComponent<SpriteRenderer>().sprite);
                break;
            case EnemyAttackType.RICE:
                var riceBullet = bullet.GetComponent<EnemyBullet>();
                riceBullet.SetTarget(transform.position - target.transform.position);
                riceBullet.SetSpeed(bulletSpeed);
                riceBullet.SetDamage(damage);
                riceBullet.SetSprite(gameObject.GetComponent<SpriteRenderer>().sprite);
                break;
            case EnemyAttackType.SOUP:
                Destroy(bullet);
                FireSoupBullet(bulletSpeed, damage, alphaStat[0], alphaStat[1]);
                break;
        }
    }

    private IEnumerator EnemyLaser()
    {
        moveAble = false;
        rigidBody.velocity = Vector2.zero;
        lineRenderer.enabled = true;
        laser = Instantiate(laserPrefab, this.transform);

        laser.GetComponent<EnemyLaser>().SetDamage(damage);
        laser.GetComponent<EnemyLaser>().SetCoolTime(bulletSpeed);
        laser.GetComponent<EnemyLaser>().SetSprite(gameObject.GetComponent<SpriteRenderer>().sprite);

        Ray2D ray = new Ray2D(transform.position, target.transform.position - transform.position);

        lineRenderer.SetPosition(0, transform.position);

        int mask = 1 << LayerMask.NameToLayer("RayTarget") | 1 << LayerMask.NameToLayer("RayWall");
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 1000f, mask);
        if (hit)
        {
            lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            lineRenderer.SetPosition(1, target.transform.position);
        }

        Vector3 start = lineRenderer.GetPosition(0);
        Vector3 end = lineRenderer.GetPosition(1);

        laser.transform.localScale = new Vector3(Vector3.Distance(start, end) * 1.25f, lineRenderer.startWidth * 1.25f, 0);
        Vector3 pos = (start + end) / 2;
        Vector2 dir = new Vector2(pos.x - end.x, pos.y - end.y);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion angleAxis = Quaternion.AngleAxis(angle, Vector3.forward);
        laser.transform.rotation = angleAxis;
        laser.transform.position = pos;

        yield return new WaitForSeconds(alphaStat[0]);

        lineRenderer.SetPosition(1, transform.position);
        lineRenderer.enabled = false;
        Destroy(laser);
        coolTime = attackCoolTime;
        moveAble = true;
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
            bullet.GetComponent<EnemyBullet>().SetSprite(gameObject.GetComponent<SpriteRenderer>().sprite);
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
