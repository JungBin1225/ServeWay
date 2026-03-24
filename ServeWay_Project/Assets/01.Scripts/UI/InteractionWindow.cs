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
    public GameObject moveStage;
    public GameObject startMaked;
    public GameObject moveWarn;

    private bool foodGetAble;
    private GameObject foodObject;
    private bool ingredGetAble;
    private bool cookAble;
    private bool refrigeAble;
    private bool moveStageAble;
    private bool alreadyMaked;
    private bool moveAble;

    void Start()
    {
        foodGetAble = false;
        ingredGetAble = false;
        cookAble = false;
        refrigeAble = false;
        moveStageAble = false;
        alreadyMaked = false;
        moveAble = true;

        foodGet.SetActive(false);
        ingredGet.SetActive(false);
        cookInteraction.SetActive(false);
        refrigeratorOpen.SetActive(false);
        moveStage.SetActive(false);
        startMaked.SetActive(false);
        moveWarn.SetActive(false);
    }

    void Update()
    {
        if(foodGetAble)
        {
            if(!ingredGetAble && !cookAble && !refrigeAble && !moveStageAble && !alreadyMaked)
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
            if(!foodGetAble && !cookAble && !refrigeAble && !moveStageAble && !alreadyMaked)
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
            if (!foodGetAble && !ingredGetAble && !refrigeAble && !moveStageAble && !alreadyMaked)
            {
                cookInteraction.SetActive(true);
            }
        }
        else
        {
            cookInteraction.SetActive(false);
        }

        if (alreadyMaked)
        {
            if (!foodGetAble && !ingredGetAble && !refrigeAble && !moveStageAble && !cookAble)
            {
                startMaked.SetActive(true);
            }
        }
        else
        {
            startMaked.SetActive(false);
        }

        if (refrigeAble)
        {
            if (!foodGetAble && !ingredGetAble && !cookAble && !moveStageAble && !alreadyMaked)
            {
                refrigeratorOpen.SetActive(true);
            }
        }
        else
        {
            refrigeratorOpen.SetActive(false);
        }

        if(moveStageAble)
        {
            if(!foodGetAble && !ingredGetAble && !cookAble && !refrigeAble && !alreadyMaked)
            {
                if(moveAble)
                {
                    moveStage.SetActive(true);
                }
                else
                {
                    moveWarn.SetActive(true);
                }
            }
        }
        else
        {
            if (moveAble)
            {
                moveStage.SetActive(false);
            }
            else
            {
                moveWarn.SetActive(false);
            }
        }
    }

    public void SetFoodGetAble(bool able)
    {
        foodGetAble = able;
    }

    public void SetFoodObject(GameObject foodObject)
    {
        this.foodObject = foodObject;
    }

    public GameObject GetFoodObject()
    {
        return foodObject;
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

    public void SetMoveStageAble(bool able, bool moveAble)
    {
        moveStageAble = able;
        this.moveAble = moveAble;
    }

    public void AlreadyMaked(bool able)
    {
        alreadyMaked = able;
    }
}
