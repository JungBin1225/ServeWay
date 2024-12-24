using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private PlayerController player;
    private SpriteRenderer playerSprite;
    private SpriteRenderer weaponSprite;
    private LineRenderer lineRenderer;
    private Vector3 mousePos;
    private float coolTime;
    private bool shootAble;
    private Ray2D ray;
    private RaycastHit2D hit;
    private bool isClicked;
    private bool isLaser;
    private GameObject laser;
    private FoodData foodData;
    private DataController data;
    private InventoryManager inventory;
    private AudioSource audio;

    public string weaponName;
    public Food_Grade grade;
    public Food_MainIngred mainIngred;
    public Food_Nation nation;
    public Create_Success success;
    public float shootDuration;
    public float damage;
    public float speed;
    public GameObject bulletPrefab;
    public GameObject laserPrefab;
    public GameObject effectPrefab;
    public GameObject dropPrefab;
    public List<float> alphaStat;
    public Color32 bulletColor;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        data = FindObjectOfType<DataController>();
        playerSprite = player.GetComponent<SpriteRenderer>();
        weaponSprite = GetComponent<SpriteRenderer>();
        inventory = GameManager.gameManager.inventory;
        audio = GetComponent<AudioSource>();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.enabled = false;

        coolTime = 0;
        shootAble = true;
        isClicked = false;
        isLaser = false;
        foodData = data.FindFood(weaponName);
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

        if (Input.GetMouseButton(0) && player.controllAble)
        {
            isClicked = true;
            if (shootAble && mainIngred != Food_MainIngred.NOODLE)
            {
                GenerateBullet(speed, damage, mousePos);
                coolTime = shootDuration * inventory.increase_BulletCoolTime;
            }
            else if(player.controllAble && mainIngred == Food_MainIngred.NOODLE && !isLaser)
            {
                GenerateBullet(speed, damage, mousePos);
            }
        }
        else if(Input.GetMouseButtonUp(0) || !player.controllAble)
        {
            isClicked = false;
        }
    }

    private void OnDisable()
    {
        if(isLaser)
        {
            lineRenderer.SetPosition(1, transform.position);
            lineRenderer.enabled = false;
            isLaser = false;
            if (laser != null)
            {
                Destroy(laser);
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
        int tofuDamage = 1;

        if(inventory.isTofu)
        {
            if(Random.Range(1, 101) <= 20)
            {
                tofuDamage = 2;
            }
        }

        var bulletController = bullet.GetComponent<BulletController>();
        switch(mainIngred)
        {
            case Food_MainIngred.BREAD:
                Instantiate(effectPrefab, transform);
                var explosionbulletController = bullet.GetComponent<ExplosionBullet>();
                explosionbulletController.SetTarget(-transform.up);
                explosionbulletController.SetSpeed(speed * inventory.increase_BulletSpeed);
                explosionbulletController.SetDamage(damage * inventory.increase_Damage * tofuDamage);
                explosionbulletController.SetFood(foodData);
                explosionbulletController.SetRadius(alphaStat[0]);
                break;
            case Food_MainIngred.MEAT:
                Instantiate(effectPrefab, transform);
                bulletController = bullet.GetComponent<BulletController>();
                bulletController.SetTarget(-transform.up);
                bulletController.SetSpeed(speed * inventory.increase_BulletSpeed);
                bulletController.SetDamage(damage * inventory.increase_Damage * tofuDamage);
                bulletController.SetFood(foodData);
                bulletController.SetColor(bulletColor);
                break;
            case Food_MainIngred.RICE:
                Instantiate(effectPrefab, transform);
                bulletController = bullet.GetComponent<BulletController>();
                bulletController.SetTarget(-transform.up);
                bulletController.SetSpeed(speed * inventory.increase_BulletSpeed);
                bulletController.SetDamage(damage * inventory.increase_Damage * tofuDamage);
                bulletController.SetFood(foodData);
                bulletController.SetColor(bulletColor);
                break;
            case Food_MainIngred.SOUP:
                Destroy(bullet);
                Instantiate(effectPrefab, transform);
                GenerateSoupBullet(speed * inventory.increase_BulletSpeed, damage * inventory.increase_Damage, alphaStat[0], alphaStat[1]);
                break;
            case Food_MainIngred.NOODLE:
                Destroy(bullet);
                StartCoroutine(GenerateLaser());
                break;
            /*default:
                bulletController = bullet.GetComponent<BulletController>();
                bulletController.SetTarget(-transform.up);
                bulletController.SetSpeed(speed);
                bulletController.SetDamage(damage);
                bulletController.SetNation(nation);
                break;*/
        }
    }

    public void GenerateSoupBullet(float speed, float damage, float radius, float bulletAmount)
    {
        /*for (int j = 0; j < 4; j++)
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
        }*/

        float startAngle = (radius * 10) / 2;
        float differAngle = (radius * 10) / (bulletAmount - 1);

        for(int i = 0; i < bulletAmount; i++)
        {
            int tofuDamage = 1;

            if (inventory.isTofu)
            {
                if (Random.Range(1, 101) <= 20)
                {
                    tofuDamage = 2;
                }
            }

            GameObject bullet = Instantiate(bulletPrefab, transform.position + (mousePos - transform.position).normalized, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, startAngle - (differAngle * i))));
            bullet.GetComponent<BulletController>().SetTarget(-bullet.transform.up);
            bullet.GetComponent<BulletController>().SetSpeed(speed);
            bullet.GetComponent<BulletController>().SetDamage(damage * tofuDamage);
            bullet.GetComponent<BulletController>().SetFood(foodData);
            bullet.GetComponent<BulletController>().SetColor(bulletColor);
        }
    }

    public IEnumerator GenerateLaser()
    {
        lineRenderer.enabled = true;
        isLaser = true;
        laser = Instantiate(laserPrefab, this.transform);
        GameObject effect = Instantiate(effectPrefab, transform);
        GameObject hiteffect = null;

        laser.GetComponent<LaserController>().SetDamage(damage * inventory.increase_Damage);
        laser.GetComponent<LaserController>().SetCoolTime(shootDuration * inventory.increase_BulletCoolTime);
        laser.GetComponent<LaserController>().SetFood(foodData);

        while (isClicked)
        {
            if(effect == null)
            {
                effect = Instantiate(effectPrefab, transform);
                audio.Play();
            }
            ray = new Ray2D(transform.position, mousePos - transform.position);

            lineRenderer.SetPosition(0, transform.position);

            int mask = 1 << LayerMask.NameToLayer("RayTarget") | 1 << LayerMask.NameToLayer("RayWall");
            hit = Physics2D.Raycast(ray.origin, ray.direction, 1000f, mask);
            if (hit)
            {
                lineRenderer.SetPosition(1, hit.point);
            }
            else
            {
                lineRenderer.SetPosition(1, mousePos);
            }

            Vector3 start = lineRenderer.GetPosition(0);
            Vector3 end = lineRenderer.GetPosition(1);

            laser.transform.localScale = new Vector3(Vector3.Distance(start, end) * 1.25f, lineRenderer.startWidth * 1.25f, 0);
            Vector3 pos = (start + end) / 2;
            Vector2 dir = new Vector2(pos.x - end.x, pos.y - end.y);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion angleAxis = Quaternion.AngleAxis(angle, Vector3.forward);
            laser.transform.rotation = angleAxis;
            laser.transform.position = pos;

            if (hiteffect == null && hit)
            {
                hiteffect = Instantiate(effectPrefab, hit.point, Quaternion.Euler(angleAxis.eulerAngles + new Vector3(0, 0, -90)));
            }

            yield return null;
        }

        lineRenderer.SetPosition(1, transform.position);
        lineRenderer.enabled = false;
        isLaser = false;
        Destroy(laser);
        yield return null;
    }

    public void InitWeapon()
    {
        FoodData foodInfo = FindObjectOfType<DataController>().FindFood(weaponName);
        alphaStat = new List<float>();

        GetComponent<SpriteRenderer>().sprite = foodInfo.foodSprite;
        grade = foodInfo.grade;
        mainIngred = foodInfo.mainIngred;
        nation = foodInfo.nation;
        alphaStat = foodInfo.alphaStat;
        bulletPrefab = foodInfo.bulletPrefab;
        effectPrefab = foodInfo.effectPrefab;
        bulletColor = foodInfo.bulletColor;

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

    public void FoodInvisible()
    {
        weaponSprite.color = new Color(1, 1, 1, 0);
    }

    public void FoodVisible()
    {
        weaponSprite.color = new Color(1, 1, 1, 1);
    }
}
