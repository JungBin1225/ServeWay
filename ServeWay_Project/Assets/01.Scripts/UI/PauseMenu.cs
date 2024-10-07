using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    [SerializeField] private GameObject pauseBG;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject pauseMenuBtns;

    [SerializeField] private GameObject optionBG;
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private GameObject optionMenuBtns;

    [SerializeField] private GameObject warningGroup;

    [SerializeField] private AudioSource menuOpen;
    [SerializeField] private AudioSource menuClick;

    private int warningType;  // 0: init, 1: to title, 2: quit game

    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
        warningType = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !optionBG.activeSelf && !optionPanel.activeSelf && !optionMenuBtns.activeSelf)
        {
            Debug.Log("esc");
            if (!isPaused)  // Pause
            {
                if (Time.timeScale == 1)
                {
                    Pause();
                }
            }
            else  // Resume
            {
                Debug.Log("resume");
                Resume();
            }
        }
    }

    void Pause()
    {
        Time.timeScale = 0;
        Debug.Log(Time.timeScale);

        pauseBG.SetActive(true);
        pausePanel.SetActive(true);
        pauseMenuBtns.SetActive(true);

        isPaused = true;
        menuOpen.Play();
    }

    void Resume()
    {
        Debug.Log("resume func start");
        Debug.Log("btn " + pauseMenuBtns.activeSelf);
        Debug.Log("panel " + pausePanel.activeSelf);
        Debug.Log("bg " + pauseBG.activeSelf);
        isPaused = false;
        pauseMenuBtns.SetActive(false);
        pausePanel.SetActive(false);
        pauseBG.SetActive(false);

        Time.timeScale = 1;
        Debug.Log("resume func end");
        Debug.Log("btn " + pauseMenuBtns.activeSelf);
        Debug.Log("panel " + pausePanel.activeSelf);
        Debug.Log("bg " + pauseBG.activeSelf);
    }

    public void ResumeBtn() // ����ϱ�
    {
        Resume();
        menuClick.Play();
    }

    public void OptionBtn() // ����
    {
        menuClick.Play();
        Debug.Log("���� ��ư Ŭ��");

        pauseMenuBtns.SetActive(false);
        pausePanel.SetActive(false);
        pauseBG.SetActive(false);

        optionBG.SetActive(true);
        optionPanel.SetActive(true);
        optionMenuBtns.SetActive(true);
        menuOpen.Play();
    }

    public void ToTitleSceneBtn()   // Ÿ��Ʋȭ������
    {
        menuClick.Play();
        Debug.Log("Ÿ��Ʋȭ������ ��ư Ŭ��");
        warningType = 1;
        popWarningPanel();
    }

    public void QuitGameBtn()   // ��������
    {
        menuClick.Play();
        Debug.Log("�������� ��ư Ŭ��");
        warningType = 2;
        popWarningPanel();
    }

    private void popWarningPanel()
    {
        warningGroup.SetActive(true);
        menuOpen.Play();
    }

    public void WarningConfirmBtn()
    {
        if (warningType == 1)
        {
            warningType = 0;
            SceneManager.LoadScene("TitleScene");
        }
        else if (warningType == 2)
        {
            warningType = 0;
            Application.Quit();
        }

        menuClick.Play();
    }

    public void WarningCancelBtn()
    {
        warningGroup.SetActive(false);
        menuClick.Play();
    }

    public void OnButtonPressed(RectTransform text)
    {
        text.offsetMin -= new Vector2(0, 15);
        text.offsetMax -= new Vector2(0, 15);
        menuClick.Play();
    }

    public void OnButtonRelease(RectTransform text)
    {
        text.offsetMin += new Vector2(0, 15);
        text.offsetMax += new Vector2(0, 15);
        menuClick.Play();
    }

    public void OnWarningPressed(RectTransform text)
    {
        text.offsetMin -= new Vector2(0, 10);
        text.offsetMax -= new Vector2(0, 10);
        menuClick.Play();
    }

    public void OnWarningRelease(RectTransform text)
    {
        text.offsetMin += new Vector2(0, 10);
        text.offsetMax += new Vector2(0, 10);
        menuClick.Play();
    }
}
