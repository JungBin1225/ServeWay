using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tile Sprite Data", menuName = "Scriptable Object/Tile Sprite Data", order = int.MaxValue - 3)]
public class TileSpriteData : ScriptableObject
{
    public TileSprite camping_Map;

    public TileSprite GetNowStageSprite(Stage_Theme theme)
    {
        switch(theme)
        {
            case Stage_Theme.CAMPING:
                return camping_Map;

            case Stage_Theme.BAR:
                return camping_Map; //map tile

            case Stage_Theme.CAFE:
                return camping_Map;

            case Stage_Theme.NORMAL:
                return camping_Map;

            case Stage_Theme.RESTORANT:
                return camping_Map;

            case Stage_Theme.SCHOOL:
                return camping_Map;

            case Stage_Theme.STREET:
                return camping_Map;

            default:
                return camping_Map;
        }
    }
}
