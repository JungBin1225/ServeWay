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
                Debug.Log("������ ����");
                SceneManager.LoadScene("OpeningCutScene");
                break;
            case BTNType.Start:
                Debug.Log("���� ����");
                CanvasGroupOn(startGroup);
                mainGroup.interactable = false;
                mainGroup.blocksRaycasts = false;
                break;
            case BTNType.Option:
                CanvasGroupOff(mainGroup);
                CanvasGroupOn(optionGroup);
                Debug.Log("����");
                break;
            case BTNType.Sound:
                Debug.Log("����_�Ҹ�");
                if (isSound)
                {
                    Debug.Log("���� OFF");
                } else
                {
                    Debug.Log("���� ON");
                }
                isSound = !isSound;
                break;
            case BTNType.OptionBack:
                Debug.Log("���� ȭ�� ������");
                CanvasGroupOff(optionGroup);
                CanvasGroupOn(mainGroup);
                break;
            case BTNType.New:
                Debug.Log("�����ϱ�");
                SceneManager.LoadScene("Test");
                break;
            case BTNType.Continue:
                Debug.Log("�̾��ϱ�");
                break;
            case BTNType.Quit:
                Debug.Log("���� ����");
                Application.Quit();
                break;
            case BTNType.CloseStart:
                Debug.Log("���� ȭ�� �ݱ�");
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
