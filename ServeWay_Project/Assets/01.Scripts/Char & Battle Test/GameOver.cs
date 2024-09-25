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

    private WeaponSlot getFood;
    private DataController data;
    private PlayerHealth hp;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnEnable()
    {
        int playTime = (int)(GameManager.gameManager.playTime + Time.timeSinceLevelLoad);
        getFood = FindObjectOfType<WeaponSlot>();
        data = FindObjectOfType<DataController>();
        hp = FindObjectOfType<PlayerHealth>();

        floor.text = string.Format("주소: {0}층", GameManager.gameManager.stage);
        time.text = string.Format("플레이 시간\t\t1\t{0}:{1}", playTime / 60, playTime % 60);
        food.text = string.Format("보유한 음식\t\t{0}", getFood.WeaponCount());
        ingred.text = string.Format("보유한 재료\t\t{0}", GameManager.gameManager.inventory.GetInventoryAmount());

        List<string> foodName = getFood.ReturnWeaponList();
        int i = 0;
        foreach(string food in foodName)
        {
            foodList.transform.GetChild(i).gameObject.SetActive(true);
            foodList.transform.GetChild(i).GetComponent<Image>().sprite = data.FindFood(food).foodSprite;
            i++;
        }
        for(; i < foodList.transform.childCount; i++)
        {
            foodList.transform.GetChild(i).gameObject.SetActive(false);
        }

        List<Sprite> topIngred = GameManager.gameManager.inventory.GetTopIngred();
        i = 0;
        foreach(Sprite ingred in topIngred)
        {
            ingredList.transform.GetChild(i).gameObject.SetActive(true);
            ingredList.transform.GetChild(i).GetComponent<Image>().sprite = ingred;
            i++;
        }
        for (; i < ingredList.transform.childCount; i++)
        {
            ingredList.transform.GetChild(i).gameObject.SetActive(false);
        }

        deathImage.sprite = hp.getDeathImage();
    }

    public void OnConfirm()
    {
        //Time.timeScale = 1;

        GameManager.gameManager.charData.saveFile.Reset();
        GameManager.gameManager.ClearInventory();

        UnityEditor.EditorUtility.SetDirty(GameManager.gameManager.charData.saveFile);

        SceneManager.LoadScene("StartMap");
    }
}
