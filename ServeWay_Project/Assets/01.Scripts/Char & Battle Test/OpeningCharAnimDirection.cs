using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningCharAnimDirection : MonoBehaviour
{
    public string direction;

    private Animator anim;
    private Vector3 pos;
    void Start()
    {
        anim = GetComponent<Animator>();
        pos = transform.position;

        SetDir(direction);
    }

    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        pos = transform.position;

        SetDir(direction);
    }

    void Update()
    {
        if(direction == "walk" || direction == "stop")
        {
            if (pos != transform.position)
            {
                anim.SetBool("walk", true);
            }
            else
            {
                anim.SetBool("walk", false);
            }

            pos = transform.position;
        }

        if(SceneManager.GetActiveScene().name.Contains("Opening") && gameObject.name.Contains("Player") && direction == "back")
        {
            if (pos != transform.position)
            {
                anim.SetBool("side", false);
                anim.SetBool("front", false);
                anim.SetBool("back", true);
            }
            else
            {
                anim.SetBool("back", false);
                anim.SetBool("front", false);
                anim.SetBool("side", true);
            }

            pos = transform.position;
        }
        else if(SceneManager.GetActiveScene().name.Contains("Ending") && gameObject.name.Contains("Player"))
        {
            anim.speed = 2;
        }
    }

    public void SetDir(string direction)
    {
        switch (direction)
        {
            case "front":
                anim.SetBool("back", false);
                anim.SetBool("side", false);
                anim.SetBool("front", true);
                break;
            case "back":
                anim.SetBool("side", false);
                anim.SetBool("front", false);
                anim.SetBool("back", true);
                break;
            case "side":
                anim.SetBool("back", false);
                anim.SetBool("front", false);
                anim.SetBool("side", true);
                break;

            case "walk":
                anim.SetBool("walk", true);
                break;
            case "stop":
                anim.SetBool("walk", false);
                break;
        }
    }
}
