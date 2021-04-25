using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Gameboard : MonoBehaviour
{

    // lets try tracking as bools ....[horizontal][vertical] (aka, [column][row], [x][y])
    public static bool[,] grid;
    public static int columns = 100;
    public static int rows = 50;
    public static Mutex m = new Mutex(false, "gameBoardMutex");
    public static bool Validate(int x, int y)
    { 
        m.WaitOne();
        // check if moving to this location is valid
        // within bounds?
        if (x < 0 || x > columns - 1) {
            m.ReleaseMutex();
            return false;
        }
        if (y < 0 || y > rows - 1) {
            m.ReleaseMutex();
            return false;
        }

        // empty destination?
        if (grid[x, y]) {
            m.ReleaseMutex();
            return false;
        }
        m.ReleaseMutex();
        return true;

    }
    public static void Occupy (int x, int y)
    {
        m.WaitOne();
        if (grid[x, y])
        {
            Debug.Log("[Error] - occupying an already occupied grid cell");
            m.ReleaseMutex();
            return;
        }
        grid[x, y] = true;
        m.ReleaseMutex();
    }

    public static void Vacate (int x, int y)
    {
        m.WaitOne();
        if (!grid[x, y])
        {
            Debug.Log("[Error] - vacating an already vacated grid cell");
            m.ReleaseMutex();
            return;
        }
        grid[x, y] = false;
        m.ReleaseMutex();
    }

    void OnEnable()
    {
        grid = new bool[columns, rows];
        // TODO: place obstacles
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var p in players)
        {
            grid[(int)p.transform.position.x, (int)p.transform.position.y] = true;
        }
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var e in enemies)
        {
            grid[(int)e.transform.position.x, (int)e.transform.position.y] = true;
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
