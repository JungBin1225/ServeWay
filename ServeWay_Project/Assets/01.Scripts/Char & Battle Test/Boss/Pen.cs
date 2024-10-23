using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pen : MonoBehaviour
{
    public float damage;
    public Sprite sprite;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            List<Sprite> sprites = new List<Sprite>();
            sprites.Add(sprite);
            collision.gameObject.GetComponent<PlayerHealth>().PlayerDamaged(damage, sprites);
        }
    }
}
