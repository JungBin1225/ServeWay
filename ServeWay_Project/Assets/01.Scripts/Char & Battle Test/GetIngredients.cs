using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetIngredients : MonoBehaviour
{
    private bool getAble;
    private InventoryManager inventory;
    private InteractionWindow interaction;

    public Ingred_Name itemName;
    void Start()
    {
        inventory = FindObjectOfType<InventoryManager>();
        interaction = FindObjectOfType<InteractionWindow>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (getAble && interaction.ingredGet.activeSelf)
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

    public void SetSprite(Sprite sprite)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
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
