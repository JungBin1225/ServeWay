using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoutuberController : MonoBehaviour
{
    private MissionManager mission;
    private Rigidbody2D rigidbody;
    private SpriteRenderer renderer;
    private BossController bossCon;
    private Animator anim;
    private DataController dataController;
    private GameObject player;
    private GameObject bulletParent;
    private GameObject effectParent;
    private GameObject summonObject;
    private Vector2 minPos;
    private Vector2 maxPos;
    private List<Sprite> sprites;
    private LineRenderer line;
    private AudioSource audio;
    private float coolTime;
    private bool isAttack;
    private bool isCharge;
    private bool isTouch;
    private bool playerDamaged;
    private FoodData algorithmFood;
    private float algorithmCoolTime;
    private float machineGunAmount;
    private bool isLeft;

    public int test;
    public BossRoom room;
    public GameObject damageEffect;
    public GameObject riceBulletPrefab;
    public GameObject explosionPrefab;
    public GameObject algorithmPrefab;
    public GameObject scanEffect;
    public GameObject dashDust;
    public List<AudioClip> attackSound;
    public float speed;
    public float chargeSpeed;
    public float attackCoolTime;
    public float bulletSpeed;
    public float bulletDamage;
    public float explosionSpeed;
    public float explosionDamage;
    public float explosionRadius;
    public float attackRange;
    public bool isAlgorithm;
    public Food_Nation nation;
    public Boss_Job job;

    void Start()
    {
        mission = FindObjectOfType<MissionManager>();
        dataController = FindObjectOfType<DataController>();
        rigidbody = GetComponent<Rigidbody2D>();
        bossCon = GetComponent<BossController>();
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        line = GetComponent<LineRenderer>();
        renderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        bulletParent = GameObject.Find("BulletList");
        effectParent = GameObject.Find("EffectList");
        summonObject = GameObject.Find("SummonList");
        sprites = new List<Sprite>();
        sprites.Add(dataController.FindBossSprite(Boss_Job.YOUTUBER));

        bossCon.nation = this.nation;
        bossCon.room = this.room;
        bossCon.job = this.job;
        SetIncreaseByStage();
        //GameManager.gameManager.mission.boss = this.gameObject;

        coolTime = attackCoolTime;
        line.enabled = false;
        isAttack = false;
        isAlgorithm = false;
        isCharge = false;
        isTouch = false;
        playerDamaged = false;
        isLeft = true;

        minPos = new Vector2(room.transform.position.x - (room.transform.localScale.x / 2), room.transform.position.y - (room.transform.localScale.y / 2));
        maxPos = new Vector2(room.transform.position.x + (room.transform.localScale.x / 2), room.transform.position.y + (room.transform.localScale.y / 2));

        StartCoroutine(EnemyMove());
    }

    void Update()
    {
        if (coolTime > 0)
        {
            coolTime -= Time.deltaTime;
        }

        if(isAlgorithm)
        {
            if(playerDamaged)
            {
                mission.OccurreEvent(13, 0);
                playerDamaged = false;
            }
        }

        if (!isAttack && rigidbody.velocity.x < 0)
        {
            isLeft = true;
        }
        else if (!isAttack && rigidbody.velocity.x > 0)
        {
            isLeft = false;
        }
        else if (isAttack && transform.position.x - player.transform.position.x > 0)
        {
            isLeft = true;
        }
        else if (isAttack && transform.position.x - player.transform.position.x < 0)
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
        anim.SetTrigger("walk");
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
                StartCoroutine(AlgorithmPattern());
                break;
            case 1:
                StartCoroutine(ExplosionPattern());
                break;
            case 2:
                StartCoroutine(MachineGunPattern());
                break;
            case 3:
                StartCoroutine(ChargePattern());
                break;
        }
    }

    private IEnumerator ExplosionPattern()
    {
        isAttack = true;
        anim.SetInteger("attacktype", 1);
        anim.SetTrigger("attack");

        audio.loop = false;
        audio.clip = attackSound[0];
        audio.volume = 1.0f;
        audio.pitch = 1.0f;
        audio.Play();

        yield return new WaitForSeconds(0.3f);

        GameObject explosionBullet = Instantiate(explosionPrefab, transform.position, transform.rotation, bulletParent.transform);
        var breadBullet = explosionBullet.GetComponent<YoutuderExplosion>();
        breadBullet.SetTarget(transform.position - player.transform.position);
        breadBullet.SetSpeed(explosionSpeed);
        breadBullet.SetDamage(explosionDamage);
        breadBullet.SetRadius(explosionRadius);
        breadBullet.SetSprite(sprites);

        isAttack = false;
        yield return new WaitForSeconds(0.3f);

        coolTime = attackCoolTime;
        StartCoroutine(EnemyMove());
    }

    private IEnumerator AlgorithmPattern()
    {
        anim.SetTrigger("attackend");
        FoodData playerFood = new FoodData();
        string food = player.GetComponent<PlayerController>().weaponSlot.GetHoldWeapon();
        if (food != null)
        {
            playerFood = dataController.FindFood(food);
        }
        algorithmFood = playerFood;

        GameObject scan = Instantiate(scanEffect, player.transform.position + new Vector3(0, 0.2f, 0), Quaternion.Euler(0, 0, 0), player.transform);

        yield return new WaitForSeconds(0.5f);

        anim.SetInteger("attacktype", 4);
        anim.SetTrigger("attack");

        audio.loop = false;
        audio.clip = attackSound[3];
        audio.volume = 1.0f;
        audio.pitch = 1.0f;
        audio.Play();

        Vector3 target = room.transform.position;
        while (Vector3.Distance(target, transform.position) > 0.5f)
        {
            rigidbody.velocity = new Vector2(target.x - transform.position.x, target.y - transform.position.y).normalized * 10;
            yield return null;
        }
        rigidbody.velocity = new Vector2(0, 0);

        anim.SetInteger("attacktype", 2);
        anim.SetTrigger("attack");
        yield return new WaitForSeconds(0.5f);

        Destroy(scan);
        isAlgorithm = true;

        audio.loop = true;
        audio.clip = attackSound[1];
        audio.volume = 0.4f;
        audio.pitch = 0.8f;
        audio.Play();

        for (int i = 0; i < 60; i++)
        {
            GameObject algorithm = Instantiate(algorithmPrefab, AlgorithmPos(), Quaternion.Euler(0, 0, 0), summonObject.transform);

            Vector2 dir = new Vector2(algorithm.transform.position.x - transform.position.x, algorithm.transform.position.y - transform.position.y);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion angleAxis = Quaternion.AngleAxis(angle, Vector3.forward);
            algorithm.transform.rotation = angleAxis;

            algorithm.GetComponent<Algorithm>().target = algorithm.transform.position - transform.position;
            algorithm.GetComponent<Algorithm>().speed = bulletSpeed;
            algorithm.GetComponent<Algorithm>().damage = bulletDamage;
            algorithm.GetComponent<Algorithm>().food = algorithmFood.foodSprite;
            algorithm.GetComponent<Algorithm>().boss = this.gameObject;
            algorithm.GetComponent<Algorithm>().sprite = GetComponent<SpriteRenderer>().sprite;

            yield return new WaitForSeconds(algorithmCoolTime);
        }
        yield return new WaitUntil(() => FindObjectOfType<Algorithm>() == null);
        yield return new WaitForSeconds(0.2f);

        audio.Stop();
        isAlgorithm = false;
        playerDamaged = false;
        isAttack = false;
        coolTime = attackCoolTime;
        StartCoroutine(EnemyMove());
    }

    private IEnumerator MachineGunPattern()
    {
        isAttack = true;
        anim.SetInteger("attacktype", 3);
        anim.SetTrigger("attack");
        yield return new WaitForSeconds(0.3f);

        audio.loop = true;
        audio.clip = attackSound[2];
        audio.volume = 1.0f;
        audio.pitch = 1.0f;
        audio.Play();

        rigidbody.velocity = Vector2.zero;

        for(int n = 0; n < 2; n++)
        {
            for (int i = 0; i < 4; i++)
            {
                int type = Random.Range(1, 5);
                float posX = Random.Range(minPos.x + 0.5f, maxPos.x - 0.5f);
                float posY = Random.Range(minPos.y + 0.5f, maxPos.y - 0.5f);
                Vector3 pos = Vector3.zero;
                Vector3 size = Vector3.zero;

                if (type == 1 || type == 2)
                {
                    pos = new Vector3(room.transform.position.x, posY, 0);
                    size = new Vector3(room.transform.localScale.x, 0.85f, 1);
                }
                else
                {
                    pos = new Vector3(posX, room.transform.position.y, 0);
                    size = new Vector3(0.85f, room.transform.localScale.y, 1);
                }


                GameObject bullet = Instantiate(riceBulletPrefab, pos, Quaternion.Euler(0, 0, 0), bulletParent.transform);
                bullet.transform.localScale = size;
                bullet.GetComponent<YoutuberComment>().type = type;
                bullet.GetComponent<YoutuberComment>().SetDamage(bulletDamage);
                bullet.GetComponent<YoutuberComment>().SetSprite(sprites);
            }

            yield return new WaitUntil(() => FindObjectOfType<YoutuberComment>() == null);
        }
        
        yield return new WaitForSeconds(0.3f);

        audio.Stop();
        isAttack = false;
        rigidbody.velocity = Vector2.zero;
        coolTime = attackCoolTime;
        StartCoroutine(EnemyMove());
    }

    private IEnumerator ChargePattern()
    {
        isAttack = true;
        line.enabled = true;
        anim.SetTrigger("attackend");
        Vector3 target = new Vector3(0, 0, 0);

        float time = 0;
        while (time < 1.5f)
        {
            Ray2D ray = new Ray2D(transform.position, player.transform.position - transform.position);

            line.SetPosition(0, transform.position);

            int mask = 1 << LayerMask.NameToLayer("RayWall") | 1 << LayerMask.NameToLayer("TileMap");
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 1000f, mask);
            if (hit)
            {
                line.SetPosition(1, hit.point);
                target = hit.point;
            }
            else
            {
                line.SetPosition(1, player.transform.position);
            }

            time += Time.deltaTime;
            yield return null;
        }

        isAttack = false;
        anim.SetInteger("attacktype", 4);
        anim.SetTrigger("attack");

        line.SetPosition(1, transform.position);
        line.enabled = false;

        audio.loop = false;
        audio.clip = attackSound[3];
        audio.volume = 1.0f;
        audio.pitch = 1.0f;
        audio.Play();

        if (target.x > transform.position.x)
        {
            GameObject dust1 = Instantiate(dashDust, new Vector3(transform.position.x - 0.27f, transform.position.y - 0.025f), Quaternion.Euler(0, 0, 0), effectParent.transform);
            GameObject dust2 = Instantiate(dashDust, new Vector3(transform.position.x - 0.58f, transform.position.y - 0.025f), Quaternion.Euler(0, 0, 0), effectParent.transform);
            dust1.GetComponent<SpriteRenderer>().flipX = true;
            dust2.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            Instantiate(dashDust, new Vector3(transform.position.x + 0.27f, transform.position.y - 0.025f), Quaternion.Euler(0, 0, 0), effectParent.transform);
            Instantiate(dashDust, new Vector3(transform.position.x + 0.58f, transform.position.y - 0.025f), Quaternion.Euler(0, 0, 0), effectParent.transform);
        }

        rigidbody.velocity = new Vector2(target.x - transform.position.x, target.y - transform.position.y).normalized * chargeSpeed;
        isCharge = true;

        yield return new WaitUntil(() => isTouch);

        isCharge = false;
        isTouch = false;
        rigidbody.velocity = Vector2.zero;

        yield return new WaitForSeconds(0.3f);
        isAttack = false;
        coolTime = attackCoolTime;
        StartCoroutine(EnemyMove());
    }

    private Vector3 AlgorithmPos()
    {
        int type = Random.Range(0, 2); // 0->Vertical 1->Horizontal
        float direction = Mathf.Sign(Random.Range(-1, 1)); // 0->(+) 1->(-)
        Vector3 result = new Vector3(0, 0, 0);
        


        float posX = Random.Range(-(room.transform.localScale.x / 2), room.transform.localScale.x / 2);
        float posY = Random.Range(-(room.transform.localScale.y / 2), room.transform.localScale.y / 2);
        if (type == 0)
        {
            result = new Vector3(room.transform.position.x + ((room.transform.localScale.x / 2) * direction), room.transform.position.y + posY, 0);
        }
        else
        {
            result = new Vector3(room.transform.position.x + posX, room.transform.position.y + ((room.transform.localScale.y / 2) * direction), 0);
        }

        return result;
    }

    public FoodData GetAlgorithmFood()
    {
        return algorithmFood;
    }

    public void PlayerAlgorithmDamage()
    {
        playerDamaged = true;
    }

    private void SetIncreaseByStage()
    {
        int stage = GameManager.gameManager.stage - 1;

        bossCon.SetMaxHp(500 + (stage * 400));
        bossCon.SetHp(500 + (stage * 400));

        explosionRadius += (stage / 2) * 0.5f;
        algorithmCoolTime = 0.35f - (stage * 0.05f);
        machineGunAmount = 20 + (stage * 2);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            if (isCharge)
            {
                isTouch = true;
            }
            else
            {
                rigidbody.velocity *= -1;
            }
        }

        if (collision.gameObject.tag == "Player")
        {
            if (isCharge)
            {
                collision.gameObject.GetComponent<PlayerHealth>().PlayerDamaged(bulletDamage, sprites);
            }
            else
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
