using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetIngredients : MonoBehaviour
{
    private bool getAble;
    private InventoryManager inventory;

    public IngredientList.IngredientsName itemName;
    void Start()
    {
        inventory = FindObjectOfType<InventoryManager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (getAble)
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

    private void GetItem()
    {
        inventory.GetItem(this.itemName, 1);
        Destroy(this.gameObject);
    }

    /*private void DeleteItem()
    {
        inventory.DeleteItem(this.itemName, 1);
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            getAble = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            getAble = false;
        }
    }
}
