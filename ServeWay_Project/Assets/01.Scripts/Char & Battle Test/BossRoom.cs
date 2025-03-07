using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossRoom : MonoBehaviour
{
    private GameObject boss;
    private DataController data;
    private MissionManager misson;
    private BossController bossCon;
    private GameObject bossHp;
    private Image bossNowHp;


    public bool isClear;
    public GameObject intro;
    public GameObject startButton;
    public GameObject stairPrefab;
    public GameObject recipePrefab;
    public List<GameObject> doorList;
    public Food_Nation bossNation;
    public Boss_Job bossJob;

    // 미니맵
    [SerializeField] GameObject miniRoomMesh;
    private bool isVisited = false;
    [SerializeField] MinimapManager minimapMG;
    [SerializeField] MapGenerator mapGen;
    private int myRow;
    private int myCol;

    void Start()
    {
        isClear = false;

        data = FindObjectOfType<DataController>();
        intro = GameObject.Find("BossIntro");
        startButton = GameObject.Find("IntroButton");
        bossHp = GameObject.Find("BossHp");
        bossNowHp = bossHp.transform.GetChild(0).GetComponent<Image>();
        misson = FindObjectOfType<MissionManager>();
        bossNation = GameManager.gameManager.bossNations[GameManager.gameManager.stage - 1];
        bossJob = GameManager.gameManager.bossJobList[GameManager.gameManager.stage - 1];


        intro.SetActive(false);
        startButton.SetActive(false);
        bossHp.SetActive(false);

        startButton.GetComponent<Button>().onClick.AddListener(OnStartClicked);

        isVisited = false;

        // 미니맵
        minimapMG = GameObject.Find("MinimapManager").GetComponent<MinimapManager>();
        mapGen = GameObject.Find("MapGenerator").GetComponent<MapGenerator>();
        myRow = 0;
        myCol = 0;
    }

    void Update()
    {
        if(GameManager.gameManager.isBossStage)
        {
            if(boss != null)
            {
                bossNowHp.fillAmount = 1 - (bossCon.GetHp() / bossCon.GetMaxHp());
            }
        }
        else
        {
            bossHp.SetActive(false);
        }
    }

    private IEnumerator BossIntro()
    {
        CloseDoor();

        string email = "";
        int num = UnityEngine.Random.Range(5, 13);
        for (int i = 0; i < num; i++)
        {
            email += RandomChar();
        }

        intro.SetActive(true);
        intro.transform.GetChild(1).GetComponent<Text>().text = GameManager.gameManager.JobToString(bossJob);
        intro.transform.GetChild(2).GetComponent<Text>().text = "OOO"; //name
        intro.transform.GetChild(4).GetComponent<Text>().text = data.foodData.FoodDatas[0].EunmToString(bossNation);
        intro.transform.GetChild(6).GetComponent<Text>().text = "A-" + Convert.ToString(UnityEngine.Random.Range(0, 10000), 16) + "-" + Convert.ToString(UnityEngine.Random.Range(0, 10000), 16); //tel
        intro.transform.GetChild(8).GetComponent<Text>().text = email + "@space.com"; //email
        
        Time.timeScale = 0;
        intro.GetComponent<Animator>().SetTrigger("Start");

        yield return new WaitForSecondsRealtime(2f);

        startButton.SetActive(true);
    }

    public void OnStartClicked()
    {
        intro.SetActive(false);
        startButton.SetActive(false);
        Time.timeScale = 1;
        StartCoroutine(misson.MissionAppear());
        SpawnBoss();
    }

    private void SpawnBoss()
    {
        boss = Instantiate(data.FindBoss(bossJob), transform.position, transform.rotation);
        bossCon = boss.GetComponent<BossController>();
        bossHp.SetActive(true);

        switch(bossJob)
        {
            case Boss_Job.JOURNAL:
                var Jcontroller = boss.GetComponent<JournalController>();
                Jcontroller.room = this;
                Jcontroller.nation = this.bossNation;
                Jcontroller.job = this.bossJob;
                break;
            case Boss_Job.COOKRESEARCH:
                var Rcontroller = boss.GetComponent<ResearcherController>();
                Rcontroller.room = this;
                Rcontroller.nation = this.bossNation;
                Rcontroller.job = this.bossJob;
                break;
            case Boss_Job.CRITIC:
                var Ccontroller = boss.GetComponent<CriticController>();
                Ccontroller.room = this;
                Ccontroller.nation = this.bossNation;
                Ccontroller.job = this.bossJob;
                break;
            case Boss_Job.BLOGGER:
                var Bcontroller = boss.GetComponent<BloggerController>();
                Bcontroller.room = this;
                Bcontroller.nation = this.bossNation;
                Bcontroller.job = this.bossJob;
                break;
            case Boss_Job.YOUTUBER:
                var Ycontroller = boss.GetComponent<YoutuberController>();
                Ycontroller.room = this;
                Ycontroller.nation = this.bossNation;
                Ycontroller.job = this.bossJob;
                break;
            case Boss_Job.TEACHER:
                var Tcontroller = boss.GetComponent<TeacherController>();
                Tcontroller.room = this;
                Tcontroller.nation = this.bossNation;
                Tcontroller.job = this.bossJob;
                break;
            default:
                /*var controller = boss.GetComponent<BossController>();
                controller.room = this;
                controller.nation = this.bossNation;
                controller.job = this.bossJob;*/
                break;
        }
    }

    public void DropIngredient(int min, int max)
    {
        int dropAmount = UnityEngine.Random.Range(min, max + 1);
        float radius = 3f;

        for (int i = 0; i < dropAmount; i++)
        {
            float angle = i * Mathf.PI * 2 / dropAmount;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            Vector3 pos = transform.position + new Vector3(x, y, 0);
            float angleDegrees = -angle * Mathf.Rad2Deg;

            Ingredient ingred = RandomIngredient();
            if(i == 0)
            {
                ingred = RandomIngredient(4);
            }

            GameObject item = Instantiate(ingred.prefab, pos, Quaternion.Euler(0, 0, 0));
            item.GetComponent<GetIngredients>().itemName = ingred.name;
            item.GetComponent<GetIngredients>().SetSprite(ingred.sprite, ((int)ingred.grade));
        }
    }

    public void DropRecipe()
    {
        int dropAmount = UnityEngine.Random.Range(0, 2);
        List<string> lockedFood = new List<string>();
        foreach(FoodData food in data.foodData.FoodDatas)
        {
            if(data.FoodIngredDex.foodDex[food.foodName] == FoodDex_Status.LOCKED)
            {
                lockedFood.Add(food.foodName);
            }
        }

        if(lockedFood.Count != 0)
        {
            string foodName = lockedFood[UnityEngine.Random.Range(0, lockedFood.Count)];

            if (dropAmount == 1)
            {
                Vector3 pos = transform.position + new Vector3(0, -4f, 0);
                GameObject recipe = Instantiate(recipePrefab, pos, Quaternion.Euler(0, 0, 0));
                recipe.GetComponent<GetRecipe>().foodName = foodName;
                recipe.GetComponent<GetRecipe>().roomPos = transform.position;
            }
        }
    }

    private Ingredient RandomIngredient()
    {
        int grade = UnityEngine.Random.Range(0, 100) + 1;

        int num = 0;
        if (grade <= 55)
        {
            num = 1;
        }
        else if (grade <= 80)
        {
            num = 2;
        }
        else if (grade <= 95)
        {
            num = 3;
        }
        else
        {
            num = 4;
        }

        List<Ingredient> ingredList = data.GetGradeList(num);
        int randomIndex = UnityEngine.Random.Range(0, ingredList.Count);

        return ingredList[randomIndex];
    }

    private Ingredient RandomIngredient(int star)
    {
        List<Ingredient> ingredList = data.GetGradeList(star);
        int randomIndex = UnityEngine.Random.Range(0, ingredList.Count);

        return ingredList[randomIndex];
    }

    public void CloseDoor()
    {
        foreach (GameObject door in doorList)
        {
            for (int i = 0; i < door.transform.childCount; i++)
            {
                door.GetComponent<DoorAnimation>().CloseDoor();
            }
        }
    }

    public void OpenDoor()
    {
        foreach (GameObject door in doorList)
        {
            for (int i = 0; i < door.transform.childCount; i++)
            {
                door.GetComponent<DoorAnimation>().OpenDoor();
            }
        }
    }

    public void ActiveStair()
    {
        GameObject stair = Instantiate(stairPrefab, transform.position, transform.rotation);
    }

    // 미니맵 좌표
    private void setMiniRowCol()
    {
        KeyValuePair<int, int> bossPos = mapGen.BossGridNum();
        myCol = bossPos.Key;    // x
        myRow = bossPos.Value;  // y
    }

    private string RandomChar()
    {
        string list = "1234567890abcdefghijklmnopqrstuvwxyz";
        int index = UnityEngine.Random.Range(0, list.Length);

        return list[index].ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !isClear)
        {
            // 미니맵
            setMiniRowCol();
            if (!isVisited)
            {
                isVisited = true;
                // miniMapMeshGroup 게임 오브젝트의 자식 오브젝트로 방의 메시 프리팹 생성
                GameObject tmp = Instantiate(miniRoomMesh, transform);
                minimapMG.PutMesh(tmp, myCol, myRow);
                
            }

            GameObject.Find("miniPlayer").transform.position = gameObject.transform.position;

            // 보스방 작동
            GameManager.gameManager.isBossStage = true;
            StartCoroutine(BossIntro());
        }
    }
}
