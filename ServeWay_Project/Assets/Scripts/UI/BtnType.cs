using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BtnType : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
                SceneManager.LoadScene("SampleScene");
                break;
            case BTNType.Start:
                Debug.Log("���� ����");
                CanvasGroupOn(startGroup);
                mainGroup.interactable = false;
                mainGroup.blocksRaycasts = false;
                break;
            case BTNType.Option:
                CanvasGroupOn(optionGroup);
                CanvasGroupOff(mainGroup);
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
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }
    public void CanvasGroupOff(CanvasGroup cg)
    {
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonScale.localScale = defaultScale * 1.2f;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        buttonScale.localScale = defaultScale;
    }
}
