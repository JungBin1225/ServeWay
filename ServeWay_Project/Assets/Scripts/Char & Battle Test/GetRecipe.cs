using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRecipe : MonoBehaviour
{
    private bool getAble;
    private FoodIngredDex dex;

    public string foodName;

    void Start()
    {
        dex = FindObjectOfType<DataController>().FoodIngredDex;
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
    }

    private void GetItem()
    {
        dex.UpdateFoodDex(foodName, FoodDex_Status.RECIPE);
        Destroy(this.gameObject);
    }

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
