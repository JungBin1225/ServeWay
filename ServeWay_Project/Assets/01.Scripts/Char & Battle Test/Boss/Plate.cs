using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plate : MonoBehaviour
{
    public int index;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Boss" && collision.gameObject.GetComponent<ResearcherController>().GetIndex().Equals(index))
        {
            collision.gameObject.GetComponent<ResearcherController>().plateTouched();
            Destroy(this.gameObject);
        }
    }
}
