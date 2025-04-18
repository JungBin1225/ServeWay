using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 구현 끝나면 세이브파일 연동 작업도 해줘야 함 */

public class MinimapManager : MonoBehaviour
{

    private struct miniRoomStruct
    {
        public GameObject miniRoomMesh;
        public bool up;
        public bool down;
        public bool left;
        public bool right;

        public miniRoomStruct(GameObject mesh)
        {
            this.miniRoomMesh = mesh;
            this.up = false;
            this.down = false;
            this.left = false;
            this.right = false;
        }
    }

    public static MinimapManager minimapMG;

    const int NUM_ROOM = 5;
    private miniRoomStruct[,] miniRoomMeshList = new miniRoomStruct[NUM_ROOM, NUM_ROOM];    // make space to store miniRoomStruct

    [SerializeField] GameObject miniRoadMesh;

    private void Awake()
    {
        if (minimapMG == null)
            minimapMG = this;

        else if (minimapMG != this)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the miniRoomStruct Array to null
        for (int i = 0; i < NUM_ROOM; i++)
        {
            for (int j = 0; j < NUM_ROOM; j++)
            {
                miniRoomMeshList[i, j] = new miniRoomStruct(null);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PutMesh(GameObject tmp, int col, int row)
    {
        // col:열:x  row:행:y
        miniRoomMeshList[col, row].miniRoomMesh = tmp;

        DrawRoad(col, row);
    }

    private void DrawRoad(int col, int row)
    {
        // col:열:x
        if (col > 0)    // left road (col - 1, row)
        {
            if (miniRoomMeshList[col - 1, row].miniRoomMesh != null && !miniRoomMeshList[col, row].left)
            {
                float distance = (miniRoomMeshList[col, row].miniRoomMesh.transform.localPosition.x - miniRoomMeshList[col, row].miniRoomMesh.transform.localScale.x / 2) - (miniRoomMeshList[col - 1, row].miniRoomMesh.transform.localPosition.x + miniRoomMeshList[col - 1, row].miniRoomMesh.transform.localScale.x / 2);

                /* draw road */
                GameObject roadTmp = Instantiate(miniRoadMesh, GameObject.Find("minimapGroup").transform);
                // set road position
                float roadX = miniRoomMeshList[col - 1, row].miniRoomMesh.transform.localPosition.x + miniRoomMeshList[col - 1, row].miniRoomMesh.transform.localScale.x / 2 + distance / 2;
                float roomsHeight = Mathf.Max(miniRoomMeshList[col, row].miniRoomMesh.transform.localPosition.y + miniRoomMeshList[col, row].miniRoomMesh.transform.localScale.y / 2, miniRoomMeshList[col - 1, row].miniRoomMesh.transform.localPosition.y + miniRoomMeshList[col - 1, row].miniRoomMesh.transform.localScale.y / 2) - Mathf.Min(miniRoomMeshList[col, row].miniRoomMesh.transform.localPosition.y - miniRoomMeshList[col, row].miniRoomMesh.transform.localScale.y / 2, miniRoomMeshList[col - 1, row].miniRoomMesh.transform.localPosition.y - miniRoomMeshList[col - 1, row].miniRoomMesh.transform.localScale.y / 2);
                float roadY = Mathf.Max(miniRoomMeshList[col, row].miniRoomMesh.transform.localPosition.y + miniRoomMeshList[col, row].miniRoomMesh.transform.localScale.y / 2, miniRoomMeshList[col - 1, row].miniRoomMesh.transform.localPosition.y + miniRoomMeshList[col - 1, row].miniRoomMesh.transform.localScale.y / 2) - roomsHeight / 2;
                
                roadTmp.transform.localPosition = new Vector3(roadX, roadY, 0.0f);
                // set road roatation
                roadTmp.transform.localRotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
                // set road scale
                roadTmp.transform.localScale = new Vector3(distance, 5.0f, 0.0f);

                /* check road */
                miniRoomMeshList[col - 1, row].right = true;
                miniRoomMeshList[col, row].left = true;

                AddRoad(miniRoomMeshList[col - 1, row].miniRoomMesh, roadTmp);
                AddRoad(miniRoomMeshList[col, row].miniRoomMesh, roadTmp);
            }
        }
        if (col < NUM_ROOM - 1)  // right road (col + 1, row)
        {
            if (miniRoomMeshList[col + 1, row].miniRoomMesh != null && !miniRoomMeshList[col, row].right)
            {
                float distance = (miniRoomMeshList[col + 1, row].miniRoomMesh.transform.localPosition.x - miniRoomMeshList[col + 1, row].miniRoomMesh.transform.localScale.x / 2) - (miniRoomMeshList[col , row].miniRoomMesh.transform.localPosition.x + miniRoomMeshList[col , row].miniRoomMesh.transform.localScale.x / 2);

                /* draw road */
                GameObject roadTmp = Instantiate(miniRoadMesh, GameObject.Find("minimapGroup").transform);
                // set road position
                float roadX = miniRoomMeshList[col, row].miniRoomMesh.transform.localPosition.x + miniRoomMeshList[col, row].miniRoomMesh.transform.localScale.x / 2 + distance / 2;
                float roomsHeight = Mathf.Max(miniRoomMeshList[col, row].miniRoomMesh.transform.localPosition.y + miniRoomMeshList[col, row].miniRoomMesh.transform.localScale.y / 2, miniRoomMeshList[col + 1, row].miniRoomMesh.transform.localPosition.y + miniRoomMeshList[col + 1, row].miniRoomMesh.transform.localScale.y / 2) - Mathf.Min(miniRoomMeshList[col, row].miniRoomMesh.transform.localPosition.y - miniRoomMeshList[col, row].miniRoomMesh.transform.localScale.y / 2, miniRoomMeshList[col + 1, row].miniRoomMesh.transform.localPosition.y - miniRoomMeshList[col + 1, row].miniRoomMesh.transform.localScale.y / 2);
                float roadY = Mathf.Max(miniRoomMeshList[col, row].miniRoomMesh.transform.localPosition.y + miniRoomMeshList[col, row].miniRoomMesh.transform.localScale.y / 2, miniRoomMeshList[col + 1, row].miniRoomMesh.transform.localPosition.y + miniRoomMeshList[col + 1, row].miniRoomMesh.transform.localScale.y / 2) - roomsHeight / 2;

                roadTmp.transform.localPosition = new Vector3(roadX, roadY, 0.0f);
                // set road roatation
                roadTmp.transform.localRotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
                // set road scale
                roadTmp.transform.localScale = new Vector3(distance, 5.0f, 0.0f);

                /* check road */
                miniRoomMeshList[col, row].right = true;
                miniRoomMeshList[col + 1, row].left = true;

                AddRoad(miniRoomMeshList[col, row].miniRoomMesh, roadTmp);
                AddRoad(miniRoomMeshList[col + 1, row].miniRoomMesh, roadTmp);
            }
        }
        if (col <= 0 || col >= NUM_ROOM)
        {
            
        }

        // row:행:y
        if (row > 0)    // upper road (col, row - 1)
        {
            if (miniRoomMeshList[col, row - 1].miniRoomMesh != null && !miniRoomMeshList[col, row].up)
            {
                float distance = (miniRoomMeshList[col, row - 1].miniRoomMesh.transform.localPosition.y - miniRoomMeshList[col, row - 1].miniRoomMesh.transform.localScale.y / 2) - (miniRoomMeshList[col, row].miniRoomMesh.transform.localPosition.y + miniRoomMeshList[col, row].miniRoomMesh.transform.localScale.y / 2);

                /* draw road */
                GameObject roadTmp = Instantiate(miniRoadMesh, GameObject.Find("minimapGroup").transform);
                // set road position
                float roomsWidth = Mathf.Max(miniRoomMeshList[col, row].miniRoomMesh.transform.localPosition.x + miniRoomMeshList[col, row].miniRoomMesh.transform.localScale.x / 2, miniRoomMeshList[col, row - 1].miniRoomMesh.transform.localPosition.x + miniRoomMeshList[col, row - 1].miniRoomMesh.transform.localScale.x / 2) - Mathf.Min(miniRoomMeshList[col, row].miniRoomMesh.transform.localPosition.x - miniRoomMeshList[col, row].miniRoomMesh.transform.localScale.x / 2, miniRoomMeshList[col, row - 1].miniRoomMesh.transform.localPosition.x - miniRoomMeshList[col, row - 1].miniRoomMesh.transform.localScale.x / 2);
                float roadX = Mathf.Max(miniRoomMeshList[col, row].miniRoomMesh.transform.localPosition.x + miniRoomMeshList[col, row].miniRoomMesh.transform.localScale.x / 2, miniRoomMeshList[col, row - 1].miniRoomMesh.transform.localPosition.x + miniRoomMeshList[col, row - 1].miniRoomMesh.transform.localScale.x / 2) - roomsWidth / 2;
                float roadY = miniRoomMeshList[col, row - 1].miniRoomMesh.transform.localPosition.y - miniRoomMeshList[col, row - 1].miniRoomMesh.transform.localScale.y / 2 - distance / 2;

                roadTmp.transform.localPosition = new Vector3(roadX, roadY, 0.0f);
                // set road roatation
                roadTmp.transform.localRotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
                // set road scale
                roadTmp.transform.localScale = new Vector3(5.0f, distance, 0.0f);

                /* check road */
                miniRoomMeshList[col, row - 1].down = true;
                miniRoomMeshList[col, row].up = true;

                AddRoad(miniRoomMeshList[col, row - 1].miniRoomMesh, roadTmp);
                AddRoad(miniRoomMeshList[col, row].miniRoomMesh, roadTmp);
            }
        }
        if (row < NUM_ROOM - 1)  // lower road (col, row + 1)
        {
            if (miniRoomMeshList[col, row + 1].miniRoomMesh != null && !miniRoomMeshList[col, row].down)
            {
                float distance = (miniRoomMeshList[col, row].miniRoomMesh.transform.localPosition.y - miniRoomMeshList[col, row].miniRoomMesh.transform.localScale.y / 2) - (miniRoomMeshList[col, row + 1].miniRoomMesh.transform.localPosition.y + miniRoomMeshList[col, row + 1].miniRoomMesh.transform.localScale.y / 2);

                /* draw road */
                GameObject roadTmp = Instantiate(miniRoadMesh, GameObject.Find("minimapGroup").transform);
                // set road position
                float roomsWidth = Mathf.Max(miniRoomMeshList[col, row].miniRoomMesh.transform.localPosition.x + miniRoomMeshList[col, row].miniRoomMesh.transform.localScale.x / 2, miniRoomMeshList[col, row + 1].miniRoomMesh.transform.localPosition.x + miniRoomMeshList[col, row + 1].miniRoomMesh.transform.localScale.x / 2) - Mathf.Min(miniRoomMeshList[col, row].miniRoomMesh.transform.localPosition.x - miniRoomMeshList[col, row].miniRoomMesh.transform.localScale.x / 2, miniRoomMeshList[col, row + 1].miniRoomMesh.transform.localPosition.x - miniRoomMeshList[col, row + 1].miniRoomMesh.transform.localScale.x / 2);
                float roadX = Mathf.Max(miniRoomMeshList[col, row].miniRoomMesh.transform.localPosition.x + miniRoomMeshList[col, row].miniRoomMesh.transform.localScale.x / 2, miniRoomMeshList[col, row + 1].miniRoomMesh.transform.localPosition.x + miniRoomMeshList[col, row + 1].miniRoomMesh.transform.localScale.x / 2) - roomsWidth / 2;
                float roadY = miniRoomMeshList[col, row + 1].miniRoomMesh.transform.localPosition.y + miniRoomMeshList[col, row + 1].miniRoomMesh.transform.localScale.y / 2 + distance / 2;

                roadTmp.transform.localPosition = new Vector3(roadX, roadY, 0.0f);
                // set road roatation
                roadTmp.transform.localRotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
                // set road scale
                roadTmp.transform.localScale = new Vector3(5.0f, distance, 0.0f);

                /* check road */
                miniRoomMeshList[col, row].down = true;
                miniRoomMeshList[col, row + 1].up = true;

                AddRoad(miniRoomMeshList[col, row].miniRoomMesh, roadTmp);
                AddRoad(miniRoomMeshList[col, row + 1].miniRoomMesh, roadTmp);
            }
        }
        if (row <= 0 || row >= NUM_ROOM)
        {
            
        }
    }

    private void AddRoad(GameObject room, GameObject road)
    {
        if(room.GetComponent<EnemyGenerator>() != null)
        {
            room.GetComponent<EnemyGenerator>().AddMinimapRoad(road);
        }
        else
        {
            room.GetComponent<BossRoom>().AddMinimapRoad(road);
        }
    }
}
