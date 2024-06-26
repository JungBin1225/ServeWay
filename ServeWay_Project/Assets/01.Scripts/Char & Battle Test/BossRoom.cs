using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossRoom : MonoBehaviour
{
    private GameObject boss;
    private DataController data;
    private MissonManager misson;

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
        misson = FindObjectOfType<MissonManager>();
        bossNation = GameManager.gameManager.bossNations[GameManager.gameManager.stage - 1];
        bossJob = GameManager.gameManager.bossJobList[GameManager.gameManager.stage - 1];


        intro.SetActive(false);
        startButton.SetActive(false);

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

    }

    private IEnumerator BossIntro()
    {
        CloseDoor();
        intro.SetActive(true);
        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(2f);

        startButton.SetActive(true);
    }

    public void OnStartClicked()
    {
        intro.SetActive(false);
        startButton.SetActive(false);
        Time.timeScale = 1;
        StartCoroutine(misson.MissonAppear());
        SpawnBoss();
    }

    private void SpawnBoss()
    {
        boss = Instantiate(data.FindBoss(bossJob), transform.position, transform.rotation);

        switch(bossJob)
        {
            case Boss_Job.JOURNAL:
                var Jcontroller = boss.GetComponent<JournalController>();
                Jcontroller.room = this;
                Jcontroller.nation = this.bossNation;
                Jcontroller.job = this.bossJob;
                break;
            default:
                var controller = boss.GetComponent<BossController>();
                controller.room = this;
                controller.nation = this.bossNation;
                controller.job = this.bossJob;
                break;
        }
    }

    public void DropIngredient(int min, int max)
    {
        int dropAmount = Random.Range(min, max + 1);
        float radius = 5f;

        for (int i = 0; i < dropAmount; i++)
        {
            float angle = i * Mathf.PI * 2 / dropAmount;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            Vector3 pos = transform.position + new Vector3(x, y, 0);
            float angleDegrees = -angle * Mathf.Rad2Deg;

            Ingredient ingred = RandomIngredient();
            GameObject item = Instantiate(ingred.prefab, pos, Quaternion.Euler(0, 0, 0));
            item.GetComponent<GetIngredients>().itemName = ingred.name;
            item.GetComponent<GetIngredients>().SetSprite(ingred.sprite);
        }
    }

    public void DropRecipe()
    {
        int dropAmount = Random.Range(0, 2);
        List<string> lockedFood = new List<string>();
        foreach(FoodData food in data.foodData.FoodDatas)
        {
            if(data.FoodIngredDex.foodDex[food.foodName] == FoodDex_Status.LOCKED)
            {
                lockedFood.Add(food.foodName);
            }
        }

        string foodName = lockedFood[Random.Range(0, lockedFood.Count)];

        if(dropAmount == 1)
        {
            Vector3 pos = transform.position + new Vector3(0, -6f, 0);
            GameObject recipe = Instantiate(recipePrefab, pos, Quaternion.Euler(0, 0, 0));
            recipe.GetComponent<GetRecipe>().foodName = foodName;
        }
    }

    private Ingredient RandomIngredient()
    {
        int grade = Random.Range(0, 100) + 1;

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
        int randomIndex = Random.Range(0, ingredList.Count);

        return ingredList[randomIndex];
    }

    public void CloseDoor()
    {
        foreach (GameObject door in doorList)
        {
            door.SetActive(true);
        }
    }

    public void OpenDoor()
    {
        foreach (GameObject door in doorList)
        {
            door.SetActive(false);
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
