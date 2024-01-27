using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;
    public Transform hand;

    float xRotation;
    float yRotation;

    public bool canUseMouse;

    [SerializeField] private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager.playerCamera = transform;
    }

    private void Update()
    {
        if (!canUseMouse) return;

        // get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // rotate cam and orientation
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
