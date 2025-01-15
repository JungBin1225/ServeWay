using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearcherController : MonoBehaviour
{
    private MissionManager mission;
    private Rigidbody2D rigidbody;
    private BossController bossCon;
    private GameObject player;
    private List<Vector3> platePos;
    private Vector2 minPos;
    private Vector2 maxPos;
    private List<Sprite> sprites;
    private float coolTime;
    private float soupTime;
    private bool isAttack;
    private bool isCharge;
    private bool plateTouch;
    private int plateIndex;

    public int test;
    public BossRoom room;
    public GameObject damageEffect;
    public GameObject soupBulletPrefab;
    public GameObject ladelPrefab;
    public GameObject platePrefab;
    public GameObject soupPrefab;
    public float hp;
    public float speed;
    public float chargeSpeed;
    public float attackCoolTime;
    public float soupCoolTime;
    public float bulletSpeed;
    public float bulletDamage;
    public float chargeDamage;
    public float soupDamage;
    public Food_Nation nation;
    public Boss_Job job;

    void Start()
    {
        mission = FindObjectOfType<MissionManager>();
        rigidbody = GetComponent<Rigidbody2D>();
        bossCon = GetComponent<BossController>();
        player = GameObject.FindGameObjectWithTag("Player");
        sprites = new List<Sprite>();
        sprites.Add(gameObject.GetComponent<SpriteRenderer>().sprite);

        bossCon.nation = this.nation;
        bossCon.room = this.room;
        bossCon.job = this.job;
        bossCon.SetHp(hp);
        bossCon.SetMaxHp(hp);
        //GameManager.gameManager.mission.boss = this.gameObject;

        platePos = new List<Vector3>();
        coolTime = attackCoolTime;
        soupTime = 0;
        isAttack = false;
        isCharge = false;
        plateTouch = false;
        plateIndex = 0;

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

        if(soupTime > 0)
        {
            soupTime -= Time.deltaTime;
        }

        if(isAttack)
        {
            rigidbody.velocity = Vector2.zero;
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
        if(soupTime > 0)
        {
            index = Random.Range(0, 3);
        }

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
                StartCoroutine(LadlePattern());
                break;
            case 2:
                StartCoroutine(ChargePattern());
                break;
            case 3:
                StartCoroutine(FloorSoupPattern());
                break;
        }
    }

    private IEnumerator ShotGunPattern()
    {
        isAttack = true;
        yield return new WaitForSeconds(0.35f);

        float radius = 10;
        float bulletAmount = 7;
        for (int j = 0; j < 4; j++)
        {
            float startAngle = (radius * 10) / 2;
            float differAngle = (radius * 10) / (bulletAmount - 1);

            for (int i = 0; i < bulletAmount; i++)
            {
                GameObject bullet = Instantiate(soupBulletPrefab, transform.position, Quaternion.Euler(Quaternion.FromToRotation(Vector3.up, player.transform.position - transform.position).eulerAngles + new Vector3(0, 0, startAngle - (differAngle * i))));
                bullet.GetComponent<EnemyBullet>().SetTarget(-bullet.transform.up);
                bullet.GetComponent<EnemyBullet>().SetSpeed(bulletSpeed);
                bullet.GetComponent<EnemyBullet>().SetDamage(bulletDamage);
                bullet.GetComponent<EnemyBullet>().SetSprite(sprites);
            }
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(0.3f);

        isAttack = false;
        coolTime = attackCoolTime;
        StartCoroutine(EnemyMove());
    }

    private IEnumerator LadlePattern()
    {
        isAttack = true;
        yield return new WaitForSeconds(0.2f);

        GameObject ladle = Instantiate(ladelPrefab, transform.position, Quaternion.Euler(0, 0, 0));
        ladle.GetComponent<Ladle>().start = transform.position;
        ladle.GetComponent<Ladle>().target = player;
        ladle.GetComponent<Ladle>().damage = bulletDamage;
        ladle.GetComponent<Ladle>().sprite = GetComponent<SpriteRenderer>().sprite;

        yield return new WaitUntil(() => FindObjectOfType<Ladle>() == null);
        yield return new WaitForSeconds(0.5f);

        isAttack = false;
        coolTime = attackCoolTime;
        StartCoroutine(EnemyMove());
    }

    private IEnumerator ChargePattern()
    {
        isAttack = true;
        int amount = Random.Range(2, 5);
        RandomPlate(amount);

        yield return new WaitForSeconds(0.5f);

        plateIndex = 100;
        for (int i = 0; i < platePos.Count; i++)
        {
            GameObject plate = Instantiate(platePrefab, transform.position, Quaternion.Euler(0, 0, 0));
            plate.GetComponent<Plate>().index = i;
            plate.GetComponent<Plate>().target = platePos[i];
            plate.GetComponent<Plate>().damage = 1;
            plate.GetComponent<Plate>().sprite = GetComponent<SpriteRenderer>().sprite;
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < platePos.Count; i++)
        {
            isCharge = true;
            plateIndex = i;
            rigidbody.velocity = new Vector2(platePos[i].x - transform.position.x, platePos[i].y - transform.position.y).normalized * chargeSpeed;

            yield return new WaitUntil(() => plateTouch);
            yield return new WaitForSeconds(0.1f);

            plateTouch = false;
            isCharge = false;
            rigidbody.velocity = new Vector2(0, 0);
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(1);

        platePos.Clear();
        isCharge = false;
        isAttack = false;
        coolTime = attackCoolTime;
        StartCoroutine(EnemyMove());
    }

    private IEnumerator FloorSoupPattern()
    {
        isAttack = true;

        if(soupTime > 0)
        {
            isAttack = false;
            StartCoroutine(EnemyMove());
        }
        else
        {
            float posX = Random.Range(-(room.transform.localScale.x / 2) + 3f, (room.transform.localScale.x / 2) - 3f);
            float posY = Random.Range(-(room.transform.localScale.y / 2) + 3f, (room.transform.localScale.y / 2) - 3f);
            Vector3 target = new Vector3(room.transform.position.x + posX, room.transform.position.y + posY, 0);

            yield return new WaitForSeconds(0.3f);

            GameObject soup = Instantiate(soupPrefab, target, Quaternion.Euler(0, 0, 0));
            soup.GetComponent<FloorSoup>().damage = soupDamage;
            soup.GetComponent<FloorSoup>().durationTime = soupCoolTime;
            soup.GetComponent<FloorSoup>().sprite = GetComponent<SpriteRenderer>().sprite;
            yield return new WaitForSeconds(0.7f);

            isAttack = false;
            coolTime = attackCoolTime;
            soupTime = soupCoolTime;
            StartCoroutine(EnemyMove());
        }
    }

    private void RandomPlate(int amount)
    {
        if(platePos.Count <= amount)
        {
            float posX = Random.Range(-(room.transform.localScale.x / 2) + 1, (room.transform.localScale.x / 2) - 1);
            float posY = Random.Range(-(room.transform.localScale.y / 2) + 1, (room.transform.localScale.y / 2) - 1);
            Vector3 target = new Vector3(room.transform.position.x + posX, room.transform.position.y + posY, 0);
            Vector3 prevTarget = transform.position;
            if(platePos.Count != 0)
            {
                prevTarget = platePos[platePos.Count - 1];
            }
            
            if(Vector3.Distance(target, prevTarget) < 5)
            {
                RandomPlate(amount);
            }
            else
            {
                platePos.Add(target);
                RandomPlate(amount);
            }
        }
        else
        {
            return;
        }
    }

    public void plateTouched()
    {
        plateTouch = true;
    }

    public int GetIndex()
    {
        return plateIndex;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            rigidbody.velocity *= -1;
        }

        if (collision.gameObject.tag == "Player" && isCharge)
        {
            collision.gameObject.GetComponent<PlayerHealth>().PlayerDamaged(chargeDamage, sprites);
        }
    }
}
