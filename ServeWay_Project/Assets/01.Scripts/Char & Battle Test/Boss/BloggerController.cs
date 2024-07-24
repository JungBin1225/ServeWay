using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloggerController : MonoBehaviour
{
    private MissonManager misson;
    private Rigidbody2D rigidbody;
    private BossController bossCon;
    private GameObject player;
    private Vector2 minPos;
    private Vector2 maxPos;
    private List<Vector3> commentPos;
    private float coolTime;
    private LineRenderer line;
    private GameObject laser;
    private bool isAttack;
    private bool isLaser;

    public int test;
    public BossRoom room;
    public GameObject damageEffect;
    public GameObject bulletPrefab;
    public GameObject commentPrefab;
    public GameObject pictureObject;
    public GameObject laserPrefab;
    public PolygonCollider2D pictureCollider;
    public float hp;
    public float speed;
    public float attackCoolTime;
    public float chargeSpeed;
    public float bulletSpeed;
    public float bulletDamage;
    public float commentDamage;
    public float pictureDamage;
    public Food_Nation nation;
    public Boss_Job job;

    void Start()
    {
        misson = FindObjectOfType<MissonManager>();
        rigidbody = GetComponent<Rigidbody2D>();
        bossCon = GetComponent<BossController>();
        line = GetComponent<LineRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        commentPos = new List<Vector3>();

        bossCon.nation = this.nation;
        bossCon.room = this.room;
        bossCon.SetHp(hp);
        //GameManager.gameManager.mission.boss = this.gameObject;

        coolTime = attackCoolTime;
        line.enabled = false;
        isAttack = false;
        isLaser = false;

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

        if(index == 2 && isLaser)
        {
            index = Random.Range(0, 2);
        }

        switch (index)
        {
            case 0:
                StartCoroutine(CommentPattern());
                break;
            case 1:
                StartCoroutine(picturePattern());
                break;
            case 2:
                StartCoroutine(LaserPattern());
                break;
            case 3:

                break;
        }
    }

    private IEnumerator CommentPattern()
    {
        isAttack = true;
        commentPos.Clear();
        RandomPos(3);

        yield return new WaitForSeconds(0.35f);

        foreach(Vector3 pos in commentPos)
        {
            float rot = Random.Range(0, 360);
            GameObject comment = Instantiate(commentPrefab, pos, Quaternion.Euler(0, 0, rot));
            comment.GetComponent<Comment>().damage = commentDamage;
            yield return null;
        }
        yield return new WaitForSeconds(0.5f);

        isAttack = false;
        coolTime = attackCoolTime * 1.5f;
        StartCoroutine(EnemyMove());
    }

    private IEnumerator picturePattern()
    {
        isAttack = true;
        pictureObject.SetActive(true);
        pictureCollider.enabled = false;

        Vector3 target = player.transform.position;

        Vector2 dir = new Vector2(transform.position.x - target.x, transform.position.y - target.y);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion angleAxis = Quaternion.AngleAxis(angle - 45, Vector3.forward);
        pictureObject.GetComponent<RectTransform>().rotation = angleAxis;

        while (Vector3.Distance(target, transform.position) > 2)
        {
            rigidbody.velocity = new Vector2(target.x - transform.position.x, target.y - transform.position.y).normalized * 10;
            yield return null;
        }

        rigidbody.velocity = new Vector2(0, 0);
        pictureCollider.enabled = true;
        yield return new WaitForSeconds(0.2f);

        pictureObject.SetActive(false);
        pictureCollider.enabled = false;
        isAttack = false;
        coolTime = attackCoolTime;
        StartCoroutine(EnemyMove());
    }

    private IEnumerator LaserPattern()
    {
        isAttack = true;
        isLaser = true;
        yield return new WaitForSeconds(0.2f);

        line.enabled = true;
        laser = Instantiate(laserPrefab, this.transform);

        laser.GetComponent<EnemyLaser>().SetDamage(bulletDamage);
        laser.GetComponent<EnemyLaser>().SetCoolTime(0.5f);

        Vector3 target = player.transform.position;
        Ray2D ray = new Ray2D(transform.position, target - transform.position);

        line.SetPosition(0, transform.position);

        int mask = 1 << LayerMask.NameToLayer("RayWall");
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 1000f, mask);
        if (hit)
        {
            line.SetPosition(1, hit.point);
            Debug.Log(hit.point);
        }
        else
        {
            line.SetPosition(1, target);
        }

        Vector3 start = line.GetPosition(0);
        Vector3 end = line.GetPosition(1);

        laser.transform.localScale = new Vector3(Vector3.Distance(start, end) * 0.5f, line.startWidth * 0.5f, 0);
        Vector3 pos = (start + end) / 2;
        Vector2 dir = new Vector2(pos.x - end.x, pos.y - end.y);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion angleAxis = Quaternion.AngleAxis(angle, Vector3.forward);
        laser.transform.rotation = angleAxis;
        laser.transform.position = pos;

        yield return new WaitForSeconds(0.5f);
        isAttack = false;
        coolTime = attackCoolTime / 2;
        StartCoroutine(EnemyMove());

        yield return new WaitForSeconds(3f);
        isLaser = false;
        line.SetPosition(1, transform.position);
        line.enabled = false;
        Destroy(laser);
    }

    private void RandomPos(int amount)
    {
        if (commentPos.Count <= amount)
        {
            float posX = Random.Range(-(room.transform.localScale.x / 2) + 1, (room.transform.localScale.x / 2) - 1);
            float posY = Random.Range(-(room.transform.localScale.y / 2) + 1, (room.transform.localScale.y / 2) - 1);
            Vector3 target = new Vector3(room.transform.position.x + posX, room.transform.position.y + posY, 0);
            Vector3 prevTarget = transform.position;
            if (commentPos.Count != 0)
            {
                prevTarget = commentPos[commentPos.Count - 1];
            }

            if (Vector3.Distance(target, prevTarget) < 3)
            {
                RandomPos(amount);
            }
            else
            {
                commentPos.Add(target);
                RandomPos(amount);
            }
        }
        else
        {
            return;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            rigidbody.velocity *= -1;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealth>().PlayerDamaged(pictureDamage);
        }
    }
}
