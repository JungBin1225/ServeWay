using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;
using TMPro;

public class OpeningSignal: MonoBehaviour
{
    [SerializeField] private TalkManager talkManager;
    [SerializeField] private int PlayerTalkIndex;
    private int AnchorTalkIndex;
    private int MomTalkIndex;
    private int DiningFriendTalkIndex;
    private int DadTalkIndex;
    private int AlienMomTalkIndex;
    private int AlienSonTalkIndex;
    private int MasterTalkIndex;

    private float clickCoolTime;

    private PlayableDirector director;
    private List<double> signalTimeList;

    private int OpeningATTalkIndex;

    public TextMeshProUGUI[] PlayerText;
    public TextMeshProUGUI AnchorText;
    public TextMeshProUGUI[] MomText;
    public TextMeshProUGUI[] DiningFriendsText;
    public TextMeshProUGUI DadText;
    public TextMeshProUGUI AlienMomText;
    public TextMeshProUGUI AlienSonText;
    public TextMeshProUGUI MasterText;

    public TextMeshProUGUI ATPlayerText;
    public TextMeshProUGUI ATMasterText;

    public GameObject Fade_black;
    public GameObject Fade_white;

    void Start()
    {
        PlayerTalkIndex = 0;
        AnchorTalkIndex = 0;
        MomTalkIndex = 0;
        DiningFriendTalkIndex = 0;
        DadTalkIndex = 0;
        AlienMomTalkIndex = 0;
        AlienSonTalkIndex = 0;
        MasterTalkIndex = 0;

        OpeningATTalkIndex = 0;

        clickCoolTime = 0;

        signalTimeList = new List<double>();

        director = GetComponent<PlayableDirector>();
        TimelineAsset timeline = (TimelineAsset)director.playableAsset;
        Debug.Log(timeline.GetRootTrack(1).GetMarkerCount());

        TrackAsset track = timeline.GetRootTrack(1);
        for (int i = 0; i < track.GetMarkerCount(); i++)
        {
            IMarker marker = track.GetMarker(i);
            signalTimeList.Add(marker.time);
        }

        signalTimeList.Sort();
    }

    private void Update()
    {
        if(clickCoolTime > 0)
        {
            clickCoolTime -= Time.deltaTime;
        }
        else
        {
            clickCoolTime = 0;
        }

        if(Input.GetMouseButtonDown(0) && (!Fade_black.activeSelf && !Fade_white.activeSelf) && clickCoolTime <= 0)
        {
            foreach(double signalTime in signalTimeList)
            {
                if(director.time < signalTime && signalTime - director.time > 0.2f)
                {
                    director.time = signalTime - 0.2f;
                    break;
                }
            }

            clickCoolTime = 0.5f;
        }
    }

    /* run when this received Timeline Signal */

    // Player: 0
    public void PlayerTalkSingal()
    {
        int id = 0;
        string talkData = talkManager.GetTalk(id, PlayerTalkIndex);

        if (talkData == null)
        {
            return;
        }

        if (PlayerTalkIndex < 2)
            PlayerText[0].text = talkData;
        else if (PlayerTalkIndex < 6)
            PlayerText[1].text = talkData;
        else if (PlayerTalkIndex < 7)
            PlayerText[2].text = talkData;
        else if (PlayerTalkIndex < 11)
            PlayerText[3].text = talkData;

        PlayerTalkIndex++;
    }

    // Anchor: 1000
    public void AnchorTalkSingal()
    {
        int id = 1000;
        string talkData = talkManager.GetTalk(id, AnchorTalkIndex);

        if (talkData == null)
        {
            return;
        }

        AnchorText.text = talkData;

        AnchorTalkIndex++;
    }

    // Mom: 2000
    public void MomTalkSingal()
    {
        int id = 2000;
        string talkData = talkManager.GetTalk(id, MomTalkIndex);

        if (talkData == null)
        {
            return;
        }

        if (MomTalkIndex < 2)
            MomText[0].text = talkData;
        else if (MomTalkIndex < 4)
            MomText[1].text = talkData;
        else if (MomTalkIndex < 6)
            MomText[2].text = talkData;

        MomTalkIndex++;
    }

    // Friends: 3000
    public void DiningFriendsTalkSignal()
    {
        int id = 3000;
        string talkData = talkManager.GetTalk(id, DiningFriendTalkIndex);

        if (talkData == null)
        {
            return;
        }

        if (DiningFriendTalkIndex == 0)
        {
            DiningFriendsText[0].text = talkData;
        } else if (DiningFriendTalkIndex == 1)
        {
            DiningFriendsText[1].text = talkData;
        }

        DiningFriendTalkIndex++;
    }

    // Dad: 4000
    public void DadTalkSingal()
    {
        int id = 4000;
        string talkData = talkManager.GetTalk(id, DadTalkIndex);

        if (talkData == null)
        {
            return;
        }

        DadText.text = talkData;

        DadTalkIndex++;
    }

    // AlienMom: 5001
    public void AlienMomTalkSingal()
    {
        int id = 5001;
        string talkData = talkManager.GetTalk(id, AlienMomTalkIndex);

        if (talkData == null)
        {
            return;
        }

        AlienMomText.text = talkData;

        AlienMomTalkIndex++;
    }

    // AlienSon: 5002
    public void AlienSonTalkSingal()
    {
        int id = 5002;
        string talkData = talkManager.GetTalk(id, AlienSonTalkIndex);

        if (talkData == null)
        {
            return;
        }

        AlienSonText.text = talkData;

        AlienSonTalkIndex++;
    }

    public void MasterTalkSingal()
    {
        int id = 6000;
        string talkData = talkManager.GetTalk(id, MasterTalkIndex);

        if (talkData == null)
        {
            return;
        }

        MasterText.text = talkData;

        MasterTalkIndex++;
    }

    // Opening - After Tutorial: 6000
    public void OpeningATTalkSingal()
    {
        int id = 7000;
        string talkData = talkManager.GetTalk(id, OpeningATTalkIndex);

        if (talkData == null)
        {
            return;
        }

        if (OpeningATTalkIndex < 1)
            ATPlayerText.text = talkData;
        else if (OpeningATTalkIndex < 2)
            ATMasterText.text = talkData;

        OpeningATTalkIndex++;
    }

    public void StartTutorial()
    {
        GameManager.gameManager.SetNextStage("Tutorial");
        SceneManager.LoadScene("Loading");
    }
}
