using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    private GameObject boss;
    private BossController controller;

    public bool isClear;
    public GameObject intro;
    public GameObject startButton;
    public GameObject bossPrefab;
    public GameObject stair;
    public List<GameObject> doorList;

    void Start()
    {
        isClear = false;
        intro.SetActive(false);
        startButton.SetActive(false);
    }

    // Update is called once per frame
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
        foreach(GameObject door in doorList)
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
        stair.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && !isClear)
        {
            //GameManager.gameManager.isBossStage = true;
            StartCoroutine(BossIntro());
        }
    }
}
