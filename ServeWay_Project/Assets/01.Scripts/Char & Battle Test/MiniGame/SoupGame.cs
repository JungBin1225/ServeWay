using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SoupGame : MonoBehaviour
{
    public CreateUI createUI;
    public GameObject explanePanel;
    public GameObject gamePanel;
    public GameObject pot;
    public GameObject spoon;
    public GameObject way;
    public GameObject timer;
    public Texture2D cursorInvisible;

    private Create_Success success;
    private bool spoonDown;
    private float time;
    private float score;
    private int lastIndex;
    private List<string> wayPoint;
    private Texture2D cursorImage;

    private void OnEnable()
    {
        score = 0;
        lastIndex = 0;
        spoonDown = false;
        cursorImage = GameManager.gameManager.cursorImage;
        time = Time.realtimeSinceStartup;

        wayPoint = new List<string>();
        for(int i = 0; i < way.transform.childCount; i++)
        {
            wayPoint.Add(way.transform.GetChild(i).gameObject.name);
        }

        spoon.GetComponent<RectTransform>().anchoredPosition = new Vector3(-120, -50, 0);

        explanePanel.SetActive(true);
        gamePanel.SetActive(false);
    }

    void Update()
    {
        if(spoonDown)
        {
            Vector3 mousePos = Input.mousePosition;

            if(mousePos.x < 1115 && mousePos.x > 825 && mousePos.y < 540 && mousePos.y > 305)
            {
                spoon.GetComponent<RectTransform>().transform.position = mousePos;
            }
        }

        if (Time.realtimeSinceStartup - time <= 30)
        {
            timer.GetComponent<TMP_Text>().text = (30 - (Time.realtimeSinceStartup - time)).ToString("F1");
        }
        else
        {
            timer.GetComponent<TMP_Text>().text = "0.0";
        }
    }

    public IEnumerator GameStart()
    {
        explanePanel.SetActive(false);
        gamePanel.SetActive(true);
        time = Time.realtimeSinceStartup;

        yield return new WaitUntil(() => (score >= wayPoint.Count || Time.realtimeSinceStartup - time > 30));

        if (score >= wayPoint.Count - 5)
        {
            success = Create_Success.GREAT;
        }
        else if (score >= wayPoint.Count - 15)
        {
            success = Create_Success.SUCCESS;
        }
        else
        {
            success = Create_Success.FAIL;
        }
        Cursor.SetCursor(cursorImage, new Vector2(0.13f, 0.87f), CursorMode.Auto);
        yield return new WaitForSecondsRealtime(1.0f);

        createUI.success = success;
        createUI.OnGameCleared(this.gameObject);
    }

    public void OnPointerDown()
    {
        Cursor.SetCursor(cursorInvisible, new Vector2(0.13f, 0.87f), CursorMode.Auto);
        spoonDown = true;
    }

    public void OnPointerUp()
    {
        Cursor.SetCursor(cursorImage, new Vector2(0.13f, 0.87f), CursorMode.Auto);
        spoonDown = false;
    }

    public void OnStartClicked()
    {
        StartCoroutine(GameStart());
    }

    public void CloseWindow()
    {
        this.gameObject.SetActive(false);
    }

    public void TriggerPoint(string name)
    {
        int index = 0;
        if(wayPoint.Contains(name))
        {
            index = wayPoint.FindIndex(0, x => x.Equals(name));
            if(index > lastIndex - 1)
            {
                if(lastIndex == 0)
                {
                    if(index == 0)
                    {
                        score++;
                        lastIndex = index + 1;
                        Debug.Log(score);
                    }
                }
                else
                {
                    score++;
                    lastIndex = index + 1;
                    Debug.Log(score);
                }
            }
        }
    }
}
