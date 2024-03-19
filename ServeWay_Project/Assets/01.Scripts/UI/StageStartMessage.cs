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
        anim = message.GetComponent<Animator>();

        message.GetComponent<TMP_Text>().text = string.Format("{0}�� ����: {1}", GameManager.gameManager.stage, "ī��");
    }

    void Update()
    {
        
    }
}
