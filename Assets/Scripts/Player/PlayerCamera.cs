using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera Instance;
    public void Awake() { Instance = this; }

    public float sensX;
    public float sensY;

    [HideInInspector] public Transform orientation;
    [HideInInspector] public Transform cam;

    float xRotation;
    float yRotation;

    public bool canUseMouse;

    private void Update()
    {
        if (!canUseMouse || orientation == null) return;

        // get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // rotate cam and orientation
        cam.localRotation = Quaternion.Euler(xRotation, 0, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
