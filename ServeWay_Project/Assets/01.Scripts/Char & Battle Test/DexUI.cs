using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class DexUI : MonoBehaviour
{
    private DataController dataController;
    private int dexMod;
    private List<string> foodList;
    private List<Ingred_Name> ingredientList;
    private List<GameObject> buttonList;
    private int page;
    private bool interAble;
    private TabMenu tabMenu;
    private AudioSource clickAudio;

    public Sprite lockSprite;
    public GameObject buttonGruop;
    public GameObject infoWindow;
    public Material defultMaterial;
    public Material grayScale;
    public Animator bgAnim;
    public AudioSource flipAudio;

    void Start()
    {
        /*dataController = FindObjectOfType<DataController>();
        foodDex = transform.GetChild(0).gameObject;
        ingredDex = transform.GetChild(1).gameObject;*/
    }

    private void OnEnable()
    {
        dataController = FindObjectOfType<DataController>();
        buttonList = new List<GameObject>();
        tabMenu = transform.parent.GetComponent<TabMenu>();
        clickAudio = GetComponent<AudioSource>();
        for(int i = 0; i < buttonGruop.transform.childCount; i++)
        {
            buttonList.Add(buttonGruop.transform.GetChild(i).gameObject);
        }

        foodList = new List<string>();
        ingredientList = new List<Ingred_Name>();
        dexMod = 0;
        page = 0;
        interAble = true;

        InitList(page);

        infoWindow.SetActive(false);
    }

    private void OnDisable()
    {
        infoWindow.SetActive(false);
        interAble = true;
    }

    private void InitList(int page)
    {
        if(dexMod == 0)
        {
            foodList = new List<string>();

            foreach (string food in dataController.FoodIngredDex.foodDex.Keys)
            {
                    foodList.Add(food);
            }

            foreach(GameObject button in buttonList)
            {
                button.transform.GetChild(0).GetComponent<Image>().sprite = lockSprite;
            }

            
            for(int i = buttonList.Count * page; i < foodList.Count; i++)
            {
                if(i != buttonList.Count * page && i % buttonList.Count == 0)
                {
                    break;
                }

                switch (dataController.FoodIngredDex.foodDex[foodList[i]])
                {
                    case FoodDex_Status.CREATED:
                        buttonList[i % buttonList.Count].transform.GetChild(0).GetComponent<Image>().sprite = dataController.FindFood(foodList[i]).foodSprite;
                        buttonList[i % buttonList.Count].transform.GetChild(0).GetComponent<Image>().material = defultMaterial;
                        break;
                    case FoodDex_Status.RECIPE:
                        buttonList[i % buttonList.Count].transform.GetChild(0).GetComponent<Image>().sprite = dataController.FindFood(foodList[i]).foodSprite;
                        buttonList[i % buttonList.Count].transform.GetChild(0).GetComponent<Image>().material = grayScale;
                        break;
                    case FoodDex_Status.LOCKED:
                        buttonList[i % buttonList.Count].transform.GetChild(0).GetComponent<Image>().sprite = lockSprite;
                        buttonList[i % buttonList.Count].transform.GetChild(0).GetComponent<Image>().material = defultMaterial;

                        break;
                }

                
            }
        }
        else
        {
            ingredientList = new List<Ingred_Name>();

            foreach (Ingred_Name ingred in dataController.FoodIngredDex.ingredDex.Keys)
            {
                if(dataController.FoodIngredDex.ingredDex[ingred])
                {
                    ingredientList.Add(ingred);
                }
            }

            foreach (GameObject button in buttonList)
            {
                button.transform.GetChild(0).GetComponent<Image>().sprite = lockSprite;
                button.transform.GetChild(0).GetComponent<Image>().material = defultMaterial;
            }

            for (int i = buttonList.Count * page; i < ingredientList.Count; i++)
            {
                if (i != buttonList.Count * page && i % buttonList.Count == 0)
                {
                    break;
                }
                buttonList[i % buttonList.Count].transform.GetChild(0).GetComponent<Image>().sprite = dataController.FindIngredient(ingredientList[i]).sprite;
            }
        }
        Debug.Log(dexMod);
    }

    public void OnModChangeClicked(int index)
    {
        if(interAble && dexMod != index)
        {
            int dir = 0;
            if(index == 0)
            {
                dir = -1;
            }
            else
            {
                dir = 1;
            }

            dexMod = index;
            page = 0;

            StartCoroutine(FlipPage(dir));
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
                if (dexMod == 0)
                {
                    if (buttonList.Count * (this.page + 1) < foodList.Count)
                    {
                        this.page += 1;
                        StartCoroutine(FlipPage(1));
                    }
                }
                else
                {
                    if (buttonList.Count * (this.page + 1) < ingredientList.Count)
                    {
                        this.page += 1;
                        StartCoroutine(FlipPage(1));
                    }
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

        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);

        InitList(this.page);
        interAble = true;
        tabMenu.interAble = true;
    }

    public void OnInfoOpenClicked(Image image)
    {
        clickAudio.Play();
        if (image.sprite != lockSprite)
        {
            infoWindow.SetActive(true);
            if (dexMod == 0)
            {
                FoodData food = dataController.FindFood(image.sprite);

                infoWindow.transform.GetChild(3).GetChild(1).gameObject.SetActive(true);
                infoWindow.transform.GetChild(3).GetChild(2).gameObject.SetActive(true);
                infoWindow.transform.GetChild(3).GetChild(3).gameObject.SetActive(true);
                infoWindow.transform.GetChild(3).GetChild(4).gameObject.SetActive(true);
                infoWindow.transform.GetChild(3).GetChild(5).gameObject.SetActive(true);
                infoWindow.transform.GetChild(3).GetChild(6).gameObject.SetActive(true);

                infoWindow.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = food.foodSprite;
                infoWindow.transform.GetChild(2).GetChild(0).GetComponent<Image>().material = defultMaterial;
                infoWindow.transform.GetChild(3).GetChild(0).GetComponent<TMP_Text>().text = food.foodName;
                infoWindow.transform.GetChild(3).GetChild(1).GetComponent<TMP_Text>().text = food.EunmToString(food.grade);
                infoWindow.transform.GetChild(3).GetChild(2).GetComponent<TMP_Text>().text = food.EunmToString(food.mainIngred);
                infoWindow.transform.GetChild(3).GetChild(3).GetComponent<TMP_Text>().text = food.EunmToString(food.nation);
                infoWindow.transform.GetChild(3).GetChild(4).GetComponent<TMP_Text>().text = string.Format("포만감: {0}", food.damage);
                infoWindow.transform.GetChild(3).GetChild(5).GetComponent<TMP_Text>().text = string.Format("서빙 속도: {0}", food.speed);
                infoWindow.transform.GetChild(3).GetChild(6).GetComponent<TMP_Text>().text = string.Format("조리 속도: {0}", food.coolTime);

                if (dataController.FoodIngredDex.foodDex[food.foodName] == FoodDex_Status.RECIPE)
                {
                    infoWindow.transform.GetChild(2).GetChild(0).GetComponent<Image>().material = grayScale;
                    infoWindow.transform.GetChild(3).GetChild(1).gameObject.SetActive(false);
                    infoWindow.transform.GetChild(3).GetChild(2).gameObject.SetActive(false);
                    infoWindow.transform.GetChild(3).GetChild(3).gameObject.SetActive(false);
                    infoWindow.transform.GetChild(3).GetChild(4).gameObject.SetActive(false);
                    infoWindow.transform.GetChild(3).GetChild(5).gameObject.SetActive(false);
                    infoWindow.transform.GetChild(3).GetChild(6).gameObject.SetActive(false);
                }

                List<Ingred_Name> ingred = food.needIngredient.Keys.ToList();
                for(int i = 0; i < 6; i++)
                {
                    if(i < ingred.Count)
                    {
                        infoWindow.transform.GetChild(4).GetChild(i).gameObject.SetActive(true);
                        infoWindow.transform.GetChild(4).GetChild(i).GetComponent<Image>().sprite = dataController.FindIngredient(ingred[i]).sprite;
                        infoWindow.transform.GetChild(4).GetChild(i).GetChild(0).GetComponent<TMP_Text>().text = string.Format("X{0}", food.needIngredient[ingred[i]].ToString());
                    }
                    else
                    {
                        infoWindow.transform.GetChild(4).GetChild(i).gameObject.SetActive(false);
                    }

                    
                }
            }
            else
            {
                Ingredient ingred = dataController.FindIngredient(image.sprite);

                infoWindow.transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = ingred.sprite;
                infoWindow.transform.GetChild(2).GetChild(0).GetComponent<Image>().material = defultMaterial;
                infoWindow.transform.GetChild(3).GetChild(0).GetComponent<TMP_Text>().text = ingred.EnumToString(ingred.name);
                infoWindow.transform.GetChild(3).GetChild(1).GetComponent<TMP_Text>().text = ingred.EunmToString(ingred.grade);
                infoWindow.transform.GetChild(3).GetChild(2).gameObject.SetActive(false);
                infoWindow.transform.GetChild(3).GetChild(3).gameObject.SetActive(false);
                infoWindow.transform.GetChild(3).GetChild(4).gameObject.SetActive(true);
                infoWindow.transform.GetChild(3).GetChild(4).GetComponent<TMP_Text>().text = ingred.passive;
                infoWindow.transform.GetChild(3).GetChild(5).gameObject.SetActive(false);
                infoWindow.transform.GetChild(3).GetChild(6).gameObject.SetActive(false);

                for(int i = 0; i < 6; i++)
                {
                    infoWindow.transform.GetChild(4).GetChild(i).gameObject.SetActive(false);
                }
            }
        }
    }

    public void OnInfoCloseClicked()
    {
        clickAudio.Play();
        infoWindow.SetActive(false);
    }
}
