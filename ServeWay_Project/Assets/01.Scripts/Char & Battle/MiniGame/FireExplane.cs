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
    public List<GameObject> bubbles;

    private bool isMove;
    void Start()
    {
        
    }

    private void OnEnable()
    {
        isMove = false;
        key.sprite = space;
        effect.SetActive(false);
        targetBar.anchoredPosition = new Vector3(200, 0, 0);
        fire.anchoredPosition = new Vector3(0, -25, 0);

        StartCoroutine(explane());
        foreach (GameObject bubble in bubbles)
        {
            StartCoroutine(bubbleAppear(bubble));
        }
    }

    void Update()
    {
        if(isMove)
        {
            if (fire.anchoredPosition.x < 275.0f)
            {
                fire.anchoredPosition += new Vector2(0.25f, 0);
            }
            else
            {
                fire.anchoredPosition = new Vector3(275.0f, -25, 0);
            }

            key.sprite = spacePress;
            effect.SetActive(true);
        }
        else
        {
            if(fire.anchoredPosition.x > 0)
            {
                fire.anchoredPosition -= new Vector2(0.4f, 0);
            }
            else
            {
                fire.anchoredPosition = new Vector3(0, -25, 0);
            }

            key.sprite = space;
            effect.SetActive(false);
        }

        rangeButton.rotation = Quaternion.Euler(0, 0, 180 - fire.anchoredPosition.x * 0.48f);
    }

    private void OnDisable()
    {
        StopCoroutine(explane());
    }

    IEnumerator explane()
    {
        float time;
        float spacetime;

        while(true)
        {
            time = 0;
            spacetime = 0;

            while (time < 3)
            {
                if(Mathf.Abs(targetBar.anchoredPosition.x - 200) > 0.15f)
                {
                    targetBar.anchoredPosition += new Vector2(0.15f, 0);
                }

                if(spacetime > 0.2f)
                {
                    spacetime = 0;
                    if (targetBar.anchoredPosition.x - fire.anchoredPosition.x > -40)
                    {
                        isMove = true;
                    }
                    else
                    {
                        isMove = false;
                    }
                }

                yield return null;
                time += Time.unscaledDeltaTime;
                spacetime += Time.unscaledDeltaTime;
            }

            time = 0;
            spacetime = 0;

            while (time < 3)
            {
                if (Mathf.Abs(targetBar.anchoredPosition.x - 100) > 0.15f)
                {
                    targetBar.anchoredPosition -= new Vector2(0.15f, 0);
                }

                if (spacetime > 0.2f)
                {
                    spacetime = 0;
                    if (targetBar.anchoredPosition.x - fire.anchoredPosition.x > -40)
                    {
                        isMove = true;
                    }
                    else
                    {
                        isMove = false;
                    }
                }

                yield return null;
                time += Time.unscaledDeltaTime;
                spacetime += Time.unscaledDeltaTime;
            }
        }
    }

    private IEnumerator bubbleAppear(GameObject bubble)
    {
        bubble.SetActive(false);
        float time = Random.Range(0.5f, 2.0f);
        yield return new WaitForSecondsRealtime(time);

        while (true)
        {
            bubble.SetActive(true);
            yield return new WaitForSecondsRealtime(1.5f);

            bubble.SetActive(false);
            time = Random.Range(0.5f, 2.0f);
            yield return new WaitForSecondsRealtime(time);
        }
    }
}
