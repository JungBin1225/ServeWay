using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DexUI : MonoBehaviour
{
    private DataController dataController;
    private GameObject foodDex;
    private GameObject ingredDex;

    public Sprite lockSprite;

    void Start()
    {
        /*dataController = FindObjectOfType<DataController>();
        foodDex = transform.GetChild(0).gameObject;
        ingredDex = transform.GetChild(1).gameObject;*/
    }

    private void OnEnable()
    {
        dataController = FindObjectOfType<DataController>();
        foodDex = transform.GetChild(0).gameObject;
        ingredDex = transform.GetChild(1).gameObject;
        InitList();
    }

    private void InitList()
    {
        for(int i = 0; i < foodDex.transform.childCount; i++)
        {
            List<FoodInfo> foodList = dataController.FoodInfoList.foodInfo;
            int index = i;

            if(i >= dataController.FoodInfoList.foodInfo.Count)
            {
                foodList = dataController.FoodInfoList.start_foodInfo;
                index = i - dataController.FoodInfoList.foodInfo.Count;
            }

            if(dataController.FoodIngredDex.foodDex[foodList[index].foodName])
            {
                foodDex.transform.GetChild(i).GetComponent<Image>().sprite = foodList[index].foodSprite;
            }
            else
            {
                foodDex.transform.GetChild(i).GetComponent<Image>().sprite = lockSprite;
            }
        }

        for (int i = 0; i < ingredDex.transform.childCount; i++)
        {
            if (dataController.FoodIngredDex.ingredDex[dataController.IngredientList.ingredientList[i].name])
            {
                ingredDex.transform.GetChild(i).GetComponent<Image>().sprite = dataController.IngredientList.ingredientList[i].sprite;
            }
            else
            {
                ingredDex.transform.GetChild(i).GetComponent<Image>().sprite = lockSprite;
            }
        }
    }
}
