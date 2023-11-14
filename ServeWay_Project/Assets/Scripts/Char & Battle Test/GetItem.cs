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

    public string name;
    public Create_Success success;
    public GameObject weaponPrefab;
    void Start()
    {
        getAble = false;
        weaponSlot = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().weaponSlot;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
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

    public void SetSprite()
    {
        GetComponent<SpriteRenderer>().sprite = FindObjectOfType<DataController>().FoodInfoList.FindFood(name).foodSprite;
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
