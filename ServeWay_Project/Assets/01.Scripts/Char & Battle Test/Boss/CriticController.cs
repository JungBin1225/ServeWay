using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticController : MonoBehaviour
{
    private MissonManager misson;
    private Rigidbody2D rigidbody;
    private BossController bossCon;
    private GameObject player;
    private Vector2 minPos;
    private Vector2 maxPos;
    private float coolTime;
    private bool isAttack;

    public int test;
    public BossRoom room;
    public GameObject damageEffect;
    public List<GameObject> bulletPrefab;
    public float hp;
    public float speed;
    public float attackCoolTime;
    public float bulletSpeed;
    public float bulletDamage;
    public Food_Nation nation;
    public Boss_Job job;

    void Start()
    {
        misson = FindObjectOfType<MissonManager>();
        rigidbody = GetComponent<Rigidbody2D>();
        bossCon = GetComponent<BossController>();
        player = GameObject.FindGameObjectWithTag("Player");

        bossCon.nation = this.nation;
        bossCon.room = this.room;
        bossCon.SetHp(hp);
        //GameManager.gameManager.mission.boss = this.gameObject;

        coolTime = attackCoolTime;
        isAttack = false;

        minPos = new Vector2(room.transform.position.x - (room.GetComponent<BoxCollider2D>().size.x / 2), room.transform.position.y - (room.GetComponent<BoxCollider2D>().size.y / 2));
        maxPos = new Vector2(room.transform.position.x + (room.GetComponent<BoxCollider2D>().size.x / 2), room.transform.position.y + (room.GetComponent<BoxCollider2D>().size.y / 2));

        StartCoroutine(EnemyMove());
    }

    void Update()
    {
        if (coolTime > 0)
        {
            coolTime -= Time.deltaTime;
        }
    }

    private IEnumerator EnemyMove()
    {
        while (bossCon.GetHp() != 0 && coolTime > 0)
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
        if (test > 0 && test < 5)
        {
            index = test - 1;
        }

        switch (index)
        {
            case 0:
                StartCoroutine(ShotGunPattern());
                break;
            case 1:
                
                break;
            case 2:
                StartCoroutine(MachineGunPattern());
                break;
            case 3:

                break;
        }
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
            int index = Random.Range(0, 2);

            GameObject bullet = Instantiate(bulletPrefab[index], transform.position, rot);
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

    private IEnumerator ShotGunPattern()
    {
        isAttack = true;
        yield return new WaitForSeconds(0.35f);
        
        float radius = 2.5f;
        for (int j = 0; j < 4; j++)
        {
            int rate = Random.Range(0, 6);
            int rateTemp = 0;

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

                    float speed = Random.Range(bulletSpeed - 0.5f, bulletSpeed + 1.5f);
                    int index = 0;
                    if(rateTemp < rate)
                    {
                        index = 0; //full
                    }
                    else
                    {
                        index = 1; //empty
                    }
                    GameObject bullet = Instantiate(bulletPrefab[index], pos, rot);
                    bullet.GetComponent<EnemyBullet>().SetTarget(new Vector3(-x, -y, 0));
                    bullet.GetComponent<EnemyBullet>().SetSpeed(speed);
                    bullet.GetComponent<EnemyBullet>().SetDamage(bulletDamage);

                    rateTemp++;
                }
            }
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(0.3f);

        isAttack = false;
        coolTime = attackCoolTime;
        StartCoroutine(EnemyMove());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            rigidbody.velocity *= -1;
        }
    }
}
