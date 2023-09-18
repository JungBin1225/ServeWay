using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�� ����
public enum RoomType
{
    ROOM_START,
    ROOM_NORMAL,
    ROOM_KITCHEN,
    ROOM_BOSS
    
};

public class Room
{
    public Rect nodeRect; //������ �簢�� ����
    public Rect roomRect; //���� �� ���� �簢�� ����
    public int isCreated; //������ �����Ǿ����� + �������������� ���� �Ǻ���
    public RoomType roomType; //���� ����
    public GameObject enemyGenerator; //���� ���ʹ� ���ʷ����� ����

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


