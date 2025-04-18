using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MissionManager : MonoBehaviour
{
    const int NUM_MISSION = 3;

    public delegate void missionDelegate(int missionID, float increase);
    public event missionDelegate missionEvent;
    public List<string> missionName;
    public GameObject missionUI;
    public List<GameObject> missionText;
    public int clearAmount;

    private string damagedFoodName;
    private List<float> targetAmount;
    private List<float> nowAmount;
    private Dictionary<int, GameObject> matchedUI;
    private Dictionary<string, float> threeFoodScore;

    void Start()
    {
        clearAmount = 0;
        damagedFoodName = "";
        targetAmount = new List<float>();
        nowAmount = new List<float>();
        matchedUI = new Dictionary<int, GameObject>();
        threeFoodScore = new Dictionary<string, float>();
        for (int i = 0; i < missionName.Count; i++)
        {
            targetAmount.Add(0);
            nowAmount.Add(0);
        }

        for (int i = 0; i < missionName.Count; i++)
        {
            missionName[i] = missionName[i].Replace("\\n", "\n");
        }

        MissionDisappear();
        SetMission(NUM_MISSION);
    }


    void Update()
    {

    }

    public bool isClear()
    {
        return (clearAmount == NUM_MISSION);
    }

    public void OccurreEvent(int missionID, float increase)
    {
        missionEvent(missionID, increase);
    }

    public void OccurreEvent(int missionID, float increase, string foodName)
    {
        damagedFoodName = foodName;
        missionEvent(missionID, increase);
    }

    public void SetMission(int num)
    {
        List<int> missionList = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        List<int> missionIndex = new List<int>();
        for(int i = 0; i < num; i++)
        {
            int randomIndex = Random.Range(0, missionList.Count);
            if(i == 0)
            {
                randomIndex = 0;
            }

            missionIndex.Add(missionList[randomIndex]);
            missionList.RemoveAt(randomIndex);
        }

        //보스 데미지는 무조건 들어감

        for (int i = 0; i < num; i++)
        {
            RandomMission(i, missionIndex);
        }
    }

    public void RandomMission(int textIndex, List<int> missionIndex)
    {
        int index = missionIndex[textIndex];
        /*if (textIndex == 1) //test
        {
            index = 9;
        }*/

        if (index == 9)
        {
            switch(GameManager.gameManager.bossJobList[GameManager.gameManager.stage - 1])
            {
                case Boss_Job.JOURNAL:
                    index = 9;
                    break;
                case Boss_Job.COOKRESEARCH:
                    index = 10;
                    break;
                case Boss_Job.CRITIC:
                    index = 11;
                    break;
                case Boss_Job.BLOGGER:
                    index = 12;
                    break;
                case Boss_Job.YOUTUBER:
                    index = 13;
                    break;
                case Boss_Job.TEACHER:
                    index = 14;
                    break;
            }
        }

        if (SceneManager.GetActiveScene().name.Contains("Tutorial"))
        {
            index = textIndex;
        }

        switch (index)
        {
            case 0:
                missionEvent += BossDamage;
                targetAmount[index] = Random.Range(20, 50);

                matchedUI.Add(index, missionText[textIndex]);
                Food_Nation nation;
                if(SceneManager.GetActiveScene().name.Contains("Tutorial"))
                {
                    nation = Food_Nation.KOREA;
                }
                else
                {
                    nation = GameManager.gameManager.bossNations[GameManager.gameManager.stage - 1];
                }

                FoodData food = new FoodData();
                missionText[textIndex].GetComponent<TMP_Text>().text = string.Format(missionName[index], targetAmount[index].ToString(), nowAmount[index].ToString("F0"), food.EunmToString(nation));
                break;
            case 1:
                missionEvent += DashInTime;
                targetAmount[index] = Random.Range(2, 5);

                matchedUI.Add(index, missionText[textIndex]);
                missionText[textIndex].GetComponent<TMP_Text>().text = string.Format(missionName[index], targetAmount[index].ToString(), nowAmount[index].ToString());
                break;
            case 2:
                missionEvent += NoHitInTime;
                targetAmount[index] = Random.Range(20, 60);

                matchedUI.Add(index, missionText[textIndex]);
                missionText[textIndex].GetComponent<TMP_Text>().text = string.Format(missionName[index], targetAmount[index].ToString(), nowAmount[index].ToString("F1"));
                break;
            case 3:
                missionEvent += DamageNoDash;
                targetAmount[index] = Random.Range(20, 50);

                matchedUI.Add(index, missionText[textIndex]);
                missionText[textIndex].GetComponent<TMP_Text>().text = string.Format(missionName[index], targetAmount[index].ToString(), nowAmount[index].ToString("F0"));
                break;
            case 4:
                missionEvent += DashAvoid;
                targetAmount[index] = Random.Range(5, 8);

                matchedUI.Add(index, missionText[textIndex]);
                missionText[textIndex].GetComponent<TMP_Text>().text = string.Format(missionName[index], targetAmount[index].ToString(), nowAmount[index].ToString());
                break;
            case 5:
                missionEvent += NotNationDamage;
                targetAmount[index] = Random.Range(20, 50);

                matchedUI.Add(index, missionText[textIndex]);
                Food_Nation nation_2 = GameManager.gameManager.bossNations[GameManager.gameManager.stage - 1];
                FoodData food_2 = new FoodData();
                missionText[textIndex].GetComponent<TMP_Text>().text = string.Format(missionName[index], targetAmount[index].ToString(), nowAmount[index].ToString("F0"), food_2.EunmToString(nation_2));
                break;
            case 6:
                missionEvent += ThreeFood;
                targetAmount[index] = Random.Range(20, 50);

                matchedUI.Add(index, missionText[textIndex]);
                missionText[textIndex].GetComponent<TMP_Text>().text = string.Format(missionName[index], "3", threeFoodScore.Count, targetAmount[index].ToString());
                break;
            case 7:
                missionEvent += DontStop;
                targetAmount[index] = Random.Range(10, 16);

                matchedUI.Add(index, missionText[textIndex]);
                missionText[textIndex].GetComponent<TMP_Text>().text = string.Format(missionName[index], targetAmount[index].ToString(), nowAmount[index].ToString("F1"));
                break;
            case 8:
                missionEvent += ChargeToBoss;
                targetAmount[index] = Random.Range(3, 5);

                matchedUI.Add(index, missionText[textIndex]);
                missionText[textIndex].GetComponent<TMP_Text>().text = string.Format(missionName[index], targetAmount[index].ToString(), nowAmount[index].ToString());
                break;
            case 9:
                missionEvent += JournalBossMission;
                targetAmount[index] = 3;

                matchedUI.Add(index, missionText[textIndex]);
                missionText[textIndex].GetComponent<TMP_Text>().color = new Color(1, 0, 0);
                missionText[textIndex].GetComponent<TMP_Text>().text = string.Format(missionName[index], targetAmount[index].ToString(), nowAmount[index].ToString());
                break;
            case 10:
                missionEvent += ResearchBossMission;
                targetAmount[index] = 1;

                matchedUI.Add(index, missionText[textIndex]);
                missionText[textIndex].GetComponent<TMP_Text>().color = new Color(1, 0, 0);
                missionText[textIndex].GetComponent<TMP_Text>().text = string.Format(missionName[index], targetAmount[index].ToString(), nowAmount[index].ToString());
                break;
            case 11:
                missionEvent += CriticBossMission;
                targetAmount[index] = 5;

                matchedUI.Add(index, missionText[textIndex]);
                missionText[textIndex].GetComponent<TMP_Text>().color = new Color(1, 0, 0);
                missionText[textIndex].GetComponent<TMP_Text>().text = string.Format(missionName[index], targetAmount[index].ToString(), nowAmount[index].ToString());
                break;
            case 12:
                missionEvent += BloggerBossMission;
                targetAmount[index] = 2;

                matchedUI.Add(index, missionText[textIndex]);
                missionText[textIndex].GetComponent<TMP_Text>().color = new Color(1, 0, 0);
                missionText[textIndex].GetComponent<TMP_Text>().text = string.Format(missionName[index], targetAmount[index].ToString(), nowAmount[index].ToString());
                break;
            case 13:
                missionEvent += YoutuberBossMission;
                targetAmount[index] = Random.Range(20, 50);

                matchedUI.Add(index, missionText[textIndex]);
                missionText[textIndex].GetComponent<TMP_Text>().color = new Color(1, 0, 0);
                missionText[textIndex].GetComponent<TMP_Text>().text = string.Format(missionName[index], targetAmount[index].ToString(), nowAmount[index].ToString("F0"));
                break;
            case 14:
                missionEvent += TeacherBossMission;
                targetAmount[index] = 2;

                matchedUI.Add(index, missionText[textIndex]);
                missionText[textIndex].GetComponent<TMP_Text>().color = new Color(1, 0, 0);
                missionText[textIndex].GetComponent<TMP_Text>().text = string.Format(missionName[index], targetAmount[index].ToString(), nowAmount[index].ToString());
                break;
        }
    }

    public void BossDamage(int missionID, float increase)
    {
        if (missionID == 0)
        {
            bool isClear = (nowAmount[missionID] >= targetAmount[missionID]);

            if (!isClear)
            {
                nowAmount[missionID] += increase;
            }

            if (isClear != (nowAmount[missionID] >= targetAmount[missionID]))
            {
                matchedUI[missionID].GetComponent<TMP_Text>().color = new Color(0, 1, 0);
                matchedUI[missionID].transform.GetChild(1).gameObject.SetActive(true);
                clearAmount++;
                //완료했으면 UI에 완료한 표시 if success, show in UI
            }

            Food_Nation nation = GameManager.gameManager.bossNations[GameManager.gameManager.stage - 1];
            if (SceneManager.GetActiveScene().name.Contains("Tutorial"))
            {
                nation = Food_Nation.KOREA;
            }
            FoodData food = new FoodData();
            matchedUI[missionID].GetComponent<TMP_Text>().text = string.Format(missionName[missionID], targetAmount[missionID].ToString(), nowAmount[missionID].ToString("F0"), food.EunmToString(nation));
            //UI에 수치 갱신 reload num in UI
        }
    }

    public void DashInTime(int missionID, float increase)
    {
        if (missionID == 1)
        {
            bool isClear = (nowAmount[missionID] >= targetAmount[missionID]);

            if (increase == 0 && !isClear)
            {
                if (nowAmount[missionID] != 0)
                {
                    nowAmount[missionID] = 0;
                }
            }
            else if (!isClear)
            {
                nowAmount[missionID] += increase;
            }

            if (isClear != (nowAmount[missionID] >= targetAmount[missionID]))
            {
                matchedUI[missionID].GetComponent<TMP_Text>().color = new Color(0, 1, 0);
                matchedUI[missionID].transform.GetChild(1).gameObject.SetActive(true);
                clearAmount++;
                //완료했으면 UI에 완료한 표시 if success, show in UI
            }

            matchedUI[missionID].GetComponent<TMP_Text>().text = string.Format(missionName[missionID], targetAmount[missionID].ToString(), nowAmount[missionID].ToString());
            //UI에 수치 갱신 reload num in UI
        }
    }

    public void NoHitInTime(int missionID, float increase)
    {
        if (missionID == 2)
        {
            bool isClear = (nowAmount[missionID] >= targetAmount[missionID]);

            if (increase == 0 && !isClear)
            {
                nowAmount[missionID] = 0;
            }
            else if (!isClear)
            {
                nowAmount[missionID] += increase;
            }

            if (isClear != (nowAmount[missionID] >= targetAmount[missionID]))
            {
                matchedUI[missionID].GetComponent<TMP_Text>().color = new Color(0, 1, 0);
                matchedUI[missionID].transform.GetChild(1).gameObject.SetActive(true);
                clearAmount++;
                //완료했으면 UI에 완료한 표시 if success, show in UI
            }

            matchedUI[missionID].GetComponent<TMP_Text>().text = string.Format(missionName[missionID], targetAmount[missionID].ToString(), nowAmount[missionID].ToString("F1"));
            //UI에 수치 갱신 reload num in UI
        }
    }

    public void DamageNoDash(int missionID, float increase)
    {
        if (missionID == 3)
        {
            bool isClear = (nowAmount[missionID] >= targetAmount[missionID]);

            if (increase == 0 && !isClear)
            {
                nowAmount[missionID] = 0;
            }
            else if (!isClear)
            {
                nowAmount[missionID] += increase;
            }

            if (isClear != (nowAmount[missionID] >= targetAmount[missionID]))
            {
                matchedUI[missionID].GetComponent<TMP_Text>().color = new Color(0, 1, 0);
                matchedUI[missionID].transform.GetChild(1).gameObject.SetActive(true);
                clearAmount++;
                //완료했으면 UI에 완료한 표시 if success, show in UI
            }

            matchedUI[missionID].GetComponent<TMP_Text>().text = string.Format(missionName[missionID], targetAmount[missionID].ToString(), nowAmount[missionID].ToString("F0"));
            //UI에 수치 갱신 reload num in UI
        }
    }

    public void DashAvoid(int missionID, float increase)
    {
        if(missionID == 4)
        {
            bool isClear = (nowAmount[missionID] >= targetAmount[missionID]);

            if (!isClear)
            {
                nowAmount[missionID] += increase;
            }

            if (isClear != (nowAmount[missionID] >= targetAmount[missionID]))
            {
                matchedUI[missionID].GetComponent<TMP_Text>().color = new Color(0, 1, 0);
                matchedUI[missionID].transform.GetChild(1).gameObject.SetActive(true);
                clearAmount++;
                //완료했으면 UI에 완료한 표시 if success, show in UI
            }

            matchedUI[missionID].GetComponent<TMP_Text>().text = string.Format(missionName[missionID], targetAmount[missionID].ToString(), nowAmount[missionID].ToString());
            //UI에 수치 갱신 reload num in UI
        }
    }

    public void NotNationDamage(int missionID, float increase)
    {
        if (missionID == 5)
        {
            bool isClear = (nowAmount[missionID] >= targetAmount[missionID]);

            if (!isClear)
            {
                nowAmount[missionID] += increase;
            }

            if (isClear != (nowAmount[missionID] >= targetAmount[missionID]))
            {
                matchedUI[missionID].GetComponent<TMP_Text>().color = new Color(0, 1, 0);
                matchedUI[missionID].transform.GetChild(1).gameObject.SetActive(true);
                clearAmount++;
                //완료했으면 UI에 완료한 표시 if success, show in UI
            }

            Food_Nation nation = GameManager.gameManager.bossNations[GameManager.gameManager.stage - 1];
            FoodData food = new FoodData();
            matchedUI[missionID].GetComponent<TMP_Text>().text = string.Format(missionName[missionID], targetAmount[missionID].ToString(), nowAmount[missionID].ToString("F0"), food.EunmToString(nation));
            //UI에 수치 갱신 reload num in UI
        }
    }

    public void ThreeFood(int missionID, float increase)
    {
        if(missionID == 6)
        {
            bool isScore = true;
            foreach(float score in threeFoodScore.Values)
            {
                if(score < targetAmount[missionID])
                {
                    isScore = false;
                }
            }

            bool isClear = (threeFoodScore.Count == 3 && isScore);

            if(!isClear)
            {
                if(threeFoodScore.ContainsKey(damagedFoodName))
                {
                    threeFoodScore[damagedFoodName] += increase;
                    Debug.Log(damagedFoodName + " " +threeFoodScore[damagedFoodName]);
                }
                else
                {
                    threeFoodScore.Add(damagedFoodName, increase);
                    Debug.Log(damagedFoodName + " " + threeFoodScore[damagedFoodName]);
                }
            }

            float successScore = 0;
            isScore = true;
            foreach (float score in threeFoodScore.Values)
            {
                if (score < targetAmount[missionID])
                {
                    isScore = false;
                }
                else
                {
                    successScore++;
                }
            }
            if (isClear != (threeFoodScore.Count == 3 && isScore))
            {
                matchedUI[missionID].GetComponent<TMP_Text>().color = new Color(0, 1, 0);
                matchedUI[missionID].transform.GetChild(1).gameObject.SetActive(true);
                clearAmount++;
                //완료했으면 UI에 완료한 표시 if success, show in UI
            }

            matchedUI[missionID].GetComponent<TMP_Text>().text = string.Format(missionName[missionID], "3", successScore, targetAmount[missionID].ToString());
        }
    }

    public void DontStop(int missionID, float increase)
    {
        if (missionID == 7)
        {
            bool isClear = (nowAmount[missionID] >= targetAmount[missionID]);

            if (increase == 0 && !isClear)
            {
                nowAmount[missionID] = 0;
            }
            else if (!isClear)
            {
                nowAmount[missionID] += increase;
            }

            if (isClear != (nowAmount[missionID] >= targetAmount[missionID]))
            {
                matchedUI[missionID].GetComponent<TMP_Text>().color = new Color(0, 1, 0);
                matchedUI[missionID].transform.GetChild(1).gameObject.SetActive(true);
                clearAmount++;
                //완료했으면 UI에 완료한 표시 if success, show in UI
            }

            matchedUI[missionID].GetComponent<TMP_Text>().text = string.Format(missionName[missionID], targetAmount[missionID].ToString(), nowAmount[missionID].ToString("F1"));
            //UI에 수치 갱신 reload num in UI
        }
    }

    public void ChargeToBoss(int missionID, float increase)
    {
        if (missionID == 8)
        {
            bool isClear = (nowAmount[missionID] >= targetAmount[missionID]);

            if (!isClear)
            {
                nowAmount[missionID] += increase;
            }

            if (isClear != (nowAmount[missionID] >= targetAmount[missionID]))
            {
                matchedUI[missionID].GetComponent<TMP_Text>().color = new Color(0, 1, 0);
                matchedUI[missionID].transform.GetChild(1).gameObject.SetActive(true);
                clearAmount++;
                //완료했으면 UI에 완료한 표시 if success, show in UI
            }

            matchedUI[missionID].GetComponent<TMP_Text>().text = string.Format(missionName[missionID], targetAmount[missionID].ToString(), nowAmount[missionID].ToString());
            //UI에 수치 갱신 reload num in UI
        }
    }

    public void JournalBossMission(int missionID, float increase)
    {
        if (missionID == 9)
        {
            bool isClear = (nowAmount[missionID] >= targetAmount[missionID]);

            if (!isClear)
            {
                nowAmount[missionID] += increase;
            }

            if (isClear != (nowAmount[missionID] >= targetAmount[missionID]))
            {
                matchedUI[missionID].GetComponent<TMP_Text>().color = new Color(0, 1, 0);
                matchedUI[missionID].transform.GetChild(1).gameObject.SetActive(true);
                clearAmount++;
                //완료했으면 UI에 완료한 표시 if success, show in UI
            }

            matchedUI[missionID].GetComponent<TMP_Text>().text = string.Format(missionName[missionID], targetAmount[missionID].ToString(), nowAmount[missionID].ToString());
            //UI에 수치 갱신 reload num in UI
        }
    }

    public void ResearchBossMission(int missionID, float increase)
    {
        if (missionID == 10)
        {
            bool isClear = (nowAmount[missionID] >= targetAmount[missionID]);

            if (!isClear)
            {
                nowAmount[missionID] += increase;
            }

            if (isClear != (nowAmount[missionID] >= targetAmount[missionID]))
            {
                matchedUI[missionID].GetComponent<TMP_Text>().color = new Color(0, 1, 0);
                matchedUI[missionID].transform.GetChild(1).gameObject.SetActive(true);
                clearAmount++;
                //완료했으면 UI에 완료한 표시 if success, show in UI
            }

            matchedUI[missionID].GetComponent<TMP_Text>().text = string.Format(missionName[missionID], targetAmount[missionID].ToString(), nowAmount[missionID].ToString());
            //UI에 수치 갱신 reload num in UI
        }
    }

    public void CriticBossMission(int missionID, float increase)
    {
        if (missionID == 11)
        {
            bool isClear = (nowAmount[missionID] >= targetAmount[missionID]);

            if (!isClear)
            {
                nowAmount[missionID] += increase;
            }

            if (isClear != (nowAmount[missionID] >= targetAmount[missionID]))
            {
                matchedUI[missionID].GetComponent<TMP_Text>().color = new Color(0, 1, 0);
                matchedUI[missionID].transform.GetChild(1).gameObject.SetActive(true);
                clearAmount++;
                //완료했으면 UI에 완료한 표시 if success, show in UI
            }

            matchedUI[missionID].GetComponent<TMP_Text>().text = string.Format(missionName[missionID], targetAmount[missionID].ToString(), nowAmount[missionID].ToString());
            //UI에 수치 갱신 reload num in UI
        }
    }

    public void BloggerBossMission(int missionID, float increase)
    {
        if (missionID == 12)
        {
            bool isClear = (nowAmount[missionID] >= targetAmount[missionID]);

            if (!isClear)
            {
                nowAmount[missionID] += increase;
            }

            if (isClear != (nowAmount[missionID] >= targetAmount[missionID]))
            {
                matchedUI[missionID].GetComponent<TMP_Text>().color = new Color(0, 1, 0);
                matchedUI[missionID].transform.GetChild(1).gameObject.SetActive(true);
                clearAmount++;
                //완료했으면 UI에 완료한 표시 if success, show in UI
            }

            matchedUI[missionID].GetComponent<TMP_Text>().text = string.Format(missionName[missionID], targetAmount[missionID].ToString(), nowAmount[missionID].ToString());
            //UI에 수치 갱신 reload num in UI
        }
    }

    public void YoutuberBossMission(int missionID, float increase)
    {
        if (missionID == 13)
        {
            bool isClear = (nowAmount[missionID] >= targetAmount[missionID]);

            if (increase == 0 && !isClear)
            {
                if (nowAmount[missionID] != 0)
                {
                    nowAmount[missionID] = 0;
                }
            }
            else if (!isClear)
            {
                nowAmount[missionID] += increase;
            }

            if (isClear != (nowAmount[missionID] >= targetAmount[missionID]))
            {
                matchedUI[missionID].GetComponent<TMP_Text>().color = new Color(0, 1, 0);
                matchedUI[missionID].transform.GetChild(1).gameObject.SetActive(true);
                clearAmount++;
                //완료했으면 UI에 완료한 표시 if success, show in UI
            }

            matchedUI[missionID].GetComponent<TMP_Text>().text = string.Format(missionName[missionID], targetAmount[missionID].ToString(), nowAmount[missionID].ToString("F0"));
            //UI에 수치 갱신 reload num in UI
        }
    }

    public void TeacherBossMission(int missionID, float increase)
    {
        if (missionID == 14)
        {
            bool isClear = (nowAmount[missionID] >= targetAmount[missionID]);

            if (!isClear)
            {
                nowAmount[missionID] += increase;
            }

            if (isClear != (nowAmount[missionID] >= targetAmount[missionID]))
            {
                matchedUI[missionID].GetComponent<TMP_Text>().color = new Color(0, 1, 0);
                matchedUI[missionID].transform.GetChild(1).gameObject.SetActive(true);
                clearAmount++;
                //완료했으면 UI에 완료한 표시 if success, show in UI
            }

            matchedUI[missionID].GetComponent<TMP_Text>().text = string.Format(missionName[missionID], targetAmount[missionID].ToString(), nowAmount[missionID].ToString());
            //UI에 수치 갱신 reload num in UI
        }
    }

    public IEnumerator MissionAppear()
    {
        while(missionUI.GetComponent<RectTransform>().localPosition.x < -735)
        {
            missionUI.GetComponent<RectTransform>().localPosition += new Vector3(5, 0, 0);
            yield return null;
        }

        missionUI.GetComponent<RectTransform>().localPosition = new Vector3(-735, 200, 0);
    }

    public void MissionDisappear()
    {
        missionUI.GetComponent<RectTransform>().localPosition = new Vector3(-1300, 200, 0);
    }
}
