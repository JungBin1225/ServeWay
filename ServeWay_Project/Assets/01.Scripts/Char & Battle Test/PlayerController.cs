using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer renderer;
    private Rigidbody2D rigidBody;
    private Vector3 mousePos;
    private float coolTime;
    private Vector2 moveVel;
    private MissionManager misson;
    private float missonTime;
    private PlayerHealth playerHealth;
    private DataController foodData;
    private HoldWeapon holdWeapon;
    private InventoryManager inventory;

    public float speed;
    public float chargeSpeed;
    public float chargeLength;
    public float chargeCooltime;

    public bool controllAble;
    public bool isCharge;

    public WeaponSlot weaponSlot;
    public AudioSource chargeSound;

    // skill coolTime
    //[SerializeField] GameObject chargeCoolTime;
    
    void Start()
    {
        controllAble = true;
        isCharge = false;
        coolTime = 0;
        missonTime = 0;

        anim = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
        misson = FindObjectOfType<MissionManager>();
        playerHealth = gameObject.GetComponent<PlayerHealth>();
        foodData = FindObjectOfType<DataController>();
        holdWeapon = FindObjectOfType<HoldWeapon>();
        inventory = GameManager.gameManager.inventory;

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
        else if(!controllAble && !isCharge)
        {
            rigidBody.velocity = new Vector2(0, 0);
        }

        if(coolTime > 0)
        {
            coolTime -= Time.deltaTime;
        }
        else if(coolTime < 0)
        {
            coolTime = 0;
        }

        if(Input.GetKey(KeyCode.Space) && controllAble)
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

            if(rigidBody.velocity == Vector2.zero)
            {
                misson.OccurreEvent(7, 0);
            }
            else
            {
                misson.OccurreEvent(7, Time.deltaTime);
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
                anim.SetInteger("direction", 2);
                renderer.flipX = false;
            }
            else
            {
                anim.SetInteger("direction", 4);
                renderer.flipX = true;
            }
        }
        else
        {
            if ((transform.position.y - (transform.localScale.y / 2)) - mouse.y < 0)
            {
                anim.SetInteger("direction", 1);
            }
            else
            {
                anim.SetInteger("direction", 3);
            }
        }
    }

    private void Move()
    {
        float xMove = Input.GetAxisRaw("Horizontal");
        float yMove = Input.GetAxisRaw("Vertical");

        moveVel = new Vector2(xMove, yMove) * speed * inventory.increase_Speed;
        rigidBody.velocity = moveVel;

        if (rigidBody.velocity.magnitude == 0 && controllAble)
        {
            anim.SetBool("isMove", false);
        }
        else
        {
            anim.SetBool("isMove", true);
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

        if(weaponSlot.GetHoldWeapon() != null)
        {
            weaponSlot.GetWeaponInfo(weaponSlot.GetHoldWeapon()).FoodInvisible();
        }

        rigidBody.velocity = Vector2.zero;
        chargeSound.Play();
        anim.SetBool("isDash", true);
        yield return new WaitForSeconds(0.1f);

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).length < anim.GetCurrentAnimatorStateInfo(0).normalizedTime); //선딜
        if(GetnowHp() <= 0)
        {
            yield break;
        }

        anim.SetTrigger("DashStart");
        isCharge = true;

        //rigidbody.velocity = chargeVel;
        rigidBody.AddForce(chargeVel * chargeSpeed * 0.2f, ForceMode2D.Impulse);

        yield return new WaitForSeconds(chargeLength * inventory.increase_ChargeSpeed); //돌진

        isCharge = false;
        anim.SetTrigger("DashFinish");
        anim.SetBool("isDash", false);

        rigidBody.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.1f);

        yield return new WaitUntil(() => anim.GetCurrentAnimatorStateInfo(0).length < anim.GetCurrentAnimatorStateInfo(0).normalizedTime); //후딜

        if (weaponSlot.GetHoldWeapon() != null)
        {
            weaponSlot.GetWeaponInfo(weaponSlot.GetHoldWeapon()).FoodVisible();
        }
            
        controllAble = true;
        coolTime = chargeCooltime * inventory.increase_ChargeCoolTime;
        if(inventory.isKimchi)
        {
            if(Random.Range(1, 101) <= 30)
            {
                coolTime = 0.1f;
            }
        }
        StartCoroutine(holdWeapon.ChargeCoolTime(chargeCooltime * inventory.increase_ChargeCoolTime));
    }

    public void InitCharactor()
    {
        if(GameManager.gameManager.charData.saveFile.weaponList.Count != 0 && !SceneManager.GetActiveScene().name.Contains("Tutorial"))
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

        Time.timeScale = 1;
    }


    public float GetnowHp()
    {
        return playerHealth.nowHp;
    }

    public float GetPlayerVel()
    {
        return rigidBody.velocity.magnitude;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Boss" && isCharge)
        {
            misson.OccurreEvent(8, 1);
        }
    }
}
