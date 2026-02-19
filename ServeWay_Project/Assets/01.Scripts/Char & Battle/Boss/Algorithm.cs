using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Algorithm : MonoBehaviour
{
    private GameObject effectParent;
    private SpriteRenderer spriteRenderer;
    private bool touched;

    public GameObject cursor;
    public Vector3 target;
    public float speed;
    public float damage;
    public Sprite sprite;
    public Sprite food;
    public GameObject boss;
    public GameObject sound;
    public List<Sprite> sprites;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = food;
        effectParent = GameObject.Find("EffectList");
        touched = false;

        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Count)];
    }

    void Update()
    {
        if(!touched)
        {
            Fire();
        }


        if(boss == null)
        {
            Destroy(this.gameObject);
        }
    }

    public void Fire()
    {
        Vector3 dir = new Vector3(target.x, target.y, 0);

        transform.position -= dir.normalized * Time.deltaTime * speed;
    }

    private IEnumerator DestroyBullet()
    {
        cursor.SetActive(true);
        cursor.transform.localRotation = Quaternion.Euler(0, 0, -transform.rotation.eulerAngles.z);

        yield return new WaitForSeconds(0.2f);

        Vector3 dir = new Vector3(target.x, target.y, 0);
        float dis = transform.localScale.x;

        while (transform.localScale.x > 0.01f)
        {
            transform.localScale = transform.localScale - new Vector3(dis * Time.deltaTime * speed * 1.5f, dis * Time.deltaTime * speed * 1.5f, 0);
            transform.position -= dir.normalized * Time.deltaTime * speed * 1.5f;
            yield return null;
        }

        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && !touched)
        {
            List<Sprite> sprites = new List<Sprite>();
            sprites.Add(sprite);
            collision.gameObject.GetComponent<PlayerHealth>().PlayerDamaged(damage, sprites);
            FindObjectOfType<YoutuberController>().PlayerAlgorithmDamage();
            Destroy(this.gameObject);
        }

        if(collision.gameObject.tag == "Boss")
        {
            touched = true;
            Instantiate(sound, transform.position, Quaternion.Euler(0, 0, 0), effectParent.transform);
            StartCoroutine(DestroyBullet());
        }
    }
}
