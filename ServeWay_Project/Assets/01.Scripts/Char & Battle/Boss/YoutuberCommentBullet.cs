using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoutuberCommentBullet : MonoBehaviour
{
    private float damage;
    private List<Sprite> sprites;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetSprite(List<Sprite> sprites)
    {
        this.sprites = sprites;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealth>().PlayerDamaged(damage, sprites);
        }
    }


}
