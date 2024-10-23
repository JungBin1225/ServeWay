using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    protected Vector3 target;
    protected float damage;
    protected float speed;
    protected List<Sprite> sprite;

    public GameObject destroyEffect;

    protected void Start()
    {

    }

    protected void Update()
    {
        Fire();
    }

    public void SetTarget(Vector3 target)
    {
        this.target = target;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void SetSprite(List<Sprite> sprite)
    {
        this.sprite = sprite;
    }

    public void Fire()
    {
        Vector3 dir = new Vector3(target.x, target.y, 0);

        transform.position -= dir.normalized * Time.deltaTime * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealth>().PlayerDamaged(damage, sprite);
        }

        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Wall")
        {
            Instantiate(destroyEffect, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
