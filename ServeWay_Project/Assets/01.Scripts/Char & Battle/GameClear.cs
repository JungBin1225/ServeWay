using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameClear : MonoBehaviour
{
    [SerializeField] private TMP_Text time;
    [SerializeField] private TMP_Text food;
    [SerializeField] private TMP_Text ingred;
    [SerializeField] private GameObject foodList;
    [SerializeField] private GameObject ingredList;
    [SerializeField] private GameObject textGroup;
    [SerializeField] private GameObject button;
    [SerializeField] private GameObject license;

    private WeaponSlot getFood;
    private DataController data;
    private int clickCount;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickCount++;
        }
    }

    private void OnEnable()
    {
        getFood = FindObjectOfType<WeaponSlot>();
        data = FindObjectOfType<DataController>();
        clickCount = 0;

        InitText();
        for (int i = 1; i < textGroup.transform.childCount; i++)
        {
            textGroup.transform.GetChild(i).gameObject.SetActive(false);
        }
        button.SetActive(false);

        Time.timeScale = 0;
        StartCoroutine(ShowText());
    }

    private void InitText()
    {
        int playTime = (int)(GameManager.gameManager.playTime);

        time.text = string.Format("플레이 시간\t\t1\t{0}:{1}", playTime / 60, playTime % 60);
        food.text = string.Format("보유한 음식\t\t{0}", getFood.WeaponCount());
        ingred.text = string.Format("보유한 재료\t\t{0}", GameManager.gameManager.inventory.GetInventoryAmount());

        List<string> foodName = getFood.ReturnWeaponList();
        int i = 0;
        foreach (string food in foodName)
        {
            foodList.transform.GetChild(i).gameObject.SetActive(true);
            foodList.transform.GetChild(i).GetComponent<Image>().sprite = data.FindFood(food).foodSprite;
            i++;
        }
        for (; i < foodList.transform.childCount; i++)
        {
            foodList.transform.GetChild(i).gameObject.SetActive(false);
        }

        List<Ingredient> topIngred = GameManager.gameManager.inventory.GetTopIngred();
        i = 0;
        foreach (Ingredient ingred in topIngred)
        {
            ingredList.transform.GetChild(i).gameObject.SetActive(true);
            ingredList.transform.GetChild(i).GetComponent<Image>().sprite = ingred.sprite;
            i++;
        }
        for (; i < ingredList.transform.childCount; i++)
        {
            ingredList.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private IEnumerator ShowText()
    {
        float time = 0.5f;

        for (int i = 1; i < textGroup.transform.childCount; i++)
        {
            if (clickCount >= 2)
            {
                time = 0;
            }
            yield return new WaitForSecondsRealtime(time);
            textGroup.transform.GetChild(i).gameObject.SetActive(true);
        }
        yield return new WaitForSecondsRealtime(time);
        button.SetActive(true);
    }

    public void OnConfirm()
    {
        Time.timeScale = 1;
        bool tuto = GameManager.gameManager.charData.saveFile.isTuto;
        bool ending = GameManager.gameManager.charData.saveFile.isEnding;

        GameManager.gameManager.charData.saveFile = new SaveFile();

        bool bgmMute = false;
        bool sfxMute = false;
        
        float bgmValue = 1;
        float sfxValue = 1;

        if (PlayerPrefs.HasKey("BGM_Sound"))
        {
            bgmMute = bool.Parse(PlayerPrefs.GetString("BGM_Mute"));
            sfxMute = bool.Parse(PlayerPrefs.GetString("SFX_Mute"));

            bgmValue = PlayerPrefs.GetFloat("BGM_Sound");
            sfxValue = PlayerPrefs.GetFloat("SFX_Sound");
        }

        PlayerPrefs.DeleteAll();

        GameManager.gameManager.charData.saveFile.isTuto = tuto;
        GameManager.gameManager.charData.saveFile.isEnding = ending;

        PlayerPrefs.SetString("isTuto", tuto.ToString());
        PlayerPrefs.SetString("isEnding", ending.ToString());
        PlayerPrefs.SetString("BGM_Mute", bgmMute.ToString());
        PlayerPrefs.SetString("SFX_Mute", sfxMute.ToString());
        PlayerPrefs.SetFloat("BGM_Sound", bgmValue);
        PlayerPrefs.SetFloat("SFX_Sound", sfxValue);
        PlayerPrefs.Save();

        if(ending)
        {
            GameManager.gameManager.SetNextStage("TitleScene");
        }
        else
        {
            GameManager.gameManager.SetNextStage("EndingCutScene");
        }
        SceneManager.LoadScene("Loading");
    }

    public void OnLicenseClicked()
    {
        license.SetActive(true);
    }
}
