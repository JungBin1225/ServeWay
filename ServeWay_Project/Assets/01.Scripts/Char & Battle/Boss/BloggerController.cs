using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloggerController : MonoBehaviour
{
    private MissionManager mission;
    private Rigidbody2D rigidbody;
    private SpriteRenderer renderer;
    private BossController bossCon;
    private Animator anim;
    private GameObject player;
    private GameObject bulletParent;
    private GameObject effectParent;
    private GameObject summonObject;
    private DataController data;
    private Vector2 minPos;
    private Vector2 maxPos;
    private List<Sprite> sprites;
    private List<Vector3> commentPos;
    private float coolTime;
    private LineRenderer line;
    private GameObject laser;
    private AudioSource audio;
    private bool isAttack;
    private bool isPicture;
    private bool isLaser;
    private bool isComment;
    private bool playerCharge;
    private bool playerDamage;
    private float laserTime;
    private bool isLeft;

    public int test;
    public BossRoom room;
    public GameObject damageEffect;
    public GameObject commentPrefab;
    public GameObject pictureObject;
    public GameObject laserPrefab;
    public GameObject teleportPrefab;
    public GameObject dashDust;
    public List<AudioClip> attackSound;
    public PolygonCollider2D pictureCollider;
    public Animator pictureAnim;
    public float speed;
    public float attackCoolTime;
    public float chargeSpeed;
    public float bulletSpeed;
    public float bulletDamage;
    public float commentDamage;
    public float pictureDamage;
    public float attackRange;
    public Food_Nation nation;
    public Boss_Job job;

    void Start()
    {
        mission = FindObjectOfType<MissionManager>();
        rigidbody = GetComponent<Rigidbody2D>();
        bossCon = GetComponent<BossController>();
        renderer = GetComponent<SpriteRenderer>();
        line = GetComponent<LineRenderer>();
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        data = FindObjectOfType<DataController>();
        player = GameObject.FindGameObjectWithTag("Player");
        bulletParent = GameObject.Find("BulletList");
        effectParent = GameObject.Find("EffectList");
        summonObject = GameObject.Find("SummonList");
        commentPos = new List<Vector3>();
        sprites = new List<Sprite>();
        sprites.Add(data.FindBossSprite(Boss_Job.BLOGGER));

        bossCon.nation = this.nation;
        bossCon.room = this.room;
        bossCon.job = this.job;
        SetIncreaseByStage();
        //GameManager.gameManager.mission.boss = this.gameObject;

        coolTime = attackCoolTime;
        line.enabled = false;
        isAttack = false;
        isPicture = false;
        isLaser = false;
        isComment = false;
        playerCharge = false;
        playerDamage = false;
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

        if(isComment)
        {
            if(player.GetComponent<PlayerController>().isCharge)
            {
                playerCharge = true;
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
                StartCoroutine(picturePattern());
                break;
            case 1:
                StartCoroutine(CommentPattern());
                break;
            case 2:
                StartCoroutine(LaserPattern());
                break;
            case 3:
                StartCoroutine(TeleportPattern());
                break;
        }
    }

    private IEnumerator CommentPattern()
    {
        isAttack = true;
        commentPos.Clear();
        RandomPos(3);

        anim.SetInteger("attacktype", 3);
        anim.SetTrigger("attack");

        audio.loop = false;
        audio.clip = attackSound[0];
        audio.volume = 1.0f;
        audio.pitch = 1.0f;
        audio.Play();

        yield return new WaitForSeconds(0.35f);

        foreach(Vector3 pos in commentPos)
        {
            float rot = Random.Range(0, 360);
            GameObject comment = Instantiate(commentPrefab, pos, Quaternion.Euler(0, 0, rot), summonObject.transform);
            comment.GetComponent<Comment>().damage = commentDamage;
            comment.GetComponent<Comment>().speed = bulletSpeed;
            comment.GetComponent<Comment>().sprite = GetComponent<SpriteRenderer>().sprite;
            yield return new WaitForSeconds(0.2f);
        }

        isComment = true;

        yield return new WaitUntil(() => FindObjectOfType<Comment>() == null);
        isComment = false;
        if(!playerCharge && !playerDamage)
        {
            mission.OccurreEvent(12, 1);
        }
        playerCharge = false;
        playerDamage = false;
        yield return new WaitForSeconds(0.2f);

        isAttack = false;
        coolTime = attackCoolTime;
        audio.Stop();
        StartCoroutine(EnemyMove());
    }

    private IEnumerator picturePattern()
    {
        isAttack = true;
        pictureObject.transform.GetChild(0).gameObject.SetActive(true);
        pictureCollider.enabled = false;

        Vector3 target = player.transform.position;

        Vector2 dir = new Vector2(transform.position.x - target.x, transform.position.y - target.y);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion angleAxis = Quaternion.AngleAxis(angle - 45, Vector3.forward);
        pictureObject.GetComponent<RectTransform>().rotation = angleAxis;

        anim.SetTrigger("walk");

        audio.loop = false;
        audio.clip = attackSound[4];
        audio.volume = 1.0f;
        audio.pitch = 1.0f;
        audio.Play();

        if (target.x > transform.position.x)
        {
            GameObject dust = Instantiate(dashDust, new Vector3(transform.position.x - 0.74f, transform.position.y + 0.13f), Quaternion.Euler(0, 0, 0), effectParent.transform);
            dust.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            Instantiate(dashDust, new Vector3(transform.position.x + 0.74f, transform.position.y + 0.13f), Quaternion.Euler(0, 0, 0), effectParent.transform);
        }

        while (Vector3.Distance(target, transform.position) > 2)
        {
            rigidbody.velocity = new Vector2(target.x - transform.position.x, target.y - transform.position.y).normalized * 7;
            yield return null;
        }

        anim.SetInteger("attacktype", 1);
        anim.SetTrigger("attack");

        rigidbody.velocity = new Vector2(0, 0);
        pictureCollider.enabled = true;
        pictureAnim.SetTrigger("picture");

        audio.loop = false;
        audio.clip = attackSound[1];
        audio.volume = 1.0f;
        audio.pitch = 1.0f;
        audio.Play();

        isPicture = true;

        yield return new WaitForSeconds(0.2f);

        pictureObject.transform.GetChild(0).gameObject.SetActive(false);
        pictureCollider.enabled = false;
        isAttack = false;
        isPicture = false;
        coolTime = attackCoolTime;
        StartCoroutine(EnemyMove());
    }

    private IEnumerator LaserPattern()
    {
        isLaser = true;
        anim.SetTrigger("attackend");

        audio.loop = true;
        audio.clip = attackSound[2];
        audio.volume = 1.0f;
        audio.pitch = 1.0f;
        audio.Play();

        line.enabled = true;
        Vector3 target = player.transform.position;
        Ray2D ray = new Ray2D(transform.position, target - transform.position);

        line.SetPosition(0, transform.position);

        int mask = 1 << LayerMask.NameToLayer("RayWall") | 1 << LayerMask.NameToLayer("TileMap");
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 1000f, mask);
        if (hit)
        {
            line.SetPosition(1, hit.point);
        }
        else
        {
            line.SetPosition(1, target);
        }

        Vector3 start = line.GetPosition(0);
        Vector3 end = line.GetPosition(1);
        Vector3 end_temp = start + ((end - start).normalized * 0.25f);

        yield return new WaitForSeconds(1f);

        anim.SetInteger("attacktype", 2);
        anim.SetTrigger("attack");

        laser = Instantiate(laserPrefab, this.transform);

        laser.GetComponent<EnemyLaser>().SetDamage(bulletDamage);
        laser.GetComponent<EnemyLaser>().SetCoolTime(0.2f);
        laser.GetComponent<EnemyLaser>().SetSprite(sprites);

        while(Vector3.Distance(start, end_temp) < Vector3.Distance(start, end))
        {
            laser.transform.localScale = new Vector3(Vector3.Distance(start, end_temp) * 0.5f, line.startWidth * 0.5f, 0);
            laser.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta = laser.transform.localScale;
            laser.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(1 / laser.transform.localScale.x, 1 / laser.transform.localScale.y, 1);
            Vector3 pos = (start + end_temp) / 2;
            Vector2 dir = new Vector2(pos.x - end_temp.x, pos.y - end_temp.y);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion angleAxis = Quaternion.AngleAxis(angle, Vector3.forward);
            laser.transform.rotation = angleAxis;
            laser.transform.position = pos;

            end_temp += ((end - start).normalized * 0.25f);
            yield return null;
        }

        laser.transform.localScale = new Vector3(Vector3.Distance(start, end) * 0.5f, line.startWidth * 0.5f, 0);
        laser.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta = laser.transform.localScale;
        laser.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().localScale = new Vector3(1 / laser.transform.localScale.x, 1 / laser.transform.localScale.y, 1);
        Vector3 pos2 = (start + end) / 2;
        Vector2 dir2 = new Vector2(pos2.x - end.x, pos2.y - end.y);
        float angle2 = Mathf.Atan2(dir2.y, dir2.x) * Mathf.Rad2Deg;
        Quaternion angleAxis2 = Quaternion.AngleAxis(angle2, Vector3.forward);
        laser.transform.rotation = angleAxis2;
        laser.transform.position = pos2;

        yield return new WaitForSeconds(laserTime);
        isLaser = false;
        isAttack = false;
        audio.Stop();
        coolTime = attackCoolTime / 2;
        
        line.SetPosition(1, transform.position);
        line.enabled = false;
        Destroy(laser);

        StartCoroutine(EnemyMove());
    }

    private IEnumerator TeleportPattern()
    {
        isAttack = true;
        GameObject tel1 = Instantiate(teleportPrefab, transform.position, Quaternion.Euler(0, 0, 0), summonObject.transform);

        anim.SetInteger("attacktype", 3);
        anim.SetTrigger("attack");

        audio.loop = false;
        audio.clip = attackSound[3];
        audio.volume = 0.5f;
        audio.pitch = 0.7f;
        audio.Play();

        yield return new WaitForSeconds(1f);

        Vector3 target = player.transform.position;
        float posX = Random.Range(-1.0f, 1.0f);
        float posY = Random.Range(-1.0f, 1.0f);
        if(posX < 0)
        {
            posX = target.x - 0.5f + posX;
        }
        else
        {
            posX = target.x + 0.5f + posX;
        }

        if (posY < 0)
        {
            posY = target.y - 0.5f + posY;
        }
        else
        {
            posY = target.y + 0.5f + posY;
        }

        GameObject tel2 = Instantiate(teleportPrefab, new Vector3(posX, posY, 0), Quaternion.Euler(0, 0, 0), summonObject.transform);
        yield return new WaitForSeconds(0.5f);

        audio.Play();

        transform.position = new Vector3(posX, posY, 0);
        isAttack = false;

        Destroy(tel1);
        Destroy(tel2);
        StartCoroutine(picturePattern());
    }

    private void RandomPos(int amount)
    {
        if (commentPos.Count < amount)
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

    public void PlayerCommentDamage()
    {
        playerDamage = true;
    }

    private void SetIncreaseByStage()
    {
        int stage = GameManager.gameManager.stage - 1;

        bossCon.SetMaxHp(500 + (stage * 400));
        bossCon.SetHp(500 + (stage * 400));

        laserTime = 2f + (stage * 0.3f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && isPicture)
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

        if (collision.gameObject.tag == "Player")
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (isAttack)
            {
                rigidbody.velocity = Vector2.zero;
            }
        }
    }
}
