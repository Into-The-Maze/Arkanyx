using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarSelect : MonoBehaviour
{
    private bool squareSelected;
    private int hotbarIndex;

    public static InventoryItem equippedItem;
    public GameObject hotbar;

    public static HotbarSelect instance;

    private void Awake() {
        squareSelected = false;
        hotbarIndex = 0;
        instance = this;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) updateHotbar(1);
        if (Input.GetKeyDown(KeyCode.Alpha2)) updateHotbar(2);
        if (Input.GetKeyDown(KeyCode.Alpha3)) updateHotbar(3);
        if (Input.GetKeyDown(KeyCode.Alpha4)) updateHotbar(4);
        if (Input.GetKeyDown(KeyCode.Alpha5)) updateHotbar(5);
        if (Input.GetKeyDown(KeyCode.Alpha6)) updateHotbar(6);
        if (Input.GetKeyDown(KeyCode.Alpha7)) updateHotbar(7);
        if (Input.GetKeyDown(KeyCode.Alpha8)) updateHotbar(8);
        if (Input.GetKeyDown(KeyCode.Alpha9)) updateHotbar(9);
        if (Input.GetKeyDown(KeyCode.Alpha0)) updateHotbar(10);
    }

    public void updateHotbar(int newHotbarIndex) {
        if (squareSelected && hotbarIndex == newHotbarIndex) {
            equippedItem = null;
            squareSelected = false;
            GetComponent<Image>().color = new Color(255, 255, 255, 0);
            EquipItem.destroyEquippedItem();
        }
        else if (squareSelected && hotbarIndex != newHotbarIndex) {
            EquipItem.destroyEquippedItem();
            if (hotbar.GetComponent<InvGrid>().inventory[0, newHotbarIndex - 1].item != null) {
                equippedItem = hotbar.GetComponent<InvGrid>().inventory[0, newHotbarIndex - 1].item;
                EquipItem.instantiateEquippedItem();
            }
            else equippedItem = null;
            squareSelected = true;
            GetComponent<Image>().color = new Color(255, 255, 255, 255);
        }
        else if (!squareSelected) {
            EquipItem.destroyEquippedItem();
            if (hotbar.GetComponent<InvGrid>().inventory[0, newHotbarIndex - 1].item != null) {
                equippedItem = hotbar.GetComponent<InvGrid>().inventory[0, newHotbarIndex - 1].item;
                EquipItem.instantiateEquippedItem();
            }
            else equippedItem = null;
            squareSelected = true;
            GetComponent<Image>().color = new Color(255, 255, 255, 255);
        }

        gameObject.transform.localPosition = new Vector2(32 * (newHotbarIndex - 1), -32);
        hotbarIndex = newHotbarIndex;
    }
}
