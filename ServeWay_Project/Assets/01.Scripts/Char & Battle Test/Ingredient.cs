using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public enum Ingred_Grade
{
    STAR_1,
    STAR_2,
    STAR_3,
    STAR_4
};

public enum Ingred_Name
{
    Pumpkin, Strawberrie, Water, Meat, Flour, Noodle, Onion,
    Lettuce, Garlic, Pepper, Rice, Salt, Sugar, Spice, Egg,
    Fish, Oil, Tomato, Sauce, Mushroom, Milk, Butter, Cheese,
    Fruit, Greenonion, Carrot, Beansprout, Seaweed, Vanilla,
    Coriander, Kimchi, Vinegar, Tofu, Wine, Salary, Radish,
    Redbean, Eggplant, Olive, Cream, Nuts, Cucumber, Honey,
    Ginger
};

[Serializable]
public class Ingredient
{
    [SerializeField] private int _Index;
    [SerializeField] private Ingred_Name _Name;
    [SerializeField] private Ingred_Grade _Grade;
    [SerializeField] private string _SpritePath;
    [SerializeField] private string _PrefabPath;
    [SerializeField] private string _Passive;

    public int index { get { return _Index; } }
    public Ingred_Name name { get { return _Name; } }
    public Ingred_Grade grade { get { return _Grade; } }
    public Sprite sprite
    {
        get
        {
            Sprite getSprite = null;
            UnityEngine.Object[] sprites = AssetDatabase.LoadAllAssetsAtPath("Assets/Prefabs/MapPrefabs/Backyard - Free/backyard.png");
            foreach (UnityEngine.Object sprite in sprites)
            {
                if (sprite.name.Contains(_SpritePath))
                {
                    getSprite = (Sprite)sprite;
                }
            }
            return getSprite;
        }
    }
    public GameObject prefab
    {
        get
        {
            GameObject prefab = (GameObject)AssetDatabase.LoadAssetAtPath(_PrefabPath, typeof(GameObject));

            return prefab;
        }
    }

    public string passive { get { return _Passive; } }

    public string EnumToString(Ingred_Name name)
    {
        switch(name)
        {
            case Ingred_Name.Pumpkin:
                return "ȣ��";
            case Ingred_Name.Strawberrie:
                return "����";
            case Ingred_Name.Water:
                return "��";
            case Ingred_Name.Meat:
                return "���";
            case Ingred_Name.Flour:
                return "�а���";
            case Ingred_Name.Noodle:
                return "��";
            case Ingred_Name.Onion:
                return "����";
            case Ingred_Name.Lettuce:
                return "�����";
            case Ingred_Name.Garlic:
                return "����";
            case Ingred_Name.Pepper:
                return "�Ǹ�";
            case Ingred_Name.Rice:
                return "��";
            case Ingred_Name.Salt:
                return "�ұ�";
            case Ingred_Name.Sugar:
                return "����";
            case Ingred_Name.Spice:
                return "��ŷ�";
            case Ingred_Name.Egg:
                return "�ް�";
            case Ingred_Name.Fish:
                return "�ػ깰";
            case Ingred_Name.Oil:
                return "�⸧";
            case Ingred_Name.Tomato:
                return "�丶��";
            case Ingred_Name.Sauce:
                return "��";
            case Ingred_Name.Mushroom:
                return "����";
            case Ingred_Name.Milk:
                return "����";
            case Ingred_Name.Butter:
                return "����";
            case Ingred_Name.Cheese:
                return "ġ��";
            case Ingred_Name.Fruit:
                return "����";
            case Ingred_Name.Greenonion:
                return "��";
            case Ingred_Name.Carrot:
                return "���";
            case Ingred_Name.Beansprout:
                return "����";
            case Ingred_Name.Seaweed:
                return "��";
            case Ingred_Name.Vanilla:
                return "�ٴҶ�";
            case Ingred_Name.Coriander:
                return "���";
            case Ingred_Name.Kimchi:
                return "��ġ";
            case Ingred_Name.Vinegar:
                return "����";
            case Ingred_Name.Tofu:
                return "�κ�";
            case Ingred_Name.Wine:
                return "����";
            case Ingred_Name.Salary:
                return "������";
            case Ingred_Name.Radish:
                return "��";
            case Ingred_Name.Redbean:
                return "��";
            case Ingred_Name.Eggplant:
                return "����";
            case Ingred_Name.Olive:
                return "�ø���";
            case Ingred_Name.Cream:
                return "ũ��";
            case Ingred_Name.Nuts:
                return "�߰���";
            case Ingred_Name.Cucumber:
                return "����";
            case Ingred_Name.Honey:
                return "��";
            case Ingred_Name.Ginger:
                return "����";
            default:
                return "";
        }
    }

    public string EunmToString(Ingred_Grade grade)
    {
        switch (grade)
        {
            case Ingred_Grade.STAR_1:
                return "1��";
            case Ingred_Grade.STAR_2:
                return "2��";
            case Ingred_Grade.STAR_3:
                return "3��";
            case Ingred_Grade.STAR_4:
                return "4��";
            default:
                return "";
        }
    }
}
