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
    private bool isAttack;

    public int test;
    public BossRoom room;
    public GameObject damageEffect;
    public GameObject bulletPrefab;
    public GameObject commentPrefab;
    public float hp;
    public float speed;
    public float attackCoolTime;
    public float chargeSpeed;
    public float bulletSpeed;
    public float bulletDamage;
    public float commentDamage;
    public Food_Nation nation;
    public Boss_Job job;

    void Start()
    {
        misson = FindObjectOfType<MissonManager>();
        rigidbody = GetComponent<Rigidbody2D>();
        bossCon = GetComponent<BossController>();
        player = GameObject.FindGameObjectWithTag("Player");
        commentPos = new List<Vector3>();

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
                StartCoroutine(CommentPattern());
                break;
            case 1:

                break;
            case 2:

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
}
