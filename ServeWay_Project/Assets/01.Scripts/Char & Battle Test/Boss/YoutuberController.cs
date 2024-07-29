using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoutuberController : MonoBehaviour
{
    private MissonManager misson;
    private Rigidbody2D rigidbody;
    private BossController bossCon;
    private GameObject player;
    private Vector2 minPos;
    private Vector2 maxPos;
    private LineRenderer line;
    private float coolTime;
    private bool isAttack;
    private bool isCharge;
    private bool isTouch;

    public int test;
    public BossRoom room;
    public GameObject damageEffect;
    public GameObject bulletPrefab;
    public GameObject explosionPrefab;
    public GameObject algorithmPrefab;
    public float hp;
    public float speed;
    public float chargeSpeed;
    public float attackCoolTime;
    public float bulletSpeed;
    public float bulletDamage;
    public float explosionSpeed;
    public float explosionDamage;
    public float explosionRadius;
    public bool isAlgorithm;
    public Food_Nation nation;
    public Boss_Job job;

    void Start()
    {
        misson = FindObjectOfType<MissonManager>();
        rigidbody = GetComponent<Rigidbody2D>();
        bossCon = GetComponent<BossController>();
        line = GetComponent<LineRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");

        bossCon.nation = this.nation;
        bossCon.room = this.room;
        bossCon.job = this.job;
        bossCon.SetHp(hp);
        //GameManager.gameManager.mission.boss = this.gameObject;

        coolTime = attackCoolTime;
        line.enabled = false;
        isAttack = false;
        isAlgorithm = false;
        isCharge = false;
        isTouch = false;

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
                StartCoroutine(ExplosionPattern());
                break;
            case 1:
                StartCoroutine(AlgorithmPattern());
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
        yield return new WaitForSeconds(0.3f);

        GameObject explosionBullet = Instantiate(explosionPrefab, transform.position, transform.rotation);
        var breadBullet = explosionBullet.GetComponent<EnemyExplosionBullet>();
        breadBullet.SetTarget(transform.position - player.transform.position);
        breadBullet.SetSpeed(explosionSpeed);
        breadBullet.SetDamage(explosionDamage);
        breadBullet.SetRadius(explosionRadius);

        yield return new WaitForSeconds(0.3f);

        isAttack = false;
        coolTime = attackCoolTime;
        StartCoroutine(EnemyMove());
    }

    private IEnumerator AlgorithmPattern()
    {
        isAttack = true;
        yield return new WaitForSeconds(0.1f);

        Vector3 target = room.transform.position;
        while (Vector3.Distance(target, transform.position) > 0.5f)
        {
            rigidbody.velocity = new Vector2(target.x - transform.position.x, target.y - transform.position.y).normalized * 10;
            yield return null;
        }
        rigidbody.velocity = new Vector2(0, 0);
        yield return new WaitForSeconds(0.2f);

        isAlgorithm = true;
        for(int i = 0; i < 40; i++)
        {
            GameObject algorithm = Instantiate(algorithmPrefab, AlgorithmPos(), Quaternion.Euler(0, 0, 0));

            Vector2 dir = new Vector2(algorithm.transform.position.x - transform.position.x, algorithm.transform.position.y - transform.position.y);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion angleAxis = Quaternion.AngleAxis(angle, Vector3.forward);
            algorithm.transform.rotation = angleAxis;

            algorithm.GetComponent<Algorithm>().target = algorithm.transform.position - transform.position;
            algorithm.GetComponent<Algorithm>().speed = bulletSpeed;
            algorithm.GetComponent<Algorithm>().damage = bulletDamage;

            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(1.5f);

        isAlgorithm = false;
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

    private IEnumerator ChargePattern()
    {
        isAttack = true;
        line.enabled = true;
        Vector3 target = new Vector3(0, 0, 0);

        float time = 0;
        while (time < 1.5f)
        {
            Ray2D ray = new Ray2D(transform.position, player.transform.position - transform.position);

            line.SetPosition(0, transform.position);

            int mask = 1 << LayerMask.NameToLayer("RayWall");
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

        line.SetPosition(1, transform.position);
        line.enabled = false;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            if(isCharge)
            {
                isTouch = true;
            }
            else
            {
                rigidbody.velocity *= -1;
            }
        }

        if(collision.gameObject.tag == "Player" && isCharge)
        {
            collision.gameObject.GetComponent<PlayerHealth>().PlayerDamaged(bulletDamage);
        }
    }
}
