using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    YOUTUBER,
    TEACHER
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
    public float playTime;

    private string nextStage;

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
        playTime = 0;
        bossNations = new List<Food_Nation>();
        stageThemes = new List<Stage_Theme>();
        bossJobList = new List<Boss_Job>();
        charData = gameObject.GetComponent<CharData>();
        inventory = gameObject.GetComponent<InventoryManager>();
        isBossStage = false;
        menuAble = true;
        Cursor.SetCursor(cursorImage, new Vector2(0.13f, 0.87f), CursorMode.Auto);
        nextStage = "";

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
        bossNations = RandomNation();
        stageThemes = RandomTheme();
        bossJobList = RandomJob();
    }

    public void ClearInventory()
    {
        inventory.inventory.Clear();
    }

    public List<Food_Nation> RandomNation()
    {
        List<int> list = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        List<Food_Nation> nationList = new List<Food_Nation>();

        for(int i = 0; i < 7; i++)
        {
            int index = Random.Range(0, list.Count);
            if (i == 0)
            {
                index = 0;
            }

            nationList.Add((Food_Nation)list[index]);
            list.RemoveAt(index);
        }

        return nationList;
    }

    public List<Stage_Theme> RandomTheme()
    {
        List<int> list = new List<int> { 0, 1, 2, 3, 4, 5, 6 };
        List<Stage_Theme> themeList = new List<Stage_Theme>();

        for (int i = 0; i < 7; i++)
        {
            int index = Random.Range(0, list.Count);
            if (i == 0)
            {
                index = 3;
            }

            themeList.Add((Stage_Theme)list[index]);
            list.RemoveAt(index);
        }

        return themeList;
    }

    public List<Boss_Job> RandomJob()
    {
        List<int> list = new List<int> { 0, 1, 2, 3, 4 };
        List<Boss_Job> jobList = new List<Boss_Job>();

        for (int i = 0; i < 5; i++)
        {
            int index = Random.Range(0, list.Count);
            if (i == 0)
            {
                index = 0;
            }

            jobList.Add((Boss_Job)list[index]);
            list.RemoveAt(index);
        }

        jobList.Add(jobList[0]);
        jobList.Add(Boss_Job.TEACHER);

        return jobList;
    }

    public string JobToString(Boss_Job job)
    {
        switch(job)
        {
            case Boss_Job.JOURNAL:
                return "저널리스트";
            case Boss_Job.CRITIC:
                return "평론가";
            case Boss_Job.COOKRESEARCH:
                return "요리연구가";
            case Boss_Job.BLOGGER:
                return "블로거";
            case Boss_Job.YOUTUBER:
                return "유튜버";
            case Boss_Job.TEACHER:
                return "사부님";
            default:
                return "";
        }
    }

    public string ThemeToString(Stage_Theme theme)
    {
        switch(theme)
        {
            case Stage_Theme.BAR:
                return "술집";
            case Stage_Theme.CAFE:
                return "카페";
            case Stage_Theme.CAMPING:
                return "캠핑장";
            case Stage_Theme.NORMAL:
                return "일반 식당";
            case Stage_Theme.RESTORANT:
                return "레스토랑";
            case Stage_Theme.SCHOOL:
                return "급식실";
            case Stage_Theme.STREET:
                return "길거리 식당";
            default:
                return "";
        }
    }

    public Boss_Job StringToJob(string name)
    {
        switch (name)
        {
            case "JOURNAL":
                return Boss_Job.JOURNAL;
            case "CRITIC":
                return Boss_Job.CRITIC;
            case "COOKRESEARCH":
                return Boss_Job.COOKRESEARCH;
            case "BLOGGER":
                return Boss_Job.BLOGGER;
            case "YOUTUBER":
                return Boss_Job.YOUTUBER;
            case "TEACHER":
                return Boss_Job.TEACHER;
            default:
                return Boss_Job.JOURNAL;
        }
    }

    public Stage_Theme StringToTheme(string name)
    {
        switch (name)
        {
            case "BAR":
                return Stage_Theme.BAR;
            case "CAFE":
                return Stage_Theme.CAFE;
            case "CAMPING":
                return Stage_Theme.CAMPING;
            case "NORMAL":
                return Stage_Theme.NORMAL;
            case "RESTORANT":
                return Stage_Theme.RESTORANT;
            case "SCHOOL":
                return Stage_Theme.SCHOOL;
            case "STREET":
                return Stage_Theme.STREET;
            default:
                return Stage_Theme.BAR;
        }
    }

    public Food_Nation StringToNation(string name)
    {
        switch(name)
        {
            case "KOREA":
                return Food_Nation.KOREA;
            case "JAPAN":
                return Food_Nation.JAPAN;
            case "CHINA":
                return Food_Nation.CHINA;
            case "FRANCE":
                return Food_Nation.FRANCE;
            case "GERMANY":
                return Food_Nation.GERMANY;
            case "INDONESIA":
                return Food_Nation.INDONESIA;
            case "ITALY":
                return Food_Nation.ITALY;
            case "SPAIN":
                return Food_Nation.SPAIN;
            case "USA":
                return Food_Nation.USA;
            case "THAILAND":
                return Food_Nation.THAILAND;
            default:
                return Food_Nation.KOREA;
        }
    }

    public void SetNextStage(string name)
    {
        nextStage = name;
    }

    public string GetNextStage()
    {
        return nextStage;
    }
}
