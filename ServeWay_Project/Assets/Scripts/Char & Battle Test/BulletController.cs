using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float speed;
    private float damage;
    private Vector3 target;


    void Start()
    {

    }

    void Update()
    {
        Fire();
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetTarget(Vector3 target)
    {
        this.target = target;
    }

    public void Fire()
    {
        Vector3 dir = new Vector3(target.x, target.y, 0);

        transform.position -= dir * Time.deltaTime * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyController>().GetDamage(damage, this.transform.position);
        }
        else if(collision.tag == "Boss")
        {
            collision.gameObject.GetComponent<BossController>().GetDamage(damage, this.transform.position);
        }

        if (collision.tag == "Enemy" || collision.tag == "Boss" || collision.tag == "Wall")
        {
            Destroy(this.gameObject);
        }
    }
}
