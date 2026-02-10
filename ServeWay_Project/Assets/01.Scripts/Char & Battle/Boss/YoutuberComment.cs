using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoutuberComment : MonoBehaviour
{
    private float damage;
    private List<Sprite> sprites;
    private GameObject bulletParent;

    public int type;
    public GameObject bulletPrefab;

    void Start()
    {
        bulletParent = GameObject.Find("BulletList");
        StartCoroutine(Fire());
    }

    void Update()
    {
        
    }

    private IEnumerator Fire()
    {
        yield return new WaitForSeconds(1f);
        //GetComponent<SpriteRenderer>().enabled = false;

        Vector3 pos = Vector3.zero;
        Quaternion rot = Quaternion.Euler(0, 0, 0);
        Vector3 dir = Vector3.zero;
        float targetDir = 0;

        switch(type)
        {
            case 1:
                pos = new Vector3(transform.position.x + (transform.localScale.x / 2), transform.position.y, 0);
                rot = Quaternion.Euler(0, 0, 0);
                dir = new Vector3(-1, 0, 0);
                targetDir = transform.localScale.x;
                break;
            case 2:
                pos = new Vector3(transform.position.x - (transform.localScale.x / 2), transform.position.y, 0);
                rot = Quaternion.Euler(0, 0, 180);
                dir = new Vector3(1, 0, 0);
                targetDir = transform.localScale.x;
                break;
            case 3:
                pos = new Vector3(transform.position.x, transform.position.y + (transform.localScale.y / 2), 0);
                rot = Quaternion.Euler(0, 0, 90);
                dir = new Vector3(0, -1, 0);
                targetDir = transform.localScale.y;
                break;
            case 4:
                pos = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y / 2), 0);
                rot = Quaternion.Euler(0, 0, -90);
                dir = new Vector3(0, 1, 0);
                targetDir = transform.localScale.y;
                break;
        }

        GameObject bullet = Instantiate(bulletPrefab, pos, rot, bulletParent.transform);
        bullet.GetComponent<YoutuberCommentBullet>().SetDamage(damage);
        bullet.GetComponent<YoutuberCommentBullet>().SetSprite(sprites);

        SpriteRenderer sprite = bullet.GetComponent<SpriteRenderer>();
        BoxCollider2D collider = bullet.GetComponent<BoxCollider2D>();
        sprite.size = new Vector2(0.1f, 0.95f);
        collider.size = new Vector2(0.1f, 0.95f);
        
        while(sprite.size.x < 2.57f)
        {
            sprite.size = new Vector2(sprite.size.x + (Time.deltaTime * 15), 0.95f);
            collider.size = new Vector2(collider.size.x + (Time.deltaTime * 15), 0.95f);
            yield return null;
        }
        sprite.size = new Vector2(2.57f, 0.95f);
        collider.size = new Vector2(2.57f, 0.95f);

        while(targetDir > 0)
        {
            bullet.transform.position += dir * Time.deltaTime * 15;
            targetDir -= Time.deltaTime * 15;
            yield return null;
        }

        Destroy(bullet);
        Destroy(this.gameObject);
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetSprite(List<Sprite> sprites)
    {
        this.sprites = sprites;
    }
}
