using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextDialog : MonoBehaviour
{
    public RectTransform speechArea;
    public RectTransform clickUI;

    private OpeningSignal timeLine;
    private EndingController ending;

    private void OnEnable()
    {
        timeLine = FindObjectOfType<OpeningSignal>();
        ending = FindObjectOfType<EndingController>();
        clickUI.gameObject.SetActive(false);
    }

    void Start()
    {

    }

    void Update()
    {
        if (timeLine != null)
        {
            Opening();
        }
        else if(ending != null)
        {
            Ending();
        }

        float posX = speechArea.anchoredPosition.x + ((speechArea.sizeDelta.x * (1 - speechArea.pivot.x)) - 25.0f);
        float posY = speechArea.anchoredPosition.y + ((speechArea.sizeDelta.y * (1 - speechArea.pivot.y)) - (speechArea.sizeDelta.y - 32.0f));
        clickUI.anchoredPosition = new Vector2(posX, posY);
    }

    private void Opening()
    {
        if (timeLine.GetClickAble())
        {
            clickUI.gameObject.SetActive(true);
        }
        else
        {
            clickUI.gameObject.SetActive(false);
        }
    }

    private void Ending()
    {
        if (ending.GetClickAble())
        {
            clickUI.gameObject.SetActive(true);
        }
        else
        {
            clickUI.gameObject.SetActive(false);
        }
    }
}
