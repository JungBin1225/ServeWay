using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowExplane : MonoBehaviour
{
    public List<Animator> arrows;
    public Image key;

    public Sprite up_n;
    public Sprite up_p;
    public Sprite down_n;
    public Sprite down_p;
    public Sprite left_n;
    public Sprite left_p;
    public Sprite right_n;
    public Sprite right_p;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnEnable()
    {
        StartCoroutine(Explane());
    }

    private void OnDisable()
    {
        StopCoroutine(Explane());
    }

    private IEnumerator Explane()
    {
        while (true)
        {
            for (int j = 0; j < 5; j++)
            {
                arrows[j].gameObject.SetActive(true);
            }

            for (int j = 0; j < 5; j++)
            {
                switch(j)
                {
                    case 0:
                        key.sprite = up_n;
                        break;
                    case 1:
                        key.sprite = down_n;
                        break;
                    case 2:
                        key.sprite = right_n;
                        break;
                    case 3:
                        key.sprite = left_n;
                        break;
                    case 4:
                        key.sprite = left_n;
                        break;
                }
                yield return new WaitForSecondsRealtime(0.2f);

                switch (j)
                {
                    case 0:
                        key.sprite = up_p;
                        break;
                    case 1:
                        key.sprite = down_p;
                        break;
                    case 2:
                        key.sprite = right_p;
                        break;
                    case 3:
                        key.sprite = left_p;
                        break;
                    case 4:
                        key.sprite = left_p;
                        break;
                }
                arrows[j].SetTrigger("Correct");
                yield return new WaitForSecondsRealtime(0.2f);
            }

            for (int j = 0; j < 5; j++)
            {
                arrows[j].gameObject.SetActive(false);
            }
        }
    }
}
