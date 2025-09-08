using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingFade : MonoBehaviour
{
    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        image = GetComponent<Image>();

        if(image.color.a == 1)
        {
            StartCoroutine(FadeIn());
        }
        else
        {
            StartCoroutine(FadeOut());
        }
    }

    void Update()
    {
        
    }

    IEnumerator FadeIn()
    {
        while(image.color.a > 0)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - (Time.deltaTime * 0.5f));
            yield return null;
        }

        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
    }

    IEnumerator FadeOut()
    {
        while (image.color.a < 1)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + (Time.deltaTime * 0.5f));
            yield return null;
        }

        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);

        if(gameObject.name.Contains("Black"))
        {
            StartCoroutine(FadeIn());
        }
    }
}
