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
    public GameObject rangeButton;
    public AudioSource audio;

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
        float barLoc = Random.Range(0.0f, 500.0f);

        targetBar.GetComponent<RectTransform>().anchoredPosition = new Vector3(barLoc, 0, 0);

        while ((Time.realtimeSinceStartup - time) - now < 20)
        {
            if(isPress)
            {
                if (spaceBar.GetComponent<RectTransform>().anchoredPosition.x < 575.0f)
                {
                    spaceBar.GetComponent<RectTransform>().anchoredPosition += new Vector2(0.5f, 0);
                }
                else
                {
                    spaceBar.GetComponent<RectTransform>().anchoredPosition = new Vector3(575.0f, 0, 0);
                }
            }
            else
            {
                if(spaceBar.GetComponent<RectTransform>().anchoredPosition.x > 0)
                {
                    spaceBar.GetComponent<RectTransform>().anchoredPosition -= new Vector2(0.8f, 0);
                }
                else
                {
                    spaceBar.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
                }
            }
            rangeButton.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, 180 - spaceBar.GetComponent<RectTransform>().anchoredPosition.x * 0.21f);

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

            audio.volume = spaceBar.GetComponent<RectTransform>().anchoredPosition.x / 600;
            if(audio.volume < 0.1f)
            {
                audio.volume = 0.1f;
            }

            frameTime = Time.realtimeSinceStartup;
            yield return null;
        }

        if(score >= 8)
        {
            success = Create_Success.GREAT;
        }
        else if(score >= 5)
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
        createUI.menuClick.Play();
    }

    public void CloseWindow()
    {
        this.gameObject.SetActive(false);
        createUI.menuClick.Play();
    }
}
