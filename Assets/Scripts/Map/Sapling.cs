using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sapling : MonoBehaviour
{
    [SerializeField] private GameObject treePrefab;
    [SerializeField] private int timeToGrow;

    private void Start()
    {
        StartCoroutine(growing());
    }

    public IEnumerator growing()
    {
        yield return new WaitForSeconds(timeToGrow);

        PhotonNetwork.Instantiate(treePrefab.name, transform.position, Quaternion.identity);
        PhotonNetwork.Destroy(this.gameObject);
    }
}
