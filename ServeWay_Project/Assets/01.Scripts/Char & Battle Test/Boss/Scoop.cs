using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoop : MonoBehaviour
{
    private bool isTouch;

    public string touchedObject;

    void Start()
    {
        isTouch = false;
    }

    void Update()
    {
        
    }

    public bool GetTouch()
    {
        return isTouch;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isTouch && (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Boss"))
        {
            touchedObject = collision.gameObject.tag;
            isTouch = true;
        }
    }
}
