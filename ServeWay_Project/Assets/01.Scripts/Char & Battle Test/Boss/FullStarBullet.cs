using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullStarBullet : MonoBehaviour
{
    private MissionManager mission;

    void Start()
    {
        mission = FindObjectOfType<MissionManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            mission.OccurreEvent(11, 1);
        }
    }
}
