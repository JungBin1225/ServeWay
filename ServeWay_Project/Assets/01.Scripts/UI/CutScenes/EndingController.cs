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
    private float clickCoolTime;

    void Start()
    {
        clickCoolTime = 0;

        signalTimeList = new List<double>();

        TimelineAsset timeline = (TimelineAsset)director.playableAsset;
        Debug.Log(timeline.GetRootTrack(0).GetMarkerCount());

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
        if (clickCoolTime > 0)
        {
            clickCoolTime -= Time.deltaTime;
        }
        else
        {
            clickCoolTime = 0;
        }

        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && (!fade_black.activeSelf && !fade_white.activeSelf) && clickCoolTime <= 0)
        {
            foreach (double signalTime in signalTimeList)
            {
                if (director.time < signalTime && signalTime - director.time > 0.2f)
                {
                    director.time = signalTime - 0.2f;
                    break;
                }
            }

            clickCoolTime = 0.35f;
        }
    }

    public void skip()
    {

    }
}
