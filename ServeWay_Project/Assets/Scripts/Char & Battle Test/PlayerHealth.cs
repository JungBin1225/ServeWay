using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHp;
    public float nowHp;

    private PlayerController playerController;
    private MissonManager misson;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        misson = FindObjectOfType<MissonManager>();
        nowHp = maxHp;
    }

    void Update()
    {
        if (GameManager.gameManager.isBossStage)
        {
            misson.OccurreEvent(2, Time.deltaTime);
        }
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
}
