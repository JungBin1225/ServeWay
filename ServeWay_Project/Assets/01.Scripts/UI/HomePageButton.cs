using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomePageButton : MonoBehaviour
{
    [SerializeField] private string link;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnPageClicked()
    {
        Application.OpenURL(link);
        Debug.Log("Open Link");
    }
}
