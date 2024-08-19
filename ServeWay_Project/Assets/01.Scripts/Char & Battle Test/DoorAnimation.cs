using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    public BoxCollider2D collider2D;
    public Animator leftAnim_W;
    public Animator rightAnim_W;
    public Animator leftAnim_L;
    public Animator rightAnim_L;

    void Start()
    {
        /*if(Mathf.Abs(transform.rotation.eulerAngles.z) == 90)
        {
            leftAnim_L.gameObject.SetActive(true);
            rightAnim_L.gameObject.SetActive(true);
            leftAnim_W.gameObject.SetActive(false);
            rightAnim_W.gameObject.SetActive(false);
        }
        else
        {
            
        }*/

        leftAnim_L.gameObject.SetActive(false);
        rightAnim_L.gameObject.SetActive(false);
        leftAnim_W.gameObject.SetActive(true);
        rightAnim_W.gameObject.SetActive(true);

        collider2D.enabled = false;
    }

    void Update()
    {
        
    }

    public void OpenDoor()
    {
        collider2D.enabled = false;

        /*if (Mathf.Abs(transform.rotation.eulerAngles.z) == 90)
        {
            leftAnim_L.SetTrigger("Open");
            rightAnim_L.SetTrigger("Open");
        }
        else
        {
            
        }*/

        leftAnim_W.SetTrigger("Open");
        rightAnim_W.SetTrigger("Open");
    }

    public void CloseDoor()
    {
        collider2D.enabled = true;

        /*if (Mathf.Abs(transform.rotation.eulerAngles.z) == 90 )
        {
            leftAnim_L.SetTrigger("Close");
            rightAnim_L.SetTrigger("Close");
        }
        else
        {
            
        }*/

        leftAnim_W.SetTrigger("Close");
        rightAnim_W.SetTrigger("Close");
    }
}
