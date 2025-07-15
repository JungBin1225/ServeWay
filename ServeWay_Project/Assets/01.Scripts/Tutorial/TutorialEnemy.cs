using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialEnemy : MonoBehaviour
{
    public float maxHp;
    public GameObject bulletPrefab;
    private GameObject bulletParent;
    private GameObject effectParent;
    public float bulletDamage;
    public float bulletSpeed;
    public bool attackAble;
    public ChargingTutorial tutorial;
    public RectTransform hpImage;
    public GameObject damageEffect;

    private float hp;
    private GameObject target;
    private bool moveAble;
    private Rigidbody2D rigidBody;
    private SpriteRenderer spriteRenderer;
    private List<Sprite> sprites;
    private bool touchWall;
    private float hp_width;

    private void Start()
    {
        hp = maxHp;
        moveAble = true;
        attackAble = false;
        touchWall = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        sprites = new List<Sprite>();
        sprites.Add(spriteRenderer.sprite);
        target = GameObject.FindGameObjectWithTag("Player");
        bulletParent = GameObject.Find("BulletList");
        effectParent = GameObject.Find("EffectList");
        rigidBody = GetComponent<Rigidbody2D>();
        hp_width = hpImage.rect.width;
        hpImage.sizeDelta = new Vector2(0, 8);

        StartCoroutine(Fire());
    }

    private void Update()
    {
        if (hp <= 0)
        {
            Destroy(this.gameObject);
        }

        if(target.transform.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    private IEnumerator Fire()
    {
        yield return new WaitUntil(() => attackAble);

        for(int i = 0; i < 6; i++)
        {
            FireSoupBullet(bulletSpeed, bulletDamage, 6, 3);
            tutorial.AddMissonAmount();
            yield return new WaitForSeconds(0.5f);
        }

        attackAble = false;
    }

    private void FireSoupBullet(float speed, float damage, float radius, float bulletAmount)
    {
        float startAngle = (radius * 10) / 2;
        float differAngle = (radius * 10) / (bulletAmount - 1);
        Vector3 fromAngle = Quaternion.FromToRotation(Vector3.up, target.transform.position - transform.position).eulerAngles;

        for (int i = 0; i < bulletAmount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(fromAngle + new Vector3(0, 0, startAngle - (differAngle * i))), bulletParent.transform);
            bullet.GetComponent<EnemyBullet>().SetTarget(-bullet.transform.up);
            bullet.GetComponent<EnemyBullet>().SetSpeed(speed);
            bullet.GetComponent<EnemyBullet>().SetDamage(damage);
            bullet.GetComponent<EnemyBullet>().SetSprite(sprites);
        }
    }

    private IEnumerator Knockback(GameObject player)
    {
        moveAble = false;
        GetComponent<BoxCollider2D>().isTrigger = true;
        rigidBody.velocity = Vector2.zero;
        rigidBody.AddForce((transform.position - player.transform.position).normalized * 15000, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.2f);

        rigidBody.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.2f);

        GetComponent<BoxCollider2D>().isTrigger = false;
        moveAble = true;
    }

    public void GetKnockBack(GameObject player)
    {
        if (moveAble)
        {
            StartCoroutine(Knockback(player));
        }
    }

    public void GetDamage(float damage, Vector3 effectPos)
    {
        GameObject effect = Instantiate(damageEffect, transform.position, transform.rotation, effectParent.transform);

        hp -= damage;
        if (hp <= 0)
        {
            hpImage.sizeDelta = new Vector2(hp_width, 8);
        }
        else
        {
            hpImage.sizeDelta = new Vector2(hp_width * (1 - (hp / maxHp)), 8);
        }
    }

    public float GetNowHp()
    {
        return hp;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!moveAble && collision.gameObject.tag == "Wall")
        {
            rigidBody.velocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!moveAble && collision.gameObject.tag == "Wall")
        {
            rigidBody.velocity = Vector2.zero;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            rigidBody.velocity = Vector2.zero;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!moveAble && collision.gameObject.tag == "Wall")
        {
            rigidBody.velocity = Vector2.zero;
        }
    }
}
