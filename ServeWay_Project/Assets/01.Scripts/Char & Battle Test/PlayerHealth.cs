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

    private PlayerController playerController;
    private MissonManager misson;
    private SpriteRenderer renderer;
    private bool isInvincible;

    void Start()
    {
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
            Time.timeScale = 0;
            gameOverUI.SetActive(true);
        }

        // UI test
        /*if (Input.GetKey(KeyCode.K))
        {
            nowHp -= 2;
        }*/
    }

    public void PlayerDamaged(float damage)
    {
        if (!playerController.isCharge && !isInvincible)
        {
            nowHp -= damage;
            if (GameManager.gameManager.isBossStage)
            {
                misson.OccurreEvent(2, 0);
            }

            StartCoroutine(Invincible());
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
}
