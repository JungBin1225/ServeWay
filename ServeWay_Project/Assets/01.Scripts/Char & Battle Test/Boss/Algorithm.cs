using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Algorithm : MonoBehaviour
{
    public Vector3 target;
    public float speed;
    public float damage;

    void Start()
    {
        
    }

    void Update()
    {
        Fire();
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
            collision.gameObject.GetComponent<PlayerHealth>().PlayerDamaged(damage);
            Destroy(this.gameObject);
        }

        if(collision.gameObject.tag == "Boss")
        {
            Destroy(this.gameObject);
        }
    }
}
