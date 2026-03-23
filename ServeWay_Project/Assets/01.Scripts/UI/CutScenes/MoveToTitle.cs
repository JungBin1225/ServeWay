using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MoveToTitle : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnEnable()
    {
        if(GetComponent<Image>() == null)
        {
            MoveTitle();
        }
    }

    public void MoveTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
