using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rigidBody;
    private Vector3 mousePos;
    private float coolTime;
    private Vector2 moveVel;
    private MissonManager misson;
    private float missonTime;
    private PlayerHealth playerHealth;
    private DataController foodData;

    public float speed;
    public float chargeSpeed;
    public float chargeLength;
    public float chargeCooltime;

    public bool controllAble;
    public bool isCharge;

    public WeaponSlot weaponSlot;

    // weapon info panel img
    [SerializeField] GameObject weaponInfoImg;
    [SerializeField] Text weaponInfoName;

    // skill coolTime
    [SerializeField] GameObject skillCoolTime;
    [SerializeField] GameObject chargeCoolTime;
    
    void Start()
    {
        controllAble = true;
        isCharge = false;
        coolTime = 0;
        missonTime = 0;

        anim = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        misson = FindObjectOfType<MissonManager>();
        playerHealth = gameObject.GetComponent<PlayerHealth>();
        foodData = FindObjectOfType<DataController>();

        InitCharactor();
    }

    
    void Update()
    {
        if (Time.timeScale == 0) { return; }

        if (controllAble)
        {
            mousePos = UpdateMousePos();
            UpdateDirection(mousePos);

            Move();
        }

        if(coolTime > 0)
        {
            if (!chargeCoolTime.activeSelf) { chargeCoolTime.SetActive(true); }
            coolTime -= Time.deltaTime;
            chargeCoolTime.gameObject.transform.GetChild(0).GetComponent<Text>().text = coolTime.ToString("F1");

        }
        else if(coolTime < 0)
        {
            coolTime = 0;
            chargeCoolTime.SetActive(false);
        }

        if(Input.GetKeyDown(KeyCode.Space) && controllAble)
        {
            if(coolTime == 0)
            {
                StartCoroutine(UseCharge());
                if (GameManager.gameManager.isBossStage)
                {
                    if (missonTime <= 0)
                    {
                        missonTime = 30;
                    }
                    misson.OccurreEvent(1, 1);
                    misson.OccurreEvent(3, 0);
                }
            }
            else
            {
                
            }
        }

        if(GameManager.gameManager.isBossStage)
        {
            if (missonTime > 0)
            {
                missonTime -= Time.deltaTime;
            }
            else
            {
                misson.OccurreEvent(1, 0);
            }
        }
    }

    public Vector3 UpdateMousePos()
    {
        Vector3 mousePos = Input.mousePosition;

        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    private void UpdateDirection(Vector3 mouse)
    {
        if (Mathf.Abs(transform.position.x - mouse.x) > Mathf.Abs((transform.position.y - (transform.localScale.y / 2)) - mouse.y))
        {
            if (transform.position.x - mouse.x < 0)
            {
                //anim.SetInteger("direction", 2);
            }
            else
            {
                //anim.SetInteger("direction", 4);
            }
        }
        else
        {
            if ((transform.position.y - (transform.localScale.y / 2)) - mouse.y < 0)
            {
                //anim.SetInteger("direction", 1);
            }
            else
            {
                //anim.SetInteger("direction", 3);
            }
        }
    }

    private void Move()
    {
        float xMove = Input.GetAxisRaw("Horizontal");
        float yMove = Input.GetAxisRaw("Vertical");

        moveVel = new Vector2(xMove, yMove) * speed;
        rigidBody.velocity = moveVel;

        if (rigidBody.velocity.magnitude == 0 && controllAble)
        {
            //anim.SetBool("isMove", false);
        }
        else
        {
            //anim.SetBool("isMove", true);
        }
    }

    private IEnumerator UseCharge()
    {
        controllAble = false;
        Vector2 chargeVel = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y).normalized;
        /*Vector2 chargeVel = moveVel.normalized;
        if(chargeVel == Vector2.zero)
        {
            chargeVel = new Vector2(1, 0).normalized;
        }*/

        rigidBody.velocity = chargeVel * -2;

        yield return new WaitForSeconds(0.25f); //선딜

        isCharge = true;

        //rigidbody.velocity = chargeVel;
        rigidBody.AddForce(chargeVel * chargeSpeed * 0.2f, ForceMode2D.Impulse);

        yield return new WaitForSeconds(chargeLength); //돌진

        isCharge = false;

        rigidBody.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.25f); //후딜

        controllAble = true;
        coolTime = chargeCooltime;
    }

    public void InitCharactor()
    {
        if(GameManager.gameManager.charData.saveFile.weaponList.Count != 0)
        {
            weaponSlot.InitSlot();
            for (int i = 0; i < weaponSlot.gameObject.transform.childCount; i++)
            {
                weaponSlot.DeleteWeapon(weaponSlot.gameObject.transform.GetChild(0).gameObject);
            }

            weaponSlot.index = 0;
            int n = 0;
            foreach(string weapon in GameManager.gameManager.charData.saveFile.weaponList)
            {
                weaponSlot.GetWeapon(foodData.FindFood(weapon).foodPrefab.GetComponent<GetItem>().weaponPrefab, GameManager.gameManager.charData.saveFile.weaponSuccess[n], weapon);
                n++;
            }

            speed = GameManager.gameManager.charData.saveFile.playerSpeed;
            chargeSpeed = GameManager.gameManager.charData.saveFile.playerChargeSpeed;
            chargeLength = GameManager.gameManager.charData.saveFile.playerChargeLength;
            chargeCooltime = GameManager.gameManager.charData.saveFile.playerChargeCooltime;
            playerHealth.nowHp = GameManager.gameManager.charData.saveFile.playerHp;
        }
        else
        {
            weaponSlot.InitSlot();
            playerHealth.nowHp = playerHealth.maxHp;
        }

        // 무기 바꾸는 부분 나오면 수정해야 함
        string nowWeaponName = weaponSlot.gameObject.transform.GetChild(weaponSlot.index).GetChild(0).GetComponent<WeaponController>().weaponName;
        weaponInfoImg.GetComponent<Image>().sprite = FindObjectOfType<DataController>().FindFood(nowWeaponName).foodSprite;
        weaponInfoName.GetComponent<Text>().text = FindObjectOfType<DataController>().FindFood(nowWeaponName).foodName;

        Time.timeScale = 1;
    }

    public float GetnowHp()
    {
        return playerHealth.nowHp;
    }
}
