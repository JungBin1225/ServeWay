using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JournalController : MonoBehaviour
{
    private MissionManager misson;
    private Rigidbody2D rigidbody;
    private BossController bossCon;
    private Animator anim;
    private SpriteRenderer renderer;
    private SpriteRenderer effectRenderer;
    private GameObject player;
    private Vector2 minPos;
    private Vector2 maxPos;
    private List<Sprite> sprites;
    private List<string> soupMents;
    private float coolTime;
    private bool isAttack;
    private bool isPicture;
    private bool isCharge;
    private float shotGunRadius;
    private float shotGunAmount;
    private bool isLeft;

    public int test;
    public BossRoom room;
    public GameObject damageEffect;
    public GameObject riceBulletPrefab;
    public GameObject soupBulletPrefab;
    public GameObject scoopPrefab;
    public GameObject dashEffect;
    public GameObject dashDust;
    public CircleCollider2D collider;
    public Animator pictureAnim;
    public List<AudioClip> attackSound;
    public AudioSource attackAudio;
    public AudioSource faintAudio;
    public float speed;
    public float chargeSpeed;
    public float attackCoolTime;
    public float bulletSpeed;
    public float bulletDamage;
    public float pictureDamage;
    public float chargeDamage;
    public float attackRange;
    public Food_Nation nation;
    public Boss_Job job;

    void Start()
    {
        misson = FindObjectOfType<MissionManager>();
        rigidbody = GetComponent<Rigidbody2D>();
        bossCon = GetComponent<BossController>();
        anim = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        effectRenderer = dashEffect.GetComponent<SpriteRenderer>();
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
        SetIncreaseByStage();
        //GameManager.gameManager.mission.boss = this.gameObject;

        collider.enabled = false;

        coolTime = attackCoolTime;
        isAttack = false;
        isPicture = false;
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

        /*if (isAttack)
        {
            rigidbody.velocity = Vector2.zero;
        }*/


        if(rigidbody.velocity.x < 0)
        {
            isLeft = true;
        }
        else if(rigidbody.velocity.x > 0)
        {
            isLeft = false;
        }
        if(isLeft)
        {
            renderer.flipX = false;
            effectRenderer.flipX = false;
        }
        else
        {
            renderer.flipX = true;
            effectRenderer.flipX = true;
        }

        if(bossCon.GetHp() == 0)
        {
            rigidbody.velocity = Vector2.zero;
            StopAllCoroutines();
        }
    }

    private IEnumerator EnemyMove()
    {
        anim.SetTrigger("walk");
        while (bossCon.GetHp() != 0 && coolTime > 0)
        {
            float posX = Random.Range(minPos.x, maxPos.x);
            float posY = Random.Range(minPos.y, maxPos.y);
            if(attackRange < Vector3.Distance(transform.position, player.transform.position))
            {
                posX = player.transform.position.x;
                posY = player.transform.position.y;
            }

            rigidbody.velocity = new Vector2(posX - transform.position.x, posY - transform.position.y).normalized * speed;
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
        }

        rigidbody.velocity = Vector2.zero;
        if(bossCon.GetHp() != 0)
        {
            SelectPattern();
        }
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
        anim.SetInteger("attacktype", 1);
        anim.SetTrigger("attack");
        //범위 표시
        yield return new WaitForSeconds(1f);

        attackAudio.loop = false;
        attackAudio.clip = attackSound[0];
        attackAudio.volume = 0.7f;
        attackAudio.pitch = 1.0f;
        attackAudio.Play();

        pictureAnim.SetTrigger("picture");
        isPicture = true;
        collider.enabled = true;
        yield return new WaitForSeconds(0.2f);

        anim.SetTrigger("attackend");
        transform.GetChild(0).gameObject.SetActive(false);
        isPicture = false;
        collider.enabled = false;
        isAttack = false;
        coolTime = attackCoolTime;
        StartCoroutine(EnemyMove());
    }

    private IEnumerator MachineGunPattern()
    {
        anim.SetTrigger("attackend");
        isAttack = true;
        yield return new WaitForSeconds(0.3f);

        anim.SetInteger("attacktype", 2);
        anim.SetTrigger("attack");

        attackAudio.loop = true;
        attackAudio.clip = attackSound[1];
        attackAudio.volume = 1.0f;
        attackAudio.pitch = 1.0f;
        attackAudio.Play();

        string ment = riceBulletPrefab.transform.GetChild(0).GetChild(1).gameObject.name;
        ment = ment.Replace(" ", "");
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

        attackAudio.Stop();
        isAttack = false;
        rigidbody.velocity = Vector2.zero;
        coolTime = attackCoolTime;
        StartCoroutine(EnemyMove());
    }

    private IEnumerator ShotGunPattern()
    {
        anim.SetTrigger("attackend");
        isAttack = true;
        yield return new WaitForSeconds(0.35f);

        anim.SetInteger("attacktype", 3);
        anim.SetTrigger("attack");

        attackAudio.loop = true;
        attackAudio.clip = attackSound[2];
        attackAudio.volume = 1.0f;
        attackAudio.pitch = 1.0f;
        attackAudio.Play();

        for (int j = 0; j < 6; j++)
        {
            float startAngle = (shotGunRadius * 10) / 2;
            float differAngle = (shotGunRadius * 10) / (shotGunAmount - 1);
            string ment = soupMents[Random.Range(0, soupMents.Count)];

            for (int i = 0; i < shotGunAmount; i++)
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

        attackAudio.Stop();
        isAttack = false;
        coolTime = attackCoolTime;
        StartCoroutine(EnemyMove());
    }

    private IEnumerator ChargePattern()
    {
        anim.SetTrigger("attackend");
        isAttack = true;
        float posX = Random.Range(-(room.transform.localScale.x / 2) + 1, (room.transform.localScale.x / 2) - 1);
        float posY = Random.Range(-(room.transform.localScale.y / 2) + 1, (room.transform.localScale.y / 2) - 1);
        Vector3 target = new Vector3(room.transform.position.x + posX, room.transform.position.y + posY, 0);
        GameObject scoop = Instantiate(scoopPrefab, target, Quaternion.Euler(0, 0, 0));
        Scoop scoopCon = scoop.GetComponent<Scoop>();

        yield return new WaitForSeconds(0.5f);

        anim.SetInteger("attacktype", 4);
        anim.SetTrigger("attack");
        dashEffect.SetActive(true);

        attackAudio.loop = false;
        attackAudio.clip = attackSound[3];
        attackAudio.volume = 0.5f;
        attackAudio.pitch = 1.2f;
        attackAudio.Play();

        if (target.x > transform.position.x)
        {
            GameObject dust = Instantiate(dashDust, new Vector3(transform.position.x - 2.8f, transform.position.y + 0.5f), Quaternion.Euler(0, 0, 0));
            dust.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            Instantiate(dashDust, new Vector3(transform.position.x + 2.8f, transform.position.y + 0.5f), Quaternion.Euler(0, 0, 0));
        }

        rigidbody.velocity = new Vector2(target.x - transform.position.x, target.y - transform.position.y).normalized * chargeSpeed;
        isCharge = true;

        yield return new WaitUntil(() => scoopCon.GetTouch());

        dashEffect.GetComponent<Animator>().SetTrigger("end");
        rigidbody.velocity = new Vector2(0, 0);

        float faintTime = 0;
        if (scoopCon.touchedObject == "Boss")
        {
            faintTime = 0;
        }
        else if (scoopCon.touchedObject == "Player")
        {
            faintTime = 2;
            misson.OccurreEvent(9, 1);
            //기절 이펙트
        }
        Destroy(scoop);

        if(faintTime != 0)
        {
            isCharge = false;
            anim.SetTrigger("faint");
            faintAudio.Play();
        }
        yield return new WaitForSeconds(faintTime);

        faintAudio.Stop();
        isCharge = false;
        isAttack = false;
        coolTime = attackCoolTime;
        StartCoroutine(EnemyMove());
    }

    private void SetIncreaseByStage()
    {
        int stage = GameManager.gameManager.stage - 1;

        bossCon.SetMaxHp(500 + (stage * 400));
        bossCon.SetHp(500 + (stage * 400));

        shotGunRadius = 7 + (stage * 1);
        shotGunAmount = 4 + (stage * 1);
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

        if(collision.gameObject.tag == "Player")
        {
            if(isCharge)
            {
                collision.gameObject.GetComponent<PlayerHealth>().PlayerDamaged(chargeDamage, sprites);
            }
            else
            {
                rigidbody.velocity *= -1;
            }
        }
    }
}
