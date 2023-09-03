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

    public float shootDuration;
    public float damage;
    public float speed;
    public GameObject bulletPrefab;
    public GameObject effectPrefab;

    void Start()
    {
        player = transform.parent.parent.gameObject.GetComponent<PlayerController>();
        playerSprite = transform.parent.parent.gameObject.GetComponent<SpriteRenderer>();
        weaponSprite = GetComponent<SpriteRenderer>();

        coolTime = 0;
        shootAble = true;
    }

    
    void Update()
    {
        SetTransform();

        if(coolTime > 0)
        {
            coolTime -= Time.deltaTime;
            shootAble = false;
        }
        else
        {
            shootAble = true;
        }
        
        if(Input.GetMouseButton(0))
        {
            if(shootAble && player.controllAble)
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
        GameObject effect = Instantiate(effectPrefab, transform);
        bullet.GetComponent<BulletController>().SetTarget(-transform.up);
        bullet.GetComponent<BulletController>().SetSpeed(speed);
        bullet.GetComponent<BulletController>().SetDamage(damage);
    }
}
