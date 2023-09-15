using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class InvController : MonoBehaviour
{
    public GameObject player;
   
    public static bool itemSelected;

    public InvGrid mainInv;
    public static InvGrid selectedInvGrid;
    
    public RectTransform UICanvas;

    private (InventoryItem item, int stack) selectedItem;
    private GameObject selectedItemImage;
    public GameObject itemPrefab;

    private InventoryItem itemToInsert;
    private GameObject itemToInsertImage;

    private void Awake() {
        selectedInvGrid = null;
        selectedItem.item = null;
        selectedItem.stack = 0;

    }

    private void Update() {

        if (Input.GetMouseButtonDown(0) && selectedInvGrid == null && selectedItem.item != null) {
            dropItem();
        }

        if (selectedInvGrid == null) { return; }

        if (Input.GetMouseButtonDown(0) && selectedItem.item == null) {
            selectItem(getInvSquareCoords(Input.mousePosition));
            if (selectedInvGrid.name == "Hotbar") {
                HotbarSelect.instance.updateHotbar(getInvSquareCoords(Input.mousePosition).x + 1);
            }
        }
        else if (Input.GetMouseButtonDown(0) && selectedItem.item != null) {
            placeItem(getInvSquareCoords(Input.mousePosition));
            if (selectedInvGrid.name == "Hotbar") {
                HotbarSelect.instance.updateHotbar(getInvSquareCoords(Input.mousePosition).x + 1);
            }
        }
        if (Input.GetKeyDown(KeyCode.R) && selectedItem.item == null) {//test script
            createRandomItem();
        }
    }
    private void createRandomItem() { //again, test script
        selectedItem.item = ItemDropTable.i.allItems[UnityEngine.Random.Range(0, ItemDropTable.i.allItems.Count)];
        selectedItem.stack = 1;
        itemPrefab.GetComponent<UnityEngine.UI.Image>().sprite = selectedItem.item.UIImage;
        itemPrefab.name = selectedItem.item.ID;
        itemPrefab.GetComponentInChildren<Text>().text = selectedItem.stack.ToString();
        selectedItemImage = Instantiate(itemPrefab, Input.mousePosition, Quaternion.identity, UICanvas.transform);
        StartCoroutine(drag(selectedItemImage));
    }

    private void dropItem() {
        GameObject droppedItem = Instantiate(selectedItem.item.Item, player.transform.position + player.transform.forward * 1.5f + player.transform.up * 1.1f, Quaternion.identity);
        droppedItem.tag = "Item";
        var data = droppedItem.AddComponent<InventoryItemData>();
        data.Item = selectedItem.item.Item;
        data.UIImage = selectedItem.item.UIImage;
        data.ID = selectedItem.item.ID;
        data.Stackable = selectedItem.item.Stackable;
        data.Use = selectedItem.item.Use;

        selectedItem.item = null;
        selectedItem.stack = 0;
        Destroy(selectedItemImage);

        itemSelected = false;
    }
    
    public void insertItem(InventoryItemData data) {

        (int x, int y)? firstBlankSpace = null;
        (int x, int y)? firstStackSlot = null;

        for (int i = 0; i < mainInv.invHeight; i++) {
            for (int j = 0; j < mainInv.invWidth; j++) {
                if (mainInv.inventory[i, j].item == null) firstBlankSpace = (j, i);

                if (mainInv.inventory[i, j].item != null)
                    if (mainInv.inventory[i, j].item.ID == data.ID && data.Stackable) { firstStackSlot = (j, i); goto breakPoint; };
            }
        }
        breakPoint:

        if (firstStackSlot != null) {
            mainInv.inventory[firstStackSlot.Value.y, firstStackSlot.Value.x].stack += 1;
            mainInv.inventory[firstStackSlot.Value.y, firstStackSlot.Value.x].invObject.GetComponentInChildren<Text>().text = mainInv.inventory[firstStackSlot.Value.y, firstStackSlot.Value.x].stack.ToString();
        }//stack item
        else if (firstBlankSpace != null) {
            itemPrefab.GetComponent<UnityEngine.UI.Image>().sprite = data.UIImage;
            itemPrefab.name = data.ID;
            itemPrefab.GetComponentInChildren<Text>().text = "1";
            itemToInsertImage = Instantiate(itemPrefab, mainInv.transform);

            itemToInsert = ItemDropTable.i.allItems[int.Parse(data.ID)];

            mainInv.inventory[firstBlankSpace.Value.y, firstBlankSpace.Value.x].item = itemToInsert;
            mainInv.inventory[firstBlankSpace.Value.y, firstBlankSpace.Value.x].stack = 1;
            mainInv.inventory[firstBlankSpace.Value.y, firstBlankSpace.Value.x].invObject = itemToInsertImage;

            itemToInsert = null;

            itemToInsertImage.transform.localPosition = new Vector2(firstBlankSpace.Value.x * mainInv.tileWidthPx + mainInv.tileWidthPx * 0.5f, -(firstBlankSpace.Value.y * mainInv.tileHeightPx + mainInv.tileHeightPx * 0.5f));

            itemToInsertImage = null;
        }//place item in new slot
        else {
            
        }//inv full
    }

    private void placeItem((int x, int y) squareReference) {

        //if square empty, put item in square
        if (selectedInvGrid.inventory[squareReference.y, squareReference.x].item == null) {
            selectedInvGrid.inventory[squareReference.y, squareReference.x].item = selectedItem.item;
            selectedInvGrid.inventory[squareReference.y, squareReference.x].stack = selectedItem.stack;
            selectedInvGrid.inventory[squareReference.y, squareReference.x].invObject = selectedItemImage;

            selectedItem.item = null;
            selectedItem.stack = 0;
            placeItemObject(squareReference);

            itemSelected = false;
            
        }

        //if square contains the same item and item is stackable, increase stack
        else if (selectedInvGrid.inventory[squareReference.y, squareReference.x].item.ID == selectedItem.item.ID && selectedInvGrid.inventory[squareReference.y, squareReference.x].item.Stackable) {
            selectedInvGrid.inventory[squareReference.y, squareReference.x].stack += selectedItem.stack;
            selectedInvGrid.inventory[squareReference.y, squareReference.x].invObject.GetComponentInChildren<Text>().text = selectedInvGrid.inventory[squareReference.y, squareReference.x].stack.ToString();

            selectedItem.item = null;
            selectedItem.stack = 0;
            Destroy(selectedItemImage);

            itemSelected = false;
        }

        //otherwise, swap the selected item and square item around
        else {

            (InventoryItem item, int stack) tempData = selectedItem;
            GameObject tempObject = selectedItemImage;
            StopAllCoroutines();
            placeItemObject(squareReference);
            
            selectItem(squareReference);
            selectedInvGrid.inventory[squareReference.y, squareReference.x].item = tempData.item;
            selectedInvGrid.inventory[squareReference.y, squareReference.x].stack = tempData.stack;
            selectedInvGrid.inventory[squareReference.y, squareReference.x].invObject = tempObject;

            //dont think about this too hard its very unthinky
        }
        //Debug.Log($"{inventory[squareReference.y, squareReference.x].item.ID}, {inventory[squareReference.y, squareReference.x].stack}");
    }

    private void placeItemObject((int x, int y) squareReference) {
        selectedItemImage.transform.SetParent(selectedInvGrid.transform);

        selectedItemImage.transform.localPosition = new Vector2(squareReference.x * selectedInvGrid.tileWidthPx + selectedInvGrid.tileWidthPx * 0.5f, -(squareReference.y * selectedInvGrid.tileHeightPx + selectedInvGrid.tileHeightPx * 0.5f));

        selectedItemImage = null;
    }

    private void selectItem((int x, int y) squareReference) {
        if (selectedInvGrid.inventory[squareReference.y, squareReference.x].item == null) { return; }
        


        selectedItem.item = selectedInvGrid.inventory[squareReference.y, squareReference.x].item;
        selectedItem.stack = selectedInvGrid.inventory[squareReference.y, squareReference.x].stack;
        selectedItemImage = selectedInvGrid.inventory[squareReference.y, squareReference.x].invObject;

        selectedItemImage.transform.SetParent(UICanvas.transform);

        selectedInvGrid.inventory[squareReference.y, squareReference.x].item = null;
        selectedInvGrid.inventory[squareReference.y, squareReference.x].stack = 0;
        selectedInvGrid.inventory[squareReference.y, squareReference.x].invObject = null;

        StartCoroutine(drag(selectedItemImage));
        


    }

    IEnumerator drag(GameObject item) {
        itemSelected = true;
        while (selectedItem.item != null) {
            item.transform.position = Input.mousePosition;
            yield return null;
        }
    }

    private (int x, int y) getInvSquareCoords(Vector2 mousePos) {
        return ((int)((mousePos.x - selectedInvGrid.GetComponent<RectTransform>().position.x) / selectedInvGrid.tileWidthPx), (int)((selectedInvGrid.GetComponent<RectTransform>().position.y - mousePos.y) / selectedInvGrid.tileHeightPx));
    }
}
