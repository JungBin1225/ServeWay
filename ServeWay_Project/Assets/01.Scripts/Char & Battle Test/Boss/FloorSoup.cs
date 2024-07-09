using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSoup : MonoBehaviour
{
    private CircleCollider2D collider2D;

    public float damage;
    public float durationTime;

    void Start()
    {
        collider2D = GetComponent<CircleCollider2D>();
        StartCoroutine(SoupFire());
    }

    void Update()
    {
        durationTime -= Time.deltaTime;
    }

    private IEnumerator SoupFire()
    {
        while(durationTime > 0)
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
            collision.gameObject.GetComponent<PlayerHealth>().PlayerDamaged(damage);
        }
    }
}
