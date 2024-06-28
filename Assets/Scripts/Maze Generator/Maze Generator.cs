using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviourPunCallbacks
{
    [SerializeField] private MazeCell _mazeCellPrefab;
    [SerializeField] private int _mazeWidth;
    [SerializeField] private int _mazeDepth;

    [SerializeField] private MazeCell[,] mazeGrid;

    private string seedString;
    private int seedIndex;

    public Vector2 start;

    public List<int> seed;

    public Vector2 zoneX;
    public Vector2 zoneZ;

    private void Start()
    {
        if (PhotonNetwork.IsConnected == false) return;
        if (PhotonNetwork.IsMasterClient == false) return;

        seedString = Random.Range(1111111111, 2147483647).ToString() + Random.Range(1111111111, 2147483647).ToString() + Random.Range(1111111111, 2147483647).ToString() + Random.Range(1111111111, 2147483647).ToString() + Random.Range(1111111111, 2147483647).ToString();
        photonView.RPC("GenerateMazeRPC", RpcTarget.AllBuffered, seedString);
    }

    [PunRPC]
    public void GenerateMazeRPC(string seedString)
    {
        for (int i = 0; i < seedString.Length; i++)
        {
            int number = seedString[i] % 9;
            if (number == 0) number = 1;
            seed.Add(number);
        }

        seedIndex = 0;

        mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                mazeGrid[x, z] = Instantiate(_mazeCellPrefab, new Vector3(transform.position.x + x * 30, transform.position.y, transform.position.z + z * 30), Quaternion.identity, transform);
                mazeGrid[x, z].IsVisited = false;
                mazeGrid[x, z].x = x;
                mazeGrid[x, z].z = z;

                if (x >= zoneX.x && x <= zoneX.y && z >= zoneZ.x && z <= zoneZ.y)
                {
                    mazeGrid[x, z].IsVisited = true;
                    mazeGrid[x, z].ClearBackWall();
                    mazeGrid[x, z].ClearFrontWall();
                    mazeGrid[x, z].ClearLeftWall();
                    mazeGrid[x, z].ClearRightWall();
                }

                if (x == 30 && z == 41) { mazeGrid[x, z].ClearRightWall(); }
                if (x == 41 && z == 52) { mazeGrid[x, z].ClearBackWall(); }
                if (x == 52 && z == 41) { mazeGrid[x, z].ClearLeftWall(); }
                if (x == 41 && z == 30) { mazeGrid[x, z].ClearFrontWall(); }
            }
        }

        GenerateMaze(mazeGrid[(int)start.x, (int)start.y]);
    }

    public void GenerateMaze(MazeCell cell)
    {
        cell.IsVisited = true;

        List<MazeCell> nextCells = new List<MazeCell>();

        if (cell.x + 1 < _mazeWidth) nextCells.Add(mazeGrid[cell.x + 1, cell.z]);
        if (cell.x - 1 >= 0) nextCells.Add(mazeGrid[cell.x - 1, cell.z]);
        if (cell.z + 1 < _mazeDepth) nextCells.Add(mazeGrid[cell.x, cell.z + 1]);
        if (cell.z - 1 >= 0) nextCells.Add(mazeGrid[cell.x, cell.z - 1]);

        nextCells = nextCells.OrderBy(x => GetNextSeedNumber()).ToList();

        for (int i = 0; i < nextCells.Count; i++)
        {
            if (nextCells[i].IsVisited == true) { continue; }

            if (nextCells[i].x > cell.x) { nextCells[i].ClearLeftWall(); cell.ClearRightWall(); }
            if (nextCells[i].x < cell.x) { nextCells[i].ClearRightWall(); cell.ClearLeftWall(); }
            if (nextCells[i].z > cell.z) { nextCells[i].ClearBackWall(); cell.ClearFrontWall(); }
            if (nextCells[i].z < cell.z) { nextCells[i].ClearFrontWall(); cell.ClearBackWall(); }

            GenerateMaze(nextCells[i]);
        }
    }

    public int GetNextSeedNumber()
    {
        seedIndex++;
        if (seedIndex >= seed.Count) seedIndex = 0;

        return seed[seedIndex];
    }

    
    


}
