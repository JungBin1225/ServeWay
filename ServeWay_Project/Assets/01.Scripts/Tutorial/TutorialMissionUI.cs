using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialMissionUI : MonoBehaviour
{
    public float targetAmout;
    public float nowAmount;

    public TMP_Text text;
    public GameObject check;

    public bool isAppear;

    void Start()
    {
        MissonDisappear();
        isAppear = false;
    }

    void Update()
    {
        
    }

    public void SetMission(string mission, float target)
    {
        targetAmout = target;

        text.text = string.Format(mission, 0, targetAmout.ToString());
    }

    public void UpdateMission(string mission, float amount, bool isInt)
    {
        nowAmount = amount;

        if(nowAmount >= targetAmout)
        {
            nowAmount = targetAmout;
            text.color = new Color(0, 1, 0);
            check.SetActive(true);
        }

        if(isInt)
        {
            text.text = string.Format(mission, nowAmount.ToString("F0"), targetAmout.ToString());
        }
        else
        {
            text.text = string.Format(mission, nowAmount.ToString("F1"), targetAmout.ToString());
        }
    }

    public IEnumerator MissonAppear()
    {
        isAppear = true;
        while (GetComponent<RectTransform>().localPosition.x < -735)
        {
            GetComponent<RectTransform>().localPosition += new Vector3(5, 0, 0);
            yield return null;
        }

        GetComponent<RectTransform>().localPosition = new Vector3(-735, 240, 0);
    }

    public void MissonDisappear()
    {
        isAppear = false;
        GetComponent<RectTransform>().localPosition = new Vector3(-1195, 240, 0);
        text.color = new Color(0, 0, 0);
        check.SetActive(false);
    }
}
