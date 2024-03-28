using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviourPunCallbacks
{
    public bool IsMapGenerated;

    [SerializeField] private Terrain terrain;
    [SerializeField] private Transform treesParent, oresParent;

    [SerializeField] private GameObject[] treesPrefabs;
    [SerializeField] private GameObject[] oresPrefabs;
    [SerializeField] private GameObject rockPrefab;

    public void GenerateMap()
    {
        photonView.RPC("ChangeStateRPC", RpcTarget.AllBuffered);

        int n = (int)(terrain.terrainData.size.x-50);
        int half = n / 2;

        for (float z = 0; z < n; z += 4)
        {
            for (float x = 0; x < n; x += 4)
            {
                float treeValue = Mathf.PerlinNoise(x/Random.Range(60,100), z/Random.Range(60,100));
                if(treeValue > 0.65f)
                {
                    PhotonNetwork.Instantiate(treesPrefabs[Random.Range(0, treesPrefabs.Length)].name, GetPositionFromOnMap(x - half + Random.Range(-3, 3), z- half + Random.Range(-3, 3)), Quaternion.identity);
                }
                
                float oreValue = Mathf.PerlinNoise(x/Random.Range(1,50), z/Random.Range(1, 50));
                if(oreValue > 0.75f && oreValue < 0.8f)
                {
                    PhotonNetwork.Instantiate(oresPrefabs[Random.Range(0, oresPrefabs.Length)].name, GetPositionFromOnMap(x - half + Random.Range(-3, 3), z - half + Random.Range(-3, 3)), Quaternion.identity);
                }

                if(oreValue > 0.6 && oreValue < 0.61)
                {
                    PhotonNetwork.Instantiate(rockPrefab.name, GetPositionFromOnMap(x - half + Random.Range(-3, 3), z - half + Random.Range(-3, 3)), Quaternion.identity);
                }
            }
        }
    }

    public Vector3 GetPositionFromOnMap(float x, float z)
    {
        Ray ray = new Ray(new Vector3(x, 50, z), Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100))
        {
            if (hitInfo.transform.gameObject.layer == 3)
            {
                return hitInfo.point;
            }
        }

        return Vector3.zero;
    }

    [PunRPC]
    public void ChangeStateRPC()
    {
        IsMapGenerated = true;
    }
}
