using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToggler : MonoBehaviour
{
    private bool cursorLocked = true;
    private bool cursorVisible = false;

    public GameObject crosshair;
    public GameObject inventory;
    public static bool inventoryOpen;

    private void Start() {
        Cursor.visible = cursorVisible;
        Cursor.lockState = cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;

        inventoryOpen = false;
        inventory.SetActive(inventoryOpen);

        crosshair.SetActive(!inventoryOpen);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Tab) && !InvController.itemSelected) {
            toggleInv();
        }
    }

    private void toggleInv() {
        inventoryOpen = !inventoryOpen;
        inventory.SetActive(inventoryOpen);
        toggleCursor();
    }

    private void toggleCursor() {
        cursorVisible = !cursorVisible;
        cursorLocked = !cursorLocked;
        Cursor.visible = cursorVisible;
        Cursor.lockState = cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
        crosshair.SetActive(!inventoryOpen);
    }
}
