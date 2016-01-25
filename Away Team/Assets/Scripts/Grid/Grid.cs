using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    public static Grid Instance { get; protected set; }

    int height = 20, width = 20;
    public int Height { get { return height; } set { height = value; } }
    public int Width { get { return width; } set { width = value; } }

    Tile[,] grid;
    Node[,] path_grid;

    public GameObject emptyTile;

    GameObject[,] emptyTiles;

    public Transform tileHolder;


    void OnEnable()
    {
        Instance = this;
    }

    void Start()
    {
        InitGrid();
        InitPathFindingGrid();
    }

    void InitGrid()
    {
        grid = new Tile[width, height];
        emptyTiles = new GameObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y] = new Tile(x, y, TileType.EMPTY, 1);

                emptyTiles[x, y] = ObjectPool.instance.GetObjectForType("Empty Tile", true, new Vector3(x, y, 0));

                emptyTiles[x, y].transform.SetParent(tileHolder, true);
            }
        }

        Debug.Log("Created grid with " + grid.Length + " tiles!");
    }

    public void InitPathFindingGrid()
    {
        path_grid = new Node[width, height];
        //Vector3 worldBottomLeft = transform.position - Vector3.right * width / 2 - Vector3.up * height / 2;
        //Debug.Log("World Bottom left is " + worldBottomLeft);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //Vector3 worldPoint = worldBottomLeft + Vector3.right * x + Vector3.up * y;
                Vector3 worldPoint = new Vector3(x, y, 0);
               // Debug.Log("Node worldpoint " + worldPoint);
                bool walkable = grid[x, y].isWalkable;
                path_grid[x, y] = new Node(walkable, worldPoint, x, y, grid[x, y].moveCost);
            }
        }
    }

    public void OutputTileToDebug(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            Debug.Log("Tile x" + x + " y" + y + " is of type " + grid[x, y].tileType.ToString());
        }
    }

    public bool CheckMapBounds(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return true;
        }
        else
            return false;
    }

    public Tile GetTileAtCoord(int x, int y)
    {
        if (CheckMapBounds(x, y))
        {
            return grid[x, y];
        }
        else
        {
            Debug.LogError("GRID: The Tile coords you are passing to the grid are out of the grid's range! > ore < than width or height.");
            return null;
        }
    }


    // PATHFINDING:
    public Node NodeFromWorldPoint(Vector3 worldPos)
    {
        //Vector3 worldBottomLeft = transform.position - Vector3.right * width / 2 - Vector3.up * height / 2;
        //Vector3 worldPoint = worldBottomLeft + Vector3.right * worldPos.x + Vector3.up * worldPos.y;

        //int x = Mathf.RoundToInt(worldPoint.x);
        //int y = Mathf.RoundToInt(worldPoint.y);

        int x = Mathf.RoundToInt(worldPos.x);
        int y = Mathf.RoundToInt(worldPos.y);
        //if (x > width && y < height)
        //{
        //    x = width - 1;
        //}
        //else if (x < width && y < height)
        //{
        //    x = 0;
        //}
        //else if (x < width && y > height)
        //{
        //    y = height - 1;
        //}
        //else if (x < width && y < height)
        //{
        //    y = 0;
        //}
        //else if (x > width && y > height)
        //{
        //    x = width - 1;
        //    y = height - 1;
        //}
        //else if (x < width && y < height)
        //{
        //    x = 0;
        //    y = 0;
        //}

        Debug.Log("Node from world point x,y" + x + "," + y);


        return path_grid[x, y];
    }

    public int MaxSize
    {
        get
        {
            return width * height;
        }
    }

    public List<Node> GetNeighbors(Node node)
    {
        // Where is this node in the grid?
        List<Node> neighbors = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                // skip the center node
                if (x == 0 && y == 0)
                {
                    continue;
                }

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                // Check that this is within the grid
                if (checkX >= 0 && checkX < width && checkY >= 0 && checkY < height)
                {
                    neighbors.Add(path_grid[checkX, checkY]);
                }
            }
        }

        return neighbors;
    }


}
