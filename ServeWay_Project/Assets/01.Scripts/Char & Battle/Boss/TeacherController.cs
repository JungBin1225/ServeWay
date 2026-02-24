using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeacherController : MonoBehaviour
{
    private MissionManager mission;
    private Rigidbody2D rigidbody;
    private BossController bossCon;
    private DataController data;
    private GameObject player;
    private GameObject bulletParent;
    private GameObject effectParent;
    private GameObject summonObject;
    private SpriteRenderer charSprite;
    private List<FoodData> playerFood;
    private AudioSource audio;
    private Vector2 minPos;
    private Vector2 maxPos;
    private List<Sprite> sprites;
    private float coolTime;
    private GameObject laser_1;
    private GameObject laser_2;
    private GameObject laser_3;
    private bool isAttack;
    private bool isLaser;
    private int counterAmount;
    private bool playerDamaged;
    private FoodData nowFood;
    private Color32 bulletColor;
    private bool isLeft;

    public int test;
    public BossRoom room;
    public GameObject damageEffect;
    public SpriteRenderer weaponObject;
    public GameObject bulletPrefab;
    public GameObject laserPrefab;
    public GameObject explosionPrefab;
    public GameObject counterEffect;
    public GameObject testPaper;
    public GameObject scoreEffect;
    public GameObject wrongSound;
    public List<LineRenderer> lines;
    public List<AudioClip> attackSound;
    public float speed;
    public float attackCoolTime;
    public float bulletSpeed;
    public float bulletDamage;
    public float explosionSpeed;
    public float explosionDamage;
    public float explosionRadius;
    public float counterDamage;
    public float attackRange;
    public bool isCounter;
    public Food_Nation nation;
    public Boss_Job job;

    void Start()
    {
        mission = FindObjectOfType<MissionManager>();
        data = FindObjectOfType<DataController>();
        rigidbody = GetComponent<Rigidbody2D>();
        bossCon = GetComponent<BossController>();
        charSprite = GetComponent<SpriteRenderer>();
        audio = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        bulletParent = GameObject.Find("BulletList");
        effectParent = GameObject.Find("EffectList");
        summonObject = GameObject.Find("SummonList");
        sprites = new List<Sprite>();
        sprites.Add(gameObject.GetComponent<SpriteRenderer>().sprite);

        bossCon.nation = this.nation;
        bossCon.room = this.room;
        bossCon.job = this.job;
        SetIncreaseByStage();
        //GameManager.gameManager.mission.boss = this.gameObject;

        playerFood = new List<FoodData>();
        List<string> foodList = player.GetComponent<PlayerController>().weaponSlot.ReturnWeaponList();
        foreach(string food in foodList)
        {
            playerFood.Add(data.FindFood(food));
        }

        coolTime = attackCoolTime;
        counterAmount = 0;
        isCounter = false;
        isAttack = false;
        isLaser = false;
        isLeft = true;
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

        if(!isLaser)
        {
            Vector2 direction = player.transform.position - transform.position;
            weaponObject.gameObject.transform.parent.rotation = Quaternion.FromToRotation(Vector3.up, direction);
        }

        if (transform.position.y - player.transform.position.y < 0)
        {
            weaponObject.sortingOrder = charSprite.sortingOrder - 1;
        }
        else
        {
            weaponObject.sortingOrder = charSprite.sortingOrder + 1;
        }

        if (rigidbody.velocity.x < 0)
        {
            isLeft = true;
        }
        else if (rigidbody.velocity.x > 0)
        {
            isLeft = false;
        }
        if (isLeft)
        {
            charSprite.flipX = false;
        }
        else
        {
            charSprite.flipX = true;
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
        int index = Random.Range(0, 4);

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

        for(int i = 0; i < 3; i++)
        {
            lines[i].enabled = true;
            lines[i].gameObject.GetComponent<Animator>().SetBool("red", true);
            lines[i].startColor = new Color(1, 0, 0);
            lines[i].endColor = new Color(1, 0, 0);
        }

        Vector3 target = player.transform.position;
        Ray2D ray = new Ray2D(transform.position, target - transform.position);

        lines[0].SetPosition(0, transform.position);

        int mask = 1 << LayerMask.NameToLayer("RayWall") | 1 << LayerMask.NameToLayer("TileMap");
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 1000f, mask);
        if (hit)
        {
            lines[0].SetPosition(1, hit.point);
            lines[1].SetPosition(0, hit.point);
        }

        Vector2 inDirection = (hit.point - (Vector2)transform.position).normalized;
        Vector2 reflectionDir = Vector2.Reflect(inDirection, hit.normal);

        RaycastHit2D hit2 = Physics2D.Raycast(hit.point + (reflectionDir * 0.001f), reflectionDir, 1000f, mask);

        lines[1].SetPosition(1, hit2.point);
        lines[2].SetPosition(0, hit2.point);

        Ray2D ray2 = new Ray2D(hit2.point, target - (Vector3)hit2.point);

        RaycastHit2D hit3 = Physics2D.Raycast(ray2.origin + (ray2.direction * 0.001f), ray2.direction, 1000f, mask);

        lines[2].SetPosition(1, hit3.point);

        yield return new WaitForSeconds(0.75f);

        for (int i = 0; i < 3; i++)
        {
            lines[i].gameObject.GetComponent<Animator>().SetBool("red", false);
            lines[i].startColor = bulletColor;
            lines[i].endColor = bulletColor;
        }

        laser_1 = Instantiate(laserPrefab, this.transform);
        laser_2 = Instantiate(laserPrefab, this.transform);
        laser_3 = Instantiate(laserPrefab, this.transform);

        laser_1.GetComponent<EnemyLaser>().SetDamage(bulletDamage);
        laser_1.GetComponent<EnemyLaser>().SetCoolTime(0.2f);
        laser_1.GetComponent<EnemyLaser>().SetSprite(sprites);

        laser_2.GetComponent<EnemyLaser>().SetDamage(bulletDamage);
        laser_2.GetComponent<EnemyLaser>().SetCoolTime(0.2f);
        laser_2.GetComponent<EnemyLaser>().SetSprite(sprites);

        laser_3.GetComponent<EnemyLaser>().SetDamage(bulletDamage);
        laser_3.GetComponent<EnemyLaser>().SetCoolTime(0.2f);
        laser_3.GetComponent<EnemyLaser>().SetSprite(sprites);


        Vector3 start = lines[0].GetPosition(0);
        Vector3 end = lines[0].GetPosition(1);

        laser_1.transform.localScale = new Vector3(Vector3.Distance(start, end), lines[0].startWidth, 0);
        Vector3 pos = (start + end) / 2;
        Vector2 dir = new Vector2(pos.x - end.x, pos.y - end.y);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion angleAxis = Quaternion.AngleAxis(angle, Vector3.forward);
        laser_1.transform.rotation = angleAxis;
        laser_1.transform.position = pos;

        start = lines[1].GetPosition(0);
        end = lines[1].GetPosition(1);

        laser_2.transform.localScale = new Vector3(Vector3.Distance(start, end), lines[1].startWidth, 0);
        pos = (start + end) / 2;
        dir = new Vector2(pos.x - end.x, pos.y - end.y);
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angleAxis = Quaternion.AngleAxis(angle, Vector3.forward);
        laser_2.transform.rotation = angleAxis;
        laser_2.transform.position = pos;

        start = lines[2].GetPosition(0);
        end = lines[2].GetPosition(1);

        laser_3.transform.localScale = new Vector3(Vector3.Distance(start, end), lines[2].startWidth, 0);
        pos = (start + end) / 2;
        dir = new Vector2(pos.x - end.x, pos.y - end.y);
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angleAxis = Quaternion.AngleAxis(angle, Vector3.forward);
        laser_3.transform.rotation = angleAxis;
        laser_3.transform.position = pos;

        float time = 0;
        while(time < 1.5f)
        {
            audio.loop = false;
            audio.clip = attackSound[0];
            audio.volume = 0.4f;
            audio.pitch = 1.0f;
            audio.Play();

            time += 0.15f;
            yield return new WaitForSeconds(0.15f);
        }

        yield return new WaitForSeconds(0.2f);

        isLaser = false;
        for (int i = 0; i < 3; i++)
        {
            lines[i].SetPosition(1, transform.position);
            lines[i].enabled = false;
        }
        isAttack = false;
        coolTime = attackCoolTime;
        Destroy(laser_1);
        Destroy(laser_2);
        Destroy(laser_3);
        StartCoroutine(EnemyMove());
    }

    private IEnumerator ShotGunPattern()
    {
        isAttack = true;
        SetSprite(Food_MainIngred.SOUP);
        yield return new WaitForSeconds(0.35f);

        float radius = 15;
        float bulletAmount = 10;
        bulletColor = new Color32(bulletColor.r, bulletColor.g, bulletColor.g, 200);

        for (int j = 0; j < 6; j++)
        {
            audio.loop = false;
            audio.clip = attackSound[1];
            audio.volume = 1.0f;
            audio.pitch = 1.5f;
            audio.Play();

            List<Vector3> randomPos = new List<Vector3>();
            randomPos.Add((player.transform.position - transform.position).normalized);
            for(int i = 1; i < bulletAmount; i++)
            {
                Vector3 temp = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0);
                randomPos.Add(temp.normalized);
            }

            for (int i = 0; i < bulletAmount; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.FromToRotation(Vector3.up, randomPos[i]), bulletParent.transform);
                bullet.GetComponent<EnemyBullet>().SetTarget(-bullet.transform.up);
                bullet.GetComponent<EnemyBullet>().SetSpeed(bulletSpeed);
                bullet.GetComponent<EnemyBullet>().SetDamage(bulletDamage);
                bullet.GetComponent<EnemyBullet>().SetSprite(sprites);
                bullet.GetComponent<EnemyBullet>().SetColor(bulletColor);
                bullet.transform.eulerAngles = bullet.transform.eulerAngles + new Vector3(0, 0, 90);
            }
            yield return new WaitForSeconds(0.5f);
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
            audio.loop = false;
            audio.clip = attackSound[2];
            audio.volume = 1.0f;
            audio.pitch = 1.0f;
            audio.Play();

            float posX = Random.Range(-(room.transform.localScale.x / 2), (room.transform.localScale.x / 2));
            float posY = Random.Range(-(room.transform.localScale.y / 2), (room.transform.localScale.y / 2));
            Vector3 target = new Vector3(room.transform.position.x + posX, room.transform.position.y + posY, 0);
            if(i == 5)
            {
                target = player.transform.position;
            }


            GameObject explosionBullet = Instantiate(explosionPrefab, transform.position, transform.rotation, bulletParent.transform);
            explosionBullet.GetComponent<SpriteRenderer>().sprite = data.breadBulletSprite.breadBulletSprite[nowFood.foodName];
            var breadBullet = explosionBullet.GetComponent<EnemyExplosionBullet>();
            breadBullet.SetTarget(transform.position - target);
            breadBullet.SetSpeed(explosionSpeed);
            breadBullet.SetDamage(explosionDamage);
            breadBullet.SetRadius(explosionRadius);
            breadBullet.SetSprite(sprites);

            yield return new WaitForSeconds(0.4f);
        }

        yield return new WaitForSeconds(0.4f);
        isAttack = false;
        coolTime = attackCoolTime;
        StartCoroutine(EnemyMove());
    }

    private IEnumerator CounterPattern()
    {
        isAttack = true;
        testPaper.SetActive(true);
        weaponObject.gameObject.SetActive(false);

        audio.loop = false;
        audio.clip = attackSound[3];
        audio.volume = 1.0f;
        audio.pitch = 1.2f;
        audio.Play();

        yield return new WaitForSeconds(0.2f);

        float time = 0;
        float colorTime = 0;
        isCounter = true;
        while(time < 5)
        {
            if(counterAmount >= 4)
            {
                //effect
                Instantiate(scoreEffect, player.transform);
                yield return new WaitForSeconds(0.33f);

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
        yield return new WaitForSeconds(0.5f);

        testPaper.transform.GetChild(0).gameObject.SetActive(false);
        testPaper.transform.GetChild(1).gameObject.SetActive(false);
        testPaper.transform.GetChild(2).gameObject.SetActive(false);
        testPaper.transform.GetChild(3).gameObject.SetActive(false);
        testPaper.SetActive(false);
        weaponObject.gameObject.SetActive(true);

        isAttack = false;
        coolTime = attackCoolTime;
        StartCoroutine(EnemyMove());
    }

    public void AddAmount()
    {
        Instantiate(counterEffect, transform.position, transform.rotation, effectParent.transform);
        if(counterAmount < 4)
        {
            testPaper.transform.GetChild(counterAmount).gameObject.SetActive(true);
            Instantiate(wrongSound, transform.position, transform.rotation, effectParent.transform);
            counterAmount++;
        }
    }

    private void SetSprite(Food_MainIngred ingred)
    {
        foreach(FoodData food in playerFood)
        {
            if(food.mainIngred == ingred)
            {
                weaponObject.sprite = food.foodSprite;
                nowFood = food;
                bulletColor = food.bulletColor;
                return;
            }
        }

        foreach(FoodData food in data.foodData.FoodDatas)
        {
            if (food.mainIngred == ingred)
            {
                weaponObject.sprite = food.foodSprite;
                nowFood = food;
                bulletColor = food.bulletColor;
                return;
            }
        }
    }

    private void SetIncreaseByStage()
    {
        bossCon.SetMaxHp(500 + ((GameManager.gameManager.stage - 1) * 400));
        bossCon.SetHp(500 + ((GameManager.gameManager.stage - 1) * 400));
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
