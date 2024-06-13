using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum Food_Grade
{
    STAR_1,
    STAR_2,
    STAR_3,
    STAR_4
};

public enum Food_MainIngred
{
    MEAT,
    RICE,
    SOUP,
    NOODLE,
    BREAD
};

public enum Food_Nation
{
    KOREA, JAPAN, CHINA, USA,
    FRANCE, ITALY, THAILAND,
    INDONESIA, GERMANY, SPAIN
};

[Serializable]
public class NameAmount : SerializableDictionary<Ingred_Name, int> { }

[Serializable]
public class FoodData
{
    [SerializeField] private string _FoodName;
    [SerializeField] private string _FoodSpriteIndex;
    [SerializeField] private Food_Grade _Grade;
    [SerializeField] private Food_MainIngred _MainIngred;
    [SerializeField] private Food_Nation _Nation;
    [SerializeField] private float _Damage;
    [SerializeField] private float _Speed;
    [SerializeField] private float _CoolTime;
    [SerializeField] private float _SuccessDamage;
    [SerializeField] private float _SuccessSpeed;
    [SerializeField] private float _SuccessCoolTime;
    [SerializeField] private string _AlphaStat;
    [SerializeField] private string _FoodPrefabPath;
    [SerializeField] private string _BulletPrefabPath;
    [SerializeField] private string _NeedIngredient;
    private Sprite _FoodSprite;
    private GameObject _FoodPrefab;
    private GameObject _BulletPrefab;

    public string foodName { get { return _FoodName; } }
    public Food_Grade grade { get { return _Grade; } }
    public Food_MainIngred mainIngred { get { return _MainIngred; } }
    public Food_Nation nation { get { return _Nation; } }
    public float damage { get { return _Damage; } }
    public float speed { get { return _Speed; } }
    public float coolTime { get { return _CoolTime; } }
    public float successDamage { get { return _SuccessDamage; } }
    public float successSpeed { get { return _SuccessSpeed; } }
    public float successCoolTime { get { return _SuccessCoolTime; } }
    public Sprite foodSprite
    {
        get
        {
            UnityEngine.Object[] sprites = AssetDatabase.LoadAllAssetsAtPath("Assets/08.Assets/pixel_food_icons_1.2.1/Pixel_House_Icons_1.2.1-32x.png");
            //Sprite[] sprites = Resources.LoadAll<Sprite>("Assets/Prefabs/MapPrefabs/Backyard - Free/backyard.png");
            _FoodSprite = (Sprite)sprites[int.Parse(_FoodSpriteIndex) + 1];

            return _FoodSprite;
        }
    }
    public List<float> alphaStat
    {
        get
        {
            List<float> _alphaStat = new List<float>();
            string[] stats = _AlphaStat.Split("\n");
            foreach (string stat in stats)
            {
                _alphaStat.Add(float.Parse(stat));
            }

            return _alphaStat;
        }
    }
    public GameObject foodPrefab
    {
        get
        {
            _FoodPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(_FoodPrefabPath, typeof(GameObject));

            return _FoodPrefab;
        }
    }
    public GameObject bulletPrefab
    {
        get
        {
            _BulletPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(_BulletPrefabPath, typeof(GameObject));

            return _BulletPrefab;
        }
    }
    public NameAmount needIngredient
    {
        get
        {
            NameAmount need = new NameAmount();
            string[] ingreds = _NeedIngredient.Split("\n");
            foreach (string ingred in ingreds)
            {
                string[] dic = ingred.Split(",");
                if(dic[0] != "")
                {
                    need.Add((Ingred_Name)Enum.Parse(typeof(Ingred_Name), dic[0]), int.Parse(dic[1]));
                }
            }

            return need;
        }
    }

    public int EnumToInt(Food_Grade grade)
    {
        switch (grade)
        {
            case Food_Grade.STAR_1:
                return 1;
            case Food_Grade.STAR_2:
                return 2;
            case Food_Grade.STAR_3:
                return 3;
            case Food_Grade.STAR_4:
                return 4;
            default:
                return 0;
        }
    }

    public string EunmToString(Food_Grade grade)
    {
        switch (grade)
        {
            case Food_Grade.STAR_1:
                return "1성";
            case Food_Grade.STAR_2:
                return "2성";
            case Food_Grade.STAR_3:
                return "3성";
            case Food_Grade.STAR_4:
                return "4성";
            default:
                return "";
        }
    }

    public string EunmToString(Food_MainIngred ingred)
    {
        switch (ingred)
        {
            case Food_MainIngred.MEAT:
                return "고기";
            case Food_MainIngred.RICE:
                return "쌀";
            case Food_MainIngred.SOUP:
                return "국물";
            case Food_MainIngred.NOODLE:
                return "면";
            case Food_MainIngred.BREAD:
                return "빵";
            default:
                return "";
        }
    }

    public string EunmToString(Food_Nation nation)
    {
        switch (nation)
        {
            case Food_Nation.KOREA:
                return "한국";
            case Food_Nation.JAPAN:
                return "일본";
            case Food_Nation.CHINA:
                return "중국";
            case Food_Nation.USA:
                return "미국";
            case Food_Nation.FRANCE:
                return "프랑스";
            case Food_Nation.ITALY:
                return "이탈리아";
            case Food_Nation.INDONESIA:
                return "인도네시아";
            case Food_Nation.THAILAND:
                return "태국";
            case Food_Nation.GERMANY:
                return "독일";
            case Food_Nation.SPAIN:
                return "스페인";
            default:
                return "";
        }
    }
}
