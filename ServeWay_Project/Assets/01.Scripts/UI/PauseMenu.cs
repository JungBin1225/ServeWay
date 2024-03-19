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

        pauseBG.SetActive(true);
        pausePanel.SetActive(true);
        pauseMenuBtns.SetActive(true);

        isPaused = true;
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

    public void ResumeBtn() // 계속하기
    {
        Resume();
    }

    public void OptionBtn() // 설정
    {
        Debug.Log("설정 버튼 클릭");

        pauseMenuBtns.SetActive(false);
        pausePanel.SetActive(false);
        pauseBG.SetActive(false);

        optionBG.SetActive(true);
        optionPanel.SetActive(true);
        optionMenuBtns.SetActive(true);
    }

    public void ToTitleSceneBtn()   // 타이틀화면으로
    {
        Debug.Log("타이틀화면으로 버튼 클릭");
        warningType = 1;
        popWarningPanel();
    }

    public void QuitGameBtn()   // 게임종료
    {
        Debug.Log("게임종료 버튼 클릭");
        warningType = 2;
        popWarningPanel();
    }

    private void popWarningPanel()
    {
        warningGroup.SetActive(true);
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
    }

    public void WarningCancelBtn()
    {
        warningGroup.SetActive(false);
    }
}
