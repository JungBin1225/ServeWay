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
    [SerializeField] Tile roomTile; //방 내부를 구성하는 타일
    //[SerializeField] Tile wallTile; // 벽 타일
    //[SerializeField] Tile wallTopTile; //상단 벽 천장 부분, 현재 사용 X
    [SerializeField] Tile topWallTile; //상단 벽 타일
    //[SerializeField] Tile wallLeftEdgeTile; //벽 왼쪽 모서리 부분
    //[SerializeField] Tile wallRightEdgeTile; //벽 오른쪽 모서리 부분
    [SerializeField] Tile outTile; //방 외부의 타일
    [SerializeField] Tile leftWallTile; //왼쪽 벽 타일
    [SerializeField] Tile rightWallTile; //오른쪽 벽 타일
    [SerializeField] Tile bottomWallTile; //아래쪽 벽 타일
    [SerializeField] Tile bottomRightEdgeTile; //아래쪽 왼편 모서리 타일
    [SerializeField] Tile topRightEdgeTile; //위쪽 오른편 모서리 타일
    [SerializeField] Tile topLeftEdgeTile; //위쪽 왼편 모서리 타일
    [SerializeField] Tile bottomLeftEdgeTile; //아래쪽 왼편 모서리 타일
    //[SerializeField] Tile topRightInternalEdgeTile; //위쪽 오른편 내각 모서리 타일, 내각 타일은 방과 통로 연결 부분 모서리에 사용
    //[SerializeField] Tile bottomRightInternalEdgeTile; //아래쪽 오른편 내각 모서리 타일
    //[SerializeField] Tile topLeftInternalEdgeTile; //위쪽 왼편 내각 모서리 타일
    //[SerializeField] Tile bottomLeftInternalEdgeTile; //아래쪽 왼편 내각 모서리 타일

    [SerializeField] Tile roadTile; //통로 타일
    [SerializeField] Tile leftRoadEdgeTile; //통로 왼편 시작지점 타일
    [SerializeField] Tile rightRoadEdgeTile; //통로 오른편 시작지점 타일
    [SerializeField] Tile topRoadEdgeTile; //통로 윗편 시작지점 타일
    [SerializeField] Tile bottomRoadEdgeTile; //통로 아래편 시작지점 타일

    [SerializeField] Tile startRoomTile; //시작방 타일
    [SerializeField] Tile startRoomTopLeftEdgeTile; //시작방 위쪽 왼편 모서리 타일
    [SerializeField] Tile startRoomLeftWallTile; //시작방 왼쪽 벽 타일
    [SerializeField] Tile startRoomBottomLeftEdgeTile; //시작방 아래쪽 왼편 모서리 타일
    [SerializeField] Tile startRoomTopRightEdgeTile; //시작방 위쪽 오른편 모서리 타일
    [SerializeField] Tile startRoomRightWallTile; //시작방 오른쪽 벽 타일
    [SerializeField] Tile startRoomBottomRightEdgeTile; //시작방 아래쪽 오른편 모서리 타일
    [SerializeField] Tile startRoomTopWallTile; //시작방 위쪽 벽 타일
    [SerializeField] Tile startRoomBottomWallTile; //시작방 아래쪽 벽 타일

    [SerializeField] Tile kitchenTile; //주방 타일
    [SerializeField] Tile bossTile; //보스방 타일

    [SerializeField] GameObject Player;
    [SerializeField] GameObject EnemyGenerator;
    [SerializeField] GameObject BossGenerator;
    [SerializeField] GameObject createTablePrefab;
    [SerializeField] GameObject refrigeratorPrefab;
    [SerializeField] List<GameObject> doorPrefab; //up:0 down:1 right:2 left:3

    // 미니맵
    [SerializeField] Tilemap miniTileMap;
    [SerializeField] Tile miniRoomTile;
    [SerializeField] Tile miniWallTile;
    [SerializeField] Tile miniOutTile;
    // 주방 콜라이더
    GameObject KitchenCollider;
    // 시작방 콜라이더
    GameObject StartCollider;

    const int NUM_ROOM = 5; 
    Room[ , ] roomList = new Room[NUM_ROOM,NUM_ROOM];

    //시작점 좌표. 일단 좌측상단으로 고정
    int startX = 0;
    int startY = 0;
    //추후 주방 좌표, 보스방 좌표도 추가 예정...

    //맵 만드는데 필요한 변수들
    int roomCnt, tempCnt;
    int lastDepth = 1;
    int[] dx = new int[4] { -1, 1, 0, 0 }; //좌 우 하 상 
    int[] dy = new int[4] { 0, 0, 1, -1 };


    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.gameManager.charData.saveFile.isMapSave) //로드될 맵이 있으면 실행
        {
            LoadMap();
        }
        else // 로드될 맵이 없으면 새로 생성
        {
            Init(); // 초기화
        }

        DrawBackGround(); // 전체 맵 사각형 그리기
        CreateMap(); // 방이랑 길 그리기
        DisplayRoomType(); // 시작방, 주방, 보스방 표시
        SetDoor();
        // 플레이어 위치 초기화
        Player.transform.position = roomList[startY, startX].enemyGenerator.transform.position;
        GameObject.Find("miniPlayer").transform.position = roomList[startY, startX].enemyGenerator.transform.position;

        GameManager.gameManager.charData.SaveMapData(roomList, startX, startY);
    }

    void Init()
    {
        //방 리스트 초기화
        for (int i = 0; i < NUM_ROOM; i++)
        {
            for (int j = 0; j < NUM_ROOM; j++)
            {
                roomList[i, j] = new Room();
            }
        }

        //시작점 위치 정하기
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
        Debug.LogFormat("startX = {0} startY = {1}", startX, startY);
        roomList[startY, startX].isCreated = 1;

        //만들 방의 전체 개수 설정
        roomCnt = UnityEngine.Random.Range(10, 21);
        //앞으로 만들어야할 방 개수 설정
        tempCnt = roomCnt;
    }

    void DrawBackGround()
    {
        //타일 그리기 전 백그라운드 타일로 다 채우기
        for (int i = -10; i < mapSize.x + 10; i++)
        {
            for (int j = -10; j < mapSize.y + 10; j++)
            {
                tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), outTile);
            }
        }

    }

    void CreateMap()
    {

        if(!GameManager.gameManager.charData.saveFile.isMapSave) //로드될 맵이 있으면 실행하지 않음
        {
            //그래프 생성
            DFS(startX, startY, 1);
        }
        //구조에 따라 맵 생성
        Divide();

        //각 방마다 시작 방까지의 거리 계산하기 + 길 생성
        BFS(startX, startY, 1);

        //방을 둘러싸는 벽 타일 그리기
        DrawWall();
    }

    void DFS(int x,int y,int depth)
    {
        //이미 방이 다 만들어졌다면 더 로직을 진행할 필요가 없다.
        if (tempCnt == 0) return;

        //만들 수 있는 방 개수
        int curCnt = 0;
        //갈 수 있는 방향
        int[] isDir = new int[4];
        for (int i = 0; i < isDir.Length; i++)
        {
            isDir[i] = 0;
        }

        //4방향에서 갈 수 있는 방향을 파악한 후, 그 중에 한 방향을 골라 이동한다.
        for (int i = 0; i < 4; i++)
        {
            int tx = x + dx[i]; int ty = y + dy[i];
            if (tx >= 0 && ty >= 0 && tx < NUM_ROOM && ty < NUM_ROOM && roomList[ty,tx].isCreated == 0)
            {
                curCnt++;
                isDir[i] = 1;
            }
        }
        if (curCnt == 0) return; //더 나아갈 수 없다면 리턴

        //현재 방에서 만들 방의 개수를 1~만들수 있는 방향 개수 중 랜덤으로 정한다
        int temp = UnityEngine.Random.Range(1, curCnt + 1);
        curCnt = temp;

        for (int i = 0; i < 4; i++)
        {
            if (curCnt == 0) return; //방 다 만들었다면 리턴
            if (tempCnt == 0) return;

            //어디방향으로 갈지 택
            int dir = UnityEngine.Random.Range(0,4);

            //한쪽 방향 쏠림 방지
            while (isDir[dir] != 1)
            {
                dir = UnityEngine.Random.Range(0, 4);

                //무한루프 이슈 방지용 임시 땜빵..추후 수정예정
                int sum = 0;
                for (int j = 0; j < 4; j++)
                {
                    sum += isDir[j];
                }
                if (sum == 0) return;
            }
            //한번 택한 방향은 선택할 수 없게 처리
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
        
        //시작 방은 길이 1로 초기화.
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
                        //같은 길이의 다른 노드가 도착 방을 먼저 방문한 경우에도 
                        //현재 노드 - 도착 방 길을 그릴 수 있도록 한다.
                        isVisited[ty, tx] = isVisited[y, x] + 1;
                        roomList[ty, tx].isCreated = isVisited[ty, tx];
                        DrawRoad(x, y, tx, ty);
                    }
                }

            }

        }

       
    }

    void DrawRoom(float horz,float vert,int ROW,int COL, int index)
    {
        /**여기 좌표는 모두 좌측 상단 기준**/
        
        //노드 생성(디버그용)
        /*
        LineRenderer nodeRenderer = Instantiate(node).GetComponent<LineRenderer>();
        nodeRenderer.SetPosition(0, new Vector2(horz, vert - (float)mapSize.y / 5)); //좌측 하단
        nodeRenderer.SetPosition(1, new Vector2(horz + (float)mapSize.x / 5, vert- (float)mapSize.y / 5)); //우측 하단
        nodeRenderer.SetPosition(2, new Vector2(horz + (float)mapSize.x / 5, vert)); //우측 상단
        nodeRenderer.SetPosition(3, new Vector2(horz, vert)); //좌측 상단
        */

        roomList[ROW, COL].nodeRect = new Rect(horz, vert, (float)mapSize.x / 5, (float)mapSize.y / 5);

        Rect nodeRect = roomList[ROW, COL].nodeRect;

        //width 범위의 최대값을  nodeRect.width-2까지 잡아야 방끼리 겹침 문제가 안생김
        float width = UnityEngine.Random.Range(nodeRect.width / 2, nodeRect.width - 2);
        float height = UnityEngine.Random.Range(nodeRect.height/2, nodeRect.height - 1);
        float x = nodeRect.x + UnityEngine.Random.Range(1,nodeRect.width-width-1);
        float y = nodeRect.y - UnityEngine.Random.Range(1,nodeRect.height-height-1);
        if (GameManager.gameManager.charData.saveFile.isMapSave)
        {
            width = GameManager.gameManager.charData.saveFile.roomList[index].roomRect.width;
            height = GameManager.gameManager.charData.saveFile.roomList[index].roomRect.height;
            x = GameManager.gameManager.charData.saveFile.roomList[index].roomRect.x;
            y = GameManager.gameManager.charData.saveFile.roomList[index].roomRect.y;
        }
        // 노드 안의 방 생성(디버그용)
        /*
        LineRenderer roomRenderer = Instantiate(room).GetComponent<LineRenderer>();
        roomRenderer.SetPosition(0, new Vector2(x, y - height)); //좌측 하단
        roomRenderer.SetPosition(1, new Vector2(x + width, y - height)); //우측 하단
        roomRenderer.SetPosition(2, new Vector2(x + width,y)); //우측 상단
        roomRenderer.SetPosition(3, new Vector2(x, y)); //좌측 상단
        */

        roomList[ROW, COL].roomRect = new Rect(x, y, width, height);
        
        Rect roomRect = roomList[ROW, COL].roomRect;


        //** 방 크기에 에너미 제너레이터 크기를 맞추기 위한 변수 **
        //room의 맨 왼쪽 위 사각형 좌표
        Vector3 temp = new Vector3(0, 0, 0);
        //room의 맨 오른쪽 아래 사각형 좌표
        Vector3 temp2 = new Vector3(0, 0, 0);
        
     

        //룸타일 그리기
        for (float i= roomRect.x; i<roomRect.x + roomRect.width;i++)
        {
            for(float j=roomRect.y;j>roomRect.y-roomRect.height;j--)
            {
                Vector3Int tilePosition = tileMap.WorldToCell(new Vector3(i, j, 0));
                
                if(i==roomRect.x && j == roomRect.y)
                {
                    //처음
                    temp = new Vector3(tilePosition.x+0.5f,tilePosition.y+0.5f,tilePosition.z);
                    
                }

                if (tileMap.GetTile(tilePosition) == outTile)
                {
                    if(i==roomRect.x&& tileMap.GetTile(tileMap.WorldToCell(new Vector3(i - 1, j, 0))) == roomTile)
                    {
                        //만약 바로 왼쪽 벽이 룸타일이면
                        //여기도 룸타일을 생성하면 벽 구분을 할 수 없으므로 룸타일을 생성해주지 않는다.
                       
                    }
                    else
                    {
                        tileMap.SetTile(tilePosition, roomTile);
                    }
                   

                }else if(tileMap.GetTile(tilePosition) == roomTile)
                {
                    //만약 같은 위치에 여러번 룸타일이 겹친다면
                    //벽이 생기지 않을 수 있으므로 바깥타일로 교체
                    tileMap.SetTile(tilePosition, outTile);
                    //miniTileMap.SetTile(tilePosition, miniOutTile);
                }
            }
        }

        //position 피벗이 중앙임
        //룸 하나당 에너미 제너레이터도 하나씩 생성
        roomList[ROW, COL].enemyGenerator = Instantiate(EnemyGenerator);
        // 미니맵 - collider에 룸 위치 전달
        roomList[ROW, COL].enemyGenerator.GetComponent<EnemyGenerator>().myRow = ROW;
        roomList[ROW, COL].enemyGenerator.GetComponent<EnemyGenerator>().myCol = COL;

        temp2 = new Vector3(temp.x + (int)roomList[ROW, COL].roomRect.width, temp.y - (int)roomList[ROW, COL].roomRect.height);
        //room의 맨 왼쪽 위 사각형 표시
        //Instantiate(EnemyGenerator).transform.position = new Vector3(temp.x, temp.y);
        //room의 맨 오른쪽 아래 사각형 표시
        //Instantiate(EnemyGenerator).transform.position = new Vector3(temp2.x, temp2.y); 

        //EnemyGenerator 크기 room 크기에 맞추기 조정
        roomList[ROW, COL].enemyGenerator.transform.position = new Vector3( (temp.x+temp2.x)/2,(temp.y+temp2.y)/2 );
        
        //+1 안해주면 양쪽 반 칸이 모자름
        roomList[ROW, COL].enemyGenerator.transform.localScale = new Vector3((int)roomList[ROW, COL].roomRect.width + 1, (int)roomList[ROW, COL].roomRect.height + 1);

        if(COL == startX && ROW == startY)
        {
            AnnounceStartMap(roomList[ROW, COL].enemyGenerator);
        }
    }

    void DrawRoad(int x,int y,int nextX,int nextY)
    {
        //x,y nextX, nextY
        Rect fromRect = roomList[y, x].roomRect;
        Rect toRect = roomList[nextY, nextX].roomRect;
        Vector2 fromCenter = new Vector2(fromRect.x + fromRect.width/2 , fromRect.y - fromRect.height/2);
        Vector2 toCenter = new Vector2(toRect.x + toRect.width/2 , toRect.y - toRect.height/2);

        //가로 길
        //바로 오른쪽, 왼쪽 방에만 길을 그릴 수 있도록 설정하기
        if (x == nextX+1 || x == nextX-1)
        {

            for (float i = Mathf.Min(fromCenter.x, toCenter.x); i <= Mathf.Max(fromCenter.x, toCenter.x); i++)
            {
                float distanceY = (Mathf.Max(fromRect.y, toRect.y) - Mathf.Min(fromRect.y - fromRect.height, toRect.y - toRect.height)) / 2;
                float pointY = Mathf.Max(fromRect.y, toRect.y) - distanceY;

                for(int k= -1 ; k <= 1 ; k++) //타일 배치 로직 부분, 최적화 필요할듯
                {
                    Vector3Int tilePosition = tileMap.WorldToCell(new Vector3(i, pointY + k, 0));
                    if(tileMap.GetTile(tilePosition) != roomTile) //생성할 타일이 외부 타일이면 통로 타일 설치
                    {
                        if(tileMap.GetTile(tileMap.WorldToCell(new Vector3(i - 1, pointY + k, 0))) == roomTile) //왼쪽에 룸타일이 있으면 길 왼쪽 시작부 타일 설치
                        {
                            tileMap.SetTile(tilePosition, leftRoadEdgeTile);
                        }
                        else if(tileMap.GetTile(tileMap.WorldToCell(new Vector3(i + 1, pointY + k, 0))) == roomTile) //오른쪽에 룸타일이 있으면 길 오른쪽 시작부 타일 설치
                        {
                            tileMap.SetTile(tilePosition, rightRoadEdgeTile);
                        }
                        else
                            tileMap.SetTile(tilePosition, roadTile);
                    }
                }

                if (x == nextX + 1)
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

        //세로 길
        //바로 위쪽, 아래쪽 방에만 길을 그릴 수 있도록 설정하기
        if (y == nextY+1 || y == nextY-1)
        {
            for (float i = Mathf.Min(fromCenter.y, toCenter.y); i <= Mathf.Max(fromCenter.y, toCenter.y); i++)
            {
                float distanceX = (Mathf.Max(fromRect.x + fromRect.width, toRect.x + toRect.width) - Mathf.Min(fromRect.x, toRect.x)) / 2;
                float pointX = Mathf.Min(fromRect.x, toRect.x) + distanceX;

                //GameObject door;

                for (int k = -1; k <= 1; k++) //타일 배치 로직 부분, 최적화 필요할듯
                {
                    Vector3Int tilePosition = tileMap.WorldToCell(new Vector3(pointX + k, i, 0));
                    if (tileMap.GetTile(tilePosition) != roomTile) //생성할 타일이 외부 타일이면 통로 타일 설치
                    {
                        if (tileMap.GetTile(tileMap.WorldToCell(new Vector3(pointX + k, i + 1, 0))) == roomTile) //위쪽에 룸타일이 있으면 길 위쪽 시작부 타일 설치
                        {
                            tileMap.SetTile(tilePosition, topRoadEdgeTile);
                        }
                        else if (tileMap.GetTile(tileMap.WorldToCell(new Vector3(pointX + k, i - 1, 0))) == roomTile) //아래쪽에 룸타일이 있으면 길 아래쪽 시작부 타일 설치
                        {
                            tileMap.SetTile(tilePosition, bottomRoadEdgeTile);
                        }
                        else
                            tileMap.SetTile(tilePosition, roadTile);
                    }
                }

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
        //정수끼리의 나눗셈은 무조건 정수로 나와서 분모or 분자를 float 형변환 필요
        float horzPoint = 0-(float)mapSize.x/2, vertPoint = (0+mapSize.y) - (float)mapSize.y/2;
        float horzSize = (float)mapSize.x/5, vertSize = (float)mapSize.y/5;

        int index = 0;
        //세로
        for (int i = 0; i < NUM_ROOM; i++)
        {
            horzPoint = 0 - mapSize.x/2;
            //가로
            for(int j = 0; j < NUM_ROOM; j++)
            {
                if (roomList[i, j].isCreated > 0)
                {
                    //방 타일 그리기 
                    DrawRoom(horzPoint, vertPoint, i, j, index);
                }
                horzPoint += horzSize;

                index++;
            }

            vertPoint -= vertSize;
        }
        // 미니맵 - 시작 방이면 miniRoomMesh 바로 생성
        //roomList[startY, startX].enemyGenerator.GetComponent<EnemyGenerator>().GenerateMiniRoomMesh();

    }

    void DrawWall() //알고리즘과 조건문 대폭 수정함
    {
        //범위가 0~mapSize.x가 아니라 -1~mapSize.x+1인 이유는 전자로 하면 벽 타일이 방을 휘감지 못하는 사태가 생기기 때문
        for (int i = -1; i < mapSize.x + 1; i++) //타일 전체를 순회
        {
            for (int j = -1; j < mapSize.y + 1; j++)
            {
                if (tileMap.GetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0)) == outTile)
                {
                    var tempTile = tileMap.GetTile(new Vector3Int(i - mapSize.x / 2 - 1, j - mapSize.y / 2 + 0, 0)); //-1
                    var tempTile2 = tileMap.GetTile(new Vector3Int(i - mapSize.x / 2 + 1, j - mapSize.y / 2 + 0, 0)); //+1
                    if ((tempTile == roomTile)/* || (tempTile == roadTile) || (tempTile == topRoadEdgeTile) || (tempTile == bottomRoadEdgeTile)*/) //(-1, 0) 왼쪽에 룸타일, rightWall 배치
                    {
                        /*
                        if ((tileMap.GetTile(new Vector3Int(i - mapSize.x / 2 + 0, j - mapSize.y / 2 + 1, 0)) == roomTile) || (tileMap.GetTile(new Vector3Int(i - mapSize.x / 2 + 0, j - mapSize.y / 2 + 1, 0)) == leftRoadEdgeTile)) // 상단에도 룸타일이면 내각 모서리 타일 배치
                        {
                            tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), bottomRightInternalEdgeTile);
                        }
                        else if ((tileMap.GetTile(new Vector3Int(i - mapSize.x / 2 + 0, j - mapSize.y / 2 - 1, 0)) == roomTile) || (tileMap.GetTile(new Vector3Int(i - mapSize.x / 2 + 0, j - mapSize.y / 2 - 1, 0)) == leftRoadEdgeTile)) //하단에도 룸타일이면 내각 모서리 타일 배치
                        {
                            tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), topRightInternalEdgeTile);
                        }
                        else
                        {
                            tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), rightWallTile);
                        }*/
                        tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), rightWallTile);
                    }
                    else if ((tempTile2 == roomTile)/* || (tempTile2 == roadTile) || (tempTile2 == topRoadEdgeTile) || (tempTile2 == bottomRoadEdgeTile)*/) //(1, 0) 오른쪽에 룸타일, leftWall 배치
                    {
                        /*
                        if ((tileMap.GetTile(new Vector3Int(i - mapSize.x / 2 + 0, j - mapSize.y / 2 + 1, 0)) == roomTile) || (tileMap.GetTile(new Vector3Int(i - mapSize.x / 2 + 0, j - mapSize.y / 2 + 1, 0)) == rightRoadEdgeTile)) // 상단에도 룸타일이면 내각 모서리 타일 배치
                        {
                            tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), bottomLeftInternalEdgeTile);
                        }
                        else if ((tileMap.GetTile(new Vector3Int(i - mapSize.x / 2 + 0, j - mapSize.y / 2 - 1, 0)) == roomTile) || (tileMap.GetTile(new Vector3Int(i - mapSize.x / 2 + 0, j - mapSize.y / 2 - 1, 0)) == rightRoadEdgeTile)) //하단에도 룸타일이면 내각 모서리 타일 배치
                        {
                            tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), topLeftInternalEdgeTile);
                        }
                        else
                        {
                            tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), leftWallTile);
                        }*/
                        tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), leftWallTile);
                    }
                    else if (tileMap.GetTile(new Vector3Int(i - mapSize.x / 2 + 0, j - mapSize.y / 2 - 1, 0)) == roomTile) //(0, -1) 아래에 룸타일, 상단 벽 배치
                    {
                        tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), topWallTile); //상단 벽면 세로면 타일로 변경
                        //tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2 + 1, 0), wallTopTile); //벽면 타일 윗부분을 상단 타일로 변경
                    }
                    else if (tileMap.GetTile(new Vector3Int(i - mapSize.x / 2 + 0, j - mapSize.y / 2 + 1, 0)) == roomTile) //(0, 1) 위에 룸타일, 하단 벽 배치
                    {
                        tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), bottomWallTile);
                    }
                    else if (tileMap.GetTile(new Vector3Int(i - mapSize.x / 2 - 1, j - mapSize.y / 2 + 1, 0)) == roomTile) //(-1, 1) 상단좌측에 룸타일,  하단우측모서리 배치
                    {
                        tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), bottomRightEdgeTile);
                    }
                    else if (tileMap.GetTile(new Vector3Int(i - mapSize.x / 2 - 1, j - mapSize.y / 2 - 1, 0)) == roomTile) //(-1, -1) 하단좌측에 룸타일,  상단우측모서리 배치
                    {
                        tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), topRightEdgeTile);
                    }
                    else if (tileMap.GetTile(new Vector3Int(i - mapSize.x / 2 + 1, j - mapSize.y / 2 - 1, 0)) == roomTile) //(1, -1) 하단우측에 룸타일,  상단좌측모서리 배치
                    {
                        tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), topLeftEdgeTile);
                    }
                    else if (tileMap.GetTile(new Vector3Int(i - mapSize.x / 2 + 1, j - mapSize.y / 2 + 1, 0)) == roomTile) //(1, 1) 상단우측에 룸타일,  하단좌측모서리 배치
                    {
                        tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), bottomLeftEdgeTile);
                    }
                    else if (tempTile == startRoomTile) //** 이 아래로는 시작방 타일 배치 판별용 조건문 **, 구조는 위와 동일
                    {
                        tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), startRoomRightWallTile);
                    }
                    else if (tempTile2 == startRoomTile)
                    {
                        tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), startRoomLeftWallTile);
                    }
                    else if (tileMap.GetTile(new Vector3Int(i - mapSize.x / 2 + 0, j - mapSize.y / 2 - 1, 0)) == startRoomTile) //(0, -1) 아래에 룸타일, 상단 벽 배치
                    {
                        tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), startRoomTopWallTile); //상단 벽면 세로면 타일로 변경
                    }
                    else if (tileMap.GetTile(new Vector3Int(i - mapSize.x / 2 + 0, j - mapSize.y / 2 + 1, 0)) == startRoomTile) //(0, 1) 위에 룸타일, 하단 벽 배치
                    {
                        tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), startRoomBottomWallTile);
                    }
                    else if (tileMap.GetTile(new Vector3Int(i - mapSize.x / 2 - 1, j - mapSize.y / 2 + 1, 0)) == startRoomTile) //(-1, 1) 상단좌측에 룸타일,  하단우측모서리 배치
                    {
                        tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), startRoomBottomRightEdgeTile);
                    }
                    else if (tileMap.GetTile(new Vector3Int(i - mapSize.x / 2 - 1, j - mapSize.y / 2 - 1, 0)) == startRoomTile) //(-1, -1) 하단좌측에 룸타일,  상단우측모서리 배치
                    {
                        tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), startRoomTopRightEdgeTile);
                    }
                    else if (tileMap.GetTile(new Vector3Int(i - mapSize.x / 2 + 1, j - mapSize.y / 2 - 1, 0)) == startRoomTile) //(1, -1) 하단우측에 룸타일,  상단좌측모서리 배치
                    {
                        tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), startRoomTopLeftEdgeTile);
                    }
                    else if (tileMap.GetTile(new Vector3Int(i - mapSize.x / 2 + 1, j - mapSize.y / 2 + 1, 0)) == startRoomTile) //(1, 1) 상단우측에 룸타일,  하단좌측모서리 배치
                    {
                        tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), startRoomBottomLeftEdgeTile);
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

        var kitchenPos = new KeyValuePair<int, int>();
        var bossPos = new KeyValuePair<int, int>();

        if (!GameManager.gameManager.charData.saveFile.isMapSave) //로드될 맵이 있으면 이미 보스방, 주방이 결정되어 있으므로 실행X
        {
            if (bossNum > 4)
            {
                //주방은 4~lastDepth-1 사이에
                kitchenNum = (UnityEngine.Random.Range(4, bossNum));
            }
            else
            {
                //lastDepth가 4이면
                //주방은 3에 (4가 되면 겹침)
                kitchenNum = 3;
            }

            //주방 깊이랑 보스방 깊이가 같은 방이 여러개 일 수 있음.
            //그 중 방 하나를 랜덤으로 뽑아야함.
            for (int i = 0; i < NUM_ROOM; i++)
            {
                for (int j = 0; j < NUM_ROOM; j++)
                {
                    if (roomList[i, j].isCreated != 0)
                    {
                        if (startY == i && startX == j)
                        {
                            roomList[i, j].roomType = RoomType.ROOM_START;
                        }
                        else if (roomList[i, j].isCreated == kitchenNum)
                        {
                            //x,y 형태로 집어 넣기
                            kitchenIdxList.Add(new KeyValuePair<int, int>(j, i));
                        }
                        else if (roomList[i, j].isCreated == bossNum)
                        {
                            bossIdxList.Add(new KeyValuePair<int, int>(j, i));
                        }
                    }
                }
            }

            kitchenPos = kitchenIdxList[UnityEngine.Random.Range(0, kitchenIdxList.Count)];
            bossPos = bossIdxList[UnityEngine.Random.Range(0, bossIdxList.Count)];

            roomList[kitchenPos.Value, kitchenPos.Key].roomType = RoomType.ROOM_KITCHEN;
            roomList[bossPos.Value, bossPos.Key].roomType = RoomType.ROOM_BOSS;
        }
        else //로드될 맵이 있으면 주방과 보스방의 인덱스를 반환
        {
            for (int i = 0; i < NUM_ROOM; i++)
            {
                for(int j = 0; j < NUM_ROOM; j++)
                {
                    if(roomList[i, j].roomType == RoomType.ROOM_KITCHEN)
                    {
                        kitchenPos = new KeyValuePair<int, int>(j, i);
                    }

                    if(roomList[i, j].roomType == RoomType.ROOM_BOSS)
                    {
                        bossPos = new KeyValuePair<int, int>(j, i);
                    }
                }
            }

        }

        for (int k = 0; k < 3; k++)
        {
            Tile nowTile = roomTile;
            int ROW = 0, COL = 0;
            if (k == 0)
            {
                // 시작방
                ROW = startY;COL = startX;
                nowTile = startRoomTile;
                roomList[ROW, COL].enemyGenerator.GetComponent<EnemyGenerator>().nonEnemyRoom = true;
                //Destroy(roomList[ROW, COL].enemyGenerator);
            }
            else if (k == 1)
            {
                // 주방
                ROW = kitchenPos.Value; COL = kitchenPos.Key;
                nowTile = kitchenTile;

                roomList[ROW, COL].enemyGenerator.GetComponent<EnemyGenerator>().nonEnemyRoom = true;
                GameObject createTable = Instantiate(createTablePrefab, roomList[ROW, COL].enemyGenerator.transform.position, Quaternion.Euler(0, 0, 0));
                GameObject refrigerator = Instantiate(refrigeratorPrefab, createTable.transform.position + new Vector3(4, 0, 0), Quaternion.Euler(0, 0, 0));
            }
            else if (k == 2)
            {
                // 보스방
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
                        //miniTileMap.SetTile(tilePosition, nowTile);
                    }
                 

                }
            }
        }

        GameObject miniKit = GameObject.Find("miniKitchen");
        GameObject miniBoss = GameObject.Find("miniBoss");

        roomList[kitchenPos.Value, kitchenPos.Key].enemyGenerator.GetComponent<EnemyGenerator>().minimapIcon = miniKit;
        roomList[bossPos.Value, bossPos.Key].enemyGenerator.GetComponent<BossRoom>().minimapIcon = miniBoss;

        miniKit.transform.position = roomList[kitchenPos.Value, kitchenPos.Key].enemyGenerator.transform.position;
        miniBoss.transform.position = roomList[bossPos.Value, bossPos.Key].enemyGenerator.transform.position;

        miniKit.SetActive(false);
        miniBoss.SetActive(false);


        for (int i = -1; i < mapSize.x + 1; i++) //시작방 벽타일 배치용 코드, 맵타일 생성 후 특정 방을 시작방으로 만들어서 시작방 지정된 후 한번 더 돌려야 함
        {
            for (int j = -1; j < mapSize.y + 1; j++)
            {
                var temtemTile = tileMap.GetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0));
                if ((temtemTile == rightWallTile) || (temtemTile == leftWallTile) || (temtemTile == topWallTile) || (temtemTile == bottomWallTile) || (temtemTile == bottomRightEdgeTile) || (temtemTile == topRightEdgeTile) || (temtemTile == topLeftEdgeTile) || (temtemTile == bottomLeftEdgeTile))
                {
                    var tempTile = tileMap.GetTile(new Vector3Int(i - mapSize.x / 2 - 1, j - mapSize.y / 2 + 0, 0)); //-1
                    var tempTile2 = tileMap.GetTile(new Vector3Int(i - mapSize.x / 2 + 1, j - mapSize.y / 2 + 0, 0)); //+1
                    if (tempTile == startRoomTile) //** 이 아래로는 시작방 타일 배치 판별용 조건문 **, 구조는 위와 동일
                    {
                        tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), startRoomRightWallTile);
                    }
                    else if (tempTile2 == startRoomTile)
                    {
                        tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), startRoomLeftWallTile);
                    }
                    else if (tileMap.GetTile(new Vector3Int(i - mapSize.x / 2 + 0, j - mapSize.y / 2 - 1, 0)) == startRoomTile) //(0, -1) 아래에 룸타일, 상단 벽 배치
                    {
                        tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), startRoomTopWallTile); //상단 벽면 세로면 타일로 변경
                    }
                    else if (tileMap.GetTile(new Vector3Int(i - mapSize.x / 2 + 0, j - mapSize.y / 2 + 1, 0)) == startRoomTile) //(0, 1) 위에 룸타일, 하단 벽 배치
                    {
                        tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), startRoomBottomWallTile);
                    }
                    else if (tileMap.GetTile(new Vector3Int(i - mapSize.x / 2 - 1, j - mapSize.y / 2 + 1, 0)) == startRoomTile) //(-1, 1) 상단좌측에 룸타일,  하단우측모서리 배치
                    {
                        tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), startRoomBottomRightEdgeTile);
                    }
                    else if (tileMap.GetTile(new Vector3Int(i - mapSize.x / 2 - 1, j - mapSize.y / 2 - 1, 0)) == startRoomTile) //(-1, -1) 하단좌측에 룸타일,  상단우측모서리 배치
                    {
                        tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), startRoomTopRightEdgeTile);
                    }
                    else if (tileMap.GetTile(new Vector3Int(i - mapSize.x / 2 + 1, j - mapSize.y / 2 - 1, 0)) == startRoomTile) //(1, -1) 하단우측에 룸타일,  상단좌측모서리 배치
                    {
                        tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), startRoomTopLeftEdgeTile);
                    }
                    else if (tileMap.GetTile(new Vector3Int(i - mapSize.x / 2 + 1, j - mapSize.y / 2 + 1, 0)) == startRoomTile) //(1, 1) 상단우측에 룸타일,  하단좌측모서리 배치
                    {
                        tileMap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), startRoomBottomLeftEdgeTile);
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
                        point = roomList[i, j].enemyGenerator.transform.position.y + ((roomList[i, j].enemyGenerator.transform.localScale.y + 1) / 2) + (doorPrefab[0].transform.localScale.y / 2);
                        GameObject door = Instantiate(doorPrefab[0], new Vector3(roomList[i, j].upXPoint, point - 0.5f, 0), Quaternion.Euler(0, 0, 0), doorPrefab[4].transform);
                        roomList[i, j].enemyGenerator.GetComponent<EnemyGenerator>().doorList.Add(door);
                    }

                    if (i < NUM_ROOM - 1 && roomList[i + 1, j].isCreated != 0)
                    {
                        point = roomList[i, j].enemyGenerator.transform.position.y - ((roomList[i, j].enemyGenerator.transform.localScale.y + 1) / 2) - (doorPrefab[1].transform.localScale.y / 2);
                        GameObject door = Instantiate(doorPrefab[1], new Vector3(roomList[i, j].downXPoint, point + 0.5f, 0), Quaternion.Euler(0, 0, 0), doorPrefab[4].transform);
                        roomList[i, j].enemyGenerator.GetComponent<EnemyGenerator>().doorList.Add(door);
                    }

                    if (j > 0 && roomList[i, j - 1].isCreated != 0)
                    {
                        point = roomList[i, j].enemyGenerator.transform.position.x - ((roomList[i, j].enemyGenerator.transform.localScale.x + 1) / 2) - 0.756f;
                        GameObject door = Instantiate(doorPrefab[3], new Vector3(point + 0.5f, roomList[i, j].rightYPoint, 0), Quaternion.Euler(0, 0, -90), doorPrefab[4].transform);
                        roomList[i, j].enemyGenerator.GetComponent<EnemyGenerator>().doorList.Add(door);
                    }

                    if (j < NUM_ROOM - 1 && roomList[i, j + 1].isCreated != 0)
                    {
                        point = roomList[i, j].enemyGenerator.transform.position.x + ((roomList[i, j].enemyGenerator.transform.localScale.x + 1) / 2) + 0.756f;
                        GameObject door = Instantiate(doorPrefab[2], new Vector3(point - 0.5f, roomList[i, j].leftYPoint, 0), Quaternion.Euler(0, 0, 90), doorPrefab[4].transform);
                        roomList[i, j].enemyGenerator.GetComponent<EnemyGenerator>().doorList.Add(door);
                    }
                }
                else if(roomList[i, j].isCreated != 0 && roomList[i, j].roomType == RoomType.ROOM_BOSS)
                {
                    float point;

                    if (i > 0 && roomList[i - 1, j].isCreated != 0)
                    {
                        point = roomList[i, j].enemyGenerator.transform.position.y + ((roomList[i, j].enemyGenerator.transform.localScale.y + 1) / 2) + (doorPrefab[0].transform.localScale.y / 2);
                        GameObject door = Instantiate(doorPrefab[0], new Vector3(roomList[i, j].upXPoint, point - 0.5f, 0), Quaternion.Euler(0, 0, 0), doorPrefab[4].transform);
                        roomList[i, j].enemyGenerator.GetComponent<BossRoom>().doorList.Add(door);
                    }

                    if (i < NUM_ROOM - 1 && roomList[i + 1, j].isCreated != 0)
                    {
                        point = roomList[i, j].enemyGenerator.transform.position.y - ((roomList[i, j].enemyGenerator.transform.localScale.y + 1) / 2) - (doorPrefab[1].transform.localScale.y / 2);
                        GameObject door = Instantiate(doorPrefab[1], new Vector3(roomList[i, j].downXPoint, point + 0.5f, 0), Quaternion.Euler(0, 0, 0), doorPrefab[4].transform);
                        roomList[i, j].enemyGenerator.GetComponent<BossRoom>().doorList.Add(door);
                    }

                    if (j > 0 && roomList[i, j - 1].isCreated != 0)
                    {
                        point = roomList[i, j].enemyGenerator.transform.position.x - ((roomList[i, j].enemyGenerator.transform.localScale.x + 1) / 2) - 0.756f;
                        GameObject door = Instantiate(doorPrefab[3], new Vector3(point + 0.5f, roomList[i, j].rightYPoint, 0), Quaternion.Euler(0, 0, -90), doorPrefab[4].transform);
                        roomList[i, j].enemyGenerator.GetComponent<BossRoom>().doorList.Add(door);
                    }

                    if (j < NUM_ROOM - 1 && roomList[i, j + 1].isCreated != 0)
                    {
                        point = roomList[i, j].enemyGenerator.transform.position.x + ((roomList[i, j].enemyGenerator.transform.localScale.x + 1) / 2) + 0.756f;
                        GameObject door = Instantiate(doorPrefab[2], new Vector3(point - 0.5f, roomList[i, j].leftYPoint, 0), Quaternion.Euler(0, 0, 90), doorPrefab[4].transform);
                        roomList[i, j].enemyGenerator.GetComponent<BossRoom>().doorList.Add(door);
                    }
                }
            }
        }
    }

    public void LoadMap()
    {
        int index = 0;
        for(int i = 0; i < NUM_ROOM; i++)
        {
            for(int j = 0; j < NUM_ROOM; j++)
            {
                roomList[i, j] = GameManager.gameManager.charData.saveFile.roomList[index];
                index++;
            }
        }
        startX = GameManager.gameManager.charData.saveFile.startX;
        startY = GameManager.gameManager.charData.saveFile.startY;

        roomList[startY, startX].isCreated = 1;
    }

    // 보스방 좌표 알리기
    public KeyValuePair<int, int> BossGridNum()
    {
        var targetPos = new KeyValuePair<int, int>();
        for (int i = 0; i < NUM_ROOM; i++)
        {
            for (int j = 0; j < NUM_ROOM; j++)
            {
                if (roomList[i, j].roomType == RoomType.ROOM_BOSS)
                {
                    targetPos = new KeyValuePair<int, int>(j, i);
                }
            }
        }
        return targetPos;
    }

    private void AnnounceStartMap(GameObject enemyGen)
    {
        enemyGen.GetComponent<EnemyGenerator>().isStartMap = true;
    }
}
