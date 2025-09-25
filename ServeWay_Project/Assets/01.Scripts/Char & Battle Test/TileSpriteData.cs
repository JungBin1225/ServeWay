using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Tile Sprite Data", menuName = "Scriptable Object/Tile Sprite Data", order = int.MaxValue - 3)]
public class TileSpriteData : ScriptableObject
{
    public TileSprite camping_Map;
    public TileSprite school_Map;
    public TileSprite bar_Map;
    public TileSprite cafe_Map;
    public TileSprite normal_Map;
    public TileSprite restorant_Map;
    public TileSprite street_Map;

    public Tile outTile;
    public Tile kitchenTile;

    public TileSprite GetNowStageSprite(Stage_Theme theme)
    {
        switch(theme)
        {
            case Stage_Theme.CAMPING:
                return camping_Map;

            case Stage_Theme.BAR:
                return bar_Map; //map tile

            case Stage_Theme.CAFE:
                return cafe_Map;

            case Stage_Theme.NORMAL:
                return normal_Map;

            case Stage_Theme.RESTORANT:
                return restorant_Map;

            case Stage_Theme.SCHOOL:
                return school_Map;

            case Stage_Theme.STREET:
                return street_Map;

            default:
                return camping_Map;
        }
    }
}
