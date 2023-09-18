using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//룸 종류
public enum RoomType
{
    ROOM_START,
    ROOM_NORMAL,
    ROOM_KITCHEN,
    ROOM_BOSS
    
};

public class Room
{
    public Rect nodeRect; //공간의 사각형 정보
    public Rect roomRect; //공간 안 방의 사각형 정보
    public int isCreated; //실제로 생성되었는지 + 시작정점까지의 길이 판별용
    public RoomType roomType; //방의 종류
    public GameObject enemyGenerator; //방의 에너미 제너레이터 정보

    public float rightYPoint;
    public float leftYPoint;
    public float upXPoint;
    public float downXPoint;

    public Room()
    {
        this.isCreated = 0;
        this.roomType = RoomType.ROOM_NORMAL;
    }

    public Vector2 center
    {
        get
        {
            return new Vector2(roomRect.x + (roomRect.width / 2), roomRect.y - (roomRect.height / 2));
        }
    }

    
}


