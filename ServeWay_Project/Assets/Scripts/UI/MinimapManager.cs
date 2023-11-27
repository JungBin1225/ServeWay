using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 구현 끝나면 세이브파일 연동 작업도 해줘야 함 */

public class MinimapManager : MonoBehaviour
{
    public static MinimapManager minimapMG;

    const int NUM_ROOM = 5;
    private GameObject[,] miniRoomMeshList = new GameObject[NUM_ROOM, NUM_ROOM];

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
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void putMesh(GameObject tmp, int row, int col)
    {
        miniRoomMeshList[row, col] = tmp;
        tmp.transform.SetParent(GameObject.Find("minimapGroup").transform);
    }
}
