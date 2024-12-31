using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public List<GameObject> enemyPrefab;
    public List<int> amountList;
    public List<GameObject> doorList;
    public int enemyAmount;
    public GameObject itemPrefab;
    public AudioSource bellSound;
    public AudioSource doorOpenSound;
    public AudioSource doorCloseSound;

    // 미니맵
    [SerializeField] GameObject miniRoomMesh;
    private bool isVisited = false;
    public bool nonEnemyRoom = false;
    public int myRow;
    public int myCol;
    [SerializeField] MinimapManager minimapMG;

    private Dictionary<GameObject, int> spawnlist;
    private BoxCollider2D boxCollider;
    private DataController data;
    private int wave;
    private bool isClear;
    private bool isSpawn;
    private bool isStarted = false;
    private int enemyCount;
    //Start() 함수가 끝까지 실행된 이후에 true로 바뀜
    //해주는 이유 : spawnlist를 초기화해주는 Start() 함수가 불리기 이전에 spawnlist를 참조하는 OnTriggerEnter2D()가 불릴 수 있기 때문이다.

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spawnlist = new Dictionary<GameObject, int>();
        data = FindObjectOfType<DataController>();
        isClear = false;
        isSpawn = false;
        isStarted = true;

        isVisited = false;

        wave = 0;
        enemyCount = 0;
        InitEnemy();

        // 미니맵
        minimapMG = GameObject.Find("MinimapManager").GetComponent<MinimapManager>();
    }

    void Update()
    {
        if(enemyAmount == 0 && !isClear)
        {
            if(wave == 0)
            {
                isClear = true;
                //StopCoroutine(SelectEnamy());
                foreach (GameObject door in doorList)
                {
                    for (int i = 0; i < door.transform.childCount; i++)
                    {
                        door.GetComponent<DoorAnimation>().OpenDoor();
                    }
                    doorOpenSound.Play();
                }

                DropIngredient(1, 4);
                //if (!nonEnemyRoom)
                    //this.gameObject.SetActive(false);
            }
            else
            {
                InitEnemy();
                StartCoroutine(SelectEnamy());
            }
        }
    }

    private IEnumerator SelectEnamy()
    {
        isSpawn = true;
        bellSound.Play();

        yield return new WaitForSeconds(0.3f);

        foreach(GameObject enemy in spawnlist.Keys)
        {
            for(int i = 0; i < spawnlist[enemy]; i++)
            {
                spawnEnemy(enemy);
                yield return new WaitForSeconds(0.3f);
            }
        }

        wave -= 1;
    }

    private void spawnEnemy(GameObject enemyPrefab)
    {
        float minX = transform.position.x - (transform.localScale.x / 2) + 2;
        float maxX = transform.position.x + (transform.localScale.x / 2) - 2;
        float minY = transform.position.y - (transform.localScale.y / 2) + 2;
        float maxY = transform.position.y + (transform.localScale.y / 2) - 2;

        float posX = Random.Range(minX, maxX);
        float posY = Random.Range(minY, maxY);

        Quaternion rot = Quaternion.Euler(0, 0, 0);

        UnityEngine.Debug.LogFormat("boxCollider.size.x : {0} boxCollider.size.y : {1}", transform.localScale.x, transform.localScale.y);
        UnityEngine.Debug.LogFormat("minX : {0} maxX : {1} posX : {2} posY : {3}", minX, maxX, posX,posY);

        GameObject enemy = Instantiate(enemyPrefab, new Vector3(posX, posY, 0), rot);
        enemy.GetComponent<EnemyController>().SetVector(new Vector2(minX, minY), new Vector2(maxX, maxY));
        enemy.GetComponent<EnemyController>().SetGenerator(this.gameObject);
        enemy.GetComponent<EnemyController>().roomCenter = transform.position;
        enemy.GetComponent<EnemyController>().SetAttackType(data);
        enemy.GetComponent<EnemySprite>().SetLayerOrder(enemyCount);
        enemyCount++;
    }

    private void DropIngredient(int min, int max)
    {
        int dropAmount = Random.Range(min, max + 1);
        float radius = 2.5f;

        for (int i = 0; i < dropAmount; i++)
        {
            float angle = i * Mathf.PI * 2 / dropAmount;
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;
            Vector3 pos = transform.position + new Vector3(x, y, 0);
            float angleDegrees = -angle * Mathf.Rad2Deg;
            
            Ingredient ingredient = RandomIngredient();
            GameObject item = Instantiate(itemPrefab, pos, Quaternion.Euler(0, 0, 0));
            item.GetComponent<GetIngredients>().itemName = ingredient.name;
            item.GetComponent<GetIngredients>().SetSprite(ingredient.sprite);
        }
    }

    public Ingredient RandomIngredient()
    {
        int grade = Random.Range(0, 100) + 1;

        int num = 0;
        if(grade <= 55)
        {
            num = 1;
        }
        else if(grade <= 80)
        {
            num = 2;
        }
        else if(grade <= 95)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject.Find("miniPlayer").transform.position = gameObject.transform.position;

        if (collision.gameObject.tag == "Player" && !isClear && isStarted)
        {
            // 미니맵
            GenerateMiniRoomMesh();

            if (!nonEnemyRoom)
            {
                // 일반 방 작동
                foreach (GameObject door in doorList)
                {
                    for (int i = 0; i < door.transform.childCount; i++)
                    {
                        door.GetComponent<DoorAnimation>().CloseDoor();
                    }
                    doorCloseSound.Play();
                }

                if (!isSpawn)
                {
                    StartCoroutine(SelectEnamy());
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            enemyAmount--;
        }
        else if(collision.gameObject.tag == "Player")
        {
            //StopCoroutine(SelectEnamy());
        }
    }

    private void InitEnemy()
    {
        switch(GameManager.gameManager.stage)
        {
            case 1:
                enemyAmount = Random.Range(7, 12);
                if(wave == 0) wave = 1;
                break;
            case 2:
                enemyAmount = Random.Range(7, 12) + 5;
                if (wave == 0) wave = 1;
                break;
            case 3:
                enemyAmount = Random.Range(7, 12) + 5;
                if (wave == 0) wave = 2;
                break;
            case 4:
                enemyAmount = Random.Range(7, 12) + 10;
                if (wave == 0) wave = 2;
                break;
            case 5:
                enemyAmount = Random.Range(7, 12) + 5;
                if (wave == 0) wave = 3;
                break;
            case 6:
                enemyAmount = Random.Range(7, 12) + 10;
                if (wave == 0) wave = 3;
                break;
            case 7:
                enemyAmount = Random.Range(7, 12) + 10;
                if (wave == 0) wave = 4;
                break;
        }
        //int num = enemyAmount;
        int num = 5;
        enemyPrefab.Clear();
        amountList.Clear();
        spawnlist.Clear();
        while(num != 0)
        {
            int amount = Random.Range(2, 6);
            if(num < amount)
            {
                amount = num;
            }

            GameObject enemy = data.enemyList.RandomEnemy();

            if(enemyPrefab.Contains(enemy))
            {
                amountList[enemyPrefab.FindIndex(o => o == enemy)] += amount;
            }
            else
            {
                enemyPrefab.Add(enemy);
                amountList.Add(amount);
            }


            num -= amount;
        }

        int n = 0;
        foreach (GameObject enemy in enemyPrefab)
        {
            spawnlist.Add(enemy, amountList[n]);
            n++;
        }
    }

    // 미니맵
    public void GenerateMiniRoomMesh()
    {
        if (!isVisited)
        {
            isVisited = true;
            // minimapGroup 게임 오브젝트의 자식 오브젝트로 방의 메시 프리팹 생성
            GameObject tmp = Instantiate(miniRoomMesh, transform);

            Debug.Log("EG: " + myCol + ", " + myRow);

            if (!minimapMG)
            {
                minimapMG = GameObject.Find("MinimapManager").GetComponent<MinimapManager>();
            }
            minimapMG.PutMesh(tmp, myCol, myRow);
        }
    }
}
