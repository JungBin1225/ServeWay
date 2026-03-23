using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditController : MonoBehaviour
{
    private List<GameObject> credits;
    private List<float> pos;
    private RectTransform rect;
    private int count;
    private float last;

    void Start()
    {
        count = 0;
        rect = GetComponent<RectTransform>();

        credits = new List<GameObject>();
        pos = new List<float>();

        for(int i = 1; i < transform.childCount; i++)
        {
            credits.Add(transform.GetChild(i).gameObject);
            credits[i - 1].SetActive(false);
        }

        last = credits[credits.Count - 1].GetComponent<RectTransform>().anchoredPosition.y;
        for (int i = 0; i < credits.Count; i++)
        {
            pos.Add(-credits[i].GetComponent<RectTransform>().anchoredPosition.y);
        }
    }

    void Update()
    {
        if (count < credits.Count && pos[count] <= rect.anchoredPosition.y + 500)
        {
            if(!credits[count].activeSelf)
            {
                credits[count].SetActive(true);
                count++;
            }
        }
    }
}
