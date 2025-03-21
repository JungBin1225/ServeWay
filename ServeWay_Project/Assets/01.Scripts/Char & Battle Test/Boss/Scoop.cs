using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoop : MonoBehaviour
{
    private bool isTouch;
    private int xOut;
    private int yOut;

    public string touchedObject;
    public GameObject arrow;
    public RectTransform arrowRect;

    void Start()
    {
        isTouch = false;
        arrowRect = arrow.GetComponent<RectTransform>();

        xOut = 0;
        yOut = 0;
    }

    void Update()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(this.gameObject.transform.position);
        
        if(screenPoint.x <= 0 || screenPoint.x >= 1 || screenPoint.y <= 0 || screenPoint.y >= 1)
        {
            arrow.SetActive(true);

            if(screenPoint.x > 0 && screenPoint.x < 1)
            {
                arrowRect.anchoredPosition = new Vector3(screenPoint.x * Screen.width, arrowRect.anchoredPosition.y, 0);
                xOut = 0;
            }
            else
            {
                if(screenPoint.x <= 0)
                {
                    arrowRect.anchoredPosition = new Vector3(0, arrowRect.anchoredPosition.y, 0);
                }
                else
                {
                    arrowRect.anchoredPosition = new Vector3(Screen.width, arrowRect.anchoredPosition.y, 0);
                }
                xOut = (int)Mathf.Sign(screenPoint.x);
            }

            if (screenPoint.y > 0 && screenPoint.y < 1)
            {
                arrowRect.anchoredPosition = new Vector3(arrowRect.anchoredPosition.x, screenPoint.y * Screen.height, 0);
                yOut = 0;
            }
            else
            {
                if (screenPoint.y <= 0)
                {
                    arrowRect.anchoredPosition = new Vector3(arrowRect.anchoredPosition.x, 0, 0);
                }
                else
                {
                    arrowRect.anchoredPosition = new Vector3(arrowRect.anchoredPosition.x, Screen.height, 0);
                }
                yOut = (int)Mathf.Sign(screenPoint.y);
            }


            switch(yOut)
            {
                case 1:
                    arrowRect.rotation = Quaternion.Euler(0, 0, -xOut * 45);
                    break;
                case 0:
                    arrowRect.rotation = Quaternion.Euler(0, 0, -xOut * 90);
                    break;
                case -1:
                    arrowRect.rotation = Quaternion.Euler(0, 0, 180 + (xOut * 45));
                    break;
            }


        }
        else
        {
            arrow.SetActive(false);
        }
    }

    public bool GetTouch()
    {
        return isTouch;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isTouch && (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Boss"))
        {
            touchedObject = collision.gameObject.tag;
            isTouch = true;
        }
    }
}
