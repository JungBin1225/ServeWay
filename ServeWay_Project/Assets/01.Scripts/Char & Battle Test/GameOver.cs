using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
    [SerializeField] private TMP_Text floor;
    [SerializeField] private TMP_Text time;
    [SerializeField] private TMP_Text food;
    [SerializeField] private TMP_Text ingred;
    [SerializeField] private GameObject foodList;
    [SerializeField] private GameObject ingredList;
    [SerializeField] private Image deathImage;
    [SerializeField] private GameObject deathImageGroup;
    [SerializeField] private GameObject textGroup;
    [SerializeField] private GameObject button;

    private WeaponSlot getFood;
    private DataController data;
    private PlayerHealth hp;
    private List<Sprite> enemySprite;
    private int clickCount;

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            clickCount++;
        }
    }

    private void OnEnable()
    {
        getFood = FindObjectOfType<WeaponSlot>();
        data = FindObjectOfType<DataController>();
        hp = FindObjectOfType<PlayerHealth>();
        clickCount = 0;

        InitText();
        for(int i = 1; i < textGroup.transform.childCount; i++)
        {
            textGroup.transform.GetChild(i).gameObject.SetActive(false);
        }
        button.SetActive(false);

        StartCoroutine(ShowText());
    }

    private void InitText()
    {
        int playTime = (int)(GameManager.gameManager.playTime + Time.timeSinceLevelLoad);

        floor.text = string.Format("주소: {0}층", GameManager.gameManager.stage);
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

        enemySprite = hp.getDeathImage();
        if(enemySprite.Count == 4)
        {
            deathImage.gameObject.SetActive(false);
            deathImageGroup.SetActive(true);

            for(int num = 0; num < enemySprite.Count; num++)
            {
                deathImageGroup.transform.GetChild(num).GetComponent<Image>().sprite = enemySprite[num];
            }

            if(enemySprite[1].name.Contains("어1"))
            {
                deathImageGroup.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector3(-7.5f, 24, 0);
                deathImageGroup.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(63, 63);
            }
            else if(enemySprite[1].name.Contains("어2"))
            {
                deathImageGroup.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector3(-7.5f, 25.5f, 0);
                deathImageGroup.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(63, 60);
            }
            else if (enemySprite[1].name.Contains("어3"))
            {
                deathImageGroup.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector3(-12, 24, 0);
                deathImageGroup.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(72, 63);
            }
            else if (enemySprite[1].name.Contains("어4"))
            {
                deathImageGroup.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector3(-9, 25.5f, 0);
                deathImageGroup.transform.GetChild(1).GetComponent<RectTransform>().sizeDelta = new Vector2(66, 66);
            }
        }
        else
        {
            deathImage.gameObject.SetActive(true);
            deathImageGroup.SetActive(false);

            deathImage.sprite = enemySprite[0];
        }
        
    }

    private IEnumerator ShowText()
    {
        float time = 0.5f;

        for (int i = 1; i < textGroup.transform.childCount; i++)
        {
            if(clickCount >= 2)
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
        //Time.timeScale = 1;

        GameManager.gameManager.charData.saveFile = new SaveFile();
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        SceneManager.LoadScene("StartMap");
    }
}
