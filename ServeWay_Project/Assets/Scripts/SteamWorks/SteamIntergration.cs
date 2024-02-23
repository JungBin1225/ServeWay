using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamIntergration : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        try
        {
            // 내 프로젝트 - 스팀 간 연결을 초기화.
            Steamworks.SteamClient.Init(480);

            PrintYourName();
            PrintFriends();
        
        
        }
        catch(System.Exception e)
        {
            Debug.Log(e);
        }
    }

    private void PrintYourName()
    {
        Debug.Log(Steamworks.SteamClient.Name);
    }

    private void PrintFriends()
    {
        foreach(var friend in Steamworks.SteamFriends.GetFriends())
        {
            Debug.Log(friend.Name);
        }
    }

    private void Update()
    {
        Steamworks.SteamClient.RunCallbacks();
    }

    private void OnApplicationQuit()
    {
        Steamworks.SteamClient.Shutdown();
    }

    public void IsThisAchievementUnlocked(string id)
    {
        var ach = new Steamworks.Data.Achievement(id);

        Debug.LogFormat("Achievement {0} status : {1}", id, ach.State);
    }

    public void UnlockAchievement(string id)
    {
        var ach = new Steamworks.Data.Achievement(id);
        ach.Trigger();

        Debug.LogFormat("Achievement {0} unlocked", id);
    }

    public void ClearAchievementStatus(string id)
    {
        var ach = new Steamworks.Data.Achievement(id);
        ach.Clear();

        Debug.LogFormat("Achievement {0} cleared", id);
    }


}
