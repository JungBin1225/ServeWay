using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBullet : BulletController
{
    private float radius;

    private bool isExplode;

    private void Start()
    {
        isExplode = false;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
    }

    public void SetRadius(float radius)
    {
        this.radius = radius;
    }

    public void Fire()
    {
        if(!isExplode)
        {
            Fire();
        }
    }

    private IEnumerator Explosion()
    {
        isExplode = true;

        GameObject effect = Instantiate(destroyEffect, transform.position, transform.rotation);
        effect.transform.localScale = new Vector3(radius * transform.localScale.x * 5, radius * transform.localScale.y * 5);
        
        gameObject.GetComponent<CircleCollider2D>().radius = radius;
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        //폭발 애니메이션 실행
        yield return new WaitForSeconds(0.2f);

        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            if(isExplode)
            {
                collision.gameObject.GetComponent<EnemyController>().GetDamage(damage, this.transform.position);
            }
        }
        else if (collision.tag == "Boss")
        {
            if(isExplode)
            {
                collision.gameObject.GetComponent<BossController>().GetDamage(damage, this.transform.position, nation);
            }
        }

        if (collision.tag == "Enemy" || collision.tag == "Boss" || collision.tag == "Wall")
        {
            if(!isExplode)
            {
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                StartCoroutine(Explosion());
            }
            
        }
    }
}
