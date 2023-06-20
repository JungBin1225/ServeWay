using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHp;
    public float nowHp;

    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        nowHp = maxHp;
    }

    void Update()
    {
        //돌진 중일때, 무적
        //if charge, ivincible
    }

    public void PlayerDamaged(float damage)
    {
        if(!playerController.isCharge)
        {
            nowHp -= damage;
        }
    }
}
