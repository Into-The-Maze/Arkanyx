using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public Transform player;

    private float mouseSensitivity;
    private float cameraVerticalRotation;

    private const float maxLookUpDegrees = -80f;
    private const float maxLookDownDegrees = 90f;

    private bool cursorLocked = false;
    private bool cursorVisible = true;

    void Start()
    {
        mouseSensitivity = 2f;
        cameraVerticalRotation = 0f;

        Cursor.visible = cursorVisible;
        Cursor.lockState = cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
    }

    void Update()
    {
        handleCameraVertical();
        handleCameraHorizontal();
    }

    private void handleCameraVertical() {
        cameraVerticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, maxLookUpDegrees, maxLookDownDegrees);
        transform.localEulerAngles = Vector3.right * cameraVerticalRotation;

    }

    private void handleCameraHorizontal() {
        player.Rotate(Input.GetAxis("Mouse X") * mouseSensitivity * Vector3.up);
    }
}
