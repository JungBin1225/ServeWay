using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnConfirm()
    {
        //Time.timeScale = 1;

        GameManager.gameManager.charData.saveFile.Reset();
        GameManager.gameManager.ClearInventory();

        UnityEditor.EditorUtility.SetDirty(GameManager.gameManager.charData.saveFile);

        SceneManager.LoadScene("StartMap");
    }
}
