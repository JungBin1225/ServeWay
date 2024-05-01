using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BreadGame : MonoBehaviour
{
    public CreateUI createUI;
    public GameObject explanePanel;
    public GameObject gamePanel;

    private Create_Success success;
    private KeyCode targetKey;
    private float time;
    private float score;
    private bool isPlaying;

    private void OnEnable()
    {
        score = 0;
        targetKey = KeyCode.RightWindows;
        time = Time.realtimeSinceStartup;
        isPlaying = false;

        explanePanel.SetActive(true);
        gamePanel.SetActive(false);
    }

    void Update()
    {
        if(isPlaying && Input.GetKeyDown(targetKey))
        {
            score++;
            Debug.Log(score);
        }
    }

    public IEnumerator GameStart()
    {
        explanePanel.SetActive(false);
        gamePanel.SetActive(true);

        float now = Time.realtimeSinceStartup - time;
        float keyNow = Time.realtimeSinceStartup - time;
        float KeyTime = Random.Range(2.0f, 6.0f);

        targetKey = RandomKey();
        gamePanel.transform.GetChild(0).GetComponent<TMP_Text>().text = targetKey.ToString();
        isPlaying = true;

        while ((Time.realtimeSinceStartup - time) - now < 20)
        {
            if((Time.realtimeSinceStartup - time) - keyNow > KeyTime)
            {
                keyNow = Time.realtimeSinceStartup - time;
                KeyTime = Random.Range(2.0f, 6.0f);
                targetKey = RandomKey();
                gamePanel.transform.GetChild(0).GetComponent<TMP_Text>().text = targetKey.ToString();
            }

            yield return null;
        }

        isPlaying = false;
        if (score >= 80)
        {
            success = Create_Success.GREAT;
        }
        else if (score >= 60)
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

    public KeyCode RandomKey()
    {
        int index = Random.Range(0, 27);

        switch(index)
        {
            case 0: return KeyCode.A;
            case 1: return KeyCode.B;
            case 2: return KeyCode.C;
            case 3: return KeyCode.D;
            case 4: return KeyCode.E;
            case 5: return KeyCode.F;
            case 6: return KeyCode.G;
            case 7: return KeyCode.H;
            case 8: return KeyCode.I;
            case 9: return KeyCode.J;
            case 10: return KeyCode.K;
            case 11: return KeyCode.L;
            case 12: return KeyCode.M;
            case 13: return KeyCode.N;
            case 14: return KeyCode.O;
            case 15: return KeyCode.P;
            case 16: return KeyCode.Q;
            case 17: return KeyCode.R;
            case 18: return KeyCode.S;
            case 19: return KeyCode.T;
            case 20: return KeyCode.U;
            case 21: return KeyCode.V;
            case 22: return KeyCode.W;
            case 23: return KeyCode.X;
            case 24: return KeyCode.Y;
            case 25: return KeyCode.Z;
            case 26: return KeyCode.Space;
            default: return KeyCode.Space;
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
