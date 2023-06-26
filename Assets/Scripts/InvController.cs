using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class InvController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject player;

    private const int invWidth = 10;
    private const int invHeight = 4;
    private const int tileWidthPx = 32;
    private const int tileHeightPx = 32;

    public static bool active;
    public static bool itemSelected;

    public RectTransform invGrid;
    public RectTransform UICanvas;

    private (InventoryItem item, int stack) selectedItem;
    private GameObject selectedItemGameObject;
    public GameObject itemPrefab;

    public static (InventoryItem item, int stack, GameObject invObject)[,] inventory;

    

    private void Awake() {
        active = false;
        selectedItem.item = null;
        selectedItem.stack = 0;
        initInventory();
    }

    private void Update() {

        if (Input.GetMouseButtonDown(0) && !active) {
            dropItem();
        }

        if (!active) { return; }

        if (Input.GetMouseButtonDown(0) && selectedItem.item == null) {
            selectItem(getInvSquareCoords(Input.mousePosition));
        }
        else if (Input.GetMouseButtonDown(0) && selectedItem.item != null) {
            placeItem(getInvSquareCoords(Input.mousePosition));
        }
        if (Input.GetKeyDown(KeyCode.R) && selectedItem.item == null) {//test script
            createRandomItem();
        }
    }

    private void dropItem() {
        GameObject droppedItem= Instantiate(selectedItem.item.Item, player.transform.position + player.transform.forward * 1.5f + player.transform.up * 1.1f, Quaternion.identity);
        var data = droppedItem.AddComponent<InventoryItemData>();
        data.Item = selectedItem.item.Item;
        data.UIImage = selectedItem.item.UIImage;
        data.ID = selectedItem.item.ID;
        data.Stackable = selectedItem.item.Stackable;

        selectedItem.item = null;
        selectedItem.stack = 0;
        Destroy(selectedItemGameObject);

        itemSelected = false;
    }

    private void placeItem((int x, int y) squareReference) {

        //if square empty, put item in square
        if (inventory[squareReference.y, squareReference.x].item == null) {
            inventory[squareReference.y, squareReference.x].item = selectedItem.item;
            inventory[squareReference.y, squareReference.x].stack = selectedItem.stack;
            inventory[squareReference.y, squareReference.x].invObject = selectedItemGameObject;

            selectedItem.item = null;
            selectedItem.stack = 0;
            placeItemObject(squareReference);

            itemSelected = false;
            
        }

        //if square contains the same item and item is stackable, increase stack
        else if (inventory[squareReference.y, squareReference.x].item.ID == selectedItem.item.ID && inventory[squareReference.y, squareReference.x].item.Stackable) {
            inventory[squareReference.y, squareReference.x].stack += selectedItem.stack;
            inventory[squareReference.y, squareReference.x].invObject.GetComponentInChildren<Text>().text = inventory[squareReference.y, squareReference.x].stack.ToString();

            selectedItem.item = null;
            selectedItem.stack = 0;
            Destroy(selectedItemGameObject);

            itemSelected = false;
        }

        //otherwise, swap the selected item and square item around
        else {

            (InventoryItem item, int stack) tempData = selectedItem;
            GameObject tempObject = selectedItemGameObject;
            StopAllCoroutines();
            placeItemObject(squareReference);
            
            selectItem(squareReference);
            inventory[squareReference.y, squareReference.x].item = tempData.item;
            inventory[squareReference.y, squareReference.x].stack = tempData.stack;
            inventory[squareReference.y, squareReference.x].invObject = tempObject;

            //dont think about this too hard its very unthinky
        }
        //Debug.Log($"{inventory[squareReference.y, squareReference.x].item.ID}, {inventory[squareReference.y, squareReference.x].stack}");
    }

    private void placeItemObject((int x, int y) squareReference) {
        selectedItemGameObject.transform.SetParent(invGrid.transform);

        selectedItemGameObject.transform.localPosition = new Vector2(squareReference.x * tileWidthPx + tileWidthPx * 0.5f, -(squareReference.y * tileHeightPx + tileHeightPx * 0.5f));

        selectedItemGameObject = null;
    }

    private void selectItem((int x, int y) squareReference) {
        if (inventory[squareReference.y, squareReference.x].item == null) { return; }
        


        selectedItem.item = inventory[squareReference.y, squareReference.x].item;
        selectedItem.stack = inventory[squareReference.y, squareReference.x].stack;
        selectedItemGameObject = inventory[squareReference.y, squareReference.x].invObject;

        selectedItemGameObject.transform.SetParent(UICanvas.transform);

        inventory[squareReference.y, squareReference.x].item = null;
        inventory[squareReference.y, squareReference.x].stack = 0;
        inventory[squareReference.y, squareReference.x].invObject = null;

        StartCoroutine(drag(selectedItemGameObject));
        


    }

    private void createRandomItem() { //again, test script
        selectedItem.item = ItemDropTable.i.allItems[UnityEngine.Random.Range(0, ItemDropTable.i.allItems.Count)];
        selectedItem.stack = 1;
        itemPrefab.GetComponent<UnityEngine.UI.Image>().sprite = selectedItem.item.UIImage;
        itemPrefab.name = selectedItem.item.ID;
        itemPrefab.GetComponentInChildren<Text>().text = selectedItem.stack.ToString();
        selectedItemGameObject = Instantiate(itemPrefab, Input.mousePosition, Quaternion.identity, UICanvas.transform);
        StartCoroutine(drag(selectedItemGameObject));
    }

    IEnumerator drag(GameObject item) {
        itemSelected = true;
        while (selectedItem.item != null) {
            item.transform.position = Input.mousePosition;
            yield return null;
        }
    }

    private (int, int) getInvSquareCoords(Vector2 mousePos) {
        return ((int)((mousePos.x - invGrid.position.x) / tileWidthPx), (int)((invGrid.position.y - mousePos.y) / tileHeightPx));
    }

    private void initInventory() {
        inventory = new (InventoryItem item, int stack, GameObject invObject)[invHeight, invWidth];
        invGrid = GetComponent<RectTransform>();

        //initialises the inventory for the first time
        for (int i = 0; i < invHeight; i++) {
            for (int j = 0; j < invWidth; j++) {
                inventory[i, j].item = null;
                inventory[i, j].stack = 0;
            }
        }

        //sets grid size based on nimber of grid squares along its width and height
        invGrid.sizeDelta = new Vector2(invWidth * tileWidthPx, invHeight * tileHeightPx);
        //positions grid exactly in centre of canvas.
        invGrid.position = new Vector2(UICanvas.sizeDelta.x * 0.5f - invGrid.sizeDelta.x * 0.5f,  UICanvas.sizeDelta.y * 0.5f + invGrid.sizeDelta.y * 0.5f);

    }

    private void refreshInventory() {
        invGrid.sizeDelta = new Vector2(invWidth * tileWidthPx, invHeight * tileHeightPx);
        invGrid.position = new Vector2(UICanvas.sizeDelta.x * 0.5f - invGrid.sizeDelta.x * 0.5f, UICanvas.sizeDelta.y * 0.5f - invGrid.sizeDelta.y * 0.5f);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        active = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        active = false;
    }
}
