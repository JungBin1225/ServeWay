using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    protected float speed;
    protected float damage;
    protected Vector3 target;
    protected Food_Nation nation;

    public GameObject destroyEffect;

    protected void Start()
    {

    }

    protected void Update()
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

    public void SetNation(Food_Nation nation)
    {
        this.nation = nation;
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
        else if (collision.tag == "Boss")
        {
            collision.gameObject.GetComponent<BossController>().GetDamage(damage, this.transform.position, nation);
        }

        if (collision.tag == "Enemy" || collision.tag == "Boss" || collision.tag == "Wall")
        {
            Instantiate(destroyEffect, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
