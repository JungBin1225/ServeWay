using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TileSprite
{
    public Tile floor;

    public Tile road_Center;
    public Tile road_Right;
    public Tile road_Left;
    public Tile road_Top;
    public Tile road_Bottom;

    public Tile wall_Right;
    public Tile wall_Left;
    public Tile wall_Top;
    public Tile wall_Bottom;

    public Tile wall_Top_Right;
    public Tile wall_Top_Left;
    public Tile wall_Bottom_Right;
    public Tile wall_Bottom_Left;
}
