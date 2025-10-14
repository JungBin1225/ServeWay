using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticController : MonoBehaviour
{
    private MissionManager mission;
    private Rigidbody2D rigidbody;
    private SpriteRenderer renderer;
    private BossController bossCon;
    private GameObject player;
    private GameObject bulletParent;
    private GameObject effectParent;
    private GameObject summonObject;
    private Vector2 minPos;
    private Vector2 maxPos;
    private float coolTime;
    private List<Sprite> sprites;
    private bool isAttack;
    private bool isCharge;
    private float shotGunRadius;
    private float shotGunAmount;
    private int machineGunAmount;
    private bool isLeft;

    public int test;
    public BossRoom room;
    public GameObject damageEffect;
    public GameObject pen;
    public GameObject explosionPenPrefab;
    public List<GameObject> bulletPrefab;
    public float speed;
    public float attackCoolTime;
    public float chargeSpeed;
    public float bulletSpeed;
    public float bulletDamage;
    public float penDamage;
    public float explosionDamage;
    public float attackRange;
    public Food_Nation nation;
    public Boss_Job job;

    void Start()
    {
        mission = FindObjectOfType<MissionManager>();
        rigidbody = GetComponent<Rigidbody2D>();
        bossCon = GetComponent<BossController>();
        renderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        bulletParent = GameObject.Find("BulletList");
        effectParent = GameObject.Find("EffectList");
        summonObject = GameObject.Find("SummonList");
        sprites = new List<Sprite>();
        sprites.Add(gameObject.GetComponent<SpriteRenderer>().sprite);

        bossCon.nation = this.nation;
        bossCon.room = this.room;
        bossCon.job = this.job;
        SetIncreaseByStage();
        //GameManager.gameManager.mission.boss = this.gameObject;

        coolTime = attackCoolTime;
        isAttack = false;
        isCharge = false;
        isLeft = true;

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

        if (rigidbody.velocity.x < 0)
        {
            isLeft = true;
        }
        else if (rigidbody.velocity.x > 0)
        {
            isLeft = false;
        }
        if (isLeft)
        {
            renderer.flipX = false;
        }
        else
        {
            renderer.flipX = true;
        }

        if (bossCon.GetHp() == 0)
        {
            rigidbody.velocity = Vector2.zero;
            Destroy(summonObject);
            StopAllCoroutines();
        }
    }

    private IEnumerator EnemyMove()
    {
        while (bossCon.GetHp() != 0 && coolTime > 0)
        {
            float posX = Random.Range(minPos.x, maxPos.x);
            float posY = Random.Range(minPos.y, maxPos.y);
            if (attackRange < Vector3.Distance(transform.position, player.transform.position))
            {
                posX = player.transform.position.x;
                posY = player.transform.position.y;
            }

            rigidbody.velocity = new Vector2(posX - transform.position.x, posY - transform.position.y).normalized * speed;

            if (attackRange < Vector3.Distance(transform.position, player.transform.position))
            {
                yield return new WaitForSeconds(Random.Range(0.2f, 0.75f));
            }
            else
            {
                yield return new WaitForSeconds(Random.Range(0.3f, 1.0f));
            }
        }

        rigidbody.velocity = Vector2.zero;
        if (bossCon.GetHp() != 0)
        {
            StartPattern();
        }
    }

    private void StartPattern()
    {
        int index = 0;

        if (attackRange > Vector3.Distance(transform.position, player.transform.position)) //사정 거리 안
        {
            if (Random.Range(0, 2) == 0)
            {
                index = 0; //근거리 패턴
            }
            else
            {
                index = Random.Range(1, 4); //원거리 패턴
            }
        }
        else //사정 거리 밖
        {
            index = Random.Range(1, 4); //원거리 패턴
        }

        if (test > 0 && test < 5)
        {
            index = test - 1;
        }

        SelectPattern(index);
    }

    private void SelectPattern(int index)
    {
        switch (index)
        {
            case 0:
                StartCoroutine(CutPattern());
                break;
            case 1:
                StartCoroutine(ShotGunPattern());
                break;
            case 2:
                StartCoroutine(MachineGunPattern());
                break;
            case 3:
                StartCoroutine(PenExplosionPattern());
                break;
        }
    }

    private IEnumerator MachineGunPattern()
    {
        isAttack = true;
        yield return new WaitForSeconds(0.3f);

        rigidbody.velocity = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y).normalized * (speed / 3);
        for (int i = 0; i < machineGunAmount; i++)
        {
            Vector2 direction = player.transform.position - transform.position;
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, direction);
            int index = Random.Range(0, 2);

            GameObject bullet = Instantiate(bulletPrefab[index], transform.position, rot, bulletParent.transform);
            bullet.GetComponent<EnemyBullet>().SetTarget(-bullet.transform.up);
            bullet.GetComponent<EnemyBullet>().SetSpeed(bulletSpeed * 2);
            bullet.GetComponent<EnemyBullet>().SetDamage(bulletDamage);
            bullet.GetComponent<EnemyBullet>().SetSprite(sprites);
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

        for (int j = 0; j < 5; j++)
        {
            float startAngle = (shotGunRadius * 10) / 2;
            float differAngle = (shotGunRadius * 10) / (shotGunAmount - 1);
            int rate = Random.Range(0, 6);
            int rateTemp = 0;

            for (int i = 0; i < shotGunAmount; i++)
            {
                float speed = Random.Range(bulletSpeed - 0.5f, bulletSpeed + 1.5f);
                int index;
                if (rateTemp == rate)
                {
                    index = 0; //full
                }
                else
                {
                    index = 1; //empty
                }

                GameObject bullet = Instantiate(bulletPrefab[index], transform.position, Quaternion.Euler(Quaternion.FromToRotation(Vector3.up, player.transform.position - transform.position).eulerAngles + new Vector3(0, 0, startAngle - (differAngle * i))), bulletParent.transform);
                bullet.GetComponent<EnemyBullet>().SetTarget(-bullet.transform.up);
                bullet.GetComponent<EnemyBullet>().SetSpeed(speed);
                bullet.GetComponent<EnemyBullet>().SetDamage(bulletDamage);
                bullet.GetComponent<EnemyBullet>().SetSprite(sprites);

                rateTemp++;
            }
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(0.3f);

        isAttack = false;
        coolTime = attackCoolTime;
        StartCoroutine(EnemyMove());
    }

    private IEnumerator CutPattern()
    {
        isAttack = true;
        yield return new WaitForSeconds(0.35f);

        isCharge = true;
        Vector3 target = player.transform.position;
        Vector2 velo = new Vector2(target.x - transform.position.x, target.y - transform.position.y).normalized * chargeSpeed;

        while(Vector3.Distance(target, transform.position) > 1)
        {
            rigidbody.velocity = velo;
            yield return null;
        }

        rigidbody.velocity = new Vector2(0, 0);
        isCharge = false;
        yield return new WaitForSeconds(0.1f);

        Vector2 direction = player.transform.position - transform.position;
        Vector3 startRot = Quaternion.FromToRotation(Vector3.down, direction).eulerAngles;
        int num = 0;

        pen.SetActive(true);
        pen.transform.GetChild(0).GetComponent<Pen>().damage = penDamage;
        pen.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, startRot.z - 25));

        while(num < 65)
        {
            pen.transform.localRotation = Quaternion.Euler(pen.transform.localRotation.eulerAngles + new Vector3(0, 0, 2));
            num++;
            yield return null;
        }

        pen.SetActive(false);
        yield return new WaitForSeconds(0.3f);

        isAttack = false;
        coolTime = attackCoolTime;
        StartCoroutine(EnemyMove());
    }

    private IEnumerator PenExplosionPattern()
    {
        isAttack = true;
        yield return new WaitForSeconds(0.5f);

        for(int i = 0; i < 3; i++)
        {
            float posX = Random.Range(-(room.transform.localScale.x / 2) + 2f, (room.transform.localScale.x / 2) - 2f);
            float posY = Random.Range(-(room.transform.localScale.y / 2) + 2f, (room.transform.localScale.y / 2) - 2f);
            Vector3 target = new Vector3(room.transform.position.x + posX, room.transform.position.y + posY, 0);

            GameObject explosionPen = Instantiate(explosionPenPrefab, target, Quaternion.Euler(0, 0, 0), summonObject.transform);
            explosionPen.GetComponent<PenExplosion>().damage = explosionDamage;

            yield return new WaitForSeconds(0.1f);
        }

        isAttack = false;
        coolTime = attackCoolTime / 4;
        StartCoroutine(EnemyMove());
    }

    private void SetIncreaseByStage()
    {
        int stage = GameManager.gameManager.stage - 1;

        bossCon.SetMaxHp(500 + (stage * 400));
        bossCon.SetHp(500 + (stage * 400));

        shotGunRadius = 7 + (stage * 1);
        shotGunAmount = 5 + (stage * 1);
        machineGunAmount = 30 + (stage * 2);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            rigidbody.velocity *= -1;
        }

        if (collision.gameObject.tag == "Player")
        {
            if (!isCharge)
            {
                if (isAttack)
                {
                    rigidbody.velocity = Vector2.zero;
                }
                else
                {
                    rigidbody.velocity *= -1;
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (isAttack && !isCharge)
            {
                rigidbody.velocity = Vector2.zero;
            }
        }
    }
}
