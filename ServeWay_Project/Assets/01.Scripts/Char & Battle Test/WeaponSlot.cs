using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponSlot : MonoBehaviour
{
    private List<GameObject> weaponList;
    public int index;
    private float changeCooltime;
    private DataController dataController;
    private HoldWeapon holdWeapon;

    void Start()
    {
        //InitSlot();
        dataController = FindObjectOfType<DataController>();
        holdWeapon = FindObjectOfType<HoldWeapon>();
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
        if(weaponList.Count > 1)
        {
            weaponList[index].SetActive(false);

            if (index >= weaponList.Count - 1)
            {
                index = 0;
            }
            else
            {
                index++;
            }

            weaponList[index].SetActive(true);
            holdWeapon.UpdateHoldWeapon(dataController.FindFood(ReturnWeaponList()[index]));
        }
    }

    public void ChangePrevWeapon()
    {
        if (weaponList.Count != 0)
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
            holdWeapon.UpdateHoldWeapon(dataController.FindFood(ReturnWeaponList()[index]));
        }
    }

    public void GetWeapon(GameObject newWeapon, Create_Success success, string name)
    {
        if(weaponList.Count < 3)
        {
            GameObject weapon = Instantiate(newWeapon, this.transform);
            weapon.GetComponentInChildren<WeaponController>().success = success;
            weapon.GetComponentInChildren<WeaponController>().weaponName = name;
            weapon.GetComponentInChildren<WeaponController>().InitWeapon();
            weaponList.Add(weapon);

            weapon.SetActive(false);
            weaponList[index].SetActive(false);

            index = weaponList.Count - 1;
            weaponList[index].SetActive(true);
            holdWeapon.UpdateHoldWeapon(dataController.FindFood(ReturnWeaponList()[index]));
        }
        else
        {
            DeleteWeapon();
            GameObject weapon = Instantiate(newWeapon, this.transform);
            weapon.GetComponentInChildren<WeaponController>().success = success;
            weapon.GetComponentInChildren<WeaponController>().weaponName = name;
            weapon.GetComponentInChildren<WeaponController>().InitWeapon();
            weaponList[index] = weapon;
            if (!weaponList[index].Equals(weapon))
            {
                weapon.SetActive(false);
            }
            else
            {
                weapon.SetActive(true);
            }
            holdWeapon.UpdateHoldWeapon(dataController.FindFood(ReturnWeaponList()[index]));
        }

        dataController.FoodIngredDex.UpdateFoodDex(name, FoodDex_Status.CREATED);
        updateIndex();
    }

    public void DeleteWeapon()
    {
        if(weaponList.Count > 1)
        {
            GameObject delete = weaponList[index];
            weaponList[index] = null;
            GameObject drop = Instantiate(delete.transform.GetChild(0).GetComponent<WeaponController>().dropPrefab, GameObject.FindGameObjectWithTag("Player").transform.position + new Vector3(0, 1, 0), Quaternion.Euler(0, 0, 0));
            drop.GetComponent<GetItem>().name = delete.transform.GetChild(0).GetComponent<WeaponController>().weaponName;
            drop.GetComponent<GetItem>().SetSprite();

            Destroy(delete);
        }
    }

    public void DeleteWeapon(GameObject weapon)
    {
        if(weaponList.Contains(weapon))
        {
            weaponList.Remove(weapon);
            Destroy(weapon);
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

        holdWeapon.UpdateHoldWeapon(dataController.FindFood(ReturnWeaponList()[index]));
    }

    public List<string> ReturnWeaponList()
    {
        List<string> nameList = new List<string>();
        foreach(GameObject weapon in weaponList)
        {
            nameList.Add(weapon.GetComponentInChildren<WeaponController>().weaponName);
        }

        return nameList;
    }

    public WeaponController GetWeaponInfo(string name)
    {
        foreach(GameObject weapon in weaponList)
        {
            if(weapon.GetComponentInChildren<WeaponController>().weaponName == name)
            {
                Debug.Log(weapon.GetComponentInChildren<WeaponController>().name);
                return weapon.GetComponentInChildren<WeaponController>();
            }
        }

        return null;
    }

    public string GetHoldWeapon()
    {
        return weaponList[index].GetComponentInChildren<WeaponController>().weaponName;
    }

    public void InitSlot()
    {
        weaponList = new List<GameObject>();

        index = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            weaponList.Add(transform.GetChild(i).gameObject);
            if (i == 0)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        if(weaponList.Count != 0 && !SceneManager.GetActiveScene().name.Contains("Tutorial"))
        {
            holdWeapon.UpdateHoldWeapon(dataController.FindFood(ReturnWeaponList()[index]));
        }
        else
        {
            holdWeapon.UpdateHoldWeapon_Null();
        }
    }

    public int WeaponCount()
    {
        return weaponList.Count;
    }
}
