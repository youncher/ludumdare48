using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameboard : MonoBehaviour
{

    // lets try tracking as bools ....[horizontal][vertical] (aka, [column][row], [x][y])
    public static bool[,] grid;
    public static int columns = 100;
    public static int rows = 50;

    public static bool Validate(int x, int y)
    { // check if moving to this location is valid
        // within bounds?
        if (x < 0 || x > columns - 1) return false;
        if (y < 0 || y > rows - 1) return false;

        // empty destination?
        if (grid[x, y]) return false;
        return true;

    }
    public static void Occupy (int x, int y)
    {
        if (grid[x, y])
        {
            Debug.Log("[Error] - occupying an already occupied grid cell");
            return;
        }
        grid[x, y] = true;
    }

    public static void Vacate (int x, int y)
    {
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
        GameObject player = GameObject.FindGameObjectsWithTag("Player")[0];
        grid[(int)player.transform.position.x, (int)player.transform.position.y] = true;

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
