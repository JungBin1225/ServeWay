using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeechBubble : MonoBehaviour
{
    public TMP_Text text;
    public GameObject speech;
    public GameObject think;

    void Start()
    {
        speech.SetActive(true);
        think.SetActive(false);
    }

    void Update()
    {
        if(text.text[0] == '(' && speech.activeSelf)
        {
            speech.SetActive(false);
            think.SetActive(true);
        }
        else if(text.text[0] != '(' && !speech.activeSelf)
        {
            speech.SetActive(true);
            think.SetActive(false);
        }
    }
}
