using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public List<GameObject> enemyPrefab;
    public List<int> amountList;
    public List<GameObject> doorList;
    public int enemyAmount;

    private Dictionary<GameObject, int> spawnlist;
    private BoxCollider2D collider;
    private bool isClear;

    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        spawnlist = new Dictionary<GameObject, int>();
        isClear = false;

        int i = 0;
        foreach(GameObject enemy in enemyPrefab)
        {
            spawnlist.Add(enemy, amountList[i]);
            i++;
        }

        enemyAmount = 0;
        foreach(int n in amountList)
        {
            enemyAmount += n;
        }
    }

    void Update()
    {
        if(enemyAmount == 0)
        {
            isClear = true;
            foreach(GameObject door in doorList)
            {
                door.SetActive(false);
                this.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator SelectEnamy()
    {
        foreach(GameObject enemy in spawnlist.Keys)
        {
            for(int i = 0; i < spawnlist[enemy]; i++)
            {
                spawnEnemy(enemy);
                yield return new WaitForSeconds(0.7f);
            }
        }
    }

    private void spawnEnemy(GameObject enemyPrefab)
    {
        float minX = transform.position.x - (collider.size.x / 2);
        float maxX = transform.position.x + (collider.size.x / 2);
        float minY = transform.position.y - (collider.size.y / 2);
        float maxY = transform.position.y + (collider.size.y / 2);

        float posX = Random.Range(minX, maxX);
        float posY = Random.Range(minY, maxY);

        Quaternion rot = Quaternion.Euler(0, 0, 0);

        GameObject enemy = Instantiate(enemyPrefab, new Vector3(posX, posY, 0), rot);
        enemy.GetComponent<EnemyController>().SetVector(new Vector2(minX, minY), new Vector2(maxX, maxY));
        enemy.GetComponent<EnemyController>().SetGenerator(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && !isClear)
        {
            foreach (GameObject door in doorList)
            {
                door.SetActive(true);
            }
            StartCoroutine(SelectEnamy());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            enemyAmount--;
        }
    }
}
