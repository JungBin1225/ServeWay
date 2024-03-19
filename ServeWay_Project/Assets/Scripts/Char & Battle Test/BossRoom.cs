using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossRoom : MonoBehaviour
{
    private GameObject boss;
    private BossController controller;
    private DataController data;

    public bool isClear;
    public GameObject intro;
    public GameObject startButton;
    public GameObject stairPrefab;
    public GameObject recipePrefab;
    public List<GameObject> doorList;
    public Boss_Nation bossNation;
    public Boss_Job bossJob;

    // �̴ϸ�
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
        bossNation = GameManager.gameManager.bossNations[GameManager.gameManager.stage - 1];
        bossJob = GameManager.gameManager.bossJobList[GameManager.gameManager.stage - 1];


        intro.SetActive(false);
        startButton.SetActive(false);

        startButton.GetComponent<Button>().onClick.AddListener(OnStartClicked);

        isVisited = false;

        // �̴ϸ�
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
        SpawnBoss();
    }

    private void SpawnBoss()
    {
        boss = Instantiate(data.bossList.bossPrefab[0], transform.position, transform.rotation);
        controller = boss.GetComponent<BossController>();
        controller.room = this;
        controller.nation = this.bossNation;
        controller.job = this.bossJob;
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
            Instantiate(RandomIngredient(), pos, Quaternion.Euler(0, 0, 0));
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

    private GameObject RandomIngredient()
    {
        int randomIndex = Random.Range(0, data.IngredientList.IngredientList.Count);

        return data.IngredientList.IngredientList[randomIndex].prefab;
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

    // �̴ϸ� ��ǥ
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
            // �̴ϸ�
            setMiniRowCol();
            if (!isVisited)
            {
                isVisited = true;
                // miniMapMeshGroup ���� ������Ʈ�� �ڽ� ������Ʈ�� ���� �޽� ������ ����
                GameObject tmp = Instantiate(miniRoomMesh, transform);
                minimapMG.PutMesh(tmp, myCol, myRow);
                
            }

            GameObject.Find("miniPlayer").transform.position = gameObject.transform.position;

            // ������ �۵�
            GameManager.gameManager.isBossStage = true;
            StartCoroutine(BossIntro());
        }
    }
}
