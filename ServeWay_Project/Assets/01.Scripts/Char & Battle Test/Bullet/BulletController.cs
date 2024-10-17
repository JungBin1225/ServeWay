using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletController : MonoBehaviour
{
    protected float speed;
    protected float damage;
    protected Vector3 target;
    protected FoodData food;

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

    public void SetFood(FoodData food)
    {
        this.food = food;
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
            if(SceneManager.GetActiveScene().name.Contains("Tutorial"))
            {
                collision.gameObject.GetComponent<TutorialEnemy>().GetDamage(damage, this.transform.position);
            }
            else
            {
                collision.gameObject.GetComponent<EnemyController>().GetDamage(damage);
            }
        }
        else if (collision.tag == "Boss")
        {
            if (SceneManager.GetActiveScene().name.Contains("Tutorial"))
            {
                collision.gameObject.GetComponent<TutorialBoss>().GetDamage(damage, this.transform.position, food);
            }
            else
            {
                collision.gameObject.GetComponent<BossController>().GetDamage(damage, this.transform.position, food);
            }  
        }

        if (collision.tag == "Enemy" || collision.tag == "Boss" || collision.tag == "Wall")
        {
            Instantiate(destroyEffect, transform.position, transform.rotation);
            Destroy(this.gameObject);
        }
    }
}
