using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comment : MonoBehaviour
{
    private AudioSource audio;
    private List<Sprite> sprites;
    private int bulletAmount;
    private GameObject bulletParent;
    private Animator anim;

    public List<GameObject> bulletPrefab;
    public float speed;
    public float damage;
    public Sprite sprite;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        bulletParent = GameObject.Find("BulletList");
        sprites = new List<Sprite>();
        sprites.Add(sprite);
        bulletAmount = 5;

        StartCoroutine(FireComment());
    }

    void Update()
    {
        
    }

    private IEnumerator FireComment()
    {

        yield return new WaitForSeconds(0.5f);

        for (int n = 0; n < 3; n++)
        {
            audio.Play();
            anim.SetTrigger("On");
            bulletAmount = Random.Range(6, 11);

            float startAngle = 0;
            float differAngle = 360 / (bulletAmount);

            for (int i = 0; i < bulletAmount; i++)
            {
                int index = Random.Range(0, bulletPrefab.Count);

                GameObject bullet = Instantiate(bulletPrefab[index], transform.position, Quaternion.Euler(new Vector3(0, 0, startAngle - (differAngle * i))), bulletParent.transform);
                bullet.GetComponent<EnemyBullet>().SetTarget(-bullet.transform.up);
                bullet.GetComponent<EnemyBullet>().SetSpeed(speed);
                bullet.GetComponent<EnemyBullet>().SetDamage(damage);
                bullet.GetComponent<EnemyBullet>().SetSprite(sprites);
            }
            yield return new WaitForSeconds(1);
            
            anim.SetTrigger("Off");
            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }
}
