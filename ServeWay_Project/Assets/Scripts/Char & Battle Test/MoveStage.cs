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
        if(GameManager.gameManager.stage == 7)
        {
            //Ending Scene
        }
        else
        {
            SceneManager.LoadScene(name);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(GameManager.gameManager.stage < 7)
            {
                MoveNextStage(nextStage);
            }
        }
    }
}
