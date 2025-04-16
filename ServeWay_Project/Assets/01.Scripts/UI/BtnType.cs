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

    bool isSound;

    private void Start()
    {

    }

    public void OnBtnClick()
    {
        menuClick.Play();
        switch (currentType)
        {
            case BTNType.Opening:
                GameManager.gameManager.charData.saveFile = new SaveFile();
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();

                GameManager.gameManager.SetNextStage("1_OpeningCutScene");
                SceneManager.LoadScene("Loading");
                break;
            case BTNType.Start:
                if (GameManager.gameManager.charData.saveFile.weaponList.Count == 0)
                {
                    GameManager.gameManager.charData.saveFile = new SaveFile();
                    PlayerPrefs.DeleteAll();
                    PlayerPrefs.Save();

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
                CanvasGroupOff(optionGroup);
                break;
            case BTNType.New:
                GameManager.gameManager.charData.saveFile = new SaveFile();
                PlayerPrefs.DeleteAll();
                PlayerPrefs.Save();

                if (GameManager.gameManager.charData.saveFile.isTuto)
                {
                    GameManager.gameManager.SetNextStage("Tutorial");
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
    }

    public void SetOffFork()
    {
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
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
