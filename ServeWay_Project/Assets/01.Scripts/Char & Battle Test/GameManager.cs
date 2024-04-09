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

public enum Stage_Theme
{
    STREET,
    NORMAL,
    CAFE,
    CAMPING,
    BAR,
    RESTORANT,
    SCHOOL
};

public enum Boss_Job
{
    JOURNAL,
    COOKRESEARCH,
    CRITIC,
    BLOGGER,
    YOUTUBER
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
    public List<Stage_Theme> stageThemes;
    public List<Boss_Job> bossJobList;
    public bool menuAble;

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
        stageThemes = new List<Stage_Theme>();
        bossJobList = new List<Boss_Job>();
        charData = gameObject.GetComponent<CharData>();
        inventory = gameObject.GetComponent<InventoryManager>();
        isBossStage = false;
        menuAble = true;
        Cursor.SetCursor(cursorImage, new Vector2(0.13f, 0.87f), CursorMode.Auto);

        if(charData.saveFile.weaponList.Count != 0)
        {
            stage = charData.saveFile.stage;
            bossNations = charData.saveFile.bossNations;
            stageThemes = charData.saveFile.themes;
            bossJobList = charData.saveFile.bossJobs;
        }
        else
        {
            stage = 0;
            InitList();
        }
    }

    void Update()
    {

    }

    public void InitList()
    {
        for(int i = 0; i < 7; i++)
        {
            bossNations.Add(RandomNation());
            stageThemes.Add(RandomTheme());
            bossJobList.Add(RandomJob());
        }
    }

    public void ClearInventory()
    {
        inventory.inventory.Clear();
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

    public Stage_Theme RandomTheme()
    {
        int i = Random.Range(0, 7);

        switch(i)
        {
            case 0:
                return Stage_Theme.STREET;
            case 1:
                return Stage_Theme.NORMAL;
            case 2:
                return Stage_Theme.CAFE;
            case 3:
                return Stage_Theme.CAMPING;
            case 4:
                return Stage_Theme.BAR;
            case 5:
                return Stage_Theme.RESTORANT;
            case 6:
                return Stage_Theme.SCHOOL;
            default:
                return Stage_Theme.NORMAL;
        }
    }

    public Boss_Job RandomJob()
    {
        int i = Random.Range(0, 5);

        switch (i)
        {
            case 0:
                return Boss_Job.JOURNAL;
            case 1:
                return Boss_Job.COOKRESEARCH;
            case 2:
                return Boss_Job.CRITIC;
            case 3:
                return Boss_Job.BLOGGER;
            case 4:
                return Boss_Job.YOUTUBER;
            default:
                return Boss_Job.JOURNAL;
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
