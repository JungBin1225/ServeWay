using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    private MissionManager misson;
    private float hp;
    private float maxHp;
    private bool dying;
    private Animator anim;
    private SpriteRenderer renderer;
    private GameObject effectParent;
    private YoutuberController youtuberController;
    private TeacherController teacherController;

    public BossRoom room;
    public Food_Nation nation;
    public Boss_Job job;
    public GameObject eatSound;
    public GameObject reduceDamage;

    void Start()
    {
        misson = FindObjectOfType<MissionManager>();
        dying = false;
        anim = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        effectParent = GameObject.Find("EffectList");
        youtuberController = GetComponent<YoutuberController>();
        teacherController = GetComponent<TeacherController>();
        //StartCoroutine(EnemyMove());
    }

    void Update()
    {
        if (dying)
        {
            return;
        }

        if (hp <= 0)
        {
            StartCoroutine(BossDie(0));
        }

        if(misson.isClear())
        {
            StartCoroutine(BossDie(1));
        }

    }

    public IEnumerator BossDie(int dieType)
    {
        dying = true;
        anim.SetTrigger("dead");
        Debug.Log("dead");
        while(renderer.color.a > 0)
        {
            yield return null;
        }


        room.isClear = true;

        switch(dieType)
        {
            case 0: //Hp 소진
                room.DropIngredient(4, 9);
                break;
            case 1: //미션 클리어
                room.DropIngredient(6, 13);
                room.DropRecipe();
                break;
        }

        room.OpenDoor();
        room.ActiveStair();
        misson.MissionDisappear();
        GameManager.gameManager.isBossStage = false;
        Destroy(this.gameObject);
    }

    public void GetDamage(float damage, Vector3 effectPos, FoodData food)
    {
        //GameObject effect = Instantiate(damageEffect, effectPos, transform.rotation);
        if(FindObjectOfType<EatSound>() == null)
        {
            GameObject sound = Instantiate(eatSound, transform.position, transform.rotation, effectParent.transform);
        }

        if (job == Boss_Job.YOUTUBER && youtuberController.isAlgorithm)
        {
            if(food == youtuberController.GetAlgorithmFood())
            {
                Vector3 pos = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0);
                Instantiate(reduceDamage, transform.position + pos, Quaternion.Euler(0, 0, 0));
                damage /= 0.8f;
            }
            misson.OccurreEvent(13, damage);
        }

        if(job == Boss_Job.TEACHER && teacherController.isCounter)
        {
            teacherController.AddAmount();
            damage = 0;
        }

        hp -= damage;
        
        if(food.nation.ToString() == this.nation.ToString())
        {
            misson.OccurreEvent(0, damage);
        }
        else
        {
            misson.OccurreEvent(5, damage);
        }

        misson.OccurreEvent(3, damage);
        misson.OccurreEvent(6, damage, food.foodName);
    }

    public void SetHp(float hp)
    {
        this.hp = hp;
    }

    public float GetHp()
    {
        if(hp <= 0)
        {
            return 0;
        }
        else
        {
            return hp;
        }
    }

    public void SetMaxHp(float hp)
    {
        this.maxHp = hp;
    }

    public float GetMaxHp()
    {
        return maxHp;
    }

    /*private IEnumerator AroundPattern()
    {
        isAttack = true;
        yield return new WaitForSeconds(0.3f);

        float radius = 2.5f;
        for (int i = 0; i < 20; i++)
        {
            float angle = i * Mathf.PI * 2 / 20;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            Vector3 pos = transform.position + new Vector3(x, y, 0);
            float angleDegrees = -angle * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(0, 0, angleDegrees);
            GameObject bullet = Instantiate(bulletPrefab, pos, rot);
            bullet.GetComponent<EnemyBullet>().SetTarget(new Vector3(-x, -y, 0));
            bullet.GetComponent<EnemyBullet>().SetSpeed(bulletSpeed);
            bullet.GetComponent<EnemyBullet>().SetDamage(bulletDamage);
        }
        yield return new WaitForSeconds(0.3f);

        isAttack = false;
        coolTime = attackCoolTime;
        StartCoroutine(EnemyMove());
    }*/


    /*private IEnumerator SplitPattern()
    {
        isAttack = true;
        yield return new WaitForSeconds(0.4f);

        float radius = 2.5f;
        for (int i = 0; i < 4; i++)
        {
            float angle = i * Mathf.PI * 2 / 4;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            Vector3 pos = transform.position + new Vector3(x, y, 0);
            float angleDegrees = -angle * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(0, 0, angleDegrees);
            GameObject bullet = Instantiate(splitBulletPrefab, pos, rot);
            bullet.GetComponent<SplitBullet>().SetTarget(new Vector3(-x, -y, 0));
            bullet.GetComponent<SplitBullet>().SetSpeed(bulletSpeed / 2);
            bullet.GetComponent<SplitBullet>().SetDamage(bulletDamage * 2);
            bullet.GetComponent<SplitBullet>().SetSplitSpeed(bulletSpeed);
            bullet.GetComponent<SplitBullet>().SetSplitDamage(bulletDamage);
            bullet.GetComponent<SplitBullet>().SetBigDamage(splitBulletDamage);
        }
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 8; i++)
        {
            if (i % 2 == 1)
            {
                float angle = i * Mathf.PI * 2 / 8;
                float x = Mathf.Cos(angle) * radius;
                float y = Mathf.Sin(angle) * radius;
                Vector3 pos = transform.position + new Vector3(x, y, 0);
                float angleDegrees = -angle * Mathf.Rad2Deg;
                Quaternion rot = Quaternion.Euler(0, 0, angleDegrees);
                GameObject bullet = Instantiate(splitBulletPrefab, pos, rot);
                bullet.GetComponent<SplitBullet>().SetTarget(new Vector3(-x, -y, 0));
                bullet.GetComponent<SplitBullet>().SetSpeed(bulletSpeed / 2);
                bullet.GetComponent<SplitBullet>().SetDamage(bulletDamage * 2);
                bullet.GetComponent<SplitBullet>().SetSplitSpeed(bulletSpeed);
                bullet.GetComponent<SplitBullet>().SetSplitDamage(bulletDamage);
            }
        }
        yield return new WaitForSeconds(0.3f);

        isAttack = false;
        coolTime = attackCoolTime * 2;
        StartCoroutine(EnemyMove());
    }*/
}
