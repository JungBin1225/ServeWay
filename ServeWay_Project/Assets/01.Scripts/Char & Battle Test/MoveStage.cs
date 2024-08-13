using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveStage : MonoBehaviour
{
    private PlayerController player;

    public string nextStage;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        
    }

    private void MoveNextStage(string name)
    {
        GameManager.gameManager.stage++;

        GameManager.gameManager.charData.SaveData();
        GameManager.gameManager.charData.DeleteMapData();
        if(GameManager.gameManager.stage == 8)
        {
            //Ending Scene
        }
        else
        {
            
            SceneManager.LoadScene(name);
        }
    }

    private void EndTutorial()
    {
        GameManager.gameManager.inventory.inventory.Clear();
        SceneManager.LoadScene("StartMap");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(SceneManager.GetActiveScene().name.Contains("Tutorial"))
            {
                if(player.controllAble)
                {
                    EndTutorial();
                }
            }
            else if(GameManager.gameManager.stage < 7 && player.weaponSlot.WeaponCount() != 0)
            {
                MoveNextStage(nextStage);
            }
        }
    }
}
