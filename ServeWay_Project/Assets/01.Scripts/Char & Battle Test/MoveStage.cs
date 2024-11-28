using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveStage : MonoBehaviour
{
    private PlayerController player;
    private CanvasGroup fade;

    public string nextStage;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        fade = FindObjectOfType<CanvasGroup>();
    }

    void Update()
    {
        
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
        GameManager.gameManager.SetNextStage("StartMap");
        SceneManager.LoadScene("Loading");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (SceneManager.GetActiveScene().name.Contains("Tutorial"))
            {
                if(player.controllAble)
                {
                    player.controllAble = false;
                    StartCoroutine(FadeOut(true));
                }
            }
            else if(GameManager.gameManager.stage < 7 && player.weaponSlot.WeaponCount() != 0)
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

    private IEnumerator FadeOut(bool isTuto)
    {
        

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
