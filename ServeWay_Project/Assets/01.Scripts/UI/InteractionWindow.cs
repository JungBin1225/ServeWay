using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionWindow : MonoBehaviour
{
    public GameObject foodGet;
    public GameObject ingredGet;
    public GameObject cookInteraction;
    public GameObject refrigeratorOpen;
    public GameObject time;

    private bool foodGetAble;
    private bool ingredGetAble;
    private bool cookAble;
    private bool refrigeAble;

    void Start()
    {
        foodGetAble = false;
        ingredGetAble = false;
        cookAble = false;
        refrigeAble = false;

        foodGet.SetActive(false);
        ingredGet.SetActive(false);
        cookInteraction.SetActive(false);
        refrigeratorOpen.SetActive(false);
    }

    void Update()
    {
        if(foodGetAble)
        {
            if(!ingredGetAble && !cookAble && !refrigeAble)
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
            if(!foodGetAble && !cookAble && !refrigeAble)
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
            if (!foodGetAble && !ingredGetAble && !refrigeAble)
            {
                cookInteraction.SetActive(true);
            }
        }
        else
        {
            cookInteraction.SetActive(false);
        }

        if(refrigeAble)
        {
            if (!foodGetAble && !ingredGetAble && !cookAble)
            {
                refrigeratorOpen.SetActive(true);
            }
        }
        else
        {
            refrigeratorOpen.SetActive(false);
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

    public void SetRefrigeAble(bool able)
    {
        refrigeAble = able;
    }
}
