using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer playerSprite;
    private GameObject player;

    public Animator anim;
    public bool isFront;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player");
        playerSprite = player.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(isFront)
        {
            if (transform.position.y > player.transform.position.y)
            {
                spriteRenderer.sortingOrder = 0;
            }
            else
            {
                spriteRenderer.sortingOrder = playerSprite.sortingOrder + 1;
            }
        }
    }

    public void OpenDoor()
    {
        anim.SetTrigger("Open");
    }

    public void CloseDoor()
    {
        anim.SetTrigger("Close");
    }
}
