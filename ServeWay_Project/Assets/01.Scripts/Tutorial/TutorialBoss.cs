using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialBoss : MonoBehaviour
{
    private MissionManager misson;
    private float hp;
    private float maxHp;
    private GameObject target;
    private SpriteRenderer spriteRenderer;

    public BossTutorial room;
    public Food_Nation nation;
    public GameObject stair;

    void Start()
    {
        misson = FindObjectOfType<MissionManager>();
        target = GameObject.FindGameObjectWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();

        SetHp(500);
        nation = Food_Nation.KOREA;
    }

    void Update()
    {
        if (hp <= 0 || misson.isClear())
        {
            BossDie();
        }

        if (target.transform.position.x < transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    public void BossDie()
    {
        GameManager.gameManager.isBossStage = false;
        room.DropIngredient(10, 10);
        Instantiate(stair, transform.position, Quaternion.Euler(0, 0, 0));
        Destroy(this.gameObject);
    }

    public void GetDamage(float damage, Vector3 effectPos, FoodData food)
    {
        //GameObject effect = Instantiate(damageEffect, effectPos, transform.rotation);

        hp -= damage;

        if (food.nation.ToString() == this.nation.ToString())
        {
            misson.OccurreEvent(0, damage);
        }
        misson.OccurreEvent(3, damage);
    }

    public void SetHp(float hp)
    {
        this.hp = hp;
        this.maxHp = hp;
    }

    public float GetHp()
    {
        return hp;
    }

    public float GetMaxHp()
    {
        return maxHp;
    }

}
