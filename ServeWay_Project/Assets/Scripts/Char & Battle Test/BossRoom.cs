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
    public GameObject bossPrefab;
    public GameObject stairPrefab;
    public List<GameObject> doorList;
    public Boss_Nation bossNation;

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
        bossNation = GameManager.gameManager.bossNations[GameManager.gameManager.stage - 1];


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
        SpawnBoss();
    }

    private void SpawnBoss()
    {
        boss = Instantiate(bossPrefab, transform.position, transform.rotation);
        controller = boss.GetComponent<BossController>();
        controller.room = this;
        controller.nation = this.bossNation;
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

    private GameObject RandomIngredient()
    {
        int randomIndex = Random.Range(0, data.IngredientList.ingredientList.Count);

        return data.IngredientList.ingredientList[randomIndex].prefab;
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
    private void setMiniRowCol(int row, int col)
    {
        KeyValuePair<int, int> bossPos = mapGen.BossGridNum();
        myRow = bossPos.Value;
        myCol = bossPos.Key;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !isClear)
        {
            // 미니맵
            if (!isVisited)
            {
                isVisited = true;
                // miniMapMeshGroup 게임 오브젝트의 자식 오브젝트로 방의 메시 프리팹 생성
                GameObject tmp = Instantiate(miniRoomMesh, transform);
                minimapMG.putMesh(tmp, myRow, myCol);
                
            }

            GameObject.Find("miniPlayer").transform.position = gameObject.transform.position;

            // 보스방 작동
            GameManager.gameManager.isBossStage = true;
            StartCoroutine(BossIntro());
        }
    }
}
