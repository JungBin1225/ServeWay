using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : MonoBehaviour
{
    private Animator ani;

    void Start()
    {
        ani = GetComponent<Animator>();
    }


    void Update()
    {
        if (ani.GetCurrentAnimatorStateInfo(0).normalizedTime > ani.GetCurrentAnimatorStateInfo(0).length + 0.5f)
        {
            Destroy(this.gameObject);
        }
    }
}
