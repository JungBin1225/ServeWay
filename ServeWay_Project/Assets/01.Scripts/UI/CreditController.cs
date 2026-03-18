using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditController : MonoBehaviour
{
    private List<GameObject> credits;
    private RectTransform rect;
    private int count;

    void Start()
    {
        count = -1;
        rect = GetComponent<RectTransform>();

        credits = new List<GameObject>();
        for(int i = 1; i < transform.childCount; i++)
        {
            credits.Add(transform.GetChild(i).gameObject);
            credits[i - 1].SetActive(false);
        }
    }

    void Update()
    {
        int now = (int)(rect.anchoredPosition.y - 450) / 200;
        if(now <= -2)
        {
            now = -1;
        }

        if (count != now && count < credits.Count - 1)
        {
            count++;
            if(!credits[count].activeSelf)
            {
                credits[count].SetActive(true);
            }
        }
    }
}
