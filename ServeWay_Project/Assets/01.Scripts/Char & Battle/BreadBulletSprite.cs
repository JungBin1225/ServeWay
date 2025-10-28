using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BreadSprite : SerializableDictionary<string, Sprite> { }

[CreateAssetMenu(fileName = "Bread Bullet Sprite", menuName = "Scriptable Object/Bread Bullet Sprite", order = int.MaxValue)]
public class BreadBulletSprite : ScriptableObject
{
    public BreadSprite breadBulletSprite;
}
