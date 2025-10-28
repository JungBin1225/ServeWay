using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTeacher : MonoBehaviour
{
    private GameObject player;
    private SpriteRenderer playerSprite;
    private SpriteRenderer spriteRenderer;

    public GameObject speechBubble;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerSprite = player.GetComponent<SpriteRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            speechBubble.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            speechBubble.SetActive(false);
        }
    }
}
