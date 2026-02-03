using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoutuderExplosion : EnemyBullet
{
    private Vector3 player;
    private float radius;
    private bool isExplode;
    private GameObject bulletList;
    private List<YoutuberDislike> bullets;

    void Start()
    {
        isExplode = false;
        effectParent = GameObject.Find("EffectList");
        player = GameObject.Find("Player").transform.position;
        bulletList = transform.GetChild(0).gameObject;
        
        bullets = new List<YoutuberDislike>();
        for(int i = 0; i < bulletList.transform.childCount; i++)
        {
            bullets.Add(bulletList.transform.GetChild(i).gameObject.GetComponent<YoutuberDislike>());
            bullets[i].GetComponent<EnemyBullet>().SetTarget(bulletList.transform.position - bullets[i].gameObject.transform.position);
            bullets[i].GetComponent<EnemyBullet>().SetSpeed(speed * 1.5f);
            bullets[i].GetComponent<EnemyBullet>().SetDamage(damage);
            bullets[i].GetComponent<EnemyBullet>().SetSprite(sprite);
        }
    }

    void Update()
    {
        if (!isExplode)
        {
            Fire();
        }

        if(bulletList.transform.childCount == 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetRadius(float radius)
    {
        this.radius = radius / transform.localScale.x;
    }

    public void Fire()
    {
        Vector3 dir = new Vector3(target.x, target.y, 0);

        transform.position -= dir.normalized * Time.deltaTime * speed;

        if(!isExplode && Vector3.Distance(transform.position, player) < 0.5f)
        {
            isExplode = true;
            StartCoroutine(Explosion());
        }
    }

    private IEnumerator Explosion()
    {
        isExplode = true;

        GameObject effect1 = Instantiate(destroyEffect, transform.position, transform.rotation, effectParent.transform);
        effect1.transform.localScale = new Vector3(radius * transform.localScale.x * 3, radius * transform.localScale.y * 3, 0);

        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        //폭발 애니메이션 실행
        yield return new WaitForSeconds(0.1f);

        foreach (YoutuberDislike bullet in bullets)
        {
            if (bullet != null)
            {
                bullet.isFire = true;
            }
        }
    }
}
