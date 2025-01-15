using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JournalController : MonoBehaviour
{
    private MissionManager misson;
    private Rigidbody2D rigidbody;
    private BossController bossCon;
    private GameObject player;
    private Vector2 minPos;
    private Vector2 maxPos;
    private List<Sprite> sprites;
    private List<string> soupMents;
    private float coolTime;
    private bool isAttack;
    private bool isPicture;
    private bool isCharge;

    public int test;
    public BossRoom room;
    public GameObject damageEffect;
    public GameObject riceBulletPrefab;
    public GameObject soupBulletPrefab;
    public GameObject scoopPrefab;
    public CircleCollider2D collider;
    public Animator pictureAnim;
    public float hp;
    public float speed;
    public float chargeSpeed;
    public float attackCoolTime;
    public float bulletSpeed;
    public float bulletDamage;
    public float pictureDamage;
    public float chargeDamage;
    public Food_Nation nation;
    public Boss_Job job;

    void Start()
    {
        misson = FindObjectOfType<MissionManager>();
        rigidbody = GetComponent<Rigidbody2D>();
        bossCon = GetComponent<BossController>();
        player = GameObject.FindGameObjectWithTag("Player");
        sprites = new List<Sprite>();
        sprites.Add(gameObject.GetComponent<SpriteRenderer>().sprite);
        soupMents = new List<string>();
        soupMents.Add("단독보도");
        soupMents.Add("긴급속보");
        soupMents.Add("특별취재");
        soupMents.Add("LIVE");

        bossCon.nation = this.nation;
        bossCon.room = this.room;
        bossCon.job = this.job;
        bossCon.SetHp(hp);
        bossCon.SetMaxHp(hp);
        //GameManager.gameManager.mission.boss = this.gameObject;

        collider.enabled = false;

        coolTime = attackCoolTime;
        isAttack = false;
        isPicture = false;
        isCharge = false;

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

        if (isAttack)
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
        if(test > 0 && test < 5)
        {
            index = test - 1;
        }

        switch (index)
        {
            case 0:
                StartCoroutine(PicturePattern());
                break;
            case 1:
                StartCoroutine(MachineGunPattern());
                break;
            case 2:
                StartCoroutine(ShotGunPattern());
                break;
            case 3:
                StartCoroutine(ChargePattern());
                break;
        }
    }

    private IEnumerator PicturePattern()
    {
        isAttack = true;
        transform.GetChild(0).gameObject.SetActive(true);
        //범위 표시
        yield return new WaitForSeconds(1f);

        pictureAnim.SetTrigger("picture");
        isPicture = true;
        collider.enabled = true;
        yield return new WaitForSeconds(0.2f);

        transform.GetChild(0).gameObject.SetActive(false);
        isPicture = false;
        collider.enabled = false;
        isAttack = false;
        coolTime = attackCoolTime;
        StartCoroutine(EnemyMove());
    }

    private IEnumerator MachineGunPattern()
    {
        isAttack = true;
        yield return new WaitForSeconds(0.3f);

        string ment = riceBulletPrefab.transform.GetChild(0).GetChild(1).gameObject.name;
        ment = ment.Replace(" ", "");
        Debug.Log(ment);
        rigidbody.velocity = new Vector2(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y).normalized * (speed / 3);
        for (int i = 0; i < ment.Length; i++)
        {
            Vector2 direction = player.transform.position - transform.position;
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, direction);
            GameObject bullet = Instantiate(riceBulletPrefab, transform.position, rot);
            bullet.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = ment[i].ToString();
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

        float radius = 7;
        float bulletAmount = 4;
        for (int j = 0; j < 6; j++)
        {
            float startAngle = (radius * 10) / 2;
            float differAngle = (radius * 10) / (bulletAmount - 1);
            string ment = soupMents[Random.Range(0, soupMents.Count)];

            for (int i = 0; i < bulletAmount; i++)
            {
                GameObject bullet = Instantiate(soupBulletPrefab, transform.position, Quaternion.Euler(Quaternion.FromToRotation(Vector3.up, player.transform.position - transform.position).eulerAngles + new Vector3(0, 0, startAngle - (differAngle * i))));
                bullet.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = ment[i].ToString();
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

    private IEnumerator ChargePattern()
    {
        isAttack = true;
        float posX = Random.Range(-(room.transform.localScale.x / 2) + 1, (room.transform.localScale.x / 2) - 1);
        float posY = Random.Range(-(room.transform.localScale.y / 2) + 1, (room.transform.localScale.y / 2) - 1);
        Vector3 target = new Vector3(room.transform.position.x + posX, room.transform.position.y + posY, 0);
        GameObject scoop = Instantiate(scoopPrefab, target, Quaternion.Euler(0, 0, 0));
        Scoop scoopCon = scoop.GetComponent<Scoop>();

        yield return new WaitForSeconds(0.5f);

        rigidbody.velocity = new Vector2(target.x - transform.position.x, target.y - transform.position.y).normalized * chargeSpeed;
        isCharge = true;

        yield return new WaitUntil(() => scoopCon.GetTouch());

        rigidbody.velocity = new Vector2(0, 0);

        float faintTime = 0;
        if (scoopCon.touchedObject == "Boss")
        {
            faintTime = 0;
        }
        else if (scoopCon.touchedObject == "Player")
        {
            faintTime = 2;
            Debug.Log("Faint");
            misson.OccurreEvent(9, 1);
            //기절 이펙트
        }
        Destroy(scoop);

        yield return new WaitForSeconds(faintTime);

        isCharge = false;
        isAttack = false;
        coolTime = attackCoolTime;
        StartCoroutine(EnemyMove());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && isPicture)
        {
            collision.gameObject.GetComponent<PlayerHealth>().PlayerDamaged(pictureDamage, sprites);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            rigidbody.velocity *= -1;
        }

        if(collision.gameObject.tag == "Player" && isCharge)
        {
            collision.gameObject.GetComponent<PlayerHealth>().PlayerDamaged(chargeDamage, sprites);
        }
    }
}
