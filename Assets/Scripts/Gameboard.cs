using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Gameboard : MonoBehaviour
{

    // lets try tracking as bools ....[horizontal][vertical] (aka, [column][row], [x][y])
    public static bool[,] grid;
    public static int columns = 50;
    public static int rows = 25;

    public static int worldToGridRatio = 2;

    public static Vector2 ConvertGridToWorld(Vector2 worldVec) {
        return worldToGridRatio * worldVec;
    }

    public static Vector2 ConvertWorldToGrid(Vector2 gridVec) {
        return gridVec / worldToGridRatio;
    }
    
    public static bool ValidateByWorldPos(Vector2 worldPos)
    {
        if (!InBoundsByWorldPos(worldPos))
        {
            Debug.Log("[Error] - Validate request out of bounds");
        }
        var pos = ConvertWorldToGrid(worldPos);
        return Validate((int)pos.x, (int)pos.y);
    }
    public static bool Validate(int x, int y)
    { 
        // check if moving to this location is valid
        // within bounds?
        if (!InBounds(x, y))
        {
            return false;
        }
        

        // empty destination?
        if (grid[x, y]) {
            return false;
        }
        return true;

    }

    public static bool InBoundsByWorldPos(Vector2 worldPos)
    {
        var gridPos = ConvertWorldToGrid(worldPos);
        var x = (int)gridPos.x;
        var y = (int)gridPos.y;
        if (x < 0 || x > columns - 1) {
            return false;
        }
        if (y < 0 || y > rows - 1) {
            return false;
        }
        return true;
    }

    public static bool InBounds(int x, int y)
    {
        if (x < 0 || x > columns - 1) {
            return false;
        }
        if (y < 0 || y > rows - 1) {
            return false;
        }
        return true;
    }
    public static void OccupyByWorldPos(Vector2 worldPos)
    {
        if (!InBoundsByWorldPos(worldPos))
        {
            Debug.Log("[Error] - OccupyByWorldPos request out of bounds");
            return;
        }
        var pos = ConvertWorldToGrid(worldPos);
        Occupy((int)pos.x, (int)pos.y);
    }
    public static void Occupy(int x, int y)
    {
        if (!InBounds(x, y))
        {
            Debug.Log("[Error] - Occupy request out of bounds");
            return;
        }
        if (grid[x, y])
        {
            Debug.Log("[Error] - occupying an already occupied grid cell");
            return;
        }
        grid[x, y] = true;
    }

    public static void VacateByWorldPos(Vector2 worldPos)
    {
        if (!InBoundsByWorldPos(worldPos))
        {
            Debug.Log("[Error] - VacateByWorldPos request out of bounds");
            return;
        }
        var pos = ConvertWorldToGrid(worldPos);
        Vacate((int)pos.x, (int)pos.y);
    }
    public static void Vacate(int x, int y)
    {
        if (!InBounds(x, y))
        {
            Debug.Log("[Error] - Vacate request out of bounds");
            return;
        }
        if (!grid[x, y])
        {
            Debug.Log("[Error] - vacating an already vacated grid cell");
            return;
        }
        grid[x, y] = false;
    }

    void OnEnable()
    {
        grid = new bool[columns, rows];
        // TODO: place obstacles
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var p in players)
        {
            var convertedPosition = ConvertWorldToGrid(new Vector2(p.transform.position.x, p.transform.position.y));
            grid[(int)convertedPosition.x, (int)convertedPosition.y] = true;
        }
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var e in enemies)
        {
            var convertedPosition = ConvertWorldToGrid(new Vector2(e.transform.position.x, e.transform.position.y));
            grid[(int)convertedPosition.x, (int)convertedPosition.y] = true;
        }

    }
    void Start()
    {
        // Debug.Log("gameboard start");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
