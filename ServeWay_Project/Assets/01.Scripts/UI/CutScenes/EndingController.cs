using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;
using TMPro;

public class EndingController : MonoBehaviour
{
    public PlayableDirector director;
    public GameObject fade_black;
    public GameObject fade_white;

    private List<double> signalTimeList;
    private bool clickAble;

    void Start()
    {
        signalTimeList = new List<double>();
        clickAble = false;

        TimelineAsset timeline = (TimelineAsset)director.playableAsset;

        TrackAsset track = timeline.GetRootTrack(0);
        for (int i = 0; i < track.GetMarkerCount(); i++)
        {
            IMarker marker = track.GetMarker(i);
            signalTimeList.Add(marker.time);
        }

        signalTimeList.Sort();
    }

    void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && (!fade_black.activeSelf && !fade_white.activeSelf))
        {
            for (int i = 0; i < signalTimeList.Count; i++)
            {
                if(i == 0)
                {
                    if ((director.time < signalTimeList[i]) && (director.time > 3.75f && signalTimeList[i] - director.time > 0.75f))
                    {
                        director.time = signalTimeList[i] - 0.75f;
                        director.Evaluate();
                        break;
                    }
                }
                else
                {
                    if ((director.time < signalTimeList[i]) && (director.time - signalTimeList[i - 1] > 0.75f && signalTimeList[i] - director.time > 0.75f))
                    {
                        director.time = signalTimeList[i] - 0.75f;
                        director.Evaluate();
                        break;
                    }
                }
            }
        }

        for (int i = 0; i < signalTimeList.Count; i++)
        {
            if (i == 0)
            {
                if ((director.time < signalTimeList[i]) && (director.time > 3.75f && signalTimeList[i] - director.time > 0.75f))
                {
                    clickAble = true;
                    break;
                }
                else if((director.time < signalTimeList[i]) && (director.time <= 3.75f || signalTimeList[i] - director.time <= 0.75f))
                {
                    clickAble = false;
                    break;
                }
            }
            else
            {
                if ((director.time < signalTimeList[i]) && (director.time - signalTimeList[i - 1] > 0.75f && signalTimeList[i] - director.time > 0.75f))
                {
                    clickAble = true;
                    break;
                }
                else if((director.time < signalTimeList[i]) && (director.time - signalTimeList[i - 1] <= 0.75f || signalTimeList[i] - director.time <= 0.75f))
                {
                    clickAble = false;
                    break;
                }
            }
        }
    }

    public void GoToTitle()
    {
        GameManager.gameManager.charData.EndingClear();
        GameManager.gameManager.charData.DeleteAllWithoutTutoSound();

        GameManager.gameManager.SetNextStage("TitleScene");
        SceneManager.LoadScene("Loading");
    }

    public void skip()
    {

    }

    public bool GetClickAble()
    {
        return clickAble;
    }
}
