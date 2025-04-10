using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CookTutorial : MonoBehaviour
{
    public GameObject playerBox;
    public GameObject teacherBox;
    public TMP_Text playerText;
    public TMP_Text teacherText;
    public TextAsset textFile;
    public TextAsset clearText;
    public GameObject door;
    public GameObject cookUI;
    public string missionText;
    public GameObject minimapPlayer;
    public GameObject minimapTile;
    public GameObject minimapRoad;

    private PlayerController player;
    private TutorialMissionUI mission;
    private bool isTalking;
    private bool isClicked;
    private bool isMission;
    private bool isClear;
    private bool maked;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        mission = FindObjectOfType<TutorialMissionUI>();
        isTalking = false;
        isClicked = false;
        isClear = false;
        isMission = false;
        maked = false;

        missionText = missionText.Replace("\\n", "\n");
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
            if (!mission.isAppear)
            {
                StartCoroutine(mission.MissonAppear());
            }

            if (player.weaponSlot.ReturnWeaponList().Count > 0)
            {
                maked = true;
                mission.UpdateMission(missionText, 1, true);
            }
        }

        if (maked && !cookUI.activeSelf && !isClear)
        {
            isClear = true;
            for (int i = 0; i < door.transform.childCount; i++)
            {
                door.transform.GetChild(i).GetComponent<DoorAnimation>().OpenDoor();
            }
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !isClear)
        {
            mission.MissonDisappear();
            mission.SetMission(missionText, 1);

            StartCoroutine(StartDialog(textFile));
            for (int i = 0; i < door.transform.childCount; i++)
            {
                door.transform.GetChild(i).GetComponent<DoorAnimation>().CloseDoor();
            }
            GameManager.gameManager.inventory.GetItem(Ingred_Name.Kimchi, 1);
            GameManager.gameManager.inventory.GetItem(Ingred_Name.Rice, 3);
            GameManager.gameManager.inventory.GetItem(Ingred_Name.Oil, 1);
            GameManager.gameManager.inventory.GetItem(Ingred_Name.Egg, 1);
        }

        if (collision.gameObject.tag == "Player")
        {
            minimapPlayer.transform.position = transform.position;
            minimapTile.SetActive(false);
            minimapRoad.SetActive(false);
        }
    }
}
