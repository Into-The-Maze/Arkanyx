using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InvGrid : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int invWidth;
    public int invHeight;


    [HideInInspector] public int tileWidthPx = 32;
    [HideInInspector] public int tileHeightPx = 32;

    public bool Hotbar;

    public (InventoryItem item, int stack, GameObject invObject)[,] inventory;

    public RectTransform UICanvas;
    RectTransform invGrid;

    private void Awake() {
        
        invGrid = GetComponent<RectTransform>();
        initInventory();
    }

    private void initInventory() {
        inventory = new (InventoryItem item, int stack, GameObject invObject)[invHeight, invWidth];
        

        //initialises the inventory for the first time
        for (int i = 0; i < invHeight; i++) {
            for (int j = 0; j < invWidth; j++) {
                inventory[i, j].item = null;
                inventory[i, j].stack = 0;
            }
        }

        //sets grid size based on number of grid squares along its width and height
        invGrid.sizeDelta = new Vector2(invWidth * tileWidthPx, invHeight * tileHeightPx);


        //positions grid exactly in centre of canvas. Might be unnecessary depending on how this works out.
        
        //invGrid.position = new Vector2(UICanvas.position.x - invGrid.sizeDelta.x * 0.5f, UICanvas.position.y + invGrid.sizeDelta.y * 0.5f);

        //if (Hotbar) invGrid.position += new Vector3(0, -UICanvas.sizeDelta.y * 0.5f + 25, 0);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        InvController.selectedInvGrid = this;
    }

    public void OnPointerExit(PointerEventData eventData) {
        InvController.selectedInvGrid = null;
    }
}
