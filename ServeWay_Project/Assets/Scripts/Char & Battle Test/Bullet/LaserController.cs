using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    private float damage;
    private float coolTime;
    private Food_Nation nation;
    private float nowCoolTime;
    private BoxCollider2D collider;
    private bool isClicked;

    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        collider.enabled = false;
        nowCoolTime = coolTime / 2;
    }


    void Update()
    {
        StartCoroutine(FireLaser());
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetCoolTime(float coolTime)
    {
        this.coolTime = coolTime;
    }

    public void SetNation(Food_Nation nation)
    {
        this.nation = nation;
    }

    private IEnumerator FireLaser()
    {
        while(true)
        {
            if(nowCoolTime <= 0)
            {
                collider.enabled = true;
                nowCoolTime = coolTime;
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                collider.enabled = false;
                nowCoolTime -= Time.deltaTime;
            }
            yield return null;
        }
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
    }
}
