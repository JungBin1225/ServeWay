using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
    private LineRenderer lineRenderer;
    private float coolTime;
    private GameObject laser;
    private int laserMat;
    private InventoryManager inventory;
    private BoxCollider2D collider;
    private bool isWall;

    public float maxHp;
    public float speed;
    public Vector2 roomCenter;
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
    private EnemySprite anim;

    public Image hpImage;

    public GameObject damageEffect;
    public GameObject eatSound;

    void Start()
    {
        hp = maxHp;
        moveAble = true;
        isWall = false;
        coolTime = attackCoolTime;
        anim = GetComponent<EnemySprite>();
        collider = GetComponent<BoxCollider2D>();
        rigidBody = GetComponent<Rigidbody2D>();
        inventory = GameManager.gameManager.inventory;
        target = GameObject.FindGameObjectWithTag("Player");
        lineRenderer = transform.GetChild(2).gameObject.GetComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.gameObject.GetComponent<Animator>().SetInteger("Index", laserMat);
        lineRenderer.enabled = false;

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
                Destroy(this.gameObject);
            }
        }
        else
        {
            if (target.transform.position.x > transform.position.x)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                hpImage.gameObject.transform.parent.parent.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
                hpImage.gameObject.transform.parent.parent.localRotation = Quaternion.Euler(0, 180, 0);
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
            hpImage.fillAmount = 0;
        }
        else
        {
            hpImage.fillAmount = hp / maxHp;
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
                    switch(attackType)
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
                            coolTime = attackCoolTime * inventory.decrease_EnemyAttackTime;
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
                breadBullet.SetRadius(alphaStat[0]);
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
                FireSoupBullet(bulletSpeed, damage, alphaStat[0], alphaStat[1]);
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
            alphaStat[0] = link.Length;
        }


        for (int i = 0; i < alphaStat[0]; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(0, 0, 0));

            EnemyBullet riceBullet = bullet.GetComponent<EnemyBullet>();
            riceBullet.SetTarget(targetTemp);
            riceBullet.SetSpeed(bulletSpeed);
            riceBullet.SetDamage(damage);
            riceBullet.SetSprite(anim.getEnemySprite());

            if(link != "")
            {
                int index = i;
                if (!isleft)
                {
                    index = link.Length - i - 1;
                }

                if (link[index] == 'a')
                {
                    string format = "abcdefghijklmnopqrstuvwxyz";
                    riceBullet.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TMP_Text>().text = format[Random.Range(0, format.Length)].ToString();
                }
                else
                {
                    
                    riceBullet.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TMP_Text>().text = link[index].ToString();
                }
            }
            
            yield return new WaitForSeconds(0.15f);
        }
        yield return new WaitForSeconds(0.1f);

        coolTime = attackCoolTime * inventory.decrease_EnemyAttackTime;
        moveAble = true;
    }

    private IEnumerator EnemyLaser()
    {
        moveAble = false;
        rigidBody.velocity = Vector2.zero;
        lineRenderer.enabled = true;
        laser = Instantiate(laserPrefab, this.transform.position, Quaternion.Euler(0, 0, 0));

        laser.GetComponent<EnemyLaser>().SetDamage(damage);
        laser.GetComponent<EnemyLaser>().SetCoolTime(bulletSpeed);
        laser.GetComponent<EnemyLaser>().SetSprite(anim.getEnemySprite());

        Ray2D ray = new Ray2D(transform.position, target.transform.position - transform.position);

        lineRenderer.SetPosition(0, transform.position);

        int mask = 1 << LayerMask.NameToLayer("RayTarget_P") | 1 << LayerMask.NameToLayer("RayWall");
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

        laser.transform.localScale = new Vector3(Vector3.Distance(start, end) * 1.25f, lineRenderer.transform.lossyScale.y * 1.25f, 0);
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
        coolTime = attackCoolTime * inventory.decrease_EnemyAttackTime;
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
                alphaStat[0] = 1;
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
