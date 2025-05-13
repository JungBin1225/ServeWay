using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    public void FadeIn(Image image)
    {
        StopCoroutine("FadeOutCoroutine");
        StartCoroutine("FadeInCoroutine", image);
    }
    IEnumerator FadeInCoroutine(Image image)
    {
        float fadeInCount = 0;
        while (fadeInCount < 1.0f)
        {
            fadeInCount += 0.02f;
            yield return new WaitForSeconds(0.01f);
            if (image.gameObject.tag == "White")
            {
                image.color = new Color(1, 1, 1, fadeInCount);
            } else
            {
                image.color = new Color(0, 0, 0, fadeInCount);
            }
        }
    }

    public void FadeOut(Image image)
    {
        StopCoroutine("FadeInCoroutine");
        StartCoroutine("FadeOutCoroutine", image);
    }
    IEnumerator FadeOutCoroutine(Image image)
    {
        float fadeInCount = 1.0f;
        while (fadeInCount > 0.0f)
        {
            fadeInCount -= 0.02f;
            yield return new WaitForSeconds(0.01f);
            if (image.gameObject.tag == "White")
            {
                image.color = new Color(1, 1, 1, fadeInCount);
            }
            else
            {
                image.color = new Color(0, 0, 0, fadeInCount);
            }
        }
    }
}
