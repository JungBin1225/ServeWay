using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class EnemyBullet : MonoBehaviour
{
    protected Vector3 target;
    protected float damage;
    protected float speed;
    protected List<Sprite> sprite;

    public GameObject destroyEffect;
    public bool isRotate;

    protected void Start()
    {
        if(GetComponent<SpriteLibrary>() != null)
        {
            int ran = Random.Range(0, 3);
            string index = string.Format("trash_{0}", ran);
            GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteLibrary>().GetSprite("sprite", index);
        }
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

        if(isRotate)
        {
            transform.eulerAngles += new Vector3(0, 0, 180 * Time.deltaTime);
        }
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
