using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreadGame : MonoBehaviour
{
    public CreateUI createUI;
    public GameObject explanePanel;
    public GameObject gamePanel;

    private Create_Success success;
    private float time;
    private float score;

    private void OnEnable()
    {
        score = 0;
        time = Time.realtimeSinceStartup;

        explanePanel.SetActive(true);
        gamePanel.SetActive(false);
    }

    void Update()
    {
        
    }

    public IEnumerator GameStart()
    {
        float now = Time.realtimeSinceStartup - time;

        while((Time.realtimeSinceStartup - time) - now < 20)
        {
            yield return null;
        }
    }

    public void OnStartClicked()
    {
        StartCoroutine(GameStart());
    }
}
