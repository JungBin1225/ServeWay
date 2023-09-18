using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    public Texture2D cursorImage;
    public bool isBossStage;
    //public BossMission mission;

    private void Awake()
    {
        if (gameManager == null)
            gameManager = this;

        else if (gameManager != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        //mission = this.gameObject.GetComponent<BossMission>();
        isBossStage = false;
        Cursor.SetCursor(cursorImage, new Vector2(0.13f, 0.87f), CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
