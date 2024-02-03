using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInitializer : MonoBehaviour
{
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject model;

    private void Start()
    {
        model.GetComponent<RandomModelChooser>().enabled = true;
        GetComponent<UiManager>().enabled = true;
        canvas.SetActive(true);
        GetComponent<InventoryManager>().enabled = true;
        GetComponent<ArmorSystem>().enabled = true;
        cam.SetActive(true);
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<HealthSystem>().enabled = true;
        GetComponent<PlayerAudioSource>().enabled = true;
        GetComponent<Footstep>().enabled = true;
        GetComponent<PauseMenu>().enabled = true;
    }
}
