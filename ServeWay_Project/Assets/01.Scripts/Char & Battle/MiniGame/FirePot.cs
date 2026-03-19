using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePot : MonoBehaviour
{
    [SerializeField] private List<GameObject> bubbles;

    void Start()
    {
        
    }

    private void OnEnable()
    {
        foreach (GameObject bubble in bubbles)
        {
            StartCoroutine(bubbleAppear(bubble));
        }
    }

    void Update()
    {
        
    }

    private IEnumerator bubbleAppear(GameObject bubble)
    {
        bubble.SetActive(false);
        float time = Random.Range(0.5f, 2.0f);
        yield return new WaitForSecondsRealtime(time);

        while (true)
        {
            Vector3 pos = RandomPos();
            bubble.GetComponent<RectTransform>().anchoredPosition = pos;

            bubble.SetActive(true);
            yield return new WaitForSecondsRealtime(1.5f);

            bubble.SetActive(false);
            time = Random.Range(0.5f, 2.0f);
            yield return new WaitForSecondsRealtime(time);
        }
    }

    private Vector3 RandomPos()
    {
        float x = Random.Range(-60.0f, 60.0f);
        float y = 0;

        if(x >= -32 && x <= 32)
        {
            y = Random.Range(43.0f, 85.0f);
        }
        else if((x > 32 && x <= 51) || (x < -32 && x >= -51))
        {
            y = Random.Range(53.0f, 75.0f);
        }
        else
        {
            y = Random.Range(62.0f, 67.0f);
        }

        return new Vector3(x, y, 0);
    }
}
