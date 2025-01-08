using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHp;
    public float nowHp;
    public float invincibleTime;
    public GameObject gameOverUI;

    private Animator anim;
    private PlayerController playerController;
    private MissionManager misson;
    private InventoryManager inventory;
    private SpriteRenderer renderer;
    private bool isInvincible;
    private List<Sprite> damagedObject;

    void Start()
    {
        anim = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        misson = FindObjectOfType<MissionManager>();
        inventory = GameManager.gameManager.inventory;
        renderer = GetComponent<SpriteRenderer>();

        isInvincible = false;
    }

    void Update()
    {
        if (GameManager.gameManager.isBossStage)
        {
            misson.OccurreEvent(2, Time.deltaTime);
        }

        if(nowHp <= 0 && !SceneManager.GetActiveScene().name.Contains("Start"))
        {
            StartCoroutine(GameOver());
        }

        // UI test
        /*if (Input.GetKey(KeyCode.K))
        {
            nowHp -= 2;
        }*/
    }

    public void PlayerDamaged(float damage, List<Sprite> sprite)
    {
        bool isMiss = false;
        if(inventory.isCream)
        {
            if(Random.Range(1, 101) <= 5)
            {
                isMiss = true;
            }
        }

        if (!playerController.isCharge && !isInvincible && !isMiss)
        {
            nowHp -= damage;
            damagedObject = sprite;
            if (GameManager.gameManager.isBossStage)
            {
                misson.OccurreEvent(2, 0);
            }

            if(nowHp > 0)
            {
                StartCoroutine(Invincible());
            }
        }

        if(playerController.isCharge && GameManager.gameManager.isBossStage)
        {
            misson.OccurreEvent(4, 1);
        }
    }

    public void PlayerHeal(float healAmount)
    {
        if(nowHp + healAmount >= maxHp)
        {
            nowHp = maxHp;
        }
        else
        {
            nowHp += healAmount;
        }

        Debug.Log(healAmount + " Heal");
    }

    public List<Sprite> getDeathImage()
    {
        return damagedObject;
    }

    private IEnumerator Invincible()
    {
        isInvincible = true;
        int repeatInvincible = 5;
        if(inventory.isGinger)
        {
            repeatInvincible = 7;
        }

        for(int i = 0; i < repeatInvincible; i++)
        {
            if(i % 2 == 0)
            {
                renderer.color = new Color(1, 1, 1, 0.5f);
            }
            else
            {
                renderer.color = new Color(1, 1, 1, 1);
            }

            yield return new WaitForSeconds(invincibleTime / 5);
        }

        renderer.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(0.1f);

        isInvincible = false;
    }

    private IEnumerator GameOver()
    {
        
        playerController.weaponSlot.GetWeaponInfo(playerController.weaponSlot.GetHoldWeapon()).FoodInvisible();
        playerController.controllAble = false;
        float time = 0;
        anim.SetBool("isDead", true);

        while(time < 10)
        {
            if (Time.timeScale > 0.5f)
            {
                Time.timeScale -= 0.1f;
            }

            time += 0.1f;
            yield return new WaitForSecondsRealtime(0.1f);
        }
        
        Time.timeScale = 0;
        gameOverUI.SetActive(true);
    }
}
