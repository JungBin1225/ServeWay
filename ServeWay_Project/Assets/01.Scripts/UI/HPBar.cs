using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    private PlayerHealth playerHealth;
    private List<Image> hpImages;

    [SerializeField] Sprite empty;
    [SerializeField] Sprite full;
    [SerializeField] Sprite half;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();

        hpImages = new List<Image>();
        for(int i = 0; i < transform.childCount; i++)
        {
            hpImages.Add(transform.GetChild(i).GetComponent<Image>());
        }

        Debug.Log(hpImages.Count);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerHPBar();
    }

    public void PlayerHPBar()
    {
        int hp = (int)playerHealth.nowHp;

        if(hp % 2 == 0)
        {
            for(int i = 0; i < hpImages.Count; i++)
            {
                if(i < hp / 2)
                {
                    hpImages[i].sprite = full;
                }
                else
                {
                    hpImages[i].sprite = empty;
                }
            }
        }
        else
        {
            for (int i = 0; i < hpImages.Count; i++)
            {
                if (i < hp / 2)
                {
                    hpImages[i].sprite = full;
                }
                else if(i < (hp + 1) / 2)
                {
                    hpImages[i].sprite = half;
                }
                else
                {
                    hpImages[i].sprite = empty;
                }
            }
        }
    }
}
