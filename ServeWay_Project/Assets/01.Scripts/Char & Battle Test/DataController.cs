using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataController : MonoBehaviour
{
    public FoodDataSet foodData;
    public StartFoodDataSet startFoodData;
    public IngredientDataSet IngredientList;
    public EnemyList enemyList;
    public FoodIngredDex FoodIngredDex;
    public BossList bossList;
    public EnemyBulletList enemyBullet;
    public MapObjectList mapObjectList;

    private List<Ingredient> grade1List;
    private List<Ingredient> grade2List;
    private List<Ingredient> grade3List;
    private List<Ingredient> grade4List;

    private void Start()
    {
        grade1List = new List<Ingredient>();
        grade2List = new List<Ingredient>();
        grade3List = new List<Ingredient>();
        grade4List = new List<Ingredient>();

        foreach(Ingredient ingred in IngredientList.IngredientList)
        {
            switch(ingred.grade)
            {
                case Ingred_Grade.STAR_1:
                    grade1List.Add(ingred);
                    break;
                case Ingred_Grade.STAR_2:
                    grade2List.Add(ingred);
                    break;
                case Ingred_Grade.STAR_3:
                    grade3List.Add(ingred);
                    break;
                case Ingred_Grade.STAR_4:
                    grade4List.Add(ingred);
                    break;
            }
        }
    }

    public FoodData FindFood(string name)
    {
        foreach(FoodData food in foodData.FoodDatas)
        {
            if(name == food.foodName)
            {
                return food;
            }
        }

        foreach(FoodData food in startFoodData.StartFoodDatas)
        {
            if(name == food.foodName)
            {
                return food;
            }
        }

        return null;
    }

    public FoodData FindFood(Sprite sprite)
    {
        foreach (FoodData food in foodData.FoodDatas)
        {
            if (sprite == food.foodSprite)
            {
                return food;
            }
        }

        foreach (FoodData food in startFoodData.StartFoodDatas)
        {
            if (sprite == food.foodSprite)
            {
                return food;
            }
        }

        return null;
    }

    public Ingredient FindIngredient(Ingred_Name name)
    {
        foreach (Ingredient ingred in IngredientList.IngredientList)
        {
            if (name == ingred.name)
            {
                return ingred;
            }
        }

        return null;
    }

    public Ingredient FindIngredient(Sprite sprite)
    {
        foreach(Ingredient ingred in IngredientList.IngredientList)
        {
            if(sprite == ingred.sprite)
            {
                return ingred;
            }
        }

        return null;
    }

    public List<Ingredient> GetGradeList(int grade)
    {
        switch(grade)
        {
            case 1:
                return grade1List;
            case 2:
                return grade2List;
            case 3:
                return grade3List;
            case 4:
                return grade4List;
            default:
                return null;
        }
    }

    public GameObject FindBoss(Boss_Job job)
    {
        switch(job)
        {
            case Boss_Job.JOURNAL:
                return bossList.bossPrefab[0];
            case Boss_Job.COOKRESEARCH:
                return bossList.bossPrefab[1];
            case Boss_Job.CRITIC:
                return bossList.bossPrefab[2];
            case Boss_Job.BLOGGER:
                return bossList.bossPrefab[3];
            case Boss_Job.YOUTUBER:
                return bossList.bossPrefab[4];
            case Boss_Job.TEACHER:
                return bossList.bossPrefab[5];
            default:
                return bossList.bossPrefab[0];
        }
    }

    public List<FoodData> GetNationFoodList(Food_Nation nation)
    {
        List<FoodData> result = new List<FoodData>();

        foreach(FoodData food in foodData.FoodDatas)
        {
            if(food.nation == nation)
            {
                result.Add(food);
            }
        }

        return result;
    }
}
