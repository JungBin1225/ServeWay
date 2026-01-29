using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSoup : MonoBehaviour
{
    private BoxCollider2D collider2D;
    private SpriteRenderer circle;

    public float damage;
    public float durationTime;
    public GameObject tile;
    public GameObject fall;
    public Sprite sprite;

    void Start()
    {
        collider2D = GetComponent<BoxCollider2D>();
        circle = GetComponent<SpriteRenderer>();
        fall.SetActive(false);
        tile.SetActive(false);
        collider2D.enabled = false;

        StartCoroutine(SoupFire());
    }

    void Update()
    {
        durationTime -= Time.deltaTime;
    }

    private IEnumerator SoupFire()
    {
        yield return new WaitForSeconds(0.3f);

        fall.SetActive(true);
        fall.transform.localPosition = new Vector3(0, 1.5f, 0);

        while (fall.transform.localPosition.y > 0)
        {
            fall.transform.localPosition -= new Vector3(0, 1, 0) * Time.deltaTime;
            yield return null;
        }

        circle.enabled = false;
        fall.SetActive(false);
        tile.SetActive(true);


        while (durationTime > 0)
        {
            collider2D.enabled = true;
            yield return new WaitForSeconds(0.1f);

            collider2D.enabled = false;
            yield return new WaitForSeconds(0.3f);
        }

        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            List<Sprite> sprites = new List<Sprite>();
            sprites.Add(sprite);
            collision.gameObject.GetComponent<PlayerHealth>().PlayerDamaged(damage, sprites);
        }
    }
}
