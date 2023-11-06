using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private PlayerController player;
    private SpriteRenderer playerSprite;
    private SpriteRenderer weaponSprite;
    private Vector3 mousePos;
    private float coolTime;
    private bool shootAble;

    public string weaponName;
    public Food_Grade grade;
    public Food_MainIngred mainIngred;
    public Food_Nation nation;
    public Create_Success success;
    public float shootDuration;
    public float damage;
    public float speed;
    public GameObject bulletPrefab;
    public GameObject effectPrefab;
    public GameObject dropPrefab;
    public List<float> alphaStat;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerSprite = player.GetComponent<SpriteRenderer>();
        weaponSprite = GetComponent<SpriteRenderer>();

        coolTime = 0;
        shootAble = true;
    }


    void Update()
    {
        if (Time.timeScale == 0) { return; }

        SetTransform();

        if (coolTime > 0)
        {
            coolTime -= Time.deltaTime;
            shootAble = false;
        }
        else
        {
            shootAble = true;
        }

        if (Input.GetMouseButton(0))
        {
            if (shootAble && player.controllAble)
            {
                GenerateBullet(speed, damage, mousePos);
                coolTime = shootDuration;
            }

        }
    }

    private void SetTransform()
    {
        mousePos = player.UpdateMousePos();
        mousePos = new Vector3(mousePos.x, mousePos.y, 0);

        Vector3 playerPos = new Vector3(player.gameObject.transform.position.x, (player.gameObject.transform.position.y - (player.gameObject.transform.localScale.y / 2)), 0);

        transform.localPosition = (mousePos - playerPos).normalized * 0.3f;

        Vector2 direction = mousePos - playerPos;
        transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);

        if (playerPos.y - mousePos.y < 0)
        {
            weaponSprite.sortingOrder = playerSprite.sortingOrder - 1;
        }
        else
        {
            weaponSprite.sortingOrder = playerSprite.sortingOrder + 1;
        }
    }

    private void GenerateBullet(float speed, float damage, Vector3 mousePos)
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        //GameObject effect = Instantiate(effectPrefab, transform);

        var bulletController = bullet.GetComponent<BulletController>();
        switch(mainIngred)
        {
            case Food_MainIngred.BREAD:
                var explosionbulletController = bullet.GetComponent<ExplosionBullet>();
                explosionbulletController.SetTarget(-transform.up);
                explosionbulletController.SetSpeed(speed);
                explosionbulletController.SetDamage(damage);
                explosionbulletController.SetNation(nation);
                explosionbulletController.SetRadius(alphaStat[0]);
                break;
            case Food_MainIngred.MEAT:
                bulletController = bullet.GetComponent<BulletController>();
                bulletController.SetTarget(-transform.up);
                bulletController.SetSpeed(speed);
                bulletController.SetDamage(damage);
                bulletController.SetNation(nation);
                break;
            case Food_MainIngred.RICE:
                bulletController = bullet.GetComponent<BulletController>();
                bulletController.SetTarget(-transform.up);
                bulletController.SetSpeed(speed);
                bulletController.SetDamage(damage);
                bulletController.SetNation(nation);
                break;
            case Food_MainIngred.SOUP:
                Destroy(bullet);
                GenerateSoupBullet(speed, damage, mousePos, alphaStat[0], alphaStat[1]);
                break;
            default:
                bulletController = bullet.GetComponent<BulletController>();
                bulletController.SetTarget(-transform.up);
                bulletController.SetSpeed(speed);
                bulletController.SetDamage(damage);
                bulletController.SetNation(nation);
                break;
        }
    }

    public void GenerateSoupBullet(float speed, float damage, Vector3 mousePos, float radius, float bulletAmount)
    {
        for (int j = 0; j < 4; j++)
        {
            for (int i = 0; i < bulletAmount; i++)
            {
                float angle = i * Mathf.PI * 2 / bulletAmount;
                float x = Mathf.Cos(angle) * radius;
                float y = Mathf.Sin(angle) * radius;
                Vector3 pos = transform.position + new Vector3(x, y, 0);

                if (Mathf.Abs(Vector2.SignedAngle(pos - transform.position, mousePos - transform.position)) < 60)
                {
                    float angleDegrees = -angle * Mathf.Rad2Deg;
                    Quaternion rot = Quaternion.Euler(0, 0, angleDegrees);
                    GameObject bullet = Instantiate(bulletPrefab, pos, rot);
                    bullet.GetComponent<BulletController>().SetTarget(new Vector3(-x, -y, 0).normalized);
                    bullet.GetComponent<BulletController>().SetSpeed(speed);
                    bullet.GetComponent<BulletController>().SetDamage(damage);
                    bullet.GetComponent<BulletController>().SetNation(nation);
                }
            }
        }
    }

    public void InitWeapon()
    {
        FoodInfo foodInfo = FindObjectOfType<DataController>().FoodInfoList.FindFood(weaponName);
        alphaStat = new List<float>();

        grade = foodInfo.grade;
        mainIngred = foodInfo.mainIngred;
        nation = foodInfo.nation;
        alphaStat = foodInfo.alphaStat;

        switch(success)
        {
            case Create_Success.FAIL:
                shootDuration = foodInfo.coolTime + foodInfo.successCoolTime;
                damage = foodInfo.damage - foodInfo.successDamage;
                speed = foodInfo.speed - foodInfo.successSpeed;
                break;
            case Create_Success.SUCCESS:
                shootDuration = foodInfo.coolTime;
                damage = foodInfo.damage;
                speed = foodInfo.speed;
                break;
            case Create_Success.GREAT:
                shootDuration = foodInfo.coolTime - foodInfo.successCoolTime;
                damage = foodInfo.damage + foodInfo.successDamage;
                speed = foodInfo.speed + foodInfo.successSpeed;
                break;
        }
    }
}
