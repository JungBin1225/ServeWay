using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMission : MonoBehaviour
{
    public int chargeCount;
    public GameObject boss;

    void Start()
    {
        InitCount();
    }

     void Update()
    {
        if(boss != null && chargeCount >= 5)
        {
            boss.GetComponent<BossController>().BossDie();
        }
    }

    public void CheckCharge()
    {
        /*if(GameManager.gameManager.isBossStage)
        {
            chargeCount++;
        }*/
    }

    public void InitCount()
    {
        chargeCount = 0;
    }
}
