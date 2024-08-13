using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossTutorial : MonoBehaviour
{
    public GameObject playerBox;
    public GameObject teacherBox;
    public TMP_Text playerText;
    public TMP_Text teacherText;
    public TextAsset textFile;
    public TextAsset clearText;
    public GameObject door;
    public TutorialBoss boss;

    private PlayerController player;
    private DataController data;
    private MissonManager misson;
    private bool isTalking;
    private bool isClicked;
    private bool isMission;
    private bool isClear;
    private bool kiiled;
    private float time;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        data = FindObjectOfType<DataController>();
        misson = FindObjectOfType<MissonManager>();
        isTalking = false;
        isClicked = false;
        isClear = false;
        isMission = false;
        kiiled = false;
        time = 0;

        playerBox.SetActive(false);
        teacherBox.SetActive(false);
        door.SetActive(false);
    }

    void Update()
    {
        if (isTalking && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            isClicked = true;
        }

        if (!isTalking && isMission && !isClear)
        {
            if (boss == null)
            {
                player.controllAble = false;

                if (time < 2)
                {
                    time += Time.deltaTime;
                }
                else
                {
                    kiiled = true;
                }
            }
        }

        if (kiiled && !isClear)
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

    public void DropIngredient(int min, int max)
    {
        int dropAmount = Random.Range(min, max + 1);
        float radius = 5f;

        for (int i = 0; i < dropAmount; i++)
        {
            float angle = i * Mathf.PI * 2 / dropAmount;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            Vector3 pos = transform.position + new Vector3(x, y, 0);
            float angleDegrees = -angle * Mathf.Rad2Deg;

            Ingredient ingred = data.IngredientList.IngredientList[i];
            GameObject item = Instantiate(ingred.prefab, pos, Quaternion.Euler(0, 0, 0));
            item.GetComponent<GetIngredients>().itemName = ingred.name;
            item.GetComponent<GetIngredients>().SetSprite(ingred.sprite);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !isClear)
        {
            StartCoroutine(StartDialog(textFile));
            door.SetActive(true);
            GameManager.gameManager.isBossStage = true;
            StartCoroutine(misson.MissonAppear());
        }
    }
}
