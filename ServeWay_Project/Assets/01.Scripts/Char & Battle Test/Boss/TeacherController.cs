using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherController : MonoBehaviour
{
    private MissonManager misson;
    private Rigidbody2D rigidbody;
    private BossController bossCon;
    private DataController dataController;
    private GameObject player;
    private SpriteRenderer charSprite;
    private List<FoodData> playerFood;
    private Vector2 minPos;
    private Vector2 maxPos;
    private float coolTime;
    private LineRenderer line;
    private GameObject laser;
    private bool isAttack;
    private bool isLaser;

    public int test;
    public BossRoom room;
    public GameObject damageEffect;
    public SpriteRenderer weaponObject;
    public GameObject bulletPrefab;
    public GameObject laserPrefab;
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
        dataController = FindObjectOfType<DataController>();
        rigidbody = GetComponent<Rigidbody2D>();
        bossCon = GetComponent<BossController>();
        line = GetComponent<LineRenderer>();
        charSprite = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");

        bossCon.nation = this.nation;
        bossCon.room = this.room;
        bossCon.job = this.job;
        bossCon.SetHp(hp);
        //GameManager.gameManager.mission.boss = this.gameObject;

        playerFood = new List<FoodData>();
        List<string> foodList = player.GetComponent<PlayerController>().weaponSlot.ReturnWeaponList();
        foreach(string food in foodList)
        {
            playerFood.Add(dataController.FindFood(food));
        }

        coolTime = attackCoolTime;
        line.enabled = false;
        isAttack = false;
        isLaser = false;

        minPos = new Vector2(room.transform.position.x - (room.GetComponent<BoxCollider2D>().size.x / 2), room.transform.position.y - (room.GetComponent<BoxCollider2D>().size.y / 2));
        maxPos = new Vector2(room.transform.position.x + (room.GetComponent<BoxCollider2D>().size.x / 2), room.transform.position.y + (room.GetComponent<BoxCollider2D>().size.y / 2));

        weaponObject.sprite = playerFood[0].foodSprite;

        StartCoroutine(EnemyMove());
    }

    void Update()
    {
        if (coolTime > 0)
        {
            coolTime -= Time.deltaTime;
        }

        Vector2 direction = player.transform.position - transform.position;
        weaponObject.gameObject.transform.parent.rotation = Quaternion.FromToRotation(Vector3.up, direction);

        if (transform.position.y - player.transform.position.y < 0)
        {
            weaponObject.sortingOrder = charSprite.sortingOrder - 1;
        }
        else
        {
            weaponObject.sortingOrder = charSprite.sortingOrder + 1;
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

        if (index == 0 && isLaser)
        {
            index = Random.Range(1, 4);
        }

        switch (index)
        {
            case 0:
                StartCoroutine(LaserPattern());
                break;
            case 1:
                StartCoroutine(ShotGunPattern());
                break;
            case 2:
                
                break;
            case 3:
                
                break;
        }
    }

    private IEnumerator LaserPattern()
    {
        isAttack = true;
        isLaser = true;
        SetSprite(Food_MainIngred.NOODLE);
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

        yield return new WaitForSeconds(5f);
        isLaser = false;
        line.SetPosition(1, transform.position);
        line.enabled = false;
        Destroy(laser);
    }

    private IEnumerator ShotGunPattern()
    {
        isAttack = true;
        yield return new WaitForSeconds(0.35f);

        float radius = 10;
        float bulletAmount = 6;
        for (int j = 0; j < 6; j++)
        {
            float startAngle = (radius * 10) / 2;
            float differAngle = (radius * 10) / (bulletAmount - 1);

            for (int i = 0; i < bulletAmount; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.Euler(weaponObject.gameObject.transform.parent.rotation.eulerAngles + new Vector3(0, 0, startAngle - (differAngle * i))));
                bullet.GetComponent<EnemyBullet>().SetTarget(-bullet.transform.up);
                bullet.GetComponent<EnemyBullet>().SetSpeed(bulletSpeed);
                bullet.GetComponent<EnemyBullet>().SetDamage(bulletDamage);
            }
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(0.3f);

        isAttack = false;
        coolTime = attackCoolTime;
        StartCoroutine(EnemyMove());
    }

    private void SetSprite(Food_MainIngred ingred)
    {
        foreach(FoodData food in playerFood)
        {
            if(food.mainIngred == ingred)
            {
                weaponObject.sprite = food.foodSprite;
            }
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
