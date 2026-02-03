using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoutuberDislike : EnemyBullet
{
    public bool isFire;

    void Start()
    {
        isFire = false;
        effectParent = GameObject.Find("EffectList");
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
    }

    void Update()
    {
        if(isFire)
        {
            Fire();
        }
    }
}
