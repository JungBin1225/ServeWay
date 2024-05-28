using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    protected Vector3 target;
    protected float damage;
    protected float speed;

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

    public void Fire()
    {
        Vector3 dir = new Vector3(target.x, target.y, 0);

        transform.position -= dir.normalized * Time.deltaTime * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealth>().PlayerDamaged(damage);
        }

        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Wall")
        {
            Destroy(this.gameObject);
        }
    }
}
