using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [SerializeField] private GameObject _leftWall;
    [SerializeField] private GameObject _rightWall;
    [SerializeField] private GameObject _frontWall;
    [SerializeField] private GameObject _backWall;

    public int x;
    public int z;

    public bool IsVisited;
    public float distance;

    public MazeZone zone;

    public void GenerateRoom(int n, int m)
    {
        List<MazeCell> roomsCells = new List<MazeCell>();

        for(int _X = x; _X <= n+x; _X++)
        {
            for (int _Z = z; _Z <= m + z; _Z++)
            {
                if (_X >= MazeGenerator.Instance.mazeWidth || _Z >= MazeGenerator.Instance.mazeDepth) continue;
                if (MazeGenerator.Instance.mazeGrid[_X, _Z].zone != zone) continue;
                if (MazeGenerator.Instance.mazeGrid[_X, _Z].IsVisited) continue;

                MazeGenerator.Instance.mazeGrid[_X, _Z].IsVisited = true;

                /*if(_X == x && _Z == z)
                {
                    MazeGenerator.Instance.mazeGrid[_X, _Z].ClearRightWall();
                    MazeGenerator.Instance.mazeGrid[_X, _Z].ClearFrontWall();
                }
                else if(_X == x && _Z == m + z)
                {
                    MazeGenerator.Instance.mazeGrid[_X, _Z].ClearRightWall();
                    MazeGenerator.Instance.mazeGrid[_X, _Z].ClearBackWall();
                }
                else if(_X == n+x && _Z == z)
                {
                    MazeGenerator.Instance.mazeGrid[_X, _Z].ClearFrontWall();
                    MazeGenerator.Instance.mazeGrid[_X, _Z].ClearLeftWall();
                }
                else if(_X == n+x && _Z == m+z)
                {
                    MazeGenerator.Instance.mazeGrid[_X, _Z].ClearFrontWall();
                    MazeGenerator.Instance.mazeGrid[_X, _Z].ClearBackWall();
                }*/
                if (_X == x)
                {
                    MazeGenerator.Instance.mazeGrid[_X, _Z].ClearRightWall();
                    MazeGenerator.Instance.mazeGrid[_X, _Z].ClearFrontWall();
                    MazeGenerator.Instance.mazeGrid[_X, _Z].ClearBackWall();
                }
                else if (_X == n + x)
                {
                    MazeGenerator.Instance.mazeGrid[_X, _Z].ClearLeftWall();
                    MazeGenerator.Instance.mazeGrid[_X, _Z].ClearFrontWall();
                    MazeGenerator.Instance.mazeGrid[_X, _Z].ClearBackWall();
                }
                else if (_Z == z)
                {
                    MazeGenerator.Instance.mazeGrid[_X, _Z].ClearFrontWall();
                    MazeGenerator.Instance.mazeGrid[_X, _Z].ClearRightWall();
                    MazeGenerator.Instance.mazeGrid[_X, _Z].ClearLeftWall();
                }
                else if (_Z == m + z)
                {
                    MazeGenerator.Instance.mazeGrid[_X, _Z].ClearBackWall();
                    MazeGenerator.Instance.mazeGrid[_X, _Z].ClearRightWall();
                    MazeGenerator.Instance.mazeGrid[_X, _Z].ClearLeftWall();
                }
                else
                {
                    MazeGenerator.Instance.mazeGrid[_X, _Z].ClearBackWall();
                    MazeGenerator.Instance.mazeGrid[_X, _Z].ClearFrontWall();
                    MazeGenerator.Instance.mazeGrid[_X, _Z].ClearLeftWall();
                    MazeGenerator.Instance.mazeGrid[_X, _Z].ClearRightWall();
                }
            }
        }

        /*int doorX = x + n/2;

        print(doorX + " " + z);

        MazeGenerator.Instance.mazeGrid[doorX, z].ClearLeftWall();
        MazeGenerator.Instance.mazeGrid[doorX, z].ClearRightWall();
        MazeGenerator.Instance.mazeGrid[doorX, z].ClearFrontWall();
        MazeGenerator.Instance.mazeGrid[doorX, z].ClearBackWall();

        MazeGenerator.Instance.mazeGrid[doorX, z-1].ClearLeftWall();
        MazeGenerator.Instance.mazeGrid[doorX, z-1].ClearRightWall();
        MazeGenerator.Instance.mazeGrid[doorX, z-1].ClearBackWall();
        MazeGenerator.Instance.mazeGrid[doorX, z-1].ClearFrontWall();*/
    }

    public void ApplyAZone()
    {
        zone = MazeZone.A;
    }
    public void ApplyBZone()
    {
        zone = MazeZone.B;
    }
    public void ApplyCZone()
    {
        zone = MazeZone.C;
    }

    public void ClearLeftWall()
    {
        _leftWall.SetActive(false);
    }
    public void ClearRightWall()
    {
        _rightWall.SetActive(false);
    }
    public void ClearFrontWall()
    {
        _frontWall.SetActive(false);
    }
    public void ClearBackWall()
    {
        _backWall.SetActive(false);
    }
}

public enum MazeZone {None, Glade, A, B, C }