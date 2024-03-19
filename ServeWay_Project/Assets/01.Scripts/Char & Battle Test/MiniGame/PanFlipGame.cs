using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanFlipGame : MonoBehaviour
{
    public GameObject alarm;
    public CreateUI createUI;
    public GameObject explanePanel;
    public GameObject gamePanel;

    private Create_Success success;
    private bool isOver;
    private bool isClicked;

    private void OnEnable()
    {
        alarm.SetActive(false);
        isOver = false;
        isClicked = false;

        explanePanel.SetActive(true);
        gamePanel.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            isClicked = true;
        }
    }

    public IEnumerator GameStart()
    {
        explanePanel.SetActive(false);
        gamePanel.SetActive(true);

        float time = Random.Range(2.0f, 5.0f);
        float now = 0;

        while(!isOver)
        {
            now += createUI.deltaTime;

            if(now >= time)
            {
                alarm.SetActive(true);
            }

            if (now > 10.0f || isClicked)
            {
                isOver = true;
            }

            yield return null;
        }
        alarm.SetActive(false);

        float result = now - time;
        if(result >= 0 && result <= 0.5f)
        {
            success = Create_Success.GREAT;
        }
        else if(result > 0.5f && result <= 1.0f)
        {
            success = Create_Success.SUCCESS;
        }
        else
        {
            success = Create_Success.FAIL;
        }

        //show result
        Debug.Log(success);
        yield return new WaitForSecondsRealtime(1.0f);

        createUI.success = success;
        createUI.OnGameCleared(this.gameObject);
    }

    public void CloseWindow()
    {
        this.gameObject.SetActive(false);
    }

    public void OnStartClicked()
    {
        StartCoroutine(GameStart());
    }
}
