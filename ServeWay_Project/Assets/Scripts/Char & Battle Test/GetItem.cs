using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItem : MonoBehaviour
{
    private bool getAble;
    private WeaponSlot weaponSlot;

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
        weaponSlot.GetWeapon(weaponPrefab);
        Destroy(this.gameObject);
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
