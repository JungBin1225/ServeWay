using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionWindow : MonoBehaviour
{
    public GameObject foodGet;
    public GameObject ingredGet;
    public GameObject cookInteraction;
    public GameObject time;

    private bool foodGetAble;
    private bool ingredGetAble;
    private bool cookAble;

    void Start()
    {
        foodGetAble = false;
        ingredGetAble = false;
        cookAble = false;

        foodGet.SetActive(false);
        ingredGet.SetActive(false);
        cookInteraction.SetActive(false);
    }

    void Update()
    {
        if(foodGetAble)
        {
            if(!ingredGetAble && !cookAble)
            {
                foodGet.SetActive(true);
            }
        }
        else
        {
            foodGet.SetActive(false);
        }

        if(ingredGetAble)
        {
            if(!foodGetAble && !cookAble)
            {
                ingredGet.SetActive(true);
            }
        }
        else
        {
            ingredGet.SetActive(false);
        }

        if (cookAble)
        {
            if (!foodGetAble && !ingredGetAble)
            {
                cookInteraction.SetActive(true);
            }
        }
        else
        {
            cookInteraction.SetActive(false);
        }
    }

    public void SetFoodGetAble(bool able)
    {
        foodGetAble = able;
    }

    public void SetIngredGetAble(bool able)
    {
        ingredGetAble = able;
    }

    public void SetCookAble(bool able)
    {
        cookAble = able;
    }
}
