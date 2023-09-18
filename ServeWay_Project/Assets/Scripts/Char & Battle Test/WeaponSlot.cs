using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSlot : MonoBehaviour
{
    private List<GameObject> weaponList;
    private int index;
    private float changeCooltime;

    void Start()
    {
        weaponList = new List<GameObject>();
        for(int i = 0; i < transform.childCount; i++)
        {
            weaponList.Add(transform.GetChild(i).gameObject);
            if(transform.GetChild(i).gameObject.activeSelf)
            {
                index = i;
            }
        }

        changeCooltime = 0;
    }

    void Update()
    {
        if (Time.timeScale == 0) { return; }

        float wheelInput = Input.GetAxis("Mouse ScrollWheel");
        if(changeCooltime < 0)
        {
            if (wheelInput > 0)
            {
                ChangePrevWeapon();
                changeCooltime = 0.2f;
            }
            else if (wheelInput < 0)
            {
                ChangeNextWeapon();
                changeCooltime = 0.3f;
            }
        }
        else
        {
            changeCooltime -= Time.deltaTime;
        }
    }

    public void ChangeNextWeapon()
    {
        weaponList[index].SetActive(false);

        if(index >= weaponList.Count - 1)
        {
            index = 0;
        }
        else
        {
            index++;
        }

        weaponList[index].SetActive(true);
    }

    public void ChangePrevWeapon()
    {
        weaponList[index].SetActive(false);

        if (index <= 0)
        {
            index = weaponList.Count - 1;
        }
        else
        {
            index--;
        }

        weaponList[index].SetActive(true);
    }

    public void GetWeapon(GameObject newWeapon)
    {
        if(weaponList.Count < 3)
        {
            GameObject weapon = Instantiate(newWeapon, this.transform);
            weaponList.Add(weapon);
            if(!weaponList[index].Equals(weapon))
            {
                weapon.SetActive(false);
            }
        }
        else
        {
            DeleteWeapon();
            GameObject weapon = Instantiate(newWeapon, this.transform);
            weaponList[index] = weapon;
            if (!weaponList[index].Equals(weapon))
            {
                weapon.SetActive(false);
            }
        }

        updateIndex();
    }

    public void DeleteWeapon()
    {
        if(weaponList.Count > 1)
        {
            GameObject delete = weaponList[index];
            weaponList[index] = null;
            GameObject drop = Instantiate(delete.transform.GetChild(0).GetComponent<WeaponController>().dropPrefab, GameObject.FindGameObjectWithTag("Player").transform.position + new Vector3(0, 1, 0), Quaternion.Euler(0, 0, 0));

            Destroy(delete);
        }
    }

    public void updateIndex()
    {
        int i = 0;

        foreach(GameObject weapon in weaponList)
        {
            weapon.transform.SetSiblingIndex(i);
            i++;
        }
    }
}
