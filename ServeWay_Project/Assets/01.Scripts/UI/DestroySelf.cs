using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    public void Destroy()
    {
        GameManager.gameManager.menuAble = true;
        Time.timeScale = 1;
        Destroy(gameObject);
    }
}
