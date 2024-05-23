using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Refrigerator : MonoBehaviour
{
    private DataController ingredData;
    private List<Ingredient> ingredList;
    private Dictionary<Ingredient, bool> refrigeList;
    private GameObject refrigeUI;
    private List<GameObject> imageList;
    private InventoryManager Inventory;
    private bool isTouch;
    private InteractionWindow interaction;


    void Start()
    {
        ingredData = FindObjectOfType<DataController>();
        interaction = FindObjectOfType<InteractionWindow>();
        refrigeUI = GameObject.Find("RefrigeratorUI");
        Inventory = FindObjectOfType<InventoryManager>();
        isTouch = false;

        imageList = new List<GameObject>();
        for(int i = 0; i < refrigeUI.transform.GetChild(0).childCount; i++)
        {
            int index = i;
            if(i < 5)
            {
                imageList.Add(refrigeUI.transform.GetChild(0).GetChild(i).gameObject);
                imageList[i].GetComponent<Button>().onClick.RemoveAllListeners();
                imageList[i].GetComponent<Button>().onClick.AddListener(delegate () { this.OnIngredClicked(imageList[index]); });
            }
            else
            {
                refrigeUI.transform.GetChild(0).GetChild(i).GetComponent<Button>().onClick.AddListener(delegate () { this.OnRefrigeClose(); });
            }
        }

        ingredList = new List<Ingredient>();
        ingredList.AddRange(ingredData.GetGradeList(1));
        ingredList.AddRange(ingredData.GetGradeList(2));

        refrigeList = new Dictionary<Ingredient, bool>();
        for(int i = 0; i < 5; i++)
        {
            int num = Random.Range(0, 2);
            bool exist = false;
            switch (num)
            {
                case 0:
                    exist = false;
                    break;
                case 1:
                    exist = true;
                    break;
            }

            if(i == 0)
            {
                exist = true;
            }
            refrigeList.Add(RandomIngred(), exist);
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

        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 0)
        {
            OnRefrigeClose();
        }
    }

    public Ingredient RandomIngred()
    {
        int index = Random.Range(0, ingredList.Count);
        if(refrigeList.ContainsKey(ingredList[index]))
        {
            return RandomIngred();
        }
        else
        {
            return ingredList[index];
        }
    }

    public void OnRefrigeClicked()
    {
        Time.timeScale = 0;
        refrigeUI.transform.GetChild(0).gameObject.SetActive(true);
        RefreshList();
    }

    public void OnRefrigeClose()
    {
        refrigeUI.transform.GetChild(0).gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void OnIngredClicked(GameObject clicked)
    {
        Ingredient ingred = ingredData.FindIngredient(clicked.GetComponent<Image>().sprite);
        Inventory.GetItem(ingred.name, 1);
        refrigeList[ingred] = false;
        RefreshList();
    }

    public void RefreshList()
    {
        int i = 0;
        foreach (Ingredient ingred in refrigeList.Keys)
        {
            if (refrigeList[ingred])
            {
                imageList[i].SetActive(true);
                imageList[i].GetComponent<Image>().sprite = ingred.sprite;
            }
            else
            {
                imageList[i].SetActive(false);
            }
            i++;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isTouch = true;
            interaction.SetRefrigeAble(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isTouch = false;
            interaction.SetRefrigeAble(false);
        }
    }
}
