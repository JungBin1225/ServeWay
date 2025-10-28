using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound Option Save", menuName = "Scriptable Object/Sound Option Save", order = int.MinValue + 4)]
public class SoundOptionSave : ScriptableObject
{
    public bool bgmMute;
    public float bgmSound;
    public bool sfxMute;
    public float sfxSound;
}
