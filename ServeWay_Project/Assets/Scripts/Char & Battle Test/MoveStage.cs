using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveStage : MonoBehaviour
{
    public string nextStage;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void MoveNextStage(string name)
    {
        //GameManager.gameManager.mission.InitCount();
        SceneManager.LoadScene(name);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            MoveNextStage(nextStage);
        }
    }
}
