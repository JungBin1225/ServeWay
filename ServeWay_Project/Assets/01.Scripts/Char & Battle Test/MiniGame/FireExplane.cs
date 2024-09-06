using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireExplane : MonoBehaviour
{
    public RectTransform targetBar;
    public RectTransform fire;
    public RectTransform rangeButton;

    public Image key;
    public Sprite space;
    public Sprite spacePress;
    public GameObject effect;

    private bool isMove;
    void Start()
    {
        
    }

    private void OnEnable()
    {
        isMove = false;
        key.sprite = space;
        effect.SetActive(false);
        targetBar.anchoredPosition = new Vector3(0, 0, 0);
        fire.anchoredPosition = new Vector3(0, -25, 0);

        StartCoroutine(explane());
    }

    void Update()
    {
        if(isMove)
        {
            if (fire.anchoredPosition.x < 275.0f)
            {
                fire.anchoredPosition += new Vector2(0.6f, 0);
            }
            else
            {
                fire.anchoredPosition = new Vector3(275.0f, 0, 0);
            }

            key.sprite = spacePress;
            effect.SetActive(true);
        }
        else
        {
            if(fire.anchoredPosition.x > 0)
            {
                fire.anchoredPosition -= new Vector2(1.1f, 0);
            }
            else
            {
                fire.anchoredPosition = new Vector3(0, 0, 0);
            }

            key.sprite = space;
            effect.SetActive(false);
        }

        rangeButton.rotation = Quaternion.Euler(0, 0, 180 - fire.anchoredPosition.x * 0.48f);
    }

    private void OnDisable()
    {
        Debug.Log("Disable");
        StopCoroutine(explane());
    }

    IEnumerator explane()
    {
        float time;

        while(true)
        {
            time = 0;

            while (time < 3)
            {
                targetBar.anchoredPosition = new Vector3(200, 0, 0);
                if(targetBar.anchoredPosition.x - fire.anchoredPosition.x > -40)
                {
                    isMove = true;
                }
                else
                {
                    isMove = false;
                }

                yield return new WaitForSecondsRealtime(0.2f);
                time += 0.2f;
            }
            time = 0;

            while(time < 3)
            {
                targetBar.anchoredPosition = new Vector3(100, 0, 0);
                if (targetBar.anchoredPosition.x - fire.anchoredPosition.x > -40)
                {
                    isMove = true;
                }
                else
                {
                    isMove = false;
                }

                yield return new WaitForSecondsRealtime(0.2f);
                time += 0.2f;
            }
        }
    }
}
