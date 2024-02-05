using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    private MissonManager misson;
    private Rigidbody2D rigidbody;
    private GameObject player;
    private Vector2 minPos;
    private Vector2 maxPos;
    private float coolTime;
    private bool isAttack;

    public BossRoom room;
    public GameObject damageEffect;
    public GameObject bulletPrefab;
    public GameObject splitBulletPrefab;
    public float hp;
    public float speed;
    public float attackCoolTime;
    public float bulletSpeed;
    public float bulletDamage;
    public float splitBulletDamage;
    public Boss_Nation nation;

    void Start()
    {
        misson = FindObjectOfType<MissonManager>();
        rigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        //GameManager.gameManager.mission.boss = this.gameObject;

        coolTime = attackCoolTime;
        isAttack = false;

        minPos = new Vector2(room.transform.position.x - (room.GetComponent<BoxCollider2D>().size.x / 2), room.transform.position.y - (room.GetComponent<BoxCollider2D>().size.y / 2));
        maxPos = new Vector2(room.transform.position.x + (room.GetComponent<BoxCollider2D>().size.x / 2), room.transform.position.y + (room.GetComponent<BoxCollider2D>().size.y / 2));

        StartCoroutine(EnemyMove());
    }

    void Update()
    {
        if (hp <= 0)
        {
            BossDie(0);
        }

        if(misson.isClear())
        {
            BossDie(1);
        }

        if (coolTime > 0)
        {
            coolTime -= Time.deltaTime;
        }
    }

    public void BossDie(int dieType)
    {
        room.isClear = true;

        switch(dieType)
        {
            case 0: //Hp 소진
                room.DropIngredient(4, 9);
                break;
            case 1: //미션 클리어
                room.DropIngredient(6, 13);
                break;
        }
        room.DropRecipe();

        room.OpenDoor();
        room.ActiveStair();
        GameManager.gameManager.isBossStage = false;
        Destroy(this.gameObject);
    }

    public void GetDamage(float damage, Vector3 effectPos, Food_Nation nation)
    {
        //GameObject effect = Instantiate(damageEffect, effectPos, transform.rotation);

        hp -= damage;
        
        if(nation.ToString() == this.nation.ToString())
        {
            misson.OccurreEvent(0, damage);
        }
        misson.OccurreEvent(3, damage);
    }

    private IEnumerator EnemyMove()
    {
        while (hp != 0 && coolTime > 0)
        {
            float posX = Random.Range(minPos.x, maxPos.x);
            float posY = Random.Range(minPos.y, maxPos.y);
            rigidbody.velocity = new Vector2(posX - transform.position.x, posY - transform.position.y).normalized * speed;
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        }

        rigidbody.velocity = Vector2.zero;
        SelectPattern();
        //boss pattern
    }

    private void SelectPattern()
    {
        int index = Random.Range(0, 4);

        switch (index)
        {
            case 0:
                StartCoroutine(AroundPattern());
                break;
            case 1:
                StartCoroutine(MachineGunPattern());
                break;
            case 2:
                StartCoroutine(SplitPattern());
                break;
            case 3:
                StartCoroutine(ShotGunPattern());
                break;
        }
    }

    private IEnumerator AroundPattern()
    {
        isAttack = true;
        yield return new WaitForSeconds(0.3f);

        float radius = 2.5f;
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
            bullet.GetComponent<EnemyBullet>().SetSpeed(bulletSpeed);
            bullet.GetComponent<EnemyBullet>().SetDamage(bulletDamage);
        }
        yield return new WaitForSeconds(0.3f);

        isAttack = false;
        coolTime = attackCoolTime;
        StartCoroutine(EnemyMove());
    }

    private IEnumerator MachineGunPattern()
    {
        isAttack = true;
        yield return new WaitForSeconds(0.3f);

        rigidbody.velocity = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y).normalized * (speed / 3);
        for (int i = 0; i < 40; i++)
        {
            Vector2 direction = player.transform.position - transform.position;
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, direction);
            GameObject bullet = Instantiate(bulletPrefab, transform.position, rot);
            bullet.GetComponent<EnemyBullet>().SetTarget(-bullet.transform.up);
            bullet.GetComponent<EnemyBullet>().SetSpeed(bulletSpeed * 2);
            bullet.GetComponent<EnemyBullet>().SetDamage(bulletDamage);
            yield return new WaitForSeconds(0.1f);
        }

        isAttack = false;
        rigidbody.velocity = Vector2.zero;
        coolTime = attackCoolTime;
        StartCoroutine(EnemyMove());
    }

    private IEnumerator SplitPattern()
    {
        isAttack = true;
        yield return new WaitForSeconds(0.4f);

        float radius = 2.5f;
        for (int i = 0; i < 4; i++)
        {
            float angle = i * Mathf.PI * 2 / 4;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            Vector3 pos = transform.position + new Vector3(x, y, 0);
            float angleDegrees = -angle * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(0, 0, angleDegrees);
            GameObject bullet = Instantiate(splitBulletPrefab, pos, rot);
            bullet.GetComponent<SplitBullet>().SetTarget(new Vector3(-x, -y, 0));
            bullet.GetComponent<SplitBullet>().SetSpeed(bulletSpeed / 2);
            bullet.GetComponent<SplitBullet>().SetDamage(bulletDamage * 2);
            bullet.GetComponent<SplitBullet>().SetSplitSpeed(bulletSpeed);
            bullet.GetComponent<SplitBullet>().SetSplitDamage(bulletDamage);
            bullet.GetComponent<SplitBullet>().SetBigDamage(splitBulletDamage);
        }
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 8; i++)
        {
            if (i % 2 == 1)
            {
                float angle = i * Mathf.PI * 2 / 8;
                float x = Mathf.Cos(angle) * radius;
                float y = Mathf.Sin(angle) * radius;
                Vector3 pos = transform.position + new Vector3(x, y, 0);
                float angleDegrees = -angle * Mathf.Rad2Deg;
                Quaternion rot = Quaternion.Euler(0, 0, angleDegrees);
                GameObject bullet = Instantiate(splitBulletPrefab, pos, rot);
                bullet.GetComponent<SplitBullet>().SetTarget(new Vector3(-x, -y, 0));
                bullet.GetComponent<SplitBullet>().SetSpeed(bulletSpeed / 2);
                bullet.GetComponent<SplitBullet>().SetDamage(bulletDamage * 2);
                bullet.GetComponent<SplitBullet>().SetSplitSpeed(bulletSpeed);
                bullet.GetComponent<SplitBullet>().SetSplitDamage(bulletDamage);
            }
        }
        yield return new WaitForSeconds(0.3f);

        isAttack = false;
        coolTime = attackCoolTime * 2;
        StartCoroutine(EnemyMove());
    }

    private IEnumerator ShotGunPattern()
    {
        isAttack = true;
        yield return new WaitForSeconds(0.35f);

        float radius = 2.5f;
        for (int j = 0; j < 4; j++)
        {
            for (int i = 0; i < 25; i++)
            {
                float angle = i * Mathf.PI * 2 / 25;
                float x = Mathf.Cos(angle) * radius;
                float y = Mathf.Sin(angle) * radius;
                Vector3 pos = transform.position + new Vector3(x, y, 0);

                if (Mathf.Abs(Vector2.SignedAngle(pos - transform.position, player.transform.position - transform.position)) < 45)
                {
                    float angleDegrees = -angle * Mathf.Rad2Deg;
                    Quaternion rot = Quaternion.Euler(0, 0, angleDegrees);
                    GameObject bullet = Instantiate(bulletPrefab, pos, rot);
                    bullet.GetComponent<EnemyBullet>().SetTarget(new Vector3(-x, -y, 0));
                    bullet.GetComponent<EnemyBullet>().SetSpeed(bulletSpeed);
                    bullet.GetComponent<EnemyBullet>().SetDamage(bulletDamage);
                }
            }
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(0.3f);

        isAttack = false;
        coolTime = attackCoolTime;
        StartCoroutine(EnemyMove());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            rigidbody.velocity *= -1;
        }
    }
}
