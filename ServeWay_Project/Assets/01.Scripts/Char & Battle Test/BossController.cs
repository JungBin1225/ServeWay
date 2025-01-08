using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    private MissionManager misson;
    private float hp;

    public BossRoom room;
    public Food_Nation nation;
    public Boss_Job job;

    void Start()
    {
        misson = FindObjectOfType<MissionManager>();
        //StartCoroutine(EnemyMove());
    }

    void Update()
    {
        if (hp <= 0)
        {
            BossDie(0);
        }

        if(misson.isClear())
        {
            BossDie(1);
        }

    }

    public void BossDie(int dieType)
    {
        room.isClear = true;

        switch(dieType)
        {
            case 0: //Hp 소진
                room.DropIngredient(4, 9);
                break;
            case 1: //미션 클리어
                room.DropIngredient(6, 13);
                break;
        }
        room.DropRecipe();

        room.OpenDoor();
        room.ActiveStair();
        GameManager.gameManager.isBossStage = false;
        Destroy(this.gameObject);
    }

    public void GetDamage(float damage, Vector3 effectPos, FoodData food)
    {
        //GameObject effect = Instantiate(damageEffect, effectPos, transform.rotation);

        if(job == Boss_Job.YOUTUBER && gameObject.GetComponent<YoutuberController>().isAlgorithm)
        {
            if(food != gameObject.GetComponent<YoutuberController>().GetAlgorithmFood())
            {
                Debug.Log("Critical");
                damage *= 1.2f;
            }
        }

        if(job == Boss_Job.TEACHER && gameObject.GetComponent<TeacherController>().isCounter)
        {
            gameObject.GetComponent<TeacherController>().AddAmount();
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
        return hp;
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
