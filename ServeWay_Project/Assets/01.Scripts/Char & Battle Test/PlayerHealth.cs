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
    private MissonManager misson;
    private SpriteRenderer renderer;
    private bool isInvincible;
    private Sprite damagedObject;

    void Start()
    {
        anim = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        misson = FindObjectOfType<MissonManager>();
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

    public void PlayerDamaged(float damage, Sprite sprite)
    {
        if (!playerController.isCharge && !isInvincible)
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

    public Sprite getDeathImage()
    {
        return damagedObject;
    }

    private IEnumerator Invincible()
    {
        isInvincible = true;

        for(int i = 0; i < 5; i++)
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
