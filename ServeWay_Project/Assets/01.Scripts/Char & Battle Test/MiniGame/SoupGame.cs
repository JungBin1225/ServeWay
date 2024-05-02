using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoupGame : MonoBehaviour
{
    public CreateUI createUI;
    public GameObject explanePanel;
    public GameObject gamePanel;
    public GameObject pot;
    public GameObject spoon;
    public GameObject nowSpeed;
    public GameObject targetSpeed;

    private Create_Success success;
    private bool spoonDown;
    private float time;
    private float score;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    private void OnEnable()
    {
        score = 0;
        spoonDown = false;
        time = Time.realtimeSinceStartup;

        minX = pot.GetComponent<RectTransform>().transform.position.x - (pot.GetComponent<RectTransform>().sizeDelta.x / 2);
        maxX = pot.GetComponent<RectTransform>().transform.position.x + (pot.GetComponent<RectTransform>().sizeDelta.x / 2);
        minY = pot.GetComponent<RectTransform>().transform.position.y - (pot.GetComponent<RectTransform>().sizeDelta.y / 2);
        maxY = pot.GetComponent<RectTransform>().transform.position.y + (pot.GetComponent<RectTransform>().sizeDelta.y / 2);

        explanePanel.SetActive(true);
        gamePanel.SetActive(false);
    }

    void Update()
    {
        if(spoonDown)
        {
            Vector3 mousePos = Input.mousePosition;

            if(mousePos.x >= minX && mousePos.x <= maxX && mousePos.y >= minY && mousePos.y <= maxY)
            {
                spoon.GetComponent<RectTransform>().transform.position = mousePos;
            }
        }
    }

    public IEnumerator GameStart()
    {
        explanePanel.SetActive(false);
        gamePanel.SetActive(true);

        float now = Time.realtimeSinceStartup - time;
        Vector3 prevLocation = spoon.GetComponent<RectTransform>().transform.position;
        float degree = 0;
        float zeroCount = 0;
        float targetDegree = Random.Range(3.0f, 5.5f);
        List<float> degreeList = new List<float>();

        targetSpeed.GetComponent<RectTransform>().anchoredPosition = new Vector3(-200 + (targetDegree * 40), 300, 0);

        while ((Time.realtimeSinceStartup - time) - now < 40)
        {
            degreeList.Add((prevLocation - spoon.GetComponent<RectTransform>().transform.position).magnitude);
            if(degreeList.Count == 5)
            {
                if(degreeList.TrueForAll(isZero))
                {
                    degree = 0;
                }
                else
                {
                    foreach(float num in degreeList)
                    {
                        if(num != 0)
                        {
                            degree += num;
                            zeroCount++;
                        }
                    }

                    degree /= zeroCount;
                }

                nowSpeed.GetComponent<RectTransform>().anchoredPosition = new Vector3(-200 + (degree * 40), 300, 0);
                if(degree <= targetDegree - 1.5f)
                {
                    Debug.Log("Late");
                    //타는 이펙트
                }
                else if(degree >= targetDegree + 1.5f)
                {
                    Debug.Log("Fast");
                    //튀는 이펙트
                }
                else
                {
                    score++;
                    Debug.Log(score);
                }

                degreeList.Clear();
                zeroCount = 0;
                degree = 0;
            }
            
            prevLocation = spoon.GetComponent<RectTransform>().transform.position;
            yield return null;
        }

        if (score >= 1200)
        {
            success = Create_Success.GREAT;
        }
        else if (score >= 800)
        {
            success = Create_Success.SUCCESS;
        }
        else
        {
            success = Create_Success.FAIL;
        }

        yield return new WaitForSecondsRealtime(1.0f);

        createUI.success = success;
        createUI.OnGameCleared(this.gameObject);
    }

    private bool isZero(float num)
    {
        return num == 0;
    }

    public void OnPointerDown()
    {
        spoonDown = true;
    }

    public void OnPointerUp()
    {
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
}
