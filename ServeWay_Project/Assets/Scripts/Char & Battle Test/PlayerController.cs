using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private FoodInfoList foodInfo;

    public float speed;
    public float chargeSpeed;
    public float chargeLength;
    public float chargeCooltime;

    public bool controllAble;
    public bool isCharge;

    public WeaponSlot weaponSlot;

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
        foodInfo = FindObjectOfType<DataController>().FoodInfoList;

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
            coolTime -= Time.deltaTime;
        }
        else if(coolTime < 0)
        {
            coolTime = 0;
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
                //Debug.Log("Cool Time!");
            }
        }

        if (missonTime > 0)
        {
            missonTime -= Time.deltaTime;
        }
        else
        {
            misson.OccurreEvent(1, 0);
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

        yield return new WaitForSeconds(0.25f); //¼±µô

        isCharge = true;

        //rigidbody.velocity = chargeVel;
        rigidBody.AddForce(chargeVel * chargeSpeed * 0.2f, ForceMode2D.Impulse);

        yield return new WaitForSeconds(chargeLength); //µ¹Áø

        //GameManager.gameManager.mission.CheckCharge();
        isCharge = false;

        rigidBody.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.25f); //ÈÄµô

        controllAble = true;
        coolTime = chargeCooltime;
    }

    public void InitCharactor()
    {
        if(GameManager.gameManager.charData.weaponList.Count != 0)
        {
            weaponSlot.InitSlot();
            for (int i = 0; i < weaponSlot.gameObject.transform.childCount; i++)
            {
                weaponSlot.DeleteWeapon(weaponSlot.gameObject.transform.GetChild(0).gameObject);
            }

            weaponSlot.index = 0;
            foreach(string weapon in GameManager.gameManager.charData.weaponList)
            {
                weaponSlot.GetWeapon(foodInfo.FindPrefabToName(weapon).GetComponent<GetItem>().weaponPrefab);
            }

            speed = GameManager.gameManager.charData.playerSpeed;
            chargeSpeed = GameManager.gameManager.charData.playerChargeSpeed;
            chargeLength = GameManager.gameManager.charData.playerChargeLength;
            chargeCooltime = GameManager.gameManager.charData.playerChargeCooltime;
            playerHealth.nowHp = GameManager.gameManager.charData.playerHp;
        }
        else
        {
            weaponSlot.InitSlot();
            playerHealth.nowHp = playerHealth.maxHp;
        }
    }

    public void SaveCharData()
    {
        GameManager.gameManager.charData.weaponList.Clear();

        for(int i = 0; i < weaponSlot.gameObject.transform.childCount; i++)
        {
            GameManager.gameManager.charData.weaponList.Add(weaponSlot.gameObject.transform.GetChild(i).GetChild(0).GetComponent<WeaponController>().weaponName);
        }

        GameManager.gameManager.charData.playerSpeed = speed;
        GameManager.gameManager.charData.playerChargeSpeed = chargeSpeed;
        GameManager.gameManager.charData.playerChargeLength = chargeLength;
        GameManager.gameManager.charData.playerChargeCooltime = chargeCooltime;
        GameManager.gameManager.charData.playerHp = playerHealth.nowHp;
    }
}
