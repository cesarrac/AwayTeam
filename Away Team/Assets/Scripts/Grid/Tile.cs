using UnityEngine;
using System.Collections;

public enum TileType
{
    FLOOR,
    EMPTY
}

public class Tile {

    public int posX { get; protected set; }
    public int posY { get; protected set; }
    
    public TileType tileType { get; protected set; }
    
    public bool isWalkable { get; protected set; }

    public int moveCost { get; protected set; }
    
    public Tile(int x, int y, TileType t, int moveCost)
    {
        posX = x;
        posY = y;
        tileType = t;

        if (moveCost > 0)
        {
            isWalkable = true;
        }
        else
        {
            isWalkable = false;
        }
    } 
}
