using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herd : MonoBehaviour
{
    [SerializeField] private int radius = 10;

    public List<GameObject> possibleAnimals = new List<GameObject>();
    [HideInInspector] public List<GameObject> animals = new List<GameObject>();

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (animals.Count > 0) return;

        for (int i = 0; i < possibleAnimals.Count; i++)
        {
            animals.Add(PhotonNetwork.Instantiate(possibleAnimals[i].name, transform.position + new Vector3(Random.Range(-radius, radius), 0, Random.Range(-radius, radius)), Quaternion.identity));
            animals[i].GetComponent<Entity>().herd = this;
        }
    }
}
