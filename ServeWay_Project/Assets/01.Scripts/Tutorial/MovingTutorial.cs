using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MovingTutorial : MonoBehaviour
{
    public GameObject playerBox;
    public GameObject teacherBox;
    public TMP_Text playerText;
    public TMP_Text teacherText;
    public TextAsset textFile;
    public TextAsset clearText;
    public GameObject door;

    private PlayerController player;
    private bool isTalking;
    private bool isClicked;
    private bool isMission;
    private bool isClear;
    private float missionAmount;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        isTalking = false;
        isClicked = false;
        isClear = false;
        isMission = false;
        missionAmount = 0;

        playerBox.SetActive(false);
        teacherBox.SetActive(false);
        door.SetActive(false);
    }

    void Update()
    {
        if(isTalking && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            isClicked = true;
        }

        if(!isTalking && isMission && !isClear)
        {
            if(player.GetPlayerVel() > 0)
            {
                missionAmount += Time.deltaTime;
            }
        }

        if(missionAmount > 5 && !isClear)
        {
            isClear = true;
            door.SetActive(false);
            StartCoroutine(StartDialog(clearText));
        }
    }

    private IEnumerator StartDialog(TextAsset text)
    {
        player.controllAble = false;
        isTalking = true;
        GameManager.gameManager.menuAble = false;

        string dialog = text.text;
        string[] message = dialog.Split('\n');

        for(int i = 0; i < message.Length; i++)
        {
            TMP_Text currentText;

            if(message[i][0].Equals('P'))
            {
                playerBox.SetActive(true);
                teacherBox.SetActive(false);
                currentText = playerText;
                currentText.text = "";
            }
            else
            {
                playerBox.SetActive(false);
                teacherBox.SetActive(true);
                currentText = teacherText;
                currentText.text = "";
            }

            for(int num = 2; num < message[i].Length; num++)
            {
                if(isClicked)
                {
                    isClicked = false;
                    currentText.text = message[i].Substring(2);
                    break;
                }
                else
                {
                    currentText.text += message[i][num];
                }
                yield return new WaitForSeconds(0.05f);
            }

            yield return new WaitUntil(() => isClicked);
            isClicked = false;
            yield return new WaitForSeconds(0.1f);
        }

        playerBox.SetActive(false);
        teacherBox.SetActive(false);
        isTalking = false;
        isMission = true;
        GameManager.gameManager.menuAble = true;
        player.controllAble = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && !isClear)
        {
            StartCoroutine(StartDialog(textFile));
            door.SetActive(true);
        }
    }
}
