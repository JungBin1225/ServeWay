using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;



public class MapGenerator : MonoBehaviour
{
    [SerializeField] Vector2Int mapSize;
    [SerializeField] private GameObject map;
    [SerializeField] private GameObject node;
    [SerializeField] private GameObject room;
    [SerializeField] private GameObject road;

    [SerializeField] Tilemap tileMap;
    [SerializeField] Tile roomTile; //���� �����ϴ� Ÿ��
    [SerializeField] Tile wallTile; //��� �ܺθ� ���������� �� Ÿ��
    [SerializeField] Tile outTile; //�� �ܺ��� Ÿ��

    [SerializeField] Tile startRoomTile; //���۹� Ÿ��
    [SerializeField] Tile kitchenTile; //�ֹ� Ÿ��
    [SerializeField] Tile bossTile; //������ Ÿ��

    [SerializeField] GameObject Player;
    [SerializeField] GameObject EnemyGenerator;
    [SerializeField] GameObject BossGenerator;
    [SerializeField] GameObject createTablePrefab;
    [SerializeField] GameObject doorPrefab;


    const int NUM_ROOM = 5; 
    Room[ , ] roomList = new Room[NUM_ROOM,NUM_ROOM];

    //������ ��ǥ. �ϴ� ����������� ����
    int startX = 0;
    int startY = 0;
    //���� �ֹ� ��ǥ, ������ ��ǥ�� �߰� ����...

    //�� ����µ� �ʿ��� ������
    int roomCnt, tempCnt;
    int lastDepth = 1;
    int[] dx = new int[4] { -1, 1, 0, 0 };
    int[] dy = new int[4] { 0, 0, 1, -1 };


    // Start is called before the first frame update
    void Start()
    {
        SetRoomList(); //�� ����Ʈ �ʱ�ȭ
        SetStartPos(); //������ ���ϱ�

        DrawBackGround(0,0); //��ü �� �簢�� �׸���
        CreateMap(); //���̶� �� �׸���

        DisplayRoomType(); //���۹�, �ֹ�, ������ ǥ��
        SetDoor();
        //�÷��̾� ��ġ �ʱ�ȭ
        Player.transform.position = new Vector3(roomList[startY, startX].roomRect.x , roomList[startY, startX].roomRect.y , 0);
    }

    void SetStartPos()
    {
        //������ ��ġ ���ϱ�
        switch (UnityEngine.Random.Range(0, 4))
        {
            case 0:
                startX = 0; startY = 0; break;
            case 1:
                startX = 4; startY = 0; break;
            case 2:
                startX = 0; startY = 4; break;
            case 3:
                startX = 4; startY = 4; break;
            default:
                startX = 0; startY = 0; break;
        }
        Debug.LogFormat("startX = {0} startY = {1}",startX,startY);
        roomList[startY, startX].isCreated = 1;
        
    }

    void SetRoomList()
    {
        for(int i = 0; i < NUM_ROOM; i++)
        {
            for(int j = 0; j < NUM_ROOM; j++)
            {
                roomList[i,j] = new Room();
            }
        }
       

    }

    void DrawBackGround(int x, int y) //x,y�� ȭ���� �߾���ġ
    {
        for(int i=-10;i<mapSize.x+10;i++)
        {
            for(int j=-10;j<mapSize.y+10;j++)
            {
                tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), outTile);
            }
        }
    }

    void CreateMap()
    {
        //10~20������ ����
        roomCnt = UnityEngine.Random.Range(10, 21);
        tempCnt = roomCnt;

        //�׷��� ����
        DFS(startX, startY, 1);

        //������ ���� �� ����
        Divide(); 

        //�� �渶�� ���� ������� �Ÿ� ����ϱ� + �� ����
        BFS(startX, startY, 1);

        DrawWall();

        for(int i=0;i<NUM_ROOM; i++)
        {
            for(int j = 0; j < NUM_ROOM; j++)
            {
                if (roomList[i,j].isCreated != 0)
                {
                    Debug.LogFormat("row : {0} col : {1} depth : {2}", i, j, roomList[i,j].isCreated);
                }
            }
        }

        Debug.LogFormat("lastDepth : {0}",lastDepth);
    }

    void DFS(int x,int y,int depth)
    {
        //�̹� ���� �� ��������ٸ� �� ������ ������ �ʿ䰡 ����.
        if (tempCnt == 0) return;

        //���� �� �ִ� �� ����
        int curCnt = 0;
        int[] isDir = new int[4];
        for (int i = 0; i < isDir.Length; i++)
        {
            isDir[i] = 0;
        }

        //4���⿡�� �� �� �ִ� ������ �ľ��� ��, �� �߿� �� ������ ��� �̵��Ѵ�.
        for (int i = 0; i < 4; i++)
        {
            int tx = x + dx[i]; int ty = y + dy[i];
            if (tx >= 0 && ty >= 0 && tx < NUM_ROOM && ty < NUM_ROOM && roomList[ty,tx].isCreated == 0)
            {
                curCnt++;
                isDir[i] = 1;
            }
        }
        if (curCnt == 0) return; //�� ���ư� �� ���ٸ� ����
        int temp = UnityEngine.Random.Range(1, curCnt + 1);
        curCnt = temp;

        for (int i = 0; i < 4; i++)
        {
            if (curCnt == 0) return; //�� �� ������ٸ� ����
            if (tempCnt == 0) return;

            //���������� ���� ��
            int dir = UnityEngine.Random.Range(0,4);

            //���� ���� �� ����
            while (isDir[dir] != 1)
            {
                dir = UnityEngine.Random.Range(0, 4);

                //���ѷ��� �̽� ������ �ӽ� ����..���� ��������
                int sum = 0;
                for (int j = 0; j < 4; j++)
                {
                    sum += isDir[j];
                }
                if (sum == 0) return;
            }
            //�ѹ� ���� ������ ������ �� ���� ó��
            isDir[dir] = 0;
            int tx = x + dx[dir];
            int ty = y + dy[dir];
            if (tx >= 0 && ty >= 0 && tx < NUM_ROOM && ty < NUM_ROOM && roomList[ty,tx].isCreated == 0)
            {

                roomList[ty,tx].isCreated = roomList[y, x].isCreated + 1;

                curCnt--;
                tempCnt--;
                DFS(tx, ty, roomList[ty, tx].isCreated);
            }
        }

       

    }

    void BFS(int startX, int startY, int depth)
    {
        int[,] isVisited = new int[NUM_ROOM, NUM_ROOM];
        for (int i = 0; i < NUM_ROOM; i++)
        {
            for(int j=0;j < NUM_ROOM; j++) isVisited[i,j] = 0;
        }
        
        //���� ���� ���� 1�� �ʱ�ȭ.
        Queue<KeyValuePair<int, int>> q = new Queue<KeyValuePair<int, int>>();
        q.Enqueue(new KeyValuePair<int, int>(startX, startY));
        isVisited[startY,startX] = 1;
        roomList[startY,startX].isCreated = 1;
        
      

        while (q.Count != 0)
        {
            int x = q.Peek().Key;
            int y = q.Peek().Value;
            q.Dequeue();

            for (int i = 0; i < 4; i++)
            {
                int tx = x + dx[i];
                int ty = y + dy[i];

                if (tx >= 0 && tx < NUM_ROOM && ty >= 0 && ty < NUM_ROOM)
                {
                    if(isVisited[ty, tx] == 0 && roomList[ty, tx].isCreated != 0)
                    {
                        isVisited[ty, tx] = isVisited[y, x] + 1;
                        roomList[ty, tx].isCreated = isVisited[ty, tx];
                        q.Enqueue(new KeyValuePair<int, int>(tx, ty));
                        DrawRoad(x, y, tx, ty);

                        lastDepth = Mathf.Max(lastDepth, roomList[ty, tx].isCreated);

                    }else if (isVisited[y,x]+1 == isVisited[ty,tx] && roomList[ty, tx].isCreated != 0)
                    {
                        //���� ������ �ٸ� ��尡 ���� ���� ���� �湮�� ��쿡�� 
                        //���� ��� - ���� �� ���� �׸� �� �ֵ��� �Ѵ�.
                        isVisited[ty, tx] = isVisited[y, x] + 1;
                        roomList[ty, tx].isCreated = isVisited[ty, tx];
                        DrawRoad(x, y, tx, ty);
                    }
                }

            }

        }

       
    }

    void DrawRoom(float horz,float vert,int ROW,int COL)
    {
        /**���� ��ǥ�� ��� ���� ��� ����**/
        
        //��� ����(����׿�)
        /*
        LineRenderer nodeRenderer = Instantiate(node).GetComponent<LineRenderer>();
        nodeRenderer.SetPosition(0, new Vector2(horz, vert - (float)mapSize.y / 5)); //���� �ϴ�
        nodeRenderer.SetPosition(1, new Vector2(horz + (float)mapSize.x / 5, vert- (float)mapSize.y / 5)); //���� �ϴ�
        nodeRenderer.SetPosition(2, new Vector2(horz + (float)mapSize.x / 5, vert)); //���� ���
        nodeRenderer.SetPosition(3, new Vector2(horz, vert)); //���� ���
        */

        roomList[ROW, COL].nodeRect = new Rect(horz, vert, (float)mapSize.x / 5, (float)mapSize.y / 5);
        Rect nodeRect = roomList[ROW, COL].nodeRect;

        //width ������ �ִ밪��  nodeRect.width-2���� ��ƾ� �波�� ��ħ ������ �Ȼ���
        float width = UnityEngine.Random.Range(nodeRect.width / 2, nodeRect.width - 2);
        float height = UnityEngine.Random.Range(nodeRect.height/2, nodeRect.height - 1);
        float x = nodeRect.x + UnityEngine.Random.Range(1,nodeRect.width-width-1);
        float y = nodeRect.y - UnityEngine.Random.Range(1,nodeRect.height-height-1);

        // ��� ���� �� ����(����׿�)
        /*
        LineRenderer roomRenderer = Instantiate(room).GetComponent<LineRenderer>();
        roomRenderer.SetPosition(0, new Vector2(x, y - height)); //���� �ϴ�
        roomRenderer.SetPosition(1, new Vector2(x + width, y - height)); //���� �ϴ�
        roomRenderer.SetPosition(2, new Vector2(x + width,y)); //���� ���
        roomRenderer.SetPosition(3, new Vector2(x, y)); //���� ���
        */


        roomList[ROW, COL].roomRect = new Rect(x, y, width, height);
        Rect roomRect = roomList[ROW, COL].roomRect;


        //** �� ũ�⿡ ���ʹ� ���ʷ����� ũ�⸦ ���߱� ���� ���� **
        //room�� �� ���� �� �簢�� ��ǥ
        Vector3 temp = new Vector3(0, 0, 0);
        //room�� �� ������ �Ʒ� �簢�� ��ǥ
        Vector3 temp2 = new Vector3(0, 0, 0);
        
     

        //��Ÿ�� �׸���
        for (float i= roomRect.x; i<roomRect.x + roomRect.width;i++)
        {
            for(float j=roomRect.y;j>roomRect.y-roomRect.height;j--)
            {
                Vector3Int tilePosition = tileMap.WorldToCell(new Vector3(i, j, 0));
                
                if(i==roomRect.x && j == roomRect.y)
                {
                    //ó��
                    temp = new Vector3(tilePosition.x+0.5f,tilePosition.y+0.5f,tilePosition.z);
                    
                }

                if ((int)i == (int)(roomRect.x + roomRect.width - 1) && (int)j == (int)(roomRect.y - roomRect.height + 1))
                {
                    //������
                    //temp2 = new Vector3(tilePosition.x+1.5f, tilePosition.y+0.5f, tilePosition.z);
                   
                }

                //Debug.LogFormat("i = {0} j = {1} {2} {3}", i,j, (roomRect.x + roomRect.width - 1), (roomRect.y - roomRect.height + 1));

                if (tileMap.GetTile(tilePosition) == outTile)
                {
                    if(i==roomRect.x&& tileMap.GetTile(tileMap.WorldToCell(new Vector3(i - 1, j, 0))) == roomTile)
                    {
                        //���� �ٷ� ���� ���� ��Ÿ���̸�
                        //���⵵ ��Ÿ���� �����ϸ� �� ������ �� �� �����Ƿ� ��Ÿ���� ���������� �ʴ´�.
                       
                    }
                    else
                    {
                        tileMap.SetTile(tilePosition, roomTile);
                    }
                   

                }else if(tileMap.GetTile(tilePosition) == roomTile)
                {
                    //���� ���� ��ġ�� ������ ��Ÿ���� ��ģ�ٸ�
                    //���� ������ ���� �� �����Ƿ� �ٱ�Ÿ�Ϸ� ��ü
                    tileMap.SetTile(tilePosition, outTile);
                }
            }
        }

        //position �ǹ��� �߾���
        //�� �ϳ��� ���ʹ� ���ʷ����͵� �ϳ��� ����
        roomList[ROW, COL].enemyGenerator = Instantiate(EnemyGenerator);

        temp2 = new Vector3(temp.x + (int)roomList[ROW, COL].roomRect.width, temp.y - (int)roomList[ROW, COL].roomRect.height);
        //room�� �� ���� �� �簢�� ǥ��
        //Instantiate(EnemyGenerator).transform.position = new Vector3(temp.x, temp.y);
        //room�� �� ������ �Ʒ� �簢�� ǥ��
        //Instantiate(EnemyGenerator).transform.position = new Vector3(temp2.x, temp2.y); 

        //EnemyGenerator ũ�� room ũ�⿡ ���߱� ����
        roomList[ROW, COL].enemyGenerator.transform.position = new Vector3( (temp.x+temp2.x)/2,(temp.y+temp2.y)/2 ); 
        roomList[ROW, COL].enemyGenerator.transform.localScale = new Vector3((int)roomList[ROW, COL].roomRect.width,  (int)roomList[ROW, COL].roomRect.height);



    }

    void DrawLine(Vector2 from, Vector2 to)
    {
        LineRenderer lineRenderer = Instantiate(road).GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, from);
        lineRenderer.SetPosition(1, to);
    }

    void DrawRoad(int x,int y,int nextX,int nextY)
    {
        //x,y nextX, nextY
        Rect fromRect = roomList[y, x].roomRect;
        Rect toRect = roomList[nextY, nextX].roomRect;
        Vector2 fromCenter = new Vector2(fromRect.x + fromRect.width/2 , fromRect.y - fromRect.height/2);
        Vector2 toCenter = new Vector2(toRect.x + toRect.width/2 , toRect.y - toRect.height/2);

        //���� ��
        //�ٷ� ������, ���� �濡�� ���� �׸� �� �ֵ��� �����ϱ�
        if (x == nextX+1 || x == nextX-1)
        {

            for (float i = Mathf.Min(fromCenter.x, toCenter.x); i <= Mathf.Max(fromCenter.x, toCenter.x); i++)
            {
                float distanceY = (Mathf.Max(fromRect.y, toRect.y) - Mathf.Min(fromRect.y - fromRect.height, toRect.y - toRect.height)) / 2;
                float pointY = Mathf.Max(fromRect.y, toRect.y) - distanceY;

                Vector3Int tilePosition = tileMap.WorldToCell(new Vector3(i, pointY, 0));
                tileMap.SetTile(tilePosition, roomTile);


                tilePosition = tileMap.WorldToCell(new Vector3(i, pointY - 1, 0));
                tileMap.SetTile(tilePosition, roomTile);


                tilePosition = tileMap.WorldToCell(new Vector3(i, pointY + 1, 0));
                tileMap.SetTile(tilePosition, roomTile);

                if(x == nextX + 1)
                {
                    if(pointY < 0)
                    {
                        roomList[y, x].rightYPoint = (int)(pointY) - 0.5f;
                        roomList[nextY, nextX].leftYPoint = (int)(pointY) - 0.5f;
                    }
                    else
                    {
                        roomList[y, x].rightYPoint = (int)(pointY) + 0.5f;
                        roomList[nextY, nextX].leftYPoint = (int)(pointY) + 0.5f;
                    }
                }
                else
                {
                    if (pointY < 0)
                    {
                        roomList[y, x].leftYPoint = (int)(pointY) - 0.5f;
                        roomList[nextY, nextX].rightYPoint = (int)(pointY) - 0.5f;
                    }
                    else
                    {
                        roomList[y, x].leftYPoint = (int)(pointY) + 0.5f;
                        roomList[nextY, nextX].rightYPoint = (int)(pointY) + 0.5f;
                    }
                }
            }
        }

        //���� ��
        //�ٷ� ����, �Ʒ��� �濡�� ���� �׸� �� �ֵ��� �����ϱ�
        if (y == nextY+1 || y == nextY-1)
        {
            for (float i = Mathf.Min(fromCenter.y, toCenter.y); i <= Mathf.Max(fromCenter.y, toCenter.y); i++)
            {
                float distanceX = (Mathf.Max(fromRect.x + fromRect.width, toRect.x + toRect.width) - Mathf.Min(fromRect.x, toRect.x)) / 2;
                float pointX = Mathf.Min(fromRect.x, toRect.x) + distanceX;

                GameObject door;

                Vector3Int tilePosition = tileMap.WorldToCell(new Vector3(pointX, i, 0));
                tileMap.SetTile(tilePosition, roomTile);


                tilePosition = tileMap.WorldToCell(new Vector3(pointX - 1, i, 0));
                tileMap.SetTile(tilePosition, roomTile);

                tilePosition = tileMap.WorldToCell(new Vector3(pointX + 1, i, 0));
                tileMap.SetTile(tilePosition, roomTile);

                if (y == nextY + 1)
                {
                    if (pointX < 0)
                    {
                        roomList[y, x].upXPoint = (int)(pointX) - 0.5f;
                        roomList[nextY, nextX].downXPoint = (int)(pointX) - 0.5f;
                    }
                    else
                    {
                        roomList[y, x].upXPoint = (int)(pointX) + 0.5f;
                        roomList[nextY, nextX].downXPoint = (int)(pointX) + 0.5f;
                    }

                }
                else
                {
                    if (pointX < 0)
                    {
                        roomList[y, x].downXPoint = (int)(pointX) - 0.5f;
                        roomList[nextY, nextX].upXPoint = (int)(pointX) - 0.5f;
                    }
                    else
                    {
                        roomList[y, x].downXPoint = (int)(pointX) + 0.5f;
                        roomList[nextY, nextX].upXPoint = (int)(pointX) + 0.5f;
                    }
                }
            }
        }
        

    }


    void Divide()
    {
        //���������� �������� ������ ������ ���ͼ� �и�or ���ڸ� float ����ȯ �ʿ�
        float horzPoint = 0-(float)mapSize.x/2, vertPoint = (0+mapSize.y) - (float)mapSize.y/2;
        float horzSize = (float)mapSize.x/5, vertSize = (float)mapSize.y/5;
       
        //����
        for (int i = 0; i < NUM_ROOM; i++)
        {
            horzPoint = 0 - mapSize.x/2;
            //����
            for(int j = 0; j < NUM_ROOM; j++)
            {
                if (roomList[i, j].isCreated > 0)
                {
                    //���� ������� ��츸 ����ϱ�
                    DrawRoom(horzPoint, vertPoint, i, j);
                }
                horzPoint += horzSize;
            }

            vertPoint -= vertSize;
        }

    }

    void DrawWall()
    {
        //������ 0~mapSize.x�� �ƴ϶� -1~mapSize.x+1�� ������ ���ڷ� �ϸ� �� Ÿ���� ���� �ְ��� ���ϴ� ���°� ����� ����
        for (int i = -1; i < mapSize.x+1; i++) //Ÿ�� ��ü�� ��ȸ
        {
            for (int j = -1; j < mapSize.y+1; j++)
            {
                if (tileMap.GetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0)) == outTile)
                {
                    //�ٱ�Ÿ�� �� ���
                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                            if (x == 0 && y == 0) continue;//�ٱ� Ÿ�� ���� 8������ Ž���ؼ� room tile�� �ִٸ� wall tile�� �ٲ��ش�.
                            if (tileMap.GetTile(new Vector3Int(i - mapSize.x / 2 + x, j - mapSize.y / 2 + y, 0)) == roomTile)
                            {
                                tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), wallTile);
                                break;
                            }
                        }
                    }
                }
            }
        }
    }

    void DisplayRoomType()
    {
        List<KeyValuePair <int, int>> bossIdxList = new List <KeyValuePair<int, int>>();
        List<KeyValuePair <int, int>> kitchenIdxList = new List <KeyValuePair<int, int>>();

        int bossNum = lastDepth; int kitchenNum = 0;
        
        if (bossNum > 4)
        {
            //�ֹ��� 4~lastDepth-1 ���̿�
            kitchenNum = (UnityEngine.Random.Range(4, bossNum)); 
        }
        else
        {
            //lastDepth�� 4�̸�
            //�ֹ��� 3�� (4�� �Ǹ� ��ħ)
            kitchenNum = 3;
        }
        
        //�ֹ� ���̶� ������ ���̰� ���� ���� ������ �� �� ����.
        //�� �� �� �ϳ��� �������� �̾ƾ���.
        for (int i = 0; i < NUM_ROOM; i++)
        {
            for (int j = 0; j < NUM_ROOM; j++)
            {
                if (roomList[i,j].isCreated != 0)
                {
                    if(startY == i && startX == j)
                    {
                        roomList[i, j].roomType = RoomType.ROOM_START;
                    }else if (roomList[i,j].isCreated == kitchenNum)
                    {
                        //x,y ���·� ���� �ֱ�
                        kitchenIdxList.Add(new KeyValuePair<int, int>(j, i));
                    }else if (roomList[i,j].isCreated == bossNum)
                    {
                        bossIdxList.Add(new KeyValuePair<int, int>(j, i));
                    }
                }
            }
        }

        var kitchenPos = kitchenIdxList[UnityEngine.Random.Range(0, kitchenIdxList.Count)];
        var bossPos = bossIdxList[UnityEngine.Random.Range(0, bossIdxList.Count)];

        roomList[kitchenPos.Value, kitchenPos.Key].roomType = RoomType.ROOM_KITCHEN;
        roomList[bossPos.Value, bossPos.Key].roomType = RoomType.ROOM_BOSS;


        for(int k = 0; k < 3; k++)
        {
            Tile nowTile = roomTile;
            int ROW = 0, COL = 0;
            if (k == 0)
            {
                ROW = startY;COL = startX;
                nowTile = startRoomTile;
                Destroy(roomList[ROW, COL].enemyGenerator);
            }
            else if (k == 1)
            {
                ROW = kitchenPos.Value; COL = kitchenPos.Key;
                nowTile = kitchenTile;

                Destroy(roomList[ROW, COL].enemyGenerator);
                GameObject createTable = Instantiate(createTablePrefab, new Vector3(roomList[ROW, COL].roomRect.x + (roomList[ROW, COL].roomRect.width / 2), roomList[ROW, COL].roomRect.y - (roomList[ROW, COL].roomRect.height / 2), 0), Quaternion.Euler(0, 0, 0));
            }
            else if (k == 2)
            {
                ROW = bossPos.Value; COL = bossPos.Key;
                nowTile = bossTile;

                Vector3 pos = roomList[ROW, COL].enemyGenerator.transform.position;
                Vector3 size = roomList[ROW, COL].enemyGenerator.transform.localScale;
                Destroy(roomList[ROW, COL].enemyGenerator);
                roomList[ROW, COL].enemyGenerator = Instantiate(BossGenerator);
                roomList[ROW, COL].enemyGenerator.transform.position = pos;
                roomList[ROW, COL].enemyGenerator.transform.localScale = size;
            }
            

            Rect roomRect = roomList[ROW, COL].roomRect;

            for (float i = roomRect.x; i < roomRect.x + roomRect.width; i++)
            {
                for (float j = roomRect.y; j > roomRect.y - roomRect.height; j--)
                {
                    Vector3Int tilePosition = tileMap.WorldToCell(new Vector3(i, j, 0));

                    if (tileMap.GetTile(tilePosition) == roomTile)
                    {

                        tileMap.SetTile(tilePosition, nowTile);
                    }
                 

                }
            }
        }
       
    }

    void SetDoor()
    {
        for(int i = 0; i < NUM_ROOM; i++)
        {
            for(int j = 0; j < NUM_ROOM; j++)
            {
                if(roomList[i, j].isCreated != 0 && roomList[i, j].roomType == RoomType.ROOM_NORMAL)
                {
                    float point;

                    if (i > 0 && roomList[i - 1, j].isCreated != 0)
                    {
                        point = roomList[i, j].enemyGenerator.transform.position.y + ((roomList[i, j].enemyGenerator.transform.localScale.y + 1) / 2) + (doorPrefab.transform.localScale.y / 2);
                        GameObject door = Instantiate(doorPrefab, new Vector3(roomList[i, j].upXPoint, point, 0), Quaternion.Euler(0, 0, 0));
                        roomList[i, j].enemyGenerator.GetComponent<EnemyGenerator>().doorList.Add(door);
                        door.SetActive(false);
                    }

                    if (i < NUM_ROOM - 1 && roomList[i + 1, j].isCreated != 0)
                    {
                        point = roomList[i, j].enemyGenerator.transform.position.y - ((roomList[i, j].enemyGenerator.transform.localScale.y + 1) / 2) - (doorPrefab.transform.localScale.y / 2);
                        GameObject door = Instantiate(doorPrefab, new Vector3(roomList[i, j].downXPoint, point, 0), Quaternion.Euler(0, 0, 180));
                        roomList[i, j].enemyGenerator.GetComponent<EnemyGenerator>().doorList.Add(door);
                        door.SetActive(false);
                    }

                    if (j > 0 && roomList[i, j - 1].isCreated != 0)
                    {
                        point = roomList[i, j].enemyGenerator.transform.position.x - ((roomList[i, j].enemyGenerator.transform.localScale.x + 1) / 2) - (doorPrefab.transform.localScale.y / 2);
                        GameObject door = Instantiate(doorPrefab, new Vector3(point, roomList[i, j].rightYPoint, 0), Quaternion.Euler(0, 0, 90));
                        roomList[i, j].enemyGenerator.GetComponent<EnemyGenerator>().doorList.Add(door);
                        door.SetActive(false);
                    }

                    if (j < NUM_ROOM - 1 && roomList[i, j + 1].isCreated != 0)
                    {
                        point = roomList[i, j].enemyGenerator.transform.position.x + ((roomList[i, j].enemyGenerator.transform.localScale.x + 1) / 2) + (doorPrefab.transform.localScale.y / 2);
                        GameObject door = Instantiate(doorPrefab, new Vector3(point, roomList[i, j].leftYPoint, 0), Quaternion.Euler(0, 0, -90));
                        roomList[i, j].enemyGenerator.GetComponent<EnemyGenerator>().doorList.Add(door);
                        door.SetActive(false);
                    }
                }
                else if(roomList[i, j].isCreated != 0 && roomList[i, j].roomType == RoomType.ROOM_BOSS)
                {
                    float point;

                    if (i > 0 && roomList[i - 1, j].isCreated != 0)
                    {
                        point = roomList[i, j].enemyGenerator.transform.position.y + ((roomList[i, j].enemyGenerator.transform.localScale.y + 1) / 2) + (doorPrefab.transform.localScale.y / 2);
                        GameObject door = Instantiate(doorPrefab, new Vector3(roomList[i, j].upXPoint, point, 0), Quaternion.Euler(0, 0, 0));
                        roomList[i, j].enemyGenerator.GetComponent<BossRoom>().doorList.Add(door);
                        door.SetActive(false);
                    }

                    if (i < NUM_ROOM - 1 && roomList[i + 1, j].isCreated != 0)
                    {
                        point = roomList[i, j].enemyGenerator.transform.position.y - ((roomList[i, j].enemyGenerator.transform.localScale.y + 1) / 2) - (doorPrefab.transform.localScale.y / 2);
                        GameObject door = Instantiate(doorPrefab, new Vector3(roomList[i, j].downXPoint, point, 0), Quaternion.Euler(0, 0, 180));
                        roomList[i, j].enemyGenerator.GetComponent<BossRoom>().doorList.Add(door);
                        door.SetActive(false);
                    }

                    if (j > 0 && roomList[i, j - 1].isCreated != 0)
                    {
                        point = roomList[i, j].enemyGenerator.transform.position.x - ((roomList[i, j].enemyGenerator.transform.localScale.x + 1) / 2) - (doorPrefab.transform.localScale.y / 2);
                        GameObject door = Instantiate(doorPrefab, new Vector3(point, roomList[i, j].rightYPoint, 0), Quaternion.Euler(0, 0, 90));
                        roomList[i, j].enemyGenerator.GetComponent<BossRoom>().doorList.Add(door);
                        door.SetActive(false);
                    }

                    if (j < NUM_ROOM - 1 && roomList[i, j + 1].isCreated != 0)
                    {
                        point = roomList[i, j].enemyGenerator.transform.position.x + ((roomList[i, j].enemyGenerator.transform.localScale.x + 1) / 2) + (doorPrefab.transform.localScale.y / 2);
                        GameObject door = Instantiate(doorPrefab, new Vector3(point, roomList[i, j].leftYPoint, 0), Quaternion.Euler(0, 0, -90));
                        roomList[i, j].enemyGenerator.GetComponent<BossRoom>().doorList.Add(door);
                        door.SetActive(false);
                    }
                }
            }
        }
    }
}
