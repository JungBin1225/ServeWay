using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetIngredients : MonoBehaviour
{
    private bool getAble;
    private InventoryManager inventory;
    private InteractionWindow interaction;

    public Ingred_Name itemName;
    public Vector3 roomPos;
    public SpriteRenderer spriteRenderer;
    public List<GameObject> starObject;
    void Start()
    {
        inventory = FindObjectOfType<InventoryManager>();
        interaction = FindObjectOfType<InteractionWindow>();
    }

    void Update()
    {
        if (getAble && interaction.ingredGet.activeSelf)
        {
            if (Input.GetKey(KeyCode.F))
            {
                GetItem();
            }
        }
        /*else if (Input.GetKeyDown(KeyCode.G))
        {
            if (getAble)
            {
                DeleteItem();
            }
        }*/
    }

    public void SetSprite(Sprite sprite, int star)
    {
        spriteRenderer.sprite = sprite;
        starObject[star].SetActive(true);
    }

    private void GetItem()
    {
        inventory.GetItem(this.itemName, 1);
        Destroy(this.gameObject);
    }

    /*private void DeleteItem()
    {
        inventory.DeleteItem(this.itemName, 1);
    }*/

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            getAble = true;
            interaction.SetIngredGetAble(true);
        }

        if(collision.tag == "Wall")
        {
            transform.position -= (transform.position - roomPos).normalized * 0.5f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            getAble = false;
            interaction.SetIngredGetAble(false);
        }
    }
}
