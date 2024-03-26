using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BtnType : MonoBehaviour, IPointerEnterHandler//, IPointerExitHandler
{
    public BTNType currentType;
    public Transform buttonScale;
    Vector3 defaultScale;

    public CanvasGroup mainGroup;
    public CanvasGroup optionGroup;
    public CanvasGroup startGroup;

    bool isSound;

    private void Start()
    {
        defaultScale = buttonScale.localScale;
    }
    public void OnBtnClick()
    {
        switch(currentType)
        {
            case BTNType.Opening:
                Debug.Log("오프닝 시작");
                SceneManager.LoadScene("OpeningCutScene");
                break;
            case BTNType.Start:
                Debug.Log("게임 시작");
                CanvasGroupOn(startGroup);
                mainGroup.interactable = false;
                mainGroup.blocksRaycasts = false;
                break;
            case BTNType.Option:
                CanvasGroupOff(mainGroup);
                CanvasGroupOn(optionGroup);
                Debug.Log("설정");
                break;
            case BTNType.Sound:
                Debug.Log("설정_소리");
                if (isSound)
                {
                    Debug.Log("사운드 OFF");
                } else
                {
                    Debug.Log("사운드 ON");
                }
                isSound = !isSound;
                break;
            case BTNType.OptionBack:
                Debug.Log("설정 화면 나가기");
                CanvasGroupOff(optionGroup);
                CanvasGroupOn(mainGroup);
                break;
            case BTNType.New:
                Debug.Log("새로하기");
                SceneManager.LoadScene("Test");
                break;
            case BTNType.Continue:
                Debug.Log("이어하기");
                break;
            case BTNType.Quit:
                Debug.Log("게임 종료");
                Application.Quit();
                break;
            case BTNType.CloseStart:
                Debug.Log("시작 화면 닫기");
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
                EventSystem.current.SetSelectedGameObject(cg.transform.Find("CloseBtn").GetChild(0).gameObject);
                break;
            case BTNType.Option:
                EventSystem.current.SetSelectedGameObject(cg.transform.Find("BackBtn").GetChild(0).gameObject);
                break;
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
        EventSystem.current.SetSelectedGameObject(GameObject.Find("Canvas").transform.Find("MainMenu").Find("StartBtn").GetChild(0).gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
    /*
    public void OnPointerExit(PointerEventData eventData)
    {
        buttonScale.localScale = defaultScale;
    }
    */
    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            buttonScale.localScale = defaultScale * 1.2f;
        } else
        {
            buttonScale.localScale = defaultScale;
        }
    }
}
