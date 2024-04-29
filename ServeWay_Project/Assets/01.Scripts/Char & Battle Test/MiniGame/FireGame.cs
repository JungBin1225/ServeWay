using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGame : MonoBehaviour
{
    public CreateUI createUI;
    public GameObject explanePanel;
    public GameObject gamePanel;
    public GameObject targetBar;
    public GameObject spaceBar;

    private Create_Success success;
    private float time;
    private float score;
    private bool isPress;
    private float frameTime;

    private void OnEnable()
    {
        score = 0;
        isPress = false;
        time = Time.realtimeSinceStartup;

        explanePanel.SetActive(true);
        gamePanel.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            isPress = true;
        }
        else
        {
            isPress = false;
        }
    }

    public IEnumerator GameStart()
    {
        explanePanel.SetActive(false);
        gamePanel.SetActive(true);

        float now = Time.realtimeSinceStartup - time;
        float barNow = Time.realtimeSinceStartup - time;
        frameTime = Time.realtimeSinceStartup;

        spaceBar.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
        targetBar.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);

        float barTime = Random.Range(4.0f, 7.0f);
        float barLoc = Random.Range(0.0f, 530.0f);

        targetBar.GetComponent<RectTransform>().anchoredPosition = new Vector3(barLoc, 0, 0);

        while ((Time.realtimeSinceStartup - time) - now < 30)
        {
            if(isPress)
            {
                if (spaceBar.GetComponent<RectTransform>().anchoredPosition.x < 580.0f)
                {
                    spaceBar.GetComponent<RectTransform>().anchoredPosition += new Vector2(2.5f, 0);
                }
                else
                {
                    spaceBar.GetComponent<RectTransform>().anchoredPosition = new Vector3(580.0f, 0, 0);
                }
            }
            else
            {
                if(spaceBar.GetComponent<RectTransform>().anchoredPosition.x > 0)
                {
                    spaceBar.GetComponent<RectTransform>().anchoredPosition -= new Vector2(3f, 0);
                }
                else
                {
                    spaceBar.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
                }
            }

            float degree = spaceBar.GetComponent<RectTransform>().anchoredPosition.x - targetBar.GetComponent<RectTransform>().anchoredPosition.x;
            if(degree > 0 && degree < targetBar.GetComponent<RectTransform>().sizeDelta.x)
            {
                score += Time.realtimeSinceStartup - frameTime;
                Debug.Log(score);
            }

            if((Time.realtimeSinceStartup - time) - barNow > barTime)
            {
                barNow = Time.realtimeSinceStartup - time;
                barTime = Random.Range(4.0f, 7.0f);
                barLoc = Random.Range(0.0f, 530.0f);
                targetBar.GetComponent<RectTransform>().anchoredPosition = new Vector3(barLoc, 0, 0);
            }

            frameTime = Time.realtimeSinceStartup;
            yield return null;
        }

        if(score >= 13)
        {
            success = Create_Success.GREAT;
        }
        else if(score >= 8)
        {
            success = Create_Success.SUCCESS;
        }
        else
        {
            success = Create_Success.FAIL;
        }

        Debug.Log(success);
        yield return new WaitForSecondsRealtime(1.0f);

        createUI.success = success;
        createUI.OnGameCleared(this.gameObject);
    }

    public void OnStartClicked()
    {
        StartCoroutine(GameStart());
    }

}
