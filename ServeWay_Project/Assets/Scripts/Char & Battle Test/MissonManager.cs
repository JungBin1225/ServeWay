using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissonManager : MonoBehaviour
{
    public delegate void missionDelegate(int missonID, float increase);
    public event missionDelegate missonEvent;

    public float bossDamage;

    void Start()
    {
        missonEvent += BossDamage;
        bossDamage = 0;
    }

    
    void Update()
    {
        
    }

    public void OccurreEvent(int missonID, float increase)
    {
        missonEvent(missonID, increase);
    }

    public void BossDamage(int missonID, float increase)
    {
        if(missonID == 1)
        {
            int targetDamage = 30;
            if(bossDamage < targetDamage)
            {
                bossDamage += increase;
            }
            else
            {
                Debug.Log("Misson Success!");
                //�Ϸ������� UI�� �Ϸ��� ǥ�� if success, show in UI
            }

            //UI�� ��ġ ���� reload num in UI
        }
    }
}
