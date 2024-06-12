using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHp;
    public float nowHp;

    private PlayerController playerController;
    private MissonManager misson;
    private GameObject gameOverUI;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        misson = FindObjectOfType<MissonManager>();
        if(!SceneManager.GetActiveScene().name.Contains("Start"))
        {
            gameOverUI = FindObjectOfType<GameOver>().gameObject;

            gameOverUI.SetActive(false);
        }
    }

    void Update()
    {
        if (GameManager.gameManager.isBossStage)
        {
            misson.OccurreEvent(2, Time.deltaTime);
        }

        if(nowHp <= 0)
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
        if (!playerController.isCharge)
        {
            nowHp -= damage;
            if (GameManager.gameManager.isBossStage)
            {
                misson.OccurreEvent(2, 0);
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
}
