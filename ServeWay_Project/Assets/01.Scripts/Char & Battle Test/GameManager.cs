using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public List<Food_Nation> bossNations;
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
        bossNations = new List<Food_Nation>();
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

    public Food_Nation RandomNation()
    {
        int i = Random.Range(0, 10);

        switch(i)
        {
            case 0:
                return Food_Nation.KOREA;
            case 1:
                return Food_Nation.JAPAN;
            case 2:
                return Food_Nation.CHINA;
            case 3:
                return Food_Nation.USA;
            case 4:
                return Food_Nation.FRANCE;
            case 5:
                return Food_Nation.ITALY;
            case 6:
                return Food_Nation.INDONESIA;
            case 7:
                return Food_Nation.THAILAND;
            case 8:
                return Food_Nation.GERMANY;
            case 9:
                return Food_Nation.SPAIN;
            default:
                return Food_Nation.KOREA;
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
}
