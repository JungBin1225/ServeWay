using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    public void FadeIn(SpriteRenderer sprite)
    {
        StopCoroutine("FadeOutCoroutine");
        StartCoroutine("FadeInCoroutine", sprite);
    }
    IEnumerator FadeInCoroutine(SpriteRenderer sprite)
    {
        float fadeInCount = 0;
        while (fadeInCount < 1.0f)
        {
            fadeInCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            if (sprite.gameObject.tag == "White")
            {
                sprite.color = new Color(1, 1, 1, fadeInCount);
            } else
            {
                sprite.color = new Color(0, 0, 0, fadeInCount);
            }
        }
    }

    public void FadeOut(SpriteRenderer sprite)
    {
        StopCoroutine("FadeInCoroutine");
        StartCoroutine("FadeOutCoroutine", sprite);
    }
    IEnumerator FadeOutCoroutine(SpriteRenderer sprite)
    {
        float fadeInCount = 1.0f;
        while (fadeInCount > 0.0f)
        {
            fadeInCount -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            if (sprite.gameObject.tag == "White")
            {
                sprite.color = new Color(1, 1, 1, fadeInCount);
            }
            else
            {
                sprite.color = new Color(0, 0, 0, fadeInCount);
            }
        }
    }
}
