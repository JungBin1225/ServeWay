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
    public GameObject timer;
    public Texture2D cursorInvisible;
    public List<GameObject> wayPoint;

    private Create_Success success;
    private bool spoonDown;
    private float time;
    private float score;
    private bool isStart;
    private Texture2D cursorImage;

    private void OnEnable()
    {
        score = 0;
        spoonDown = false;
        isStart = false;
        cursorImage = GameManager.gameManager.cursorImage;
        time = Time.realtimeSinceStartup;

        spoon.GetComponent<RectTransform>().anchoredPosition = new Vector3(-120, -50, 0);

        foreach (GameObject way in wayPoint)
        {
            way.SetActive(false);
        }

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

        if(isStart)
        {
            if (Time.realtimeSinceStartup - time <= 15)
            {
                timer.GetComponent<TMP_Text>().text = (15 - (Time.realtimeSinceStartup - time)).ToString("F1");
            }
            else
            {
                timer.GetComponent<TMP_Text>().text = "0.0";
            }
        }
    }

    public IEnumerator GameStart()
    {
        explanePanel.SetActive(false);
        gamePanel.SetActive(true);
        yield return new WaitUntil(() => spoonDown);

        isStart = true;
        time = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup - time < 15)
        {
            float coolTime = Time.realtimeSinceStartup;

            foreach (GameObject way in wayPoint)
            {
                way.SetActive(false);
            }
            yield return null;

            foreach (GameObject way in wayPoint)
            {
                Vector3 pos = new Vector3(Random.Range(835.0f, 1105.0f), Random.Range(315.0f, 530.0f), 0);
                way.SetActive(true);
                way.GetComponent<RectTransform>().transform.position = pos;
                way.GetComponent<Animator>().SetTrigger("bubble");
            }

            yield return new WaitUntil(() => ListAllFalse() || Time.realtimeSinceStartup - coolTime > 2 || Time.realtimeSinceStartup - time >= 15);
        }

        if (score >= 26)
        {
            success = Create_Success.GREAT;
        }
        else if (score >= 17)
        {
            success = Create_Success.SUCCESS;
        }
        else
        {
            success = Create_Success.FAIL;
        }
        Cursor.SetCursor(cursorImage, new Vector2(0.13f, 0.87f), CursorMode.Auto);
        yield return new WaitForSecondsRealtime(1.0f);

        isStart = false;
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
        createUI.menuClick.Play();
    }

    public void CloseWindow()
    {
        this.gameObject.SetActive(false);
        createUI.menuClick.Play();
    }

    public bool ListAllFalse()
    {
        foreach(GameObject way in wayPoint)
        {
            if(way.activeSelf)
            {
                return false;
            }
        }

        return true;
    }

    public void TriggerPoint(GameObject way)
    {
        int index = 0;
        if(wayPoint.Contains(way))
        {
            index = wayPoint.FindIndex(0, x => x.Equals(way));
            way.SetActive(false);
            score++;
            Debug.Log(score);
        }
    }
}
