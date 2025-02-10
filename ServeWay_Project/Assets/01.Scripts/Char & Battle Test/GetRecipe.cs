using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRecipe : MonoBehaviour
{
    private bool getAble;
    private FoodIngredDex dex;
    private InteractionWindow interaction;

    public string foodName;
    public Vector3 roomPos;

    void Start()
    {
        dex = FindObjectOfType<DataController>().FoodIngredDex;
        interaction = FindObjectOfType<InteractionWindow>();
    }

    void Update()
    {
        if (getAble && interaction.foodGet.activeSelf)
        {
            if (Input.GetKey(KeyCode.F))
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            getAble = true;
            interaction.SetIngredGetAble(true);
        }

        if (collision.tag == "Wall")
        {
            transform.position -= (transform.position - roomPos).normalized * 0.5f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            getAble = false;
            interaction.SetFoodGetAble(false);
        }
    }
}
