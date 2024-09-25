using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    public int index;
    public Vector3 target;
    public float damage;
    public Sprite sprite;

    private bool isMoving;
    void Start()
    {
        isMoving = true;
    }

    void Update()
    {
        if(Vector3.Distance(transform.position, target) < 1f)
        {
            isMoving = false;
        }
        else
        {
            transform.position -= (transform.position - target).normalized * 0.5f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Boss" && !isMoving && collision.gameObject.GetComponent<ResearcherController>().GetIndex().Equals(index))
        {
            collision.gameObject.GetComponent<ResearcherController>().plateTouched();
            Destroy(this.gameObject);
        }

        if(collision.tag == "Player" && isMoving)
        {
            collision.gameObject.GetComponent<PlayerHealth>().PlayerDamaged(damage, sprite);
        }
    }
}
