using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Create_Success
{
    FAIL,
    SUCCESS,
    GREAT
};

public class GetItem : MonoBehaviour
{
    private bool getAble;
    private WeaponSlot weaponSlot;
    private float time;
    private PlayerHealth player;
    private DataController dataController;
    private InteractionWindow interaction;

    public string name;
    public Create_Success success;
    public GameObject weaponPrefab;
    public SpriteRenderer spriteRenderer;
    public List<GameObject> starObject;
    void Start()
    {
        time = 0;
        getAble = false;
        weaponSlot = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().weaponSlot;
        player = FindObjectOfType<PlayerHealth>();
        dataController = FindObjectOfType<DataController>();
        interaction = FindObjectOfType<InteractionWindow>();
    }

    // Update is called once per frame
    void Update()
    {
        if(getAble && interaction.foodGet.activeSelf)
        {
            if(Input.GetKey(KeyCode.F))
            {
                time += Time.deltaTime;
            }
            else
            {
                time = 0;
            }

            if(time >= 2.0f)
            {
                EatWeapon();
            }

            if(Input.GetKeyUp(KeyCode.F))
            {
                GetWeapon();
            }

            interaction.time.GetComponent<Image>().fillAmount = 1 - (time / 2.0f);
        }
        else
        {
            time = 0;
        }
    }

    public void GetWeapon()
    {
        weaponSlot.GetWeapon(weaponPrefab, success, name);
        Destroy(this.gameObject);
    }

    public void EatWeapon()
    {
        FoodData food = dataController.FindFood(name);

        player.PlayerHeal(food.EnumToInt(food.grade));
        Destroy(this.gameObject);

    }

    public void SetSprite()
    {
        int star = ((int)FindObjectOfType<DataController>().FindFood(name).grade);

        spriteRenderer.sprite = FindObjectOfType<DataController>().FindFood(name).foodSprite;
        starObject[star].SetActive(true);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            getAble = true;
            interaction.SetFoodGetAble(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            getAble = false;
            interaction.SetFoodGetAble(false);
        }
    }
}
