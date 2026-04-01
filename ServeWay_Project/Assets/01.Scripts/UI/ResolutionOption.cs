using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResolutionOption : MonoBehaviour
{
    [SerializeField] private List<Vector2> resolutions;
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private List<Toggle> toggles;

    private int resolutionIndex;
    private int screenMode;

    void Start()
    {
        screenMode = (int)Screen.fullScreenMode;
        toggles[screenMode].isOn = true;
    }

    private void OnEnable()
    {
        dropdown.value = GetNowResolution();
        resolutionIndex = dropdown.value;

        screenMode = (int)Screen.fullScreenMode;
        toggles[screenMode].isOn = true;
    }

    void Update()
    {
        
    }

    private int GetNowResolution()
    {
        for(int i = 0; i < resolutions.Count; i++)
        {
            if(Screen.width >= resolutions[i].x)
            {
                return i;
            }
        }

        return 2;
    }

    public void OnOptionChanged(int x)
    {
        resolutionIndex = x;
        if(resolutions[resolutionIndex].x != 960 || screenMode != 0)
        {
            Screen.SetResolution((int)resolutions[resolutionIndex].x, (int)resolutions[resolutionIndex].y, (FullScreenMode)screenMode);
        }
    }

    public void FullScreen_0(bool isOn)
    {
        if(isOn && screenMode != 0)
        {
            screenMode = 0;
            if(resolutions[resolutionIndex].x != 960)
            {
                Screen.SetResolution((int)resolutions[resolutionIndex].x, (int)resolutions[resolutionIndex].y, (FullScreenMode)screenMode);
            }
        }
    }
    public void FullScreen_1(bool isOn)
    {
        if (isOn && screenMode != 1)
        {
            screenMode = 1;
            Screen.SetResolution((int)resolutions[resolutionIndex].x, (int)resolutions[resolutionIndex].y, (FullScreenMode)screenMode);
        }
    }

    public void FullScreen_3(bool isOn)
    {
        if (isOn && screenMode != 3)
        {
            screenMode = 3;
            Screen.SetResolution((int)resolutions[resolutionIndex].x, (int)resolutions[resolutionIndex].y, (FullScreenMode)screenMode);
        }
    }
}
