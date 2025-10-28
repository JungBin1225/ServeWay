using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarObject : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer playerSprite;
    private GameObject player;
    private bool isChild;

    public int originalOrder;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player");
        playerSprite = player.GetComponent<SpriteRenderer>();
        
        isChild = false;
        if(transform.childCount != 0)
        {
            isChild = true;
        }
    }

    void Update()
    {
        if (transform.position.y > player.transform.position.y)
        {
            spriteRenderer.sortingOrder = originalOrder;
        }
        else
        {
            spriteRenderer.sortingOrder = playerSprite.sortingOrder + originalOrder + 1;
        }

        if (isChild)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder + 1;
        }
    }
}
