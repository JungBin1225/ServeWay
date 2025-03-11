using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageStartMessage : MonoBehaviour
{
    public GameObject messagePrefab;

    private Animator anim;

    void Start()
    {
        GameObject message = Instantiate(messagePrefab, this.transform);
        GameManager.gameManager.menuAble = false;
        anim = message.GetComponent<Animator>();

        string theme = GameManager.gameManager.ThemeToString(GameManager.gameManager.stageThemes[GameManager.gameManager.stage - 1]);

        message.transform.GetChild(1).GetComponent<TMP_Text>().text = string.Format("{0}차 시험", GameManager.gameManager.stage);
        message.transform.GetChild(2).GetComponent<TMP_Text>().text = theme;

        Time.timeScale = 0;
    }

    void Update()
    {
        
    }
}
