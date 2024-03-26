using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReceiveNextSignal : MonoBehaviour
{
    private SpriteRenderer spr = null;
    [SerializeField] private List<Sprite> images = new List<Sprite>();

    int nowCut = -1;

    private void Start()
    {
        nowCut = -1;
        spr = gameObject.GetComponent<SpriteRenderer>();
    }

    // run when this received Timeline Signal
    public void NextSignal()
    {
        nowCut++;
        if (nowCut < images.Count)
        {
            spr.sprite = images[nowCut];
            Debug.Log(nowCut);
        } else
        {
            Debug.Log("Finish " + nowCut);
            SceneManager.LoadScene("MainTest");
        }
    }
}
