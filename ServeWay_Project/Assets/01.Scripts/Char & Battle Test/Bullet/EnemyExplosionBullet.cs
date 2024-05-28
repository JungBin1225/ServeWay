using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExplosionBullet : EnemyBullet
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

    private IEnumerator Explosion()
    {
        isExplode = true;

        gameObject.GetComponent<CircleCollider2D>().radius = radius;
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        //폭발 애니메이션 실행
        yield return new WaitForSeconds(0.2f);

        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (isExplode)
            {
                collision.gameObject.GetComponent<PlayerHealth>().PlayerDamaged(damage);
            }
        }

        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Wall")
        {
            if (!isExplode)
            {
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                StartCoroutine(Explosion());
            }
        }
    }
}
