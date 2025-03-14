using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    public Animator anim;

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void OpenDoor()
    {
        anim.SetTrigger("Open");
    }

    public void CloseDoor()
    {
        anim.SetTrigger("Close");
    }
}
