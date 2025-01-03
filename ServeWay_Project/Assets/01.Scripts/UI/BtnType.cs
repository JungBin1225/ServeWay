using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BtnType : MonoBehaviour//, IPointerEnterHandler//, IPointerExitHandler
{
    public BTNType currentType;
    public Transform buttonScale;
    Vector3 defaultScale;

    public CanvasGroup mainGroup;
    public CanvasGroup optionGroup;
    public CanvasGroup startGroup;

    public AudioSource menuOpen;
    public AudioSource menuClick;

    bool isSound;

    private void Start()
    {
        defaultScale = buttonScale.localScale;
    }
    public void OnBtnClick()
    {
        menuClick.Play();
        switch(currentType)
        {
            case BTNType.Opening:
                GameManager.gameManager.charData.saveFile.Reset();
                GameManager.gameManager.ClearInventory();

                UnityEditor.EditorUtility.SetDirty(GameManager.gameManager.charData.saveFile);

                GameManager.gameManager.SetNextStage("1_OpeningCutScene");
                SceneManager.LoadScene("Loading");
                break;
            case BTNType.Start:
                if(GameManager.gameManager.charData.saveFile.weaponList.Count == 0)
                {
                    GameManager.gameManager.charData.saveFile.Reset();
                    GameManager.gameManager.ClearInventory();

                    UnityEditor.EditorUtility.SetDirty(GameManager.gameManager.charData.saveFile);

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
                CanvasGroupOff(mainGroup);
                CanvasGroupOn(optionGroup);
                menuOpen.Play();
                break;
            case BTNType.Sound:
                break;
            case BTNType.OptionBack:
                CanvasGroupOff(optionGroup);
                CanvasGroupOn(mainGroup);
                break;
            case BTNType.New:
                GameManager.gameManager.charData.saveFile.Reset();
                GameManager.gameManager.ClearInventory();

                UnityEditor.EditorUtility.SetDirty(GameManager.gameManager.charData.saveFile);
                
                if(GameManager.gameManager.charData.saveFile.isTuto)
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
}
