using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractController : MonoBehaviour
{
    public Text prompt;

    private const float maxInteractDistance = 100f;

    private bool crRunning;

    private void Update() {

        if (UIToggler.inventoryOpen) return;

        Ray ray = new Ray(transform.position, transform.forward);
        if (itemCheck(ray) && !crRunning) {
            StartCoroutine(pickUp(ray));
            prompt.text = "Pick up (E)";
        }
        else if (itemCheck(ray) && crRunning) {
            return;
        }
        else {
            prompt.text = string.Empty;
            crRunning = false;
        }
    }

    private bool itemCheck(Ray ray) {
        return Physics.Raycast(ray, out RaycastHit hit, maxInteractDistance) && hit.collider.gameObject.CompareTag("Item");
    }

    private IEnumerator pickUp(Ray ray) {
        crRunning = true;
        while (crRunning) {
            if (Input.GetKeyDown(KeyCode.E)) {
                Physics.Raycast(ray, out RaycastHit hit, maxInteractDistance);
                Camera.main.GetComponent<InvController>().insertItem(hit.collider.gameObject.GetComponent<InventoryItemData>());
                Destroy(hit.collider.gameObject);
            }
            yield return null;
        }
    }
}
