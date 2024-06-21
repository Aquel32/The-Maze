using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInitializer : MonoBehaviour
{
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject model;

    private void Start()
    {
        cam.SetActive(true);
        model.GetComponent<RandomModelChooser>().enabled = true;
        //GetComponent<HealthSystem>().enabled = true;
        GetComponent<PlayerAudioSource>().enabled = true;
        GetComponent<Footstep>().enabled = true;
    }
}
