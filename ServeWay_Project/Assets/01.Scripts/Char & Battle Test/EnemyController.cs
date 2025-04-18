using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
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
    private float maxHp;
    private float hp;
    private GameObject target;
    private Vector2 dir;
    private Rigidbody2D rigidBody;
    private LineRenderer lineRenderer;
    private float coolTime;
    private GameObject laser;
    private int laserMat;
    private InventoryManager inventory;
    private BoxCollider2D collider;
    private bool isWall;

    private int soupAmount;
    private int soupRadius;
    private int riceAmount;
    private float laserTime;
    private float breadRadius;

    public float speed;
    public Vector2 roomCenter;
    public EnemyAttackType attackType;
    public float range;
    public Vector2 attackCoolTime;
    public float damage;
    public float bulletSpeed;
    public GameObject bulletPrefab;
    public GameObject laserPrefab;
    public bool moveAble;

    private Vector2 minPos;
    public Vector2 maxPos;
    private GameObject generator;
    private EnemySprite anim;

    public Image hpImage;

    public GameObject damageEffect;
    public GameObject eatSound;

    void Start()
    {
        hp = maxHp;
        moveAble = true;
        isWall = false;
        coolTime = Random.Range(attackCoolTime.x, attackCoolTime.y);
        anim = GetComponent<EnemySprite>();
        collider = GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        inventory = GameManager.gameManager.inventory;
        target = GameObject.FindGameObjectWithTag("Player");
        lineRenderer = transform.GetChild(2).gameObject.GetComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.gameObject.GetComponent<Animator>().SetInteger("Index", laserMat);
        lineRenderer.enabled = false;

        InitStat();

        StartCoroutine(EnemyMove());
        StartCoroutine(EnemyAttack());
    }

    
    void Update()
    {
        if(hp <= 0)
        {
            rigidBody.velocity = Vector2.zero;
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
            anim.state = EnemyState.dead;
            collider.enabled = false;
            if(anim.layerOrder[1].color.a == 0)
            {
                generator.GetComponent<EnemyGenerator>().EnemyDie();
                Destroy(this.gameObject);
            }
        }
    }

    public void GetDamage(float damage)
    {
        GameObject effect = Instantiate(damageEffect, transform.position, transform.rotation);
        GameObject sound = Instantiate(eatSound, transform.position, transform.rotation);

        hp -= damage;
        if(hp <= 0)
        {
            hpImage.fillAmount = 1;
        }
        else
        {
            hpImage.fillAmount = 1 - (hp / maxHp);
        }
    }

    private IEnumerator EnemyMove()
    {
        while(hp > 0)
        {
            dir = target.transform.position - transform.position;

            if(!isWall)
            {
                if (!moveAble)
                {
                    rigidBody.velocity = Vector2.zero;
                    anim.state = EnemyState.idle;

                    if (target.transform.position.x > transform.position.x)
                    {
                        anim.flipObject.transform.localScale = new Vector3(1f, 1f, 1f);
                    }
                    else
                    {
                        anim.flipObject.transform.localScale = new Vector3(-1f, 1f, 1f);
                    }
                }
                else
                {
                    if (dir.magnitude > range)
                    {
                        //chase target
                        rigidBody.velocity = dir.normalized * speed;
                        if (dir.normalized.x > 0)
                        {
                            anim.state = EnemyState.moveRight;
                        }
                        else
                        {
                            anim.state = EnemyState.moveLeft;
                        }
                        yield return null;
                    }
                    else
                    {
                        //move & attack
                        float posX = Random.Range(minPos.x, maxPos.x);
                        float posY = Random.Range(minPos.y, maxPos.y);
                        rigidBody.velocity = new Vector2(posX - transform.position.x, posY - transform.position.y).normalized * speed * inventory.decrease_EnemySpeed;

                        if (posX - transform.position.x > 0)
                        {
                            anim.state = EnemyState.moveRight;
                        }
                        else
                        {
                            anim.state = EnemyState.moveLeft;
                        }
                        yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
                    }
                }
            }
            yield return null;
        }

        rigidBody.velocity = Vector2.zero;
    }

    private IEnumerator EnemyAttack()
    {
        while (hp > 0)
        {
            if (coolTime > 0)
            {
                coolTime -= Time.deltaTime;
                yield return null;
            }
            else
            {
                if (dir.magnitude <= range * inventory.decrease_EnemyAttackRange && moveAble)
                {
                    switch (attackType)
                    {
                        case EnemyAttackType.NOODLE:
                            StartCoroutine(EnemyLaser());
                            break;
                        case EnemyAttackType.RICE:
                            StartCoroutine(EnemyRice());
                            break;
                        default:
                            moveAble = false;
                            rigidBody.velocity = Vector2.zero;
                            yield return new WaitForSeconds(0.2f);
                            EnemyFire();
                            yield return new WaitForSeconds(0.3f);
                            coolTime = Random.Range(attackCoolTime.x, attackCoolTime.y) * inventory.decrease_EnemyAttackTime;
                            moveAble = true;
                            break;
                    }
                }
                else
                {
                    coolTime = 0;
                    yield return null;
                }
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
                breadBullet.SetRadius(breadRadius);
                breadBullet.SetSprite(anim.getEnemySprite());
                break;
            case EnemyAttackType.MEAT:
                var meatBullet = bullet.GetComponent<EnemyBullet>();
                meatBullet.SetTarget(transform.position - target.transform.position);
                meatBullet.SetSpeed(bulletSpeed);
                meatBullet.SetDamage(damage);
                meatBullet.SetSprite(anim.getEnemySprite());
                SetBulletSprite(attackType, bullet);
                break;
            /*case EnemyAttackType.RICE:
                var riceBullet = bullet.GetComponent<EnemyBullet>();
                riceBullet.SetTarget(transform.position - target.transform.position);
                riceBullet.SetSpeed(bulletSpeed);
                riceBullet.SetDamage(damage);
                riceBullet.SetSprite(anim.getEnemySprite());
                break;*/
            case EnemyAttackType.SOUP:
                Destroy(bullet);
                FireSoupBullet(bulletSpeed, damage, soupAmount, soupRadius);
                break;
        }
    }

    private IEnumerator EnemyRice()
    {
        moveAble = false;
        rigidBody.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.2f);

        Vector3 targetTemp = transform.position - target.transform.position;
        bool isleft = (transform.position.x > target.transform.position.x);
        string link = "";
        if(bulletPrefab.transform.childCount == 1)
        {
            link = bulletPrefab.transform.GetChild(0).GetChild(0).gameObject.name;
        }


        for (int i = 0; i < riceAmount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, 0));

            EnemyBullet riceBullet = bullet.GetComponent<EnemyBullet>();
            riceBullet.SetTarget(targetTemp);
            riceBullet.SetSpeed(bulletSpeed);
            riceBullet.SetDamage(damage);
            riceBullet.SetSprite(anim.getEnemySprite());

            if(riceBullet.GetComponent<AudioSource>() != null)
            {
                if(i < 3)
                {
                    riceBullet.GetComponent<AudioSource>().Play();
                }
            }

            if (link != "")
            {
                int index = i;
                TMP_Text text = riceBullet.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TMP_Text>();

                text.text = "";

                if (link[index * 2] == 'a')
                {
                    string format = "abcdefghijklmnopqrstuvwxyz";
                    text.text += format[Random.Range(0, format.Length)].ToString();
                }
                else
                {
                    text.text += link[index * 2].ToString();
                }

                if (link[(index * 2) + 1] == 'a')
                {
                    string format = "abcdefghijklmnopqrstuvwxyz";
                    text.text += format[Random.Range(0, format.Length)].ToString();
                }
                else
                {
                    text.text += link[(index * 2) + 1].ToString();
                }

                if(!isleft)
                {
                    text.text = new string(text.text.Reverse().ToArray());
                }
            }
            
            yield return new WaitForSeconds(0.15f);
        }
        yield return new WaitForSeconds(0.1f);

        coolTime = Random.Range(attackCoolTime.x, attackCoolTime.y) * inventory.decrease_EnemyAttackTime;
        moveAble = true;
    }

    private IEnumerator EnemyLaser()
    {
        bool ishit = false;
        float length = 0;
        moveAble = false;
        rigidBody.velocity = Vector2.zero;
        lineRenderer.enabled = true;
        laser = Instantiate(laserPrefab, this.transform.position, Quaternion.Euler(0, 0, 0));

        laser.GetComponent<EnemyLaser>().SetDamage(damage);
        laser.GetComponent<EnemyLaser>().SetCoolTime(bulletSpeed);
        laser.GetComponent<EnemyLaser>().SetSprite(anim.getEnemySprite());

        Ray2D ray = new Ray2D(transform.position, target.transform.position - transform.position);

        lineRenderer.SetPosition(0, transform.position);

        int mask = 1 << LayerMask.NameToLayer("RayWall") | 1 << LayerMask.NameToLayer("RayTarget");
        while(!ishit)
        {
            length += Time.deltaTime * 7;
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, length, mask);
            if (hit)
            {
                lineRenderer.SetPosition(1, hit.point);
                ishit = true;
            }
            else
            {
                lineRenderer.SetPosition(1, transform.position + (new Vector3(ray.direction.x, ray.direction.y, 0) * length));
            }

            Vector3 start = lineRenderer.GetPosition(0);
            Vector3 end = lineRenderer.GetPosition(1);

            laser.transform.localScale = new Vector3(Vector3.Distance(start, end), lineRenderer.transform.lossyScale.y, 0);
            Vector3 pos = (start + end) / 2;
            Vector2 dir = new Vector2(pos.x - end.x, pos.y - end.y);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion angleAxis = Quaternion.AngleAxis(angle, Vector3.forward);
            laser.transform.rotation = angleAxis;
            laser.transform.position = pos;

            yield return null;
        }

        yield return new WaitForSeconds(laserTime);

        lineRenderer.SetPosition(1, transform.position);
        lineRenderer.enabled = false;
        Destroy(laser);
        coolTime = Random.Range(attackCoolTime.x, attackCoolTime.y) * inventory.decrease_EnemyAttackTime;
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
            bullet.GetComponent<EnemyBullet>().SetSprite(anim.getEnemySprite());

            if (i == bulletAmount - 1 && bullet.GetComponent<UnityEngine.U2D.Animation.SpriteLibrary>() != null)
            {
                bullet.GetComponent<SpriteRenderer>().sprite = bullet.GetComponent<UnityEngine.U2D.Animation.SpriteLibrary>().GetSprite("sprite", "Star_1");
            }
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

    public void SetAttackType(DataController data)
    {
        //attackType = (EnemyAttackType)Random.Range(0, 5);
        //attackType = EnemyAttackType.NOODLE;

        switch(attackType)
        {
            case EnemyAttackType.MEAT:
                bulletPrefab = data.enemyBullet.meatBullet[Random.Range(0, data.enemyBullet.meatBullet.Count)];
                break;
            case EnemyAttackType.SOUP:
                bulletPrefab = data.enemyBullet.soupBullet[Random.Range(0, data.enemyBullet.soupBullet.Count)];
                break;
            case EnemyAttackType.RICE:
                bulletPrefab = data.enemyBullet.riceBullet[Random.Range(0, data.enemyBullet.riceBullet.Count)];
                break;
            case EnemyAttackType.NOODLE:
                laserMat = Random.Range(0, data.enemyBullet.noodleBullet.Count) + 1;
                bulletSpeed = 0.1f;
                break;
            case EnemyAttackType.BREAD:
                bulletPrefab = data.enemyBullet.breadBullet[Random.Range(0, data.enemyBullet.breadBullet.Count)];
                break;
        }
    }

    private void SetBulletSprite(EnemyAttackType attackType, GameObject bullet)
    {
        switch(attackType)
        {
            case EnemyAttackType.MEAT:
                if (bullet.GetComponent<UnityEngine.U2D.Animation.SpriteLibrary>() != null)
                {
                    int ran = Random.Range(0, 3);
                    string index = string.Format("trash_{0}", ran);
                    bullet.GetComponent<SpriteRenderer>().sprite = bullet.GetComponent<UnityEngine.U2D.Animation.SpriteLibrary>().GetSprite("sprite", index);
                }
                break;
        }
    }

    public void SetMaxHp(float max)
    {
        maxHp = max;
        hp = maxHp;
    }

    private void InitStat()
    {
        switch(GameManager.gameManager.stage)
        {
            case 1:
                soupAmount = 5;
                soupRadius = 5;
                riceAmount = 4;
                laserTime = 2;
                break;
            case 2:
                soupAmount = 5;
                soupRadius = 5;
                riceAmount = 4;
                laserTime = 2;
                break;
            case 3:
                soupAmount = 6;
                soupRadius = 6;
                riceAmount = 5;
                laserTime = 3;
                break;
            case 4:
                soupAmount = 6;
                soupRadius = 6;
                riceAmount = 5;
                laserTime = 3;
                break;
            case 5:
                soupAmount = 7;
                soupRadius = 7;
                riceAmount = 6;
                laserTime = 4;
                break;
            case 6:
                soupAmount = 7;
                soupRadius = 7;
                riceAmount = 6;
                laserTime = 4;
                break;
            case 7:
                soupAmount = 8;
                soupRadius = 8;
                riceAmount = 7;
                laserTime = 5;
                break;
        }
        breadRadius = 2;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.gameObject.GetComponent<PlayerController>().isCharge)
        {
            StartCoroutine(Knockback(collision.gameObject));
            if(inventory.isNuts)
            {
                GetDamage(5);
            }
        }
        else if (!moveAble && collision.gameObject.tag == "Wall")
        {
            rigidBody.velocity = Vector2.zero;
        }
        else if(moveAble && collision.gameObject.tag == "Wall")
        {
            isWall = true;
            rigidBody.velocity *= -1;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            rigidBody.velocity = new Vector2(roomCenter.x - transform.position.x, roomCenter.y - transform.position.y).normalized * speed * inventory.decrease_EnemySpeed;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall" && isWall)
        {
            isWall = false;
        }
    }

}
