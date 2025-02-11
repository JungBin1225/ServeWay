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
    public List<Sprite> keyboardList;
    public List<Sprite> pressList;
    public Animator anim;
    public AudioSource audio;
    public List<AudioClip> audioList;
    public TMP_Text timer;

    private Create_Success success;
    private KeyCode targetKey;
    private float time;
    private float score;
    private float now;
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
            anim.SetTrigger(Random.Range(1, 4).ToString());
            audio.clip = audioList[Random.Range(0, 4)];
            audio.Play();

            score++;
            Debug.Log(score);
        }

        if (isPlaying)
        {
            if ((Time.realtimeSinceStartup - time) - now <= 15)
            {
                timer.text = (15 - ((Time.realtimeSinceStartup - time) - now)).ToString("F1");
            }
            else
            {
                timer.text = "0.0";
            }
        }
    }

    public IEnumerator GameStart()
    {
        explanePanel.SetActive(false);
        gamePanel.SetActive(true);

        int index = 0;
        now = Time.realtimeSinceStartup - time;
        float keyNow = Time.realtimeSinceStartup - time;
        float KeyTime = 0;

        int targetNum = 26;
        //int targetNum = Random.Range(0, 27);
        isPlaying = true;

        while ((Time.realtimeSinceStartup - time) - now < 15)
        {
            if((Time.realtimeSinceStartup - time) - keyNow > KeyTime)
            {
                index = 0;
                keyNow = Time.realtimeSinceStartup - time;
                KeyTime = Random.Range(2.0f, 4.5f);
                targetNum = Random.Range(0, 27);
                targetKey = IntToKey(targetNum);

                if(targetKey == KeyCode.Space)
                {
                    gamePanel.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 50);
                    anim.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(-120, 50, 0);
                }
                else
                {
                    gamePanel.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(75, 75);
                    anim.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(-60, 60, 0);
                }
            }

            if (index % 2 == 0)
            {
                gamePanel.transform.GetChild(1).GetComponent<Image>().sprite = keyboardList[targetNum];
            }
            else
            {
                gamePanel.transform.GetChild(1).GetComponent<Image>().sprite = pressList[targetNum];
            }
            index++;
            yield return new WaitForSecondsRealtime(0.1f);
        }

        isPlaying = false;
        if (score >= 55)
        {
            success = Create_Success.GREAT;
        }
        else if (score >= 40)
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

    public KeyCode IntToKey(int num)
    {
        switch(num)
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
        createUI.menuClick.Play();
    }

    public void CloseWindow()
    {
        this.gameObject.SetActive(false);
        createUI.menuClick.Play();
    }
}
