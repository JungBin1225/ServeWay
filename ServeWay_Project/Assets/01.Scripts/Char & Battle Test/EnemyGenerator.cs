using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public List<GameObject> doorList;
    public int enemyAmount;
    public GameObject itemPrefab;
    public AudioSource bellSound;
    public AudioSource doorOpenSound;
    public AudioSource doorCloseSound;
    public GameObject enemyAppear;
    public GameObject minimapIcon;

    // 미니맵
    [SerializeField] GameObject miniRoomMesh;
    [SerializeField] private bool isVisited = false;
    public bool nonEnemyRoom = false;
    public int myRow;
    public int myCol;
    [SerializeField] MinimapManager minimapMG;
    public bool isStartMap;

    private List<GameObject> spawnList;
    private List<GameObject> followSpawnList;
    private BoxCollider2D boxCollider;
    private DataController data;
    private int wave;
    private int followUp;
    private bool isClear;
    private bool isSpawn;
    private int noodleOrBread;
    private bool isStarted = false;
    private bool isEntered;
    private int enemyCount;
    public List<GameObject> miniRoadList;
    //Start() 함수가 끝까지 실행된 이후에 true로 바뀜
    //해주는 이유 : spawnlist를 초기화해주는 Start() 함수가 불리기 이전에 spawnlist를 참조하는 OnTriggerEnter2D()가 불릴 수 있기 때문이다.

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spawnList = new List<GameObject>();
        followSpawnList = new List<GameObject>();
        data = FindObjectOfType<DataController>();
        isClear = false;
        isSpawn = false;
        isStarted = true;
        isEntered = false;
        miniRoadList = new List<GameObject>();

        isVisited = false;

        wave = 0;
        followUp = 0;
        enemyCount = 0;
        noodleOrBread = 0;
        InitEnemy();

        if(!nonEnemyRoom)
        {
            PlaceMapObject();
        }


        // 미니맵
        minimapMG = GameObject.Find("MinimapManager").GetComponent<MinimapManager>();
        GenerateMiniRoomMesh();
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
                    door.GetComponent<DoorAnimation>().OpenDoor();
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
        Debug.Log(enemyAmount);
        yield return new WaitForSeconds(0.3f);

        foreach(GameObject enemy in spawnList)
        {
            StartCoroutine(spawnEnemy(enemy));
            yield return new WaitForSeconds(0.1f);
        }

        wave -= 1;
    }

    private IEnumerator spawnEnemy(GameObject enemyPrefab)
    {
        float minX = transform.position.x - (transform.localScale.x / 2) + 2;
        float maxX = transform.position.x + (transform.localScale.x / 2) - 2;
        float minY = transform.position.y - (transform.localScale.y / 2) + 2;
        float maxY = transform.position.y + (transform.localScale.y / 2) - 2;

        float posX = Random.Range(minX, maxX);
        float posY = Random.Range(minY, maxY);

        Quaternion rot = Quaternion.Euler(0, 0, 0);

        GameObject appear = Instantiate(enemyAppear, new Vector3(posX, posY, 0), rot);
        yield return new WaitForSeconds(0.3f);

        /*UnityEngine.Debug.LogFormat("boxCollider.size.x : {0} boxCollider.size.y : {1}", transform.localScale.x, transform.localScale.y);
        UnityEngine.Debug.LogFormat("minX : {0} maxX : {1} posX : {2} posY : {3}", minX, maxX, posX,posY);*/
        Destroy(appear);

        GameObject enemy = Instantiate(enemyPrefab, new Vector3(posX, posY, 0), rot);
        enemy.GetComponent<EnemyController>().SetVector(new Vector2(minX, minY), new Vector2(maxX, maxY));
        enemy.GetComponent<EnemyController>().SetGenerator(this.gameObject);
        enemy.GetComponent<EnemyController>().roomCenter = transform.position;
        enemy.GetComponent<EnemyController>().SetAttackType(data);
        SetEnemyHp(enemy.GetComponent<EnemyController>());
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
            item.GetComponent<GetIngredients>().SetSprite(ingredient.sprite, ((int)ingredient.grade));
            item.GetComponent<GetIngredients>().roomPos = transform.position;
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

    private void PlaceMapObject()
    {
        List<GameObject> objectList = new List<GameObject>();
        List<int> quadrant = new List<int> { 1, 2, 3, 4 };
        float sizeX = transform.localScale.x / 2;
        float sizeY = transform.localScale.y / 2;

        int objectAmount = Random.Range(0, 3);
        int triggerAmount = Random.Range(0, 5 - objectAmount);

        objectList = GetObjectList();
        switch(objectAmount)
        {
            case 1:
                int quad = quadrant[Random.Range(0, quadrant.Count)];
                quadrant.Remove(quad);

                SpawnObject(objectList[Random.Range(0, objectList.Count - 1)], quad, sizeX, sizeY);
                break;

            case 2:
                int first = quadrant[Random.Range(0, quadrant.Count)];
                quadrant.Remove(first);
                int second = quadrant[Random.Range(0, quadrant.Count)];
                quadrant.Remove(second);

                SpawnObject(objectList[Random.Range(0, objectList.Count - 1)], first, sizeX, sizeY);
                SpawnObject(objectList[Random.Range(0, objectList.Count - 1)], second, sizeX, sizeY);
                break;
        }

        for(int i = 0; i < triggerAmount; i++)
        {
            int index = quadrant[Random.Range(0, quadrant.Count)];
            quadrant.Remove(index);

            SpawnObject(objectList[objectList.Count - 1], index, sizeX, sizeY);
        }
    }

    private List<GameObject> GetObjectList()
    {
        switch(GameManager.gameManager.stageThemes[GameManager.gameManager.stage - 1])
        {
            case Stage_Theme.CAMPING:
                return data.mapObjectList.campingList;
            default:
                return data.mapObjectList.testList;
        }
    }

    private void SpawnObject(GameObject prefab, int quadrant, float sizeX, float sizeY)
    {
        int signX = 1;
        int signY = 1;

        float distanceX = prefab.GetComponent<MapObject>().width;
        float distanceY = prefab.GetComponent<MapObject>().height;

        switch (quadrant)
        {
            case 1:
                signX = 1;
                signY = 1;
                break;
            case 2:
                signX = -1;
                signY = 1;
                break;
            case 3:
                signX = -1;
                signY = -1;
                break;
            case 4:
                signX = 1;
                signY = -1;
                break;
        }

        float posX = Random.Range(transform.position.x + (signX * (distanceX + 2)), (transform.position.x + (signX * sizeX)) - (signX * (distanceX + 2)));
        float posY = Random.Range(transform.position.y + (signY * (distanceY + 2)), (transform.position.y + (signY * sizeY)) - (signY * (distanceY + 2)));

        GameObject mapObject = Instantiate(prefab, new Vector3(posX, posY, 0), Quaternion.Euler(0, 0, 0), transform);
        mapObject.transform.localScale = new Vector3(mapObject.transform.localScale.x / transform.localScale.x, mapObject.transform.localScale.y / transform.localScale.y, 1);
    }

    private void InitEnemy()
    {
        switch(GameManager.gameManager.stage)
        {
            case 1:
                enemyAmount = Random.Range(8, 11);
                followUp = 0;
                if(wave == 0) wave = 1;
                break;
            case 2:
                enemyAmount = Random.Range(11, 15);
                followUp = 0;
                if (wave == 0) wave = 1;
                break;
            case 3:
                enemyAmount = Random.Range(11, 15);
                followUp = 5;
                if (wave == 0) wave = 1;
                break;
            case 4:
                enemyAmount = Random.Range(11, 15);
                followUp = 10;
                if (wave == 0) wave = 1;
                break;
            case 5:
                enemyAmount = Random.Range(11, 15);
                followUp = 0;
                if (wave == 0) wave = 2;
                break;
            case 6:
                enemyAmount = Random.Range(11, 15);
                followUp = 5;
                if (wave == 0) wave = 2;
                break;
            case 7:
                enemyAmount = Random.Range(11, 15);
                followUp = 10;
                if (wave == 0) wave = 3;
                break;
        }
        spawnList.Clear();
        followSpawnList.Clear();

        for(int i = 0; i < enemyAmount; i++)
        {
            if(i == enemyAmount - 1)
            {
                noodleOrBread = Random.Range(0, 2);
                switch (noodleOrBread)
                {
                    case 0:
                        spawnList.Add(data.enemyList.noodleEnemy);
                        break;
                    case 1:
                        spawnList.Add(data.enemyList.breadEnemy);
                        break;
                }
            }
            else
            {
                int index = Random.Range(0, 3);
                switch (index)
                {
                    case 0:
                        spawnList.Add(data.enemyList.meatEnemy);
                        break;
                    case 1:
                        spawnList.Add(data.enemyList.riceEnemy);
                        break;
                    case 2:
                        spawnList.Add(data.enemyList.soupEnemy);
                        break;
                }
            }
        }

        for(int i = 0; i < followUp; i++)
        {
            if (i == followUp - 1)
            {
                if(GameManager.gameManager.stage >= 5)
                {
                    noodleOrBread = Random.Range(0, 2);
                }
                
                switch (noodleOrBread)
                {
                    case 0:
                        followSpawnList.Add(data.enemyList.noodleEnemy);
                        break;
                    case 1:
                        followSpawnList.Add(data.enemyList.breadEnemy);
                        break;
                }
            }
            else
            {
                int index = Random.Range(0, 3);
                switch (index)
                {
                    case 0:
                        followSpawnList.Add(data.enemyList.meatEnemy);
                        break;
                    case 1:
                        followSpawnList.Add(data.enemyList.riceEnemy);
                        break;
                    case 2:
                        followSpawnList.Add(data.enemyList.soupEnemy);
                        break;
                }
            }
        }
    }

    private void SetEnemyHp(EnemyController enemyCon)
    {
        float maxHp = 10;

        switch (GameManager.gameManager.stage)
        {
            case 1:
                maxHp = Random.Range(17, 24);
                break;
            case 2:
                maxHp = Random.Range(25, 33);
                break;
            case 3:
                maxHp = Random.Range(30, 41);
                break;
            case 4:
                maxHp = Random.Range(43, 49);
                break;
            case 5:
                maxHp = Random.Range(51, 56);
                break;
            case 6:
                maxHp = Random.Range(53, 59);
                break;
            case 7:
                maxHp = Random.Range(55, 61);
                break;
        }

        enemyCon.SetMaxHp(maxHp);
    }

    // 미니맵
    public void GenerateMiniRoomMesh()
    {
        if (!isVisited)
        {
            isVisited = true;
            Vector3 pos = transform.position + new Vector3(0, 0.35f, 0);
            Vector3 scale = transform.localScale + new Vector3(0.6f, 1.3f, 0);

            // minimapGroup 게임 오브젝트의 자식 오브젝트로 방의 메시 프리팹 생성
            GameObject tmp = Instantiate(miniRoomMesh, GameObject.Find("minimapGroup").transform);
            tmp.transform.position = pos;
            tmp.transform.localScale = scale;
            AddMinimapRoad(tmp);

            if (!minimapMG)
            {
                minimapMG = GameObject.Find("MinimapManager").GetComponent<MinimapManager>();
            }
            minimapMG.PutMesh(this.gameObject, myCol, myRow);
        }
    }

    public void AddMinimapRoad(GameObject road)
    {
        miniRoadList.Add(road);
        if(isStartMap)
        {
            road.SetActive(false);
        }
    }

    public void EnemyDie()
    {
        if (wave == 0)
        {
            if (followSpawnList.Count != 0)
            {
                StartCoroutine(spawnEnemy(followSpawnList[0]));
                followSpawnList.RemoveAt(0);
            }
            else
            {
                enemyAmount--;
            }
        }
        else
        {
            enemyAmount--;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameObject.Find("miniPlayer").transform.position = gameObject.transform.position;
        }

        if (collision.gameObject.tag == "Player" && !isClear && isStarted)
        {
            if (!nonEnemyRoom)
            {
                // 일반 방 작동
                foreach (GameObject door in doorList)
                {
                    door.GetComponent<DoorAnimation>().CloseDoor();
                    doorCloseSound.Play();
                }

                if (!isSpawn)
                {
                    StartCoroutine(SelectEnamy());
                }
            }
        }

        if (collision.gameObject.tag == "Player" && !isEntered)
        {
            isEntered = true;

            foreach(GameObject mini in miniRoadList)
            {
                mini.SetActive(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        /*if(collision.gameObject.tag == "Enemy")
        {
            enemyAmount--;
        }
        else if(collision.gameObject.tag == "Player")
        {
            //StopCoroutine(SelectEnamy());
        }*/
    }
}
