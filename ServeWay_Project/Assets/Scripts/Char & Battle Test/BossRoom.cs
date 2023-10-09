using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossRoom : MonoBehaviour
{
    private GameObject boss;
    private BossController controller;

    public bool isClear;
    public GameObject intro;
    public GameObject startButton;
    public GameObject bossPrefab;
    public GameObject stairPrefab;
    public List<GameObject> doorList;

    // 미니맵
    [SerializeField] GameObject miniRoomMesh;
    private bool isVisited = false;

    void Start()
    {
        isClear = false;

        intro = GameObject.Find("BossIntro");
        startButton = GameObject.Find("IntroButton");

        intro.SetActive(false);
        startButton.SetActive(false);

        startButton.GetComponent<Button>().onClick.AddListener(OnStartClicked);

        isVisited = false;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !isClear)
        {
            // 미니맵
            if (!isVisited)
            {
                isVisited = true;
                // miniMapMeshGroup 게임 오브젝트의 자식 오브젝트로 방의 메시 프리팹 생성
                Instantiate(miniRoomMesh, transform).transform.SetParent(GameObject.Find("miniMapMeshGroup").transform);
            }

            GameObject.Find("miniPlayer").transform.position = gameObject.transform.position;

            // 보스방 작동
            GameManager.gameManager.isBossStage = true;
            StartCoroutine(BossIntro());
        }
    }
}
