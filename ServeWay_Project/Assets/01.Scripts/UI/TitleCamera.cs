using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCamera : MonoBehaviour
{
    public GameObject mainCamera;
    public int tileNum;

    private Animator anim;
    private int index;

    void Start()
    {
        anim = GetComponent<Animator>();
        index = 1;

        StartCoroutine(StartAnimation());
    }

    void Update()
    {
        
    }

    private IEnumerator StartAnimation()
    {
        yield return new WaitForSeconds(4f);

        anim.SetTrigger("Fade");
    }

    public void moveCamera()
    {
        if(tileNum > index)
        {
            index++;
        }
        else
        {
            index = 1;
        }

        mainCamera.transform.position = new Vector3(20 * (index - 1), -0.2f, -10);
    }

    public void animEnd()
    {
        StartCoroutine(StartAnimation());
    }
}
