using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CreateUI : MonoBehaviour
{
    private List<GameObject> buttonList;
    private List<GameObject> ingredientList;
    private DataController dataController;
    private FoodDataSet foodInfo;
    private StartFoodDataSet startFoodInfo;
    private FoodData selectedFood;
    private IngredientDataSet ingredientInfo;
    private InventoryManager inventory;
    private WeaponSlot weaponSlot;
    private bool isFirst;
    private bool isMaking;
    public bool isOpen;

    public float deltaTime;
    public Create_Success success;
    public Kitchen kitchen;
    public GameObject buttonGroup;
    public GameObject explaneUI;
    public GameObject ingredientUI;
    public List<GameObject> minigameUI;
    public GameObject makingStart;
    public GameObject weaponPrefab;
    public AudioSource menuOpen;
    public AudioSource menuClick;
    public GameObject resultWindow;
    public List<Sprite> resultImage;

    void Start()
    {
        dataController = FindObjectOfType<DataController>();
        foodInfo = dataController.foodData;
        startFoodInfo = dataController.startFoodData;
        ingredientInfo = dataController.IngredientList;
        inventory = FindObjectOfType<InventoryManager>();
        weaponSlot = FindObjectOfType<WeaponSlot>();

        buttonList = new List<GameObject>();
        for (int i = 0; i < buttonGroup.transform.childCount; i++)
        {
            buttonList.Add(buttonGroup.transform.GetChild(i).gameObject);
        }

        ingredientList = new List<GameObject>();
        for(int i = 0; i < ingredientUI.transform.childCount; i++)
        {
            ingredientList.Add(ingredientUI.transform.GetChild(i).gameObject);
        }

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && isOpen && !isMaking)
        {
            CloseUI();
        }
    }

    private void OnEnable()
    {
        isOpen = true;
        isMaking = false;

        if(!isFirst)
        {
            isFirst = true;
        }
        else
        {
            menuOpen.Play();
        }
    }

    private void OnDisable()
    {
        isOpen = false;
        makingStart.SetActive(false);
    }

    public void SetList(List<string> nameList)
    {
        Debug.Log(buttonList.Count);

        for(int i = 0; i < buttonList.Count; i++)
        {
            if(i < nameList.Count)
            {
                buttonList[i].SetActive(true);
                buttonList[i].GetComponentInChildren<TMP_Text>().text = nameList[i];
            }
            else
            {
                buttonList[i].SetActive(false);
            }
        }
    }

    public void OnSelectClicked(TMP_Text text)
    {
        List<FoodData> list;

        explaneUI.SetActive(true);

        if(SceneManager.GetActiveScene().name.Contains("Start"))
        {
            list = startFoodInfo.StartFoodDatas;
        }
        else
        {
            list = foodInfo.FoodDatas;
        }

        menuClick.Play();

        foreach(FoodData food in list)
        {
            if(food.foodName == text.text)
            {
                selectedFood = food;

                explaneUI.transform.GetChild(0).GetComponent<Image>().sprite = food.foodSprite;
                explaneUI.transform.GetChild(1).GetComponent<TMP_Text>().text = food.foodName;
                explaneUI.transform.GetChild(2).GetComponent<TMP_Text>().text = food.EunmToString(food.grade);
                explaneUI.transform.GetChild(3).GetComponent<TMP_Text>().text = food.EunmToString(food.mainIngred);
                explaneUI.transform.GetChild(4).GetComponent<TMP_Text>().text = food.EunmToString(food.nation);

                int miniGameIndex = 0;
                switch(food.mainIngred)
                {
                    case Food_MainIngred.RICE:
                        miniGameIndex = 0;
                        break;
                    case Food_MainIngred.SOUP:
                        miniGameIndex = 1;
                        break;
                    case Food_MainIngred.NOODLE:
                        miniGameIndex = 2;
                        break;
                    case Food_MainIngred.BREAD:
                        miniGameIndex = 3;
                        break;
                    case Food_MainIngred.MEAT:
                        miniGameIndex = 4;
                        break;
                }
                explaneUI.transform.GetChild(5).GetComponent<Button>().onClick.RemoveAllListeners();
                explaneUI.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(delegate () { this.OnCreateClicked(miniGameIndex); });

                for (int i = 0; i < ingredientList.Count; i++)
                {
                    ingredientList[i].SetActive(false);
                }

                int index = 0;

                foreach (Ingred_Name name in food.needIngredient.Keys)
                {
                    ingredientList[index].SetActive(true);
                    ingredientList[index].GetComponent<Image>().sprite = dataController.FindIngredient(name).sprite;
                    ingredientList[index].GetComponentInChildren<TMP_Text>().text = string.Format("X {0}", food.needIngredient[name].ToString());

                    index++;
                }
            }
        }
    }

    public void OnCreateClicked(int index)
    {
        if (SceneManager.GetActiveScene().name.Contains("Start"))
        {
            StartCoroutine(MakeStartFood());
        }
        else
        {
            minigameUI[index].SetActive(true);
            menuClick.Play();
        }  
    }

    private IEnumerator MakeStartFood()
    {
        isMaking = true;
        makingStart.SetActive(true);

        yield return new WaitForSecondsRealtime(5f);

        isMaking = false;
        makingStart.SetActive(false);
        if (weaponSlot.WeaponCount() != 3)
        {
            weaponSlot.GetWeapon(weaponPrefab, Create_Success.SUCCESS, selectedFood.foodName);
        }
        else
        {
            GameObject food = Instantiate(selectedFood.foodPrefab, weaponSlot.SetDropPos(), Quaternion.Euler(0, 0, 0));
            food.GetComponent<GetItem>().name = selectedFood.foodName;
            food.GetComponent<GetItem>().SetSprite();
            food.GetComponent<GetItem>().success = Create_Success.SUCCESS;
        }

        kitchen.StartMaked();
        CloseUI();
    }

    public void OnGameCleared(GameObject gameUI)
    {
        gameUI.SetActive(false);

        if (!SceneManager.GetActiveScene().name.Contains("Start"))
        {
            foreach (Ingred_Name name in selectedFood.needIngredient.Keys)
            {
                inventory.DeleteItem(name, selectedFood.needIngredient[name]);
            }
        }

        if(weaponSlot.WeaponCount() != 3)
        {
            weaponSlot.GetWeapon(weaponPrefab, success, selectedFood.foodName);
        }
        else
        {
            GameObject food = Instantiate(selectedFood.foodPrefab, weaponSlot.SetDropPos(), Quaternion.Euler(0, 0, 0));
            food.GetComponent<GetItem>().name = selectedFood.foodName;
            food.GetComponent<GetItem>().SetSprite();
            food.GetComponent<GetItem>().success = success;
        }

        StartCoroutine(ShowResult());
    }

    private IEnumerator ShowResult()
    {
        isMaking = true;
        resultWindow.SetActive(true);
        switch (success)
        {
            case Create_Success.FAIL:
                resultWindow.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "실패...";
                resultWindow.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_Text>().color = new Color(1, 0, 0);
                resultWindow.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Image>().sprite = resultImage[0];
                break;
            case Create_Success.SUCCESS:
                resultWindow.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "성공!";
                resultWindow.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_Text>().color = new Color(1, 1, 1);
                resultWindow.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Image>().sprite = resultImage[1];
                break;
            case Create_Success.GREAT:
                resultWindow.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = "대성공!!";
                resultWindow.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_Text>().color = new Color(0, 1, 0);
                resultWindow.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Image>().sprite = resultImage[2];
                break;
        }

        yield return new WaitForSecondsRealtime(3f);

        resultWindow.SetActive(false);
        isMaking = false;

        CloseUI();
    }

    public void CloseUI()
    {
        explaneUI.SetActive(false);
        foreach(GameObject minigame in minigameUI)
        {
            minigame.SetActive(false);
        }
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}
