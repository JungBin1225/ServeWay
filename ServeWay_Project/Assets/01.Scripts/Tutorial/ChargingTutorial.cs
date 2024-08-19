using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChargingTutorial : MonoBehaviour
{
    public GameObject playerBox;
    public GameObject teacherBox;
    public TMP_Text playerText;
    public TMP_Text teacherText;
    public TextAsset textFile;
    public TextAsset clearText1;
    public TextAsset clearText2;
    public GameObject door;
    public TutorialEnemy enemy;

    private PlayerController player;
    private bool isTalking;
    private bool isClicked;
    private bool isMission;
    private bool isDamaged;
    private bool isClear;
    private bool isCharge;
    private float missionAmount;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        isTalking = false;
        isClicked = false;
        isClear = false;
        isMission = false;
        isDamaged = false;
        isCharge = false;
        missionAmount = 0;

        playerBox.SetActive(false);
        teacherBox.SetActive(false);
    }

    void Update()
    {
        if (isTalking && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            isClicked = true;
        }

        if (!isTalking && isMission && !isClear)
        {
            if(!isDamaged) //enemy attack
            {
                if(missionAmount == 0)
                {
                    enemy.attackAble = true;
                }
            }
            else //charge
            {
                if(!isCharge)
                {
                    if(player.isCharge)
                    {
                        missionAmount++;
                    }
                }
                isCharge = player.isCharge;
            }
        }

        if (missionAmount > 5 && !isClear)
        {
            if(!isDamaged)
            {
                isDamaged = true;
                enemy.attackAble = false;
                missionAmount = 0;
                StartCoroutine(StartDialog(clearText1));
            }
            else
            {
                Destroy(enemy.gameObject);
                isClear = true;
                for (int i = 0; i < door.transform.childCount; i++)
                {
                    door.transform.GetChild(i).GetComponent<DoorAnimation>().OpenDoor();
                }
                StartCoroutine(StartDialog(clearText2));
            }
        }
    }

    private IEnumerator StartDialog(TextAsset text)
    {
        player.controllAble = false;
        isTalking = true;
        GameManager.gameManager.menuAble = false;

        string dialog = text.text;
        string[] message = dialog.Split('\n');

        for (int i = 0; i < message.Length; i++)
        {
            TMP_Text currentText;

            if (message[i][0].Equals('P'))
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

            for (int num = 2; num < message[i].Length; num++)
            {
                if (isClicked)
                {
                    isClicked = false;
                    currentText.text = message[i].Substring(2);
                    break;
                }
                else
                {
                    currentText.text += message[i][num];
                }
                player.controllAble = false;
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

    public void AddMissonAmount()
    {
        missionAmount++;
        return;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !isClear)
        {
            StartCoroutine(StartDialog(textFile));
            for (int i = 0; i < door.transform.childCount; i++)
            {
                door.transform.GetChild(i).GetComponent<DoorAnimation>().CloseDoor();
            }
        }
    }
}
