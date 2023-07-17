using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    private MissonManager misson;

    public BossRoom room;
    public GameObject damageEffect;
    public float hp;

    void Start()
    {
        misson = FindObjectOfType<MissonManager>();
        //GameManager.gameManager.mission.boss = this.gameObject;
    }

    void Update()
    {
        if (hp <= 0)
        {
            BossDie();
        }
    }

    public void BossDie()
    {
        room.isClear = true;
        room.OpenDoor();
        room.ActiveStair();
        GameManager.gameManager.isBossStage = false;
        Destroy(this.gameObject);
    }

    public void GetDamage(float damage, Vector3 effectPos)
    {
        GameObject effect = Instantiate(damageEffect, effectPos, transform.rotation);

        hp -= damage;
        misson.OccurreEvent(1, damage);
    }
}
