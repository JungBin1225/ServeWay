using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

    public string foodname { get { return _FoodName; } }
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
            UnityEngine.Object[] sprites = AssetDatabase.LoadAllAssetsAtPath("Assets/Prefabs/MapPrefabs/Backyard - Free/backyard.png");
            //Sprite[] sprites = Resources.LoadAll<Sprite>("Assets/Prefabs/MapPrefabs/Backyard - Free/backyard.png");
            foreach(UnityEngine.Object sprite in sprites)
            {
                if(sprite.name.Contains(_FoodSpriteIndex))
                {
                    _FoodSprite = (Sprite)sprite;
                }
            }
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
            _FoodPrefab = Resources.Load(_FoodPrefabPath) as GameObject;

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
                need.Add((IngredientList.IngredientsName)Enum.Parse(typeof(IngredientList.IngredientsName), dic[0]), int.Parse(dic[1]));
            }

            return need;
        }
    }
}
