using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotBubble : MonoBehaviour
{
    [SerializeField] private List<GameObject> bubbles;


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnEnable()
    {
        foreach (GameObject bubble in bubbles)
        {
            StartCoroutine(bubbleAppear(bubble));
        }
    }

    private IEnumerator bubbleAppear(GameObject bubble)
    {
        bubble.SetActive(false);
        float time = Random.Range(0.5f, 3.0f);
        yield return new WaitForSeconds(time);

        while(true)
        {
            float x = Random.Range(-203.0f, 203.0f);
            bubble.GetComponent<RectTransform>().anchoredPosition = new Vector3(x, 289, 0);

            bubble.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            
            bubble.SetActive(false);
            time = Random.Range(1.0f, 3.0f);
            yield return new WaitForSeconds(time);
        }
    }
}
