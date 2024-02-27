using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanFlipGame : MonoBehaviour
{
    public GameObject alarm;
    public CreateUI createUI;

    private Create_Success success;
    private bool isOver;
    private bool isClicked;

    private void OnEnable()
    {
        alarm.SetActive(false);
        isOver = false;
        isClicked = false;

        StartCoroutine(GameStart());
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
        yield return new WaitForSeconds(1.0f);

        createUI.success = success;
        createUI.OnGameCleared(this.gameObject);
    }
}
