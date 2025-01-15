using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveStage : MonoBehaviour
{
    private PlayerController player;
    private CanvasGroup fade;
    private InteractionWindow interaction;
    private bool isTouch;
    private bool moving;

    public string nextStage;

    void Start()
    {
        interaction = FindObjectOfType<InteractionWindow>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        fade = FindObjectOfType<CanvasGroup>();

        isTouch = false;
        moving = false;
    }

    void Update()
    {
        if (isTouch)
        {
            if (Input.GetKeyDown(KeyCode.F) && Time.timeScale == 1 && !moving)
            {
                if (SceneManager.GetActiveScene().name.Contains("Tutorial"))
                {
                    if (player.controllAble)
                    {
                        player.controllAble = false;
                        StartCoroutine(FadeOut(true));
                    }
                }
                else if (GameManager.gameManager.stage < 7 && player.weaponSlot.WeaponCount() != 0)
                {
                    if (!SceneManager.GetActiveScene().name.Contains("Start"))
                    {
                        GameManager.gameManager.playTime += Time.timeSinceLevelLoad;
                    }
                    player.controllAble = false;
                    StartCoroutine(FadeOut(false));
                }
            }
        }
    }

    private void MoveNextStage(string name)
    {
        GameManager.gameManager.stage++;

        GameManager.gameManager.charData.SaveData();
        GameManager.gameManager.charData.DeleteMapData();
        GameManager.gameManager.SetNextStage(name);
        Debug.Log(GameManager.gameManager.GetNextStage());
        if(GameManager.gameManager.stage == 8)
        {
            //Ending Scene
        }
        else
        {
            SceneManager.LoadScene("Loading");
        }
    }

    private void EndTutorial()
    {
        GameManager.gameManager.inventory.inventory.Clear();
        GameManager.gameManager.SetNextStage("2_OpeningCutScene");
        SceneManager.LoadScene("Loading");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            isTouch = true;
            interaction.SetMoveStageAble(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isTouch = false;
            interaction.SetMoveStageAble(false);
        }
    }

    private IEnumerator FadeOut(bool isTuto)
    {
        moving = true;

        while(fade.alpha < 1)
        {
            fade.alpha += Time.deltaTime;
            yield return null;
        }

        if(isTuto)
        {
            EndTutorial();
        }
        else
        {
            MoveNextStage(nextStage);
        }
    }
}
