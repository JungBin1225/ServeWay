using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] Image hpBar;
    [SerializeField] Text hpText;


    // Start is called before the first frame update
    void Start()
    {
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerHPBar();
    }

    public void PlayerHPBar()
    {
        float HP = playerHealth.nowHp;
        hpBar.fillAmount = HP / 100f;
        hpText.text = string.Format("HP {0}/100", HP);
    }
}
