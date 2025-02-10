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
    private GameObject player;

    void Start()
    {
        //InitSlot();
        dataController = FindObjectOfType<DataController>();
        holdWeapon = FindObjectOfType<HoldWeapon>();
        player = GameObject.FindGameObjectWithTag("Player");
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

    public void SetIndex(int index)
    {
        this.index = index;

        if (weaponList.Count != 0)
        {
            for(int i = 0; i < weaponList.Count; i++)
            {
                if(i == this.index)
                {
                    weaponList[i].SetActive(true);
                    holdWeapon.UpdateHoldWeapon(dataController.FindFood(ReturnWeaponList()[i]));
                }
                else
                {
                    weaponList[i].SetActive(false);
                }
            }
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
            GameObject drop = Instantiate(delete.transform.GetChild(0).GetComponent<WeaponController>().dropPrefab, SetDropPos(), Quaternion.Euler(0, 0, 0));
            drop.GetComponent<GetItem>().name = delete.transform.GetChild(0).GetComponent<WeaponController>().weaponName;
            drop.GetComponent<GetItem>().SetSprite();

            Destroy(delete);
        }
    }

    public Vector3 SetDropPos()
    {
        int mask = 1 << LayerMask.NameToLayer("RayWall");

        Ray2D rayUp = new Ray2D(player.transform.position, Vector2.up);
        Ray2D rayDown = new Ray2D(player.transform.position, Vector2.down);
        Ray2D rayRight = new Ray2D(player.transform.position, Vector2.right);
        Ray2D rayLeft = new Ray2D(player.transform.position, Vector2.left);

        RaycastHit2D hitUp = Physics2D.Raycast(rayUp.origin, rayUp.direction, 1f, mask);
        RaycastHit2D hitDown = Physics2D.Raycast(rayDown.origin, rayDown.direction, 1f, mask);
        RaycastHit2D hitRight = Physics2D.Raycast(rayRight.origin, rayRight.direction, 1f, mask);
        RaycastHit2D hitLeft = Physics2D.Raycast(rayLeft.origin, rayLeft.direction, 1f, mask);

        if(!hitUp)
        {
            return player.transform.position + new Vector3(0, 1, 0);
        }
        else if(!hitDown)
        {
            return player.transform.position + new Vector3(0, -1, 0);
        }
        else if (!hitRight)
        {
            return player.transform.position + new Vector3(1, 0, 0);
        }
        else if (!hitLeft)
        {
            return player.transform.position + new Vector3(-1, 0, 0);
        }
        else
        {
            return player.transform.position + new Vector3(0, 0, 0);
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
                //Debug.Log(weapon.GetComponentInChildren<WeaponController>().name);
                return weapon.GetComponentInChildren<WeaponController>();
            }
        }

        return null;
    }

    public string GetHoldWeapon()
    {
        if(weaponList.Count == 0)
        {
            return null;
        }
        else
        {
            return weaponList[index].GetComponentInChildren<WeaponController>().weaponName;
        }
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
