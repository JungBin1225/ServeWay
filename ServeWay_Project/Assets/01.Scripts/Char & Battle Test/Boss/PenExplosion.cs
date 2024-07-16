using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenExplosion : MonoBehaviour
{
    public GameObject pen;
    public GameObject explosionEffect;
    public CircleCollider2D collider;
    public float damage;

    void Start()
    {
        pen.SetActive(false);
        collider.enabled = false;

        StartCoroutine(Explosion());
    }

    void Update()
    {
        
    }

    private IEnumerator Explosion()
    {
        yield return new WaitForSeconds(0.3f);

        pen.SetActive(true);
        pen.transform.localPosition = new Vector3(0, 5.5f, 0);

        while(pen.transform.localPosition.y > 1.5f)
        {
            pen.transform.localPosition -= new Vector3(0, 3, 0) * Time.deltaTime;
            yield return null;
        }

        pen.SetActive(false);
        collider.enabled = true;
        Instantiate(explosionEffect, transform.position, Quaternion.Euler(0, 0, 0));

        yield return new WaitForSeconds(0.1f);

        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealth>().PlayerDamaged(damage);
        }
    }
}
