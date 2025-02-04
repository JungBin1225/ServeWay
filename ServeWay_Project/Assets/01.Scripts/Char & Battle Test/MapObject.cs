using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    public int width;
    public int height;
    public Sprite changeSprite;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            if(GetComponent<BoxCollider2D>().isTrigger)
            {
                GetComponent<SpriteRenderer>().sprite = changeSprite;
            }
        }
    }
}
