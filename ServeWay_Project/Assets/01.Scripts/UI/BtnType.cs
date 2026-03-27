using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BtnType : MonoBehaviour//, IPointerEnterHandler//, IPointerExitHandler
{
    public BTNType currentType;
    Vector3 defaultScale;

    public CanvasGroup mainGroup;
    public CanvasGroup optionGroup;
    public CanvasGroup startGroup;

    public AudioSource menuOpen;
    public AudioSource menuClick;
    public AudioSource potSound;

    public BtnType cutSceneWarn;

    bool isSound;

    private void Start()
    {

    }

    public void OnBtnClick(bool opening)
    {
        menuClick.Play();
        if(opening)
        {
            cutSceneWarn.currentType = BTNType.Opening;
        }
        else
        {
            cutSceneWarn.currentType = BTNType.Ending;
        }

        CanvasGroupOn(startGroup);
        menuOpen.Play();
        mainGroup.interactable = false;
        mainGroup.blocksRaycasts = false;
    }

    public void OnBtnClick()
    {
        menuClick.Play();
        switch (currentType)
        {
            case BTNType.Opening:
                GameManager.gameManager.charData.DeleteAllWithoutTutoSound();

                GameManager.gameManager.SetNextStage("1_OpeningCutScene");
                SceneManager.LoadScene("Loading");
                break;
            case BTNType.Ending:
                GameManager.gameManager.charData.DeleteAllWithoutTutoSound();

                GameManager.gameManager.SetNextStage("EndingCutScene");
                SceneManager.LoadScene("Loading");
                break;

            case BTNType.Start:
                if (GameManager.gameManager.charData.saveFile.weaponList.Count == 0)
                {
                    GameManager.gameManager.charData.DeleteAllWithoutTutoSound();

                    if (GameManager.gameManager.charData.saveFile.isTuto)
                    {
                        GameManager.gameManager.SetNextStage("StartMap");
                    }
                    else
                    {
                        GameManager.gameManager.SetNextStage("1_OpeningCutScene");
                    }
                    SceneManager.LoadScene("Loading");
                }
                else
                {
                    CanvasGroupOn(startGroup);
                    menuOpen.Play();
                    mainGroup.interactable = false;
                    mainGroup.blocksRaycasts = false;
                }
                break;
            case BTNType.Option:
                CanvasGroupOn(optionGroup);
                menuOpen.Play();
                break;
            case BTNType.Sound:
                break;
            case BTNType.OptionBack:
                optionGroup.GetComponent<OptionMenu>().GoBack();
                CanvasGroupOff(optionGroup);

                break;
            case BTNType.New:
                GameManager.gameManager.charData.DeleteAllWithoutTutoSound();

                if (GameManager.gameManager.charData.saveFile.isTuto)
                {
                    GameManager.gameManager.SetNextStage("StartMap");
                }
                else
                {
                    GameManager.gameManager.SetNextStage("1_OpeningCutScene");
                }
                SceneManager.LoadScene("Loading");
                break;
            case BTNType.Continue:
                GameManager.gameManager.SetNextStage("MainTest");
                SceneManager.LoadScene("Loading");
                break;
            case BTNType.Quit:
                Application.Quit();
                break;
            case BTNType.CloseStart:
                CanvasGroupOff(startGroup);
                mainGroup.interactable = true;
                mainGroup.blocksRaycasts = true;
                break;
            case BTNType.Credit:
                SceneManager.LoadScene("CreditScene");
                break;
        }
    }

    public void CanvasGroupOn(CanvasGroup cg)
    {
        switch (currentType)
        {
            case BTNType.Start:
                //EventSystem.current.SetSelectedGameObject(cg.transform.Find("CloseBtn").GetChild(0).gameObject);
                break;
                /*case BTNType.Option:
                    EventSystem.current.SetSelectedGameObject(cg.transform.Find("BackBtn").GetChild(0).gameObject);
                    break;*/
        }
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }
    public void CanvasGroupOff(CanvasGroup cg)
    {
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
        //EventSystem.current.SetSelectedGameObject(GameObject.Find("Canvas").transform.Find("MainMenu").Find("StartBtn").GetChild(0).gameObject);
    }

    /*public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }*/
    /*
    public void OnPointerExit(PointerEventData eventData)
    {
        buttonScale.localScale = defaultScale;
    }
    */
    private void Update()
    {
        /*if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            buttonScale.localScale = defaultScale * 1.2f;
        } else
        {
            buttonScale.localScale = defaultScale;
        }*/
    }

    public void OpenTop()
    {
        RectTransform textObj = gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        RectTransform top = gameObject.transform.GetChild(1).gameObject.GetComponent<RectTransform>();
        TMP_Text text = gameObject.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();

        textObj.anchoredPosition = new Vector3(textObj.anchoredPosition.x, 105, 0);
        top.anchoredPosition = new Vector3(textObj.anchoredPosition.x, 190, 0);
        text.color = new Color(0, 0, 0);

        potSound.Play();
    }

    public void CloseTop()
    {
        RectTransform textObj = gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
        RectTransform top = gameObject.transform.GetChild(1).gameObject.GetComponent<RectTransform>();
        TMP_Text text = gameObject.transform.GetChild(0).gameObject.GetComponent<TMP_Text>();

        textObj.anchoredPosition = new Vector3(textObj.anchoredPosition.x, 0, 0);
        top.anchoredPosition = new Vector3(textObj.anchoredPosition.x, 117.5f, 0);
        text.color = new Color(1, 1, 1);
    }

    public void SetFork()
    {
        gameObject.transform.GetChild(1).gameObject.SetActive(true);

        potSound.Play();
    }

    public void SetOffFork()
    {
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
    }

    public void OpenPot()
    {
        RectTransform top = gameObject.transform.GetChild(1).gameObject.GetComponent<RectTransform>();
        top.anchoredPosition = new Vector3(-16.0f, 70.0f, 0);
        top.rotation = Quaternion.Euler(0, 0, 25);

        potSound.Play();
    }

    public void ClosePot()
    {
        RectTransform top = gameObject.transform.GetChild(1).gameObject.GetComponent<RectTransform>();
        top.anchoredPosition = new Vector3(0.0f, 51.0f, 0);
        top.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void Check(GameObject check)
    {
        check.SetActive(true);
    }

    public void UnCheck(GameObject check)
    {
        check.SetActive(false);
    }
}
