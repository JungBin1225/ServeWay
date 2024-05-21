using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Create_Success
{
    FAIL,
    SUCCESS,
    GREAT
};

public class GetItem : MonoBehaviour
{
    private bool getAble;
    private WeaponSlot weaponSlot;
    private float time;
    private PlayerHealth player;
    private DataController dataController;

    public string name;
    public Create_Success success;
    public GameObject weaponPrefab;
    void Start()
    {
        time = 0;
        getAble = false;
        weaponSlot = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().weaponSlot;
        player = FindObjectOfType<PlayerHealth>();
        dataController = FindObjectOfType<DataController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(getAble)
        {
            if(Input.GetKey(KeyCode.F))
            {
                time += Time.deltaTime;
            }
            else
            {
                time = 0;
            }

            if(time >= 2.0f)
            {
                EatWeapon();
            }

            if(Input.GetKeyUp(KeyCode.F))
            {
                GetWeapon();
            }
        }
        else
        {
            time = 0;
        }


        if(Input.GetKeyUp(KeyCode.F))
        {
            if(getAble)
            {
                GetWeapon();
            }
        }
    }

    public void GetWeapon()
    {
        weaponSlot.GetWeapon(weaponPrefab, success, name);
        Destroy(this.gameObject);
    }

    public void EatWeapon()
    {
        FoodData food = dataController.FindFood(name);

        player.PlayerHeal(food.EnumToInt(food.grade));
        Destroy(this.gameObject);

    }

    public void SetSprite()
    {
        GetComponent<SpriteRenderer>().sprite = FindObjectOfType<DataController>().FindFood(name).foodSprite;
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
