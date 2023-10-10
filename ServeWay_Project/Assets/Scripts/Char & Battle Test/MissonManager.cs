using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissonManager : MonoBehaviour
{
    const int NUM_MISSON = 2;

    public delegate void missionDelegate(int missonID, float increase);
    public event missionDelegate missonEvent;
    public List<string> missonName;
    public List<GameObject> missonUI;
    public int clearAmount;

    private List<float> targetAmount;
    private List<float> nowAmount;
    private Dictionary<int, GameObject> matchedUI;
    void Start()
    {
        clearAmount = 0;
        targetAmount = new List<float>();
        nowAmount = new List<float>();
        matchedUI = new Dictionary<int, GameObject>();
        for (int i = 0; i < missonName.Count; i++)
        {
            targetAmount.Add(0);
            nowAmount.Add(0);
        }

        for (int i = 0; i < missonName.Count; i++)
        {
            missonName[i] = missonName[i].Replace("\\n", "\n");
        }

        SetMisson(NUM_MISSON);
    }


    void Update()
    {

    }

    public bool isClear()
    {
        return (clearAmount == NUM_MISSON);
    }

    public void OccurreEvent(int missonID, float increase)
    {
        missonEvent(missonID, increase);
    }

    public void SetMisson(int num)
    {
        //보스 데미지는 무조건 들어감

        for (int i = 0; i < num; i++)
        {
            RandomMisson(i);
        }
    }

    public void RandomMisson(int textIndex)
    {
        int index = Random.Range(0, missonName.Count);

        if (targetAmount[index] != 0)
        {
            RandomMisson(textIndex);
            return;
        }
        switch (index)
        {
            case 0:
                missonEvent += BossDamage;
                targetAmount[index] = Random.Range(20, 50);

                matchedUI.Add(index, missonUI[textIndex]);
                missonUI[textIndex].GetComponent<TMP_Text>().text = string.Format(missonName[index], targetAmount[index].ToString(), nowAmount[index].ToString());
                break;
            case 1:
                missonEvent += DashInTime;
                targetAmount[index] = Random.Range(2, 5);

                matchedUI.Add(index, missonUI[textIndex]);
                missonUI[textIndex].GetComponent<TMP_Text>().text = string.Format(missonName[index], targetAmount[index].ToString(), nowAmount[index].ToString());
                break;
            case 2:
                missonEvent += NoHitInTime;
                targetAmount[index] = Random.Range(20, 60);

                matchedUI.Add(index, missonUI[textIndex]);
                missonUI[textIndex].GetComponent<TMP_Text>().text = string.Format(missonName[index], targetAmount[index].ToString(), nowAmount[index].ToString("F1"));
                break;
            case 3:
                missonEvent += DamageNoDash;
                targetAmount[index] = Random.Range(20, 50);

                matchedUI.Add(index, missonUI[textIndex]);
                missonUI[textIndex].GetComponent<TMP_Text>().text = string.Format(missonName[index], targetAmount[index].ToString(), nowAmount[index].ToString());
                break;
            case 4:
                break;
        }
    }

    public void BossDamage(int missonID, float increase)
    {
        if (missonID == 0)
        {
            bool isClear = (nowAmount[missonID] >= targetAmount[missonID]);

            if (!isClear)
            {
                nowAmount[missonID] += increase;
            }

            if (isClear != (nowAmount[missonID] >= targetAmount[missonID]))
            {
                matchedUI[missonID].GetComponent<TMP_Text>().color = new Color(0, 1, 0);
                matchedUI[missonID].transform.GetChild(1).gameObject.SetActive(true);
                clearAmount++;
                //완료했으면 UI에 완료한 표시 if success, show in UI
            }

            matchedUI[missonID].GetComponent<TMP_Text>().text = string.Format(missonName[missonID], targetAmount[missonID].ToString(), nowAmount[missonID].ToString());
            //UI에 수치 갱신 reload num in UI
        }
    }

    public void DashInTime(int missonID, float increase)
    {
        if (missonID == 1)
        {
            bool isClear = (nowAmount[missonID] >= targetAmount[missonID]);

            if (increase == 0 && !isClear)
            {
                if (nowAmount[missonID] != 0)
                {
                    nowAmount[missonID] = 0;
                    Debug.Log("asd");
                }
            }
            else if (!isClear)
            {
                nowAmount[missonID] += increase;
            }

            if (isClear != (nowAmount[missonID] >= targetAmount[missonID]))
            {
                matchedUI[missonID].GetComponent<TMP_Text>().color = new Color(0, 1, 0);
                matchedUI[missonID].transform.GetChild(1).gameObject.SetActive(true);
                clearAmount++;
                //완료했으면 UI에 완료한 표시 if success, show in UI
            }

            matchedUI[missonID].GetComponent<TMP_Text>().text = string.Format(missonName[missonID], targetAmount[missonID].ToString(), nowAmount[missonID].ToString());
            //UI에 수치 갱신 reload num in UI
        }
    }

    public void NoHitInTime(int missonID, float increase)
    {
        if (missonID == 2)
        {
            bool isClear = (nowAmount[missonID] >= targetAmount[missonID]);

            if (increase == 0 && !isClear)
            {
                nowAmount[missonID] = 0;
            }
            else if (!isClear)
            {
                nowAmount[missonID] += increase;
            }

            if (isClear != (nowAmount[missonID] >= targetAmount[missonID]))
            {
                matchedUI[missonID].GetComponent<TMP_Text>().color = new Color(0, 1, 0);
                matchedUI[missonID].transform.GetChild(1).gameObject.SetActive(true);
                clearAmount++;
                //완료했으면 UI에 완료한 표시 if success, show in UI
            }

            matchedUI[missonID].GetComponent<TMP_Text>().text = string.Format(missonName[missonID], targetAmount[missonID].ToString(), nowAmount[missonID].ToString("F1"));
            //UI에 수치 갱신 reload num in UI
        }
    }

    public void DamageNoDash(int missonID, float increase)
    {
        if (missonID == 3)
        {
            bool isClear = (nowAmount[missonID] >= targetAmount[missonID]);

            if (increase == 0 && !isClear)
            {
                nowAmount[missonID] = 0;
            }
            else if (!isClear)
            {
                nowAmount[missonID] += increase;
            }

            if (isClear != (nowAmount[missonID] >= targetAmount[missonID]))
            {
                matchedUI[missonID].GetComponent<TMP_Text>().color = new Color(0, 1, 0);
                matchedUI[missonID].transform.GetChild(1).gameObject.SetActive(true);
                clearAmount++;
                //완료했으면 UI에 완료한 표시 if success, show in UI
            }

            matchedUI[missonID].GetComponent<TMP_Text>().text = string.Format(missonName[missonID], targetAmount[missonID].ToString(), nowAmount[missonID].ToString());
            //UI에 수치 갱신 reload num in UI
        }
    }
}
