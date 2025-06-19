using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviourPunCallbacks
{
    public static MazeGenerator Instance;
    private void Awake() { Instance = this; }

    [SerializeField] private MazeCell _mazeCellPrefab;
    public int mazeWidth;
    public int mazeDepth;

    public MazeCell[,] mazeGrid;

    private string seedString;
    private int seedIndex;

    public List<int> seed;

    private static int GladeRadiusInCells = 10;

    public float ZoneARadius;
    public float ZoneBRadius;
    public float ZoneCRadius;

    private void Start()
    {
        if (PhotonNetwork.IsConnected == false) return;
        if (PhotonNetwork.IsMasterClient == false) return;

        ulong stringNumber = (ulong)Random.Range(1111111111, 2147483647);
        stringNumber *= stringNumber * stringNumber * stringNumber * stringNumber;

        seedString = stringNumber.ToString();
        photonView.RPC("StartGeneratingMazeRPC", RpcTarget.AllBuffered, seedString);
    }


    public float Distance(int x, int z)
    {
        return Mathf.Sqrt(Mathf.Abs(Mathf.Pow((mazeWidth / 2) - x, 2)) + Mathf.Abs(Mathf.Pow((mazeDepth / 2) - z, 2)));
    }

    [PunRPC]
    public void StartGeneratingMazeRPC(string seedString)
    {
        for (int i = 0; i < seedString.Length; i++)
        {
            int number = seedString[i] % 9;
            if (number == 0) number = 1;
            seed.Add(number);
        }

        //setting main door

        seedIndex = 0;

        mazeGrid = new MazeCell[mazeWidth, mazeDepth];

        Vector3 startPosition = new Vector3(transform.position.x - ((mazeWidth * 30)/2), transform.position.y, transform.position.z - ((mazeDepth * 30) / 2));

        Vector2 zoneX = new Vector2((mazeWidth / 2) - GladeRadiusInCells, (mazeWidth / 2) + GladeRadiusInCells);
        Vector2 zoneZ = new Vector2((mazeDepth / 2) - GladeRadiusInCells, (mazeDepth / 2) + GladeRadiusInCells);

        for (int x = 0; x < mazeWidth; x++)
        {
            for (int z = 0; z < mazeDepth; z++)
            {
                mazeGrid[x, z] = Instantiate(_mazeCellPrefab, new Vector3(startPosition.x + x * 30, startPosition.y, startPosition.z + z * 30), Quaternion.identity, transform);
                mazeGrid[x, z].IsVisited = false;
                mazeGrid[x, z].x = x;
                mazeGrid[x, z].z = z;
                mazeGrid[x, z].gameObject.name = x + " " + z;

                float distance = Distance(x, z);
                mazeGrid[x, z].distance = distance;


                if (distance <= ZoneARadius)
                {
                    //Instantiate Zone A
                    mazeGrid[x, z].ApplyAZone();
                    #region Glade Fixes
                    if (x >= zoneX.x && x <= zoneX.y && z >= zoneZ.x && z <= zoneZ.y)
                    {
                        mazeGrid[x, z].IsVisited = true;
                        mazeGrid[x, z].ClearBackWall();
                        mazeGrid[x, z].ClearFrontWall();
                        mazeGrid[x, z].ClearLeftWall();
                        mazeGrid[x, z].ClearRightWall();

                        mazeGrid[x, z].zone = MazeZone.Glade;
                    }

                    if (x == (mazeWidth / 2) - GladeRadiusInCells - 1 && z == (mazeDepth / 2)) { mazeGrid[x, z].ClearRightWall(); }
                    if (x == (mazeWidth / 2) && z == (mazeDepth / 2) + GladeRadiusInCells + 1) { mazeGrid[x, z].ClearBackWall(); }
                    if (x == (mazeWidth / 2) + GladeRadiusInCells + 1 && z == (mazeDepth / 2)) { mazeGrid[x, z].ClearLeftWall(); }
                    if (x == (mazeWidth / 2) && z == (mazeDepth / 2) - GladeRadiusInCells - 1) { mazeGrid[x, z].ClearFrontWall(); }
                    #endregion
                }
                else if (distance <= ZoneBRadius)
                {
                    //Instantiate Zone B
                    mazeGrid[x, z].ApplyBZone();

                    mazeGrid[x, z].IsVisited = true;
                    mazeGrid[x, z].ClearBackWall();
                    mazeGrid[x, z].ClearFrontWall();
                    mazeGrid[x, z].ClearLeftWall();
                    mazeGrid[x, z].ClearRightWall();
                }
                else if(distance <= ZoneCRadius)
                {
                    //Instantiate Zone C
                    mazeGrid[x, z].ApplyCZone();
                }
                else
                {
                    //Disable rest of the map
                    mazeGrid[x, z].IsVisited = true;
                    mazeGrid[x, z].ClearBackWall();
                    mazeGrid[x, z].ClearFrontWall();
                    mazeGrid[x, z].ClearLeftWall();
                    mazeGrid[x, z].ClearRightWall();

                    mazeGrid[x, z].zone = MazeZone.None;
                }
            }
        }

        for (int x = 0; x < mazeWidth; x++)
        {
            for (int z = 0; z < mazeDepth; z++)
            {
                if (mazeGrid[x,z].IsVisited) { continue; }

                if (mazeGrid[x,z].zone == MazeZone.A) GenerateAZoneMaze(mazeGrid[x, z]);
                else if (mazeGrid[x,z].zone == MazeZone.B) GenerateBZoneMaze(mazeGrid[x, z]);
                else if (mazeGrid[x,z].zone == MazeZone.C) GenerateCZoneMaze(mazeGrid[x, z]);
            }
        }
        
    }

    public void GenerateAZoneMaze(MazeCell cell)
    {
        cell.IsVisited = true;

        List<MazeCell> nextCells = new List<MazeCell>();

        if (cell.x + 1 < mazeWidth) nextCells.Add(mazeGrid[cell.x + 1, cell.z]);
        if (cell.x - 1 >= 0) nextCells.Add(mazeGrid[cell.x - 1, cell.z]);
        if (cell.z + 1 < mazeDepth) nextCells.Add(mazeGrid[cell.x, cell.z + 1]);
        if (cell.z - 1 >= 0) nextCells.Add(mazeGrid[cell.x, cell.z - 1]);

        nextCells = nextCells.OrderBy(x => GetNextSeedNumber()).ToList();

        for (int i = 0; i < nextCells.Count; i++)
        {
            if (nextCells[i].IsVisited == true) { continue; }
            if (nextCells[i].zone != MazeZone.A) { continue; }

            if (nextCells[i].x > cell.x) { nextCells[i].ClearLeftWall(); cell.ClearRightWall(); }
            if (nextCells[i].x < cell.x) { nextCells[i].ClearRightWall(); cell.ClearLeftWall(); }
            if (nextCells[i].z > cell.z) { nextCells[i].ClearBackWall(); cell.ClearFrontWall(); }
            if (nextCells[i].z < cell.z) { nextCells[i].ClearFrontWall(); cell.ClearBackWall(); }

            GenerateAZoneMaze(nextCells[i]);
        }
    }
    
    public void GenerateBZoneMaze(MazeCell cell)
    {
        cell.IsVisited = true;

        List<MazeCell> nextCells = new List<MazeCell>();

        if (cell.x + 1 < mazeWidth) nextCells.Add(mazeGrid[cell.x + 1, cell.z]);
        if (cell.x - 1 >= 0) nextCells.Add(mazeGrid[cell.x - 1, cell.z]);
        if (cell.z + 1 < mazeDepth) nextCells.Add(mazeGrid[cell.x, cell.z + 1]);
        if (cell.z - 1 >= 0) nextCells.Add(mazeGrid[cell.x, cell.z - 1]);

        nextCells = nextCells.OrderBy(x => GetNextSeedNumber()%5).ToList();

        for (int i = 0; i < nextCells.Count; i++)
        {
            if (nextCells[i].IsVisited == true) { continue; }
            if (nextCells[i].zone != MazeZone.B) { continue; }

            if (nextCells[i].x > cell.x) { nextCells[i].ClearLeftWall(); cell.ClearRightWall(); }
            if (nextCells[i].x < cell.x) { nextCells[i].ClearRightWall(); cell.ClearLeftWall(); }
            if (nextCells[i].z > cell.z) { nextCells[i].ClearBackWall(); cell.ClearFrontWall(); }
            if (nextCells[i].z < cell.z) { nextCells[i].ClearFrontWall(); cell.ClearBackWall(); }

            GenerateBZoneMaze(nextCells[i]);
        }
    }
    
    public void GenerateCZoneMaze(MazeCell cell)
    {
        cell.IsVisited = true;

        List<MazeCell> nextCells = new List<MazeCell>();

        if (cell.x + 1 < mazeWidth) nextCells.Add(mazeGrid[cell.x + 1, cell.z]);
        if (cell.x - 1 >= 0) nextCells.Add(mazeGrid[cell.x - 1, cell.z]);
        if (cell.z + 1 < mazeDepth) nextCells.Add(mazeGrid[cell.x, cell.z + 1]);
        if (cell.z - 1 >= 0) nextCells.Add(mazeGrid[cell.x, cell.z - 1]);

        nextCells = nextCells.OrderBy(x => GetNextSeedNumber()%5).ToList();

        for (int i = 0; i < nextCells.Count; i++)
        {
            if (nextCells[i].IsVisited == true) { continue; }
            if (nextCells[i].zone != MazeZone.C) { continue; }

            if (nextCells[i].x > cell.x) { nextCells[i].ClearLeftWall(); cell.ClearRightWall(); }
            if (nextCells[i].x < cell.x) { nextCells[i].ClearRightWall(); cell.ClearLeftWall(); }
            if (nextCells[i].z > cell.z) { nextCells[i].ClearBackWall(); cell.ClearFrontWall(); }
            if (nextCells[i].z < cell.z) { nextCells[i].ClearFrontWall(); cell.ClearBackWall(); }

            GenerateCZoneMaze(nextCells[i]);
        }
    }

    public int GetNextSeedNumber()
    {
        seedIndex++;
        if (seedIndex >= seed.Count) seedIndex = 0;

        return seed[seedIndex];
    }

    
    


}
