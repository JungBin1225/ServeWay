using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherController : MonoBehaviour
{
    private MissionManager mission;
    private Rigidbody2D rigidbody;
    private BossController bossCon;
    private DataController dataController;
    private GameObject player;
    private SpriteRenderer charSprite;
    private List<FoodData> playerFood;
    private Vector2 minPos;
    private Vector2 maxPos;
    private List<Sprite> sprites;
    private float coolTime;
    private LineRenderer line;
    private GameObject laser;
    private bool isAttack;
    private bool isLaser;
    private int counterAmount;
    private bool playerDamaged;
    private FoodData nowFood;

    public int test;
    public BossRoom room;
    public GameObject damageEffect;
    public SpriteRenderer weaponObject;
    public GameObject bulletPrefab;
    public GameObject laserPrefab;
    public GameObject explosionPrefab;
    public float hp;
    public float speed;
    public float attackCoolTime;
    public float bulletSpeed;
    public float bulletDamage;
    public float explosionSpeed;
    public float explosionDamage;
    public float explosionRadius;
    public float counterDamage;
    public bool isCounter;
    public Food_Nation nation;
    public Boss_Job job;

    void Start()
    {
        mission = FindObjectOfType<MissionManager>();
        dataController = FindObjectOfType<DataController>();
        rigidbody = GetComponent<Rigidbody2D>();
        bossCon = GetComponent<BossController>();
        line = GetComponent<LineRenderer>();
        charSprite = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
        sprites = new List<Sprite>();
        sprites.Add(gameObject.GetComponent<SpriteRenderer>().sprite);

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
        counterAmount = 0;
        line.enabled = false;
        isCounter = false;
        isAttack = false;
        isLaser = false;
        playerDamaged = false;

        minPos = new Vector2(room.transform.position.x - (room.GetComponent<BoxCollider2D>().size.x / 2), room.transform.position.y - (room.GetComponent<BoxCollider2D>().size.y / 2));
        maxPos = new Vector2(room.transform.position.x + (room.GetComponent<BoxCollider2D>().size.x / 2), room.transform.position.y + (room.GetComponent<BoxCollider2D>().size.y / 2));

        weaponObject.sprite = playerFood[0].foodSprite;
        nowFood = playerFood[0];

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
                StartCoroutine(ExplosionPattern());
                break;
            case 3:
                StartCoroutine(CounterPattern());
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
        laser.GetComponent<EnemyLaser>().SetSprite(sprites);

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
        SetSprite(Food_MainIngred.SOUP);
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
                bullet.GetComponent<EnemyBullet>().SetSprite(sprites);
                bullet.GetComponent<EnemyBullet>().SetColor(nowFood.bulletColor);
            }
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(0.3f);

        isAttack = false;
        coolTime = attackCoolTime;
        StartCoroutine(EnemyMove());
    }

    private IEnumerator ExplosionPattern()
    {
        isAttack = true;
        SetSprite(Food_MainIngred.BREAD);
        yield return new WaitForSeconds(0.3f);
        
        for(int i = 0; i < 6; i++)
        {
            float posX = Random.Range(-(room.transform.localScale.x / 2), (room.transform.localScale.x / 2));
            float posY = Random.Range(-(room.transform.localScale.y / 2), (room.transform.localScale.y / 2));
            Vector3 target = new Vector3(room.transform.position.x + posX, room.transform.position.y + posY, 0);
            if(i == 5)
            {
                target = player.transform.position;
            }


            GameObject explosionBullet = Instantiate(explosionPrefab, transform.position, transform.rotation);
            var breadBullet = explosionBullet.GetComponent<EnemyExplosionBullet>();
            breadBullet.SetTarget(transform.position - target);
            breadBullet.SetSpeed(explosionSpeed);
            breadBullet.SetDamage(explosionDamage);
            breadBullet.SetRadius(explosionRadius);
            breadBullet.SetSprite(sprites);

            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(0.4f);
        isAttack = false;
        coolTime = attackCoolTime;
        StartCoroutine(EnemyMove());
    }

    private IEnumerator CounterPattern()
    {
        isAttack = true;
        yield return new WaitForSeconds(0.2f);

        float time = 0;
        float colorTime = 0;
        isCounter = true;
        while(time < 5)
        {
            if(counterAmount >= 4)
            {
                //effect
                player.GetComponent<PlayerHealth>().PlayerDamaged(counterDamage, sprites);
                playerDamaged = true;
                break;
            }

            if(colorTime % 1 > 0.5f)
            {
                charSprite.color = new Color(1, 1, 1);
            }
            else
            {
                charSprite.color = new Color(1, 0, 0);
            }

            time += Time.deltaTime;
            colorTime += Time.deltaTime;
            yield return null;
        }

        charSprite.color = new Color(1, 1, 1);
        isCounter = false;
        counterAmount = 0;

        if(!playerDamaged)
        {
            mission.OccurreEvent(14, 1);
        }
        playerDamaged = false;
        yield return new WaitForSeconds(0.3f);
        isAttack = false;
        coolTime = attackCoolTime;
        StartCoroutine(EnemyMove());
    }

    public void AddAmount()
    {
        counterAmount++;
    }

    private void SetSprite(Food_MainIngred ingred)
    {
        foreach(FoodData food in playerFood)
        {
            if(food.mainIngred == ingred)
            {
                weaponObject.sprite = food.foodSprite;
                nowFood = food;
                return;
            }
        }

        foreach(FoodData food in dataController.foodData.FoodDatas)
        {
            if (food.mainIngred == ingred)
            {
                weaponObject.sprite = food.foodSprite;
                nowFood = food;
                return;
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
