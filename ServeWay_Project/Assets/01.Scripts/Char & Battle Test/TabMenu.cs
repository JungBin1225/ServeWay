using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TabMenu : MonoBehaviour
{
    public GameObject buttonGroup;
    public List<GameObject> panel;
    public bool interAble;

    private Animator anim;
    private bool isOpen;
    private int index;

    void Start()
    {
        anim = transform.GetChild(1).GetComponent<Animator>();
        isOpen = false;
        interAble = true;
        index = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && GameManager.gameManager.menuAble && interAble)
        {
            if (!isOpen)
            {
                if (Time.timeScale == 1)
                {
                    StartCoroutine(OpenBook());
                }
            }
            else
            {
                StartCoroutine(CloseBook());
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape) && isOpen && interAble)
        {
            StartCoroutine(CloseBook());
        }
    }

    private IEnumerator OpenBook()
    {
        Time.timeScale = 0;
        interAble = false;
        GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);
        transform.GetChild(1).gameObject.SetActive(true);

        anim.SetTrigger("Open");
        float time = 0;
        foreach(AnimationClip clip in anim.runtimeAnimatorController.animationClips)
        {
            if(clip.name.Equals("Open"))
            {
                time = clip.length;
            }
        }
        
        yield return new WaitForSecondsRealtime(time);

        buttonGroup.SetActive(true);

        isOpen = true;
        index = 0;
        OpenPanel(index);
        interAble = true;
    }

    private IEnumerator CloseBook()
    {
        interAble = false;
        buttonGroup.SetActive(false);
        for (int i = 0; i < buttonGroup.transform.childCount; i++)
        {
            buttonGroup.transform.GetChild(i).GetComponent<Button>().interactable = false;
            buttonGroup.transform.GetChild(i).GetComponent<Button>().interactable = true;
        }
        foreach (GameObject menu in panel)
        {
            menu.SetActive(false);
        }

        anim.SetTrigger("Close");
        float time = 0;
        foreach (AnimationClip clip in anim.runtimeAnimatorController.animationClips)
        {
            if (clip.name.Equals("Close"))
            {
                time = clip.length;
            }
        }

        yield return new WaitForSecondsRealtime(time);

        isOpen = false;
        transform.GetChild(1).gameObject.SetActive(false);
        GetComponent<Image>().color = new Color(0, 0, 0, 0);
        Time.timeScale = 1;
        interAble = true;
    }

    private IEnumerator FlipBook(int num)
    {
        interAble = false;
        foreach (GameObject menu in panel)
        {
            menu.SetActive(false);
        }

        float time = 0;
        if (index > num)
        {
            anim.SetTrigger("FlipRight");
            foreach (AnimationClip clip in anim.runtimeAnimatorController.animationClips)
            {
                if (clip.name.Equals("FlipRight"))
                {
                    time = clip.length;
                }
            }
        }
        else
        {
            anim.SetTrigger("FlipLeft");
            foreach (AnimationClip clip in anim.runtimeAnimatorController.animationClips)
            {
                if (clip.name.Equals("FlipLeft"))
                {
                    time = clip.length;
                }
            }
        }

        yield return new WaitForSecondsRealtime(time);

        index = num;
        OpenPanel(num);
        interAble = true;
    }

    public void OpenPanel(int index)
    {
        foreach (GameObject menu in panel)
        {
            menu.SetActive(false);
        }

        switch (index)
        {
            case 0:
                //mini map
                panel[index].SetActive(true);
                break;
            case 1:
                //inventory
                panel[index].SetActive(true);
                break;
            case 2:
                //food&ingred info
                panel[index].SetActive(true);
                break;
        }
    }

    private IEnumerator ButtonAnimation(bool isFlip, int num)
    {
        buttonGroup.transform.GetChild(num).GetComponent<Button>().animationTriggers.highlightedTrigger = "Normal";
        EventSystem.current.SetSelectedGameObject(null);

        float time = 0;
        if(isFlip)
        {
            foreach (AnimationClip clip in anim.runtimeAnimatorController.animationClips)
            {
                if (clip.name.Equals("FlipRight"))
                {
                    time = clip.length;
                }
            }
        }
        else
        {
            time = 0.5f;
        }

        yield return new WaitForSecondsRealtime(time);

        buttonGroup.transform.GetChild(num).GetComponent<Button>().animationTriggers.highlightedTrigger = "Highlighted";
    }

    public void OnIndexButtonClicked(int num)
    {
        bool isFlip = false;

        if(interAble && index != num)
        {
            isFlip = true;
            StartCoroutine(FlipBook(num));
        }

        StartCoroutine(ButtonAnimation(isFlip, num));
    }
}
