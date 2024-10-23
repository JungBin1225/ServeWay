using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    private float damage;
    private float coolTime;
    private Food_Nation nation;
    private List<Sprite> sprite;
    private float nowCoolTime;
    private BoxCollider2D collider;
    private bool isClicked;

    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        collider.enabled = false;
        nowCoolTime = coolTime / 2;

        StartCoroutine(FireLaser());
    }


    void Update()
    {
        
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetCoolTime(float coolTime)
    {
        this.coolTime = coolTime;
    }

    public void SetSprite(List<Sprite> sprite)
    {
        this.sprite = sprite;
    }

    private IEnumerator FireLaser()
    {
        while (true)
        {
            if (nowCoolTime <= 0)
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
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealth>().PlayerDamaged(damage, sprite);
        }
    }
}
