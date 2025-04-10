using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AttackTutorial : MonoBehaviour
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
    public string missionText;
    public GameObject minimapPlayer;
    public GameObject minimapTile;
    public GameObject minimapRoad;

    private PlayerController player;
    private TutorialMissionUI mission;
    private DataController data;
    private bool isTalking;
    private bool isClicked;
    private bool isMission;
    private bool isDamaged;
    private bool isClear;
    private bool isKill;
    private float missionAmount;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        data = FindObjectOfType<DataController>();
        mission = FindObjectOfType<TutorialMissionUI>();
        isTalking = false;
        isClicked = false;
        isClear = false;
        isMission = false;
        isDamaged = false;
        isKill = false;
        missionAmount = 0;

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

            if (!isDamaged) //enemy attack
            {
                if(enemy.GetNowHp() < enemy.maxHp / 2)
                {
                    missionAmount = 10;
                }
            }
            else //charge
            {
                if (!isKill)
                {
                    if (enemy == null)
                    {
                        missionAmount = 10;
                        mission.UpdateMission(missionText, 1, true);
                    }
                }
            }
        }

        if (missionAmount > 5 && !isClear)
        {
            if (!isDamaged)
            {
                isDamaged = true;
                enemy.attackAble = false;
                missionAmount = 0;
                StartCoroutine(StartDialog(clearText1));
            }
            else
            {
                DropIngredient(4, 4);
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

    private void DropIngredient(int min, int max)
    {
        int dropAmount = Random.Range(min, max + 1);
        float radius = 2.5f;

        for (int i = 0; i < dropAmount; i++)
        {
            float angle = i * Mathf.PI * 2 / dropAmount;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            Vector3 pos = transform.position + new Vector3(x, y, 0);
            float angleDegrees = -angle * Mathf.Rad2Deg;

            Ingredient ingredient = data.IngredientList.IngredientList[i];
            GameObject item = Instantiate(ingredient.prefab, pos, Quaternion.Euler(0, 0, 0));
            item.GetComponent<GetIngredients>().itemName = ingredient.name;
            item.GetComponent<GetIngredients>().SetSprite(ingredient.sprite, ((int)ingredient.grade));
        }
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
        }

        if (collision.gameObject.tag == "Player")
        {
            minimapPlayer.transform.position = transform.position;
            minimapTile.SetActive(false);
            minimapRoad.SetActive(false);
        }
    }
}
