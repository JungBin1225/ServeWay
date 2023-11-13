using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Boss_Nation
{
    KOREA,
    JAPAN,
    CHINA,
    USA,
    FRANCE
};

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    public Texture2D cursorImage;
    public bool isBossStage;
    public CharData charData;
    public InventoryManager inventory;
    public int stage;
    public List<Boss_Nation> bossNations;

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
        bossNations = new List<Boss_Nation>();
        charData = gameObject.GetComponent<CharData>();
        inventory = gameObject.GetComponent<InventoryManager>();
        isBossStage = false;
        Cursor.SetCursor(cursorImage, new Vector2(0.13f, 0.87f), CursorMode.Auto);

        if(charData.saveFile.weaponList.Count != 0)
        {
            stage = charData.saveFile.stage;
            bossNations = charData.saveFile.bossNations;
        }
        else
        {
            stage = 0;
            InitNation();
        }
    }

    void Update()
    {

    }

    public void InitNation()
    {
        for(int i = 0; i < 7; i++)
        {
            bossNations.Add(RandomNation());
        }
    }

    public Boss_Nation RandomNation()
    {
        int i = Random.Range(0, 5);

        switch(i)
        {
            case 0:
                return Boss_Nation.KOREA;
            case 1:
                return Boss_Nation.JAPAN;
            case 2:
                return Boss_Nation.CHINA;
            case 3:
                return Boss_Nation.USA;
            case 4:
                return Boss_Nation.FRANCE;
            default:
                return Boss_Nation.KOREA;
        }
    }

    public string NationToString(Boss_Nation nation)
    {
        switch(nation)
        {
            case Boss_Nation.KOREA:
                return "한국";
            case Boss_Nation.JAPAN:
                return "일본";
            case Boss_Nation.CHINA:
                return "중국";
            case Boss_Nation.USA:
                return "미국";
            case Boss_Nation.FRANCE:
                return "프랑스";
            default:
                return "";
        }
    }
}
