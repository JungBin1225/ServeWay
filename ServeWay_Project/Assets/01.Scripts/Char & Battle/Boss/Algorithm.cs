using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Algorithm : MonoBehaviour
{
    private GameObject effectParent;

    public Vector3 target;
    public float speed;
    public float damage;
    public Sprite sprite;
    public Sprite food;
    public GameObject boss;
    public GameObject sound;

    void Start()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = food;
        effectParent = GameObject.Find("EffectList");
    }

    void Update()
    {
        Fire();

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            List<Sprite> sprites = new List<Sprite>();
            sprites.Add(sprite);
            collision.gameObject.GetComponent<PlayerHealth>().PlayerDamaged(damage, sprites);
            FindObjectOfType<YoutuberController>().PlayerAlgorithmDamage();
            Destroy(this.gameObject);
        }

        if(collision.gameObject.tag == "Boss")
        {
            Instantiate(sound, transform.position, Quaternion.Euler(0, 0, 0), effectParent.transform);
            Destroy(this.gameObject);
        }
    }
}
