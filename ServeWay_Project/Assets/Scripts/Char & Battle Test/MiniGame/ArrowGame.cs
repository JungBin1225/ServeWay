using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowGame : MonoBehaviour
{
    public CreateUI createUI;
    public GameObject explanePanel;
    public GameObject gamePanel;

    private Create_Success success;
    private List<KeyCode> arrows;
    private int successAmount;
    private KeyCode pressedKey;
    private bool isStart;
    private bool isPressed;

    private void OnEnable()
    {
        arrows = new List<KeyCode>();
        pressedKey = KeyCode.None;
        isPressed = false;
        isStart = false;

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
    }

    private IEnumerator GameStart()
    {
        explanePanel.SetActive(false);
        gamePanel.SetActive(true);

        successAmount = 0;
        isStart = true;

        for(int i = 0; i < 4; i++)
        {
            arrows.Clear();
            for(int j = 0; j < 5; j++)
            {
                arrows.Add(SelectArrow());
            }

            for(int j = 0; j < 5; j++)
            {
                yield return new WaitUntil(() => isPressed);

                if(arrows[j] == pressedKey)
                {
                    successAmount++;
                }
                isPressed = false;
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

    public void OnStartClicked()
    {
        StartCoroutine(GameStart());
    }

    public void CloseWindow()
    {
        this.gameObject.SetActive(false);
    }
}
