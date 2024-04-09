using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabMenu : MonoBehaviour
{
    public GameObject buttonGroup;
    public List<GameObject> panel;

    private bool isOpen;
    private int index;

    void Start()
    {
        isOpen = false;
        index = 0;
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown(KeyCode.Escape)) && GameManager.gameManager.menuAble)
        {
            if (!isOpen)
            {
                if (Time.timeScale == 1)
                {
                    Time.timeScale = 0;
                    GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
                    buttonGroup.SetActive(true);

                    isOpen = true;
                    index = 0;
                    OpenPanel(index);
                }
            }
            else
            {
                isOpen = false;
                buttonGroup.SetActive(false);
                for(int i = 0; i < buttonGroup.transform.childCount; i++)
                {
                    buttonGroup.transform.GetChild(i).GetComponent<Button>().interactable = false;
                    buttonGroup.transform.GetChild(i).GetComponent<Button>().interactable = true;
                }
                foreach (GameObject menu in panel)
                {
                    menu.SetActive(false);
                }
                GetComponent<Image>().color = new Color(0, 0, 0, 0);
                Time.timeScale = 1;
            }
        }
    }

    public void OpenPanel(int index)
    {
        foreach (GameObject menu in panel)
        {
            menu.SetActive(false);
        }

        switch (index)
        {
            case 0:
                //mini map
                panel[index].SetActive(true);
                break;
            case 1:
                //inventory
                panel[index].SetActive(true);
                break;
            case 2:
                //food&ingred info
                panel[index].SetActive(true);
                break;
        }
    }

    public void OnIndexButtonClicked(int num)
    {
        index = num;
        OpenPanel(num);
    }
}
