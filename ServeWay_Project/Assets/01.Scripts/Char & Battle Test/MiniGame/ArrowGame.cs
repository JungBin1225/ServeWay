using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ArrowGame : MonoBehaviour
{
    public CreateUI createUI;
    public GameObject explanePanel;
    public GameObject gamePanel;
    public List<GameObject> arrowList;
    public GameObject timer;

    private Create_Success success;
    private List<KeyCode> arrows;
    private int successAmount;
    private KeyCode pressedKey;
    private bool isStart;
    private bool isPressed;
    private float time;

    private void OnEnable()
    {
        arrows = new List<KeyCode>();
        pressedKey = KeyCode.None;
        isPressed = false;
        isStart = false;
        time = Time.realtimeSinceStartup;

        explanePanel.SetActive(true);
        gamePanel.SetActive(false);
    }

    void Update()
    {
        if(!isPressed && isStart)
        {
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                isPressed = true;
                pressedKey = KeyCode.UpArrow;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                isPressed = true;
                pressedKey = KeyCode.DownArrow;
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                isPressed = true;
                pressedKey = KeyCode.LeftArrow;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                isPressed = true;
                pressedKey = KeyCode.RightArrow;
            }
        }

        if(Time.realtimeSinceStartup - time <= 5)
        {
            timer.GetComponent<TMP_Text>().text = (5 - (Time.realtimeSinceStartup - time)).ToString("F1");
        }
        else
        {
            timer.GetComponent<TMP_Text>().text = "0.0";
        }
    }

    private IEnumerator GameStart()
    {
        explanePanel.SetActive(false);
        gamePanel.SetActive(true);

        successAmount = 0;
        isStart = true;

        for(int i = 0; i < 4; i++)
        {
            time = Time.realtimeSinceStartup;
            arrows.Clear();
            for(int j = 0; j < 5; j++)
            {
                arrows.Add(SelectArrow());
            }

            for(int j = 0; j < 5; j++)
            {
                arrowList[j].SetActive(true);
                arrowList[j].GetComponent<TMP_Text>().text = ArrowToString(arrows[j]);
            }

            for(int j = 0; j < 5; j++)
            {
                yield return new WaitUntil(() => isPressed || Time.realtimeSinceStartup - time > 5);

                if(Time.realtimeSinceStartup - time > 5)
                {
                    isPressed = false;
                    break;
                }

                if(arrows[j] == pressedKey)
                {
                    successAmount++;
                    arrowList[j].GetComponent<Animator>().SetTrigger("Correct");
                }
                else
                {
                    arrowList[j].GetComponent<Animator>().SetTrigger("False");
                }

                isPressed = false;
            }

            yield return new WaitForSecondsRealtime(0.2f);

            for (int j = 0; j < 5; j++)
            {
                arrowList[j].SetActive(false);
            }

        }

        if(successAmount >= 20)
        {
            success = Create_Success.GREAT;
        }
        else if(successAmount < 20 && successAmount >= 15)
        {
            success = Create_Success.SUCCESS;
        }
        else
        {
            success = Create_Success.FAIL;
        }

        Debug.Log(success);
        yield return new WaitForSecondsRealtime(1.0f);

        isStart = false;
        createUI.success = success;
        createUI.OnGameCleared(this.gameObject);
    }

    public KeyCode SelectArrow()
    {
        int num = Random.Range(0, 4);

        switch(num)
        {
            case 0:
                return KeyCode.UpArrow;
            case 1:
                return KeyCode.DownArrow;
            case 2:
                return KeyCode.LeftArrow;
            case 3:
                return KeyCode.RightArrow;
            default:
                return KeyCode.UpArrow;
        }
    }

    public string ArrowToString(KeyCode key)
    {
        switch(key)
        {
            case KeyCode.UpArrow:
                return "↑";
            case KeyCode.DownArrow:
                return "↓";
            case KeyCode.LeftArrow:
                return "←";
            case KeyCode.RightArrow:
                return "→";
            default:
                return "↑";
        }
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
