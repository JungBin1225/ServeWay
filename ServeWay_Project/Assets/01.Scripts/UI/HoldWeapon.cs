using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoldWeapon : MonoBehaviour
{
    public GameObject holdWeapon;
    public GameObject chargeCoolTime;

    void Start()
    {
        chargeCoolTime.SetActive(false);
    }

    void Update()
    {
        
    }

    public void UpdateHoldWeapon(FoodData food)
    {
        holdWeapon.transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1, 1);
        holdWeapon.transform.GetChild(1).GetComponent<Image>().sprite = food.foodSprite;
        holdWeapon.transform.GetChild(2).GetComponent<Text>().text = food.foodName;
    }

    public void UpdateHoldWeapon_Null()
    {
        holdWeapon.transform.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1, 0);
        holdWeapon.transform.GetChild(2).GetComponent<Text>().text = "";
    }

    public IEnumerator ChargeCoolTime(float time)
    {
        chargeCoolTime.SetActive(true);
        chargeCoolTime.GetComponent<Image>().fillAmount = 1;
        chargeCoolTime.transform.GetChild(0).GetComponent<Text>().text = time.ToString("F1");

        float timeTemp = time;
        while(timeTemp > 0)
        {
            timeTemp -= Time.deltaTime;
            chargeCoolTime.transform.GetChild(0).GetComponent<Text>().text = timeTemp.ToString("F1");
            chargeCoolTime.GetComponent<Image>().fillAmount = (timeTemp / time);
            yield return null;
        }
        chargeCoolTime.transform.GetChild(0).GetComponent<Text>().text = "0.0";
        chargeCoolTime.SetActive(false);
    }
}
