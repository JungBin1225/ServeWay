using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    private PlayerHealth playerHealth;
    private List<Image> hpImages;
    private List<Animator> hpAnim;
    private float time;

    [SerializeField] Sprite empty;
    [SerializeField] Sprite full;
    [SerializeField] Sprite half;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();

        hpImages = new List<Image>();
        hpAnim = new List<Animator>();
        time = 0;

        for(int i = 0; i < transform.childCount; i++)
        {
            hpImages.Add(transform.GetChild(i).GetComponent<Image>());
        }

        foreach(Image image in hpImages)
        {
            hpAnim.Add(image.gameObject.GetComponent<Animator>());
        }

        StartCoroutine(HpIdleAnim());
    }

    // Update is called once per frame
    void Update()
    {
        PlayerHPBar();
    }

    public void PlayerHPBar()
    {
        int hp = (int)playerHealth.nowHp;

        if(time < 3)
        {
            time += Time.deltaTime;
        }

        if(hp % 2 == 0)
        {
            for(int i = 0; i < hpImages.Count; i++)
            {
                if(i < hp / 2)
                {
                    if(hpImages[i].sprite != full)
                    {
                        PlayHpAnim(i, 2);
                    }
                }
                else
                {
                    if (hpImages[i].sprite != empty)
                    {
                        PlayHpAnim(i, 0);
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < hpImages.Count; i++)
            {
                if (i < hp / 2)
                {
                    if (hpImages[i].sprite != full)
                    {
                        PlayHpAnim(i, 2);
                    }
                }
                else if(i < (hp + 1) / 2)
                {
                    if (hpImages[i].sprite != half)
                    {
                        PlayHpAnim(i, 1);
                    }
                }
                else
                {
                    if (hpImages[i].sprite != empty)
                    {
                        PlayHpAnim(i, 0);
                    }
                }
            }
        }
    }

    private IEnumerator HpIdleAnim()
    {
        while(true)
        {
            yield return new WaitForSeconds(2);
            foreach(Animator anim in hpAnim)
            {
                if(anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("Idle") || anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    anim.SetTrigger("idle");
                }
                
            }
        }
    }

    private void PlayHpAnim(int index, int state)
    {
        hpAnim[index].SetInteger("state", state);
        if(time >= 3)
        {
            hpAnim[index].SetTrigger("change");
        }
        else
        {
            hpAnim[index].SetTrigger("instant");
        }
    }
}
