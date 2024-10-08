using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanFlipGame : MonoBehaviour
{
    public GameObject alarm;
    public CreateUI createUI;
    public GameObject explanePanel;
    public GameObject gamePanel;
    public Animator explaneAnim;
    public Animator gameAnim;
    public GameObject warning;
    public GameObject click;
    public GameObject spaceBar;
    public AudioSource audio;

    private Create_Success success;
    private bool isStart;
    private bool isOver;
    private bool isClicked;
    private float time;

    private void OnEnable()
    {
        alarm.SetActive(false);
        isStart = false;
        isOver = false;
        isClicked = false;
        time = Time.realtimeSinceStartup;

        explanePanel.SetActive(true);
        gamePanel.SetActive(false);
        StartCoroutine(ExplaneAnim());
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isStart)
        {
            gameAnim.SetTrigger("Flip");
            isClicked = true;
        }

        if(gameAnim.GetCurrentAnimatorStateInfo(0).IsName("PanFlip"))
        {
            if(audio.volume == 1)
            {
                audio.volume = 0.3f;
            }
        }
        else
        {
            if(audio.volume != 1)
            {
                audio.volume = 1;
            }
        }
    }

    public IEnumerator GameStart()
    {
        explanePanel.SetActive(false);
        gamePanel.SetActive(true);
        isStart = true;

        float setTime = Random.Range(2.0f, 5.0f);
        float now = Time.realtimeSinceStartup - time;

        while(!isOver)
        {
            if((Time.realtimeSinceStartup - time) - now >= setTime)
            {
                alarm.SetActive(true);
            }

            if ((Time.realtimeSinceStartup - time) - now > 10.0f || isClicked)
            {
                isOver = true;
            }

            yield return null;
        }
        alarm.SetActive(false);
        isStart = false;

        float result = (Time.realtimeSinceStartup - time) - now - setTime;
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
        yield return new WaitForSecondsRealtime(2.0f);

        isStart = false;
        createUI.success = success;
        createUI.OnGameCleared(this.gameObject);
    }

    public IEnumerator ExplaneAnim()
    {
        warning.SetActive(false);
        click.SetActive(false);
        spaceBar.GetComponent<Image>().color = new Color(1, 1, 1);

        while (explanePanel.activeSelf)
        {
            yield return new WaitForSecondsRealtime(5f);
            warning.SetActive(true);
            yield return new WaitForSecondsRealtime(0.4f);
            click.SetActive(true);
            spaceBar.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);

            yield return new WaitForSecondsRealtime(0.1f);
            explaneAnim.SetTrigger("Flip");
            yield return new WaitForSecondsRealtime(0.5f);
            warning.SetActive(false);
            click.SetActive(false);
            spaceBar.GetComponent<Image>().color = new Color(1, 1, 1);
        }
    }

    public void CloseWindow()
    {
        this.gameObject.SetActive(false);
        createUI.menuClick.Play();
    }

    public void OnStartClicked()
    {
        StartCoroutine(GameStart());
        createUI.menuClick.Play();
    }
}
