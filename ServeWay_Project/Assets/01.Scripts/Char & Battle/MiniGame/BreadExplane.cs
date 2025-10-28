using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreadExplane : MonoBehaviour
{
    public Animator anim;

    public Image key;
    public Sprite a_n;
    public Sprite a_p;
    public Sprite s_n;
    public Sprite s_p;
    public Sprite t_n;
    public Sprite t_p;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnEnable()
    {
        key.sprite = a_n;
        StartCoroutine(Explane());
    }

    private void OnDisable()
    {
        StopCoroutine(Explane());
    }

    private IEnumerator Explane()
    {
        int time = 0;

        while(true)
        {
            time = 0;
            while(time < 15)
            {
                anim.SetTrigger(Random.Range(1, 4).ToString());

                if (time % 2 == 0)
                {
                    key.sprite = a_n;
                }
                else
                {
                    key.sprite = a_p;
                }
                yield return new WaitForSecondsRealtime(0.2f);
                time++;
            }

            time = 0;
            while (time < 15)
            {
                anim.SetTrigger(Random.Range(1, 4).ToString());

                if (time % 2 == 0)
                {
                    key.sprite = s_n;
                }
                else
                {
                    key.sprite = s_p;
                }
                yield return new WaitForSecondsRealtime(0.2f);
                time++;
            }

            time = 0;
            while (time < 15)
            {
                anim.SetTrigger(Random.Range(1, 4).ToString());

                if (time % 2 == 0)
                {
                    key.sprite = t_n;
                }
                else
                {
                    key.sprite = t_p;
                }
                yield return new WaitForSecondsRealtime(0.2f);
                time++;
            }
        }
    }
}
