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
        GetComponent<PlayerAudioSource>().enabled = true;
        GetComponent<Footstep>().enabled = true;

        PlayerCamera.Instance.orientation = transform;
        PlayerCamera.Instance.cam = transform.Find("Camera");

        PlayerMovement.Instance.orientation = transform;
        PlayerMovement.Instance.footstep = GetComponent<Footstep>();
        PlayerMovement.Instance.rb = GetComponent<Rigidbody>();
        PlayerMovement.Instance.rb.freezeRotation = true;

        InventoryManager.Instance.mainHand = transform.Find("Hand");
        InventoryManager.Instance.itemHolder = transform.Find("Hand").Find("ItemHolder");
        InventoryManager.Instance.playerCamera = cam.transform;
    }
}
