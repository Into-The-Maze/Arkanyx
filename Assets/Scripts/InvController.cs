using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private const int invWidth = 10;
    private const int invHeight = 4;
    private const int tileWidthPx = 32;
    private const int tileHeightPx = 32;

    public static bool active;

    public RectTransform invGrid;
    public RectTransform UICanvas;

    public static (InventoryItem item, int stack)[,] inventory;

    

    private void Start() {
        active = false;
        initInventory();
    }

    private void Update() {
        if (!active) { return; }

        if (Input.GetMouseButtonDown(0)) {
            (int x, int y) = getInvSquareCoords(Input.mousePosition);
            Debug.Log($"Found square at {y} {x}");
        }
    }


    private (int, int) getInvSquareCoords(Vector2 mousePos) {
        return ((int)((mousePos.x - invGrid.position.x) / tileWidthPx), (int)((invGrid.position.y - mousePos.y) / tileHeightPx));
    }

    private void initInventory() {
        inventory = new (InventoryItem item, int stack)[invHeight, invWidth];
        invGrid = GetComponent<RectTransform>();

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
