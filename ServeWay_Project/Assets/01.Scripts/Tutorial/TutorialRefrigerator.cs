using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialRefrigerator : MonoBehaviour
{
    private DataController ingredData;
    private GameObject refrigeUI;
    private InteractionWindow interaction;
    private InventoryManager Inventory;
    private AudioSource audio;
    private List<Ingred_Name> ingredList;
    private List<GameObject> buttonList;
    private bool isTouch;
    private bool isOpen;

    void Start()
    {
        ingredData = FindObjectOfType<DataController>();
        interaction = FindObjectOfType<InteractionWindow>();
        refrigeUI = GameObject.Find("RefrigeratorUI");
        Inventory = FindObjectOfType<InventoryManager>();
        audio = GetComponent<AudioSource>();
        isTouch = false;
        isOpen = false;

        buttonList = new List<GameObject>();
        for(int i = 0; i < refrigeUI.transform.GetChild(0).childCount - 1; i++)
        {
            buttonList.Add(refrigeUI.transform.GetChild(0).GetChild(i).gameObject);
        }

        ingredList = new List<Ingred_Name>();
        ingredList.Add(Ingred_Name.Rice);
        ingredList.Add(Ingred_Name.Kimchi);
        ingredList.Add(Ingred_Name.Rice);
        ingredList.Add(Ingred_Name.Oil);
        ingredList.Add(Ingred_Name.Egg);
        ingredList.Add(Ingred_Name.Rice);

        for(int i = 0; i < buttonList.Count; i++)
        {
            buttonList[i].GetComponent<Image>().sprite = ingredData.FindIngredient(ingredList[i]).sprite;
        }
    }

    void Update()
    {
        if (isTouch && interaction.refrigeratorOpen.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (Time.timeScale == 1)
                {
                    OnRefrigeClicked();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 0 && isOpen)
        {
            OnRefrigeClose();
        }
    }

    public void OnRefrigeClicked()
    {
        Time.timeScale = 0;
        isOpen = true;
        refrigeUI.transform.GetChild(0).gameObject.SetActive(true);
        audio.Play();
    }

    public void OnRefrigeClose()
    {
        isOpen = false;
        refrigeUI.transform.GetChild(0).gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void OnIngredClicked(int index)
    {
        Inventory.GetItem(ingredList[index], 1);
        buttonList[index].SetActive(false);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isTouch = true;
            interaction.SetRefrigeAble(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isTouch = false;
            interaction.SetRefrigeAble(false);
        }
    }
}
