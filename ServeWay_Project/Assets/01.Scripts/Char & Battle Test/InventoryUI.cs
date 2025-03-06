using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public Sprite defaultSprite;
    public GameObject info_Food;
    public GameObject info_Ingred;
    public Animator bgAnim;
    public AudioSource flipAudio;

    private List<GameObject> inventoryButtonList_0;
    private List<GameObject> inventoryButtonList_1;
    private List<GameObject> foodImageList;
    private InventoryManager inventory;
    private IngredientDataSet itemList;
    private DataController foodInfo;
    private List<string> weaponName;
    private List<Ingred_Name> ingredients;
    private int page;
    private bool interAble;
    private TabMenu tabMenu;
    private AudioSource clickAudio;

    void Start()
    {

    }

    private void OnEnable()
    {
        inventory = FindObjectOfType<InventoryManager>();
        itemList = FindObjectOfType<DataController>().IngredientList;
        foodInfo = FindObjectOfType<DataController>();
        weaponName = FindObjectOfType<WeaponSlot>().ReturnWeaponList();
        tabMenu = transform.parent.GetComponent<TabMenu>();
        clickAudio = GetComponent<AudioSource>();

        InitIngredList();

        foodImageList = new List<GameObject>();
        for (int i = 0; i < transform.GetChild(0).GetChild(0).childCount; i++)
        {
            foodImageList.Add(transform.GetChild(0).GetChild(0).GetChild(i).gameObject);
        }

        inventoryButtonList_0 = new List<GameObject>();
        for (int i = 0; i < transform.GetChild(0).GetChild(1).childCount; i++)
        {
            inventoryButtonList_0.Add(transform.GetChild(0).GetChild(1).GetChild(i).gameObject);
        }

        inventoryButtonList_1 = new List<GameObject>();
        for (int i = 0; i < transform.GetChild(1).childCount; i++)
        {
            inventoryButtonList_1.Add(transform.GetChild(1).GetChild(i).gameObject);
        }

        page = 0;
        interAble = true;

        InitPage(page);
    }

    private void OnDisable()
    {
        info_Food.SetActive(false);
        info_Ingred.SetActive(false);
        interAble = true;
    }

    void Update()
    {
        
    }

    public void InitIngredList()
    {
        ingredients = new List<Ingred_Name>();
        foreach (Ingred_Name item in inventory.inventory.Keys)
        {
            ingredients.Add(item);
        }
    }

    public void InitPage(int page)
    {
        if (page == 0)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).gameObject.SetActive(false);

            InitFood();
            InitIngred_0();
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);

            InitIngred_1();
        }
    }

    public void InitFood()
    {
        int i = 0;
        foreach(string name in weaponName)
        {
            foodImageList[i].SetActive(true);
            foodImageList[i].transform.GetChild(0).GetComponent<Image>().sprite = foodInfo.FindFood(name).foodSprite;

            i++;
        }

        if(i < foodImageList.Count)
        {
            for(; i < foodImageList.Count; i++)
            {
                foodImageList[i].SetActive(false);
            }
        }
    }

    public void InitIngred_0()
    {
        InitIngredList();

        for (int j = 0; j < inventoryButtonList_0.Count; j++)
        {
            inventoryButtonList_0[j].SetActive(true);
            inventoryButtonList_0[j].transform.GetChild(1).gameObject.SetActive(false);
            inventoryButtonList_0[j].transform.GetChild(0).GetComponent<Image>().sprite = defaultSprite;
        }

        int i = 0;
        for(; i < inventoryButtonList_0.Count; i++)
        {
            if (ingredients.Count <= i)
            {
                inventoryButtonList_0[i].SetActive(false);
            }
            else
            {
                inventoryButtonList_0[i].SetActive(true);
                inventoryButtonList_0[i].transform.GetChild(1).gameObject.SetActive(true);
                inventoryButtonList_0[i].transform.GetChild(0).GetComponent<Image>().sprite = foodInfo.FindIngredient(ingredients[i]).sprite;
                inventoryButtonList_0[i].transform.GetChild(1).GetComponent<Text>().text = inventory.inventory[ingredients[i]].ToString();
            }
        }
    }

    public void InitIngred_1()
    {
        InitIngredList();

        for (int j = 0; j < inventoryButtonList_1.Count; j++)
        {
            inventoryButtonList_1[j].SetActive(true);
            inventoryButtonList_1[j].transform.GetChild(1).gameObject.SetActive(false);
            inventoryButtonList_1[j].transform.GetChild(0).GetComponent<Image>().sprite = defaultSprite;
        }

        int i = 0;
        if (ingredients.Count > ((page - 1) * 8) + inventoryButtonList_0.Count)
        {
            for(; i < inventoryButtonList_1.Count; i++)
            {
                if (ingredients.Count <= ((page - 1) * 8) + inventoryButtonList_0.Count + i)
                {
                    inventoryButtonList_1[i].SetActive(false);
                }
                else
                {
                    inventoryButtonList_1[i].SetActive(true);
                    inventoryButtonList_1[i].transform.GetChild(1).gameObject.SetActive(true);
                    inventoryButtonList_1[i].transform.GetChild(0).GetComponent<Image>().sprite = foodInfo.FindIngredient(ingredients[((page - 1) * 8) + inventoryButtonList_0.Count + i]).sprite;
                    inventoryButtonList_1[i].transform.GetChild(1).GetComponent<Text>().text = inventory.inventory[ingredients[((page - 1) * 8) + inventoryButtonList_0.Count + i]].ToString();
                }
            }
        }
    }
    
    public void OnPageClicked(int page)
    {
        if(interAble)
        {
            if (page < 0)
            {
                if (this.page > 0)
                {
                    this.page -= 1;
                    StartCoroutine(FlipPage(-1));
                }
            }
            else
            {
                if (this.page < ((ingredients.Count - 5) / 8 + 1) && ingredients.Count > 4)
                {
                    this.page += 1;
                    StartCoroutine(FlipPage(1));
                }
            }
        }
    }

    private IEnumerator FlipPage(int dir)
    {
        interAble = false;
        tabMenu.interAble = false;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);

        float time = 0;
        if (dir < 0)
        {
            bgAnim.SetTrigger("FlipRight");
            flipAudio.Play();
            foreach (AnimationClip clip in bgAnim.runtimeAnimatorController.animationClips)
            {
                if (clip.name.Equals("FlipRight"))
                {
                    time = clip.length;
                }
            }
        }
        else
        {
            bgAnim.SetTrigger("FlipLeft");
            flipAudio.Play();
            foreach (AnimationClip clip in bgAnim.runtimeAnimatorController.animationClips)
            {
                if (clip.name.Equals("FlipLeft"))
                {
                    time = clip.length;
                }
            }
        }

        yield return new WaitForSecondsRealtime(time);

        transform.GetChild(2).gameObject.SetActive(true);

        InitPage(this.page);
        interAble = true;
        tabMenu.interAble = true;
    }

    public void OnInfoOpenClicked(Image image)
    {
        clickAudio.Play();
        if (image.sprite != defaultSprite)
        {
            if(foodInfo.FindFood(image.sprite) != null)
            {
                info_Food.SetActive(true);
                FoodData food = foodInfo.FindFood(image.sprite);
                Create_Success success = FindObjectOfType<WeaponSlot>().GetWeaponInfo(int.Parse(image.transform.parent.gameObject.name[image.transform.parent.gameObject.name.Length - 1].ToString()) - 1).success;
                string success_D = "";
                string success_S = "";
                string success_C = "";
                Color color = new Color(0, 0, 0);

                switch (success)
                {
                    case Create_Success.FAIL:
                        success_D = "-" + food.successDamage.ToString();
                        success_S = "-" + food.successSpeed.ToString();
                        success_C = "+" + food.successCoolTime.ToString();
                        color = new Color(1, 0, 0);
                        break;
                    case Create_Success.SUCCESS:
                        success_D = "+0";
                        success_S = "+0";
                        success_C = "-0";
                        color = new Color(1, 1, 1);
                        break;
                    case Create_Success.GREAT:
                        success_D = "+" + food.successDamage.ToString();
                        success_S = "+" + food.successSpeed.ToString();
                        success_C = "-" + food.successCoolTime.ToString();
                        color = new Color(0, 1, 0);
                        break;
                }

                info_Food.transform.GetChild(3).GetChild(7).GetComponent<TMP_Text>().color = color;
                info_Food.transform.GetChild(3).GetChild(8).GetComponent<TMP_Text>().color = color;
                info_Food.transform.GetChild(3).GetChild(9).GetComponent<TMP_Text>().color = color;

                info_Food.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = food.foodSprite;
                info_Food.transform.GetChild(3).GetChild(0).GetComponent<TMP_Text>().text = food.foodName;
                info_Food.transform.GetChild(3).GetChild(1).GetComponent<TMP_Text>().text = food.EunmToString(food.grade);
                info_Food.transform.GetChild(3).GetChild(2).GetComponent<TMP_Text>().text = food.EunmToString(food.mainIngred);
                info_Food.transform.GetChild(3).GetChild(3).GetComponent<TMP_Text>().text = food.EunmToString(food.nation);
                info_Food.transform.GetChild(3).GetChild(4).GetComponent<TMP_Text>().text = food.damage.ToString();
                info_Food.transform.GetChild(3).GetChild(5).GetComponent<TMP_Text>().text = food.speed.ToString();
                info_Food.transform.GetChild(3).GetChild(6).GetComponent<TMP_Text>().text = food.coolTime.ToString();
                info_Food.transform.GetChild(3).GetChild(7).GetComponent<TMP_Text>().text = string.Format("({0})", success_D);
                info_Food.transform.GetChild(3).GetChild(8).GetComponent<TMP_Text>().text = string.Format("({0})", success_S);
                info_Food.transform.GetChild(3).GetChild(9).GetComponent<TMP_Text>().text = string.Format("({0})", success_C);
            }
            else
            {
                info_Ingred.SetActive(true);
                Ingredient ingred = foodInfo.FindIngredient(image.sprite);

                info_Ingred.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = ingred.sprite;
                info_Ingred.transform.GetChild(3).GetChild(0).GetComponent<TMP_Text>().text = ingred.EnumToString(ingred.name);
                info_Ingred.transform.GetChild(3).GetChild(1).GetComponent<TMP_Text>().text = ingred.EunmToString(ingred.grade);
                info_Ingred.transform.GetChild(3).GetChild(2).GetComponent<TMP_Text>().text = ingred.passive;
                info_Ingred.transform.GetChild(3).GetChild(3).gameObject.SetActive(false);

                if (ingred.name == Ingred_Name.Cream)
                {
                    string[] passive = ingred.passive.Split('\n');
                    string text1 = passive[0];
                    string text2 = passive[1] + "\n" + passive[2];

                    info_Ingred.transform.GetChild(3).GetChild(2).gameObject.SetActive(true);
                    info_Ingred.transform.GetChild(3).GetChild(2).GetComponent<TMP_Text>().text = text1;
                    info_Ingred.transform.GetChild(3).GetChild(3).gameObject.SetActive(true);
                    info_Ingred.transform.GetChild(3).GetChild(3).GetComponent<TMP_Text>().text = text2;
                }
            }
        }
    }

    public void OnInfoCloseClicked()
    {
        clickAudio.Play();
        info_Food.SetActive(false);
        info_Ingred.SetActive(false);
    }

    /*public void OnButtonClicked(int num)
    {
        IngredientList.IngredientsName name;

        foreach(Ingredient item in itemList.ingredientList)
        {
            Image image = inventoryButtonList_0[num].GetComponent<Image>();

            if(page == 0)
            {
                image = inventoryButtonList_0[num].GetComponent<Image>();
            }
            else
            {
                image = inventoryButtonList_1[num].GetComponent<Image>();
            }

            if(item.sprite == image.sprite)
            {
                name = item.name;
                inventory.DeleteItem(name, 1);
                InitPage(this.page);
                break;
            }
        }
    }*/
}
