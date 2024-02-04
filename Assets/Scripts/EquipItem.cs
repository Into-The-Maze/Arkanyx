using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EquipItem : MonoBehaviour
{
    public static GameObject equippedObject;
    private static EquipItem instance;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject weaponPrefab;

    private static Vector3 niceEquipPosition = new(0.3f, -0.4f, 0.2f);
    private static Quaternion niceEquipRotation = Quaternion.Euler(0f, 90f, 30f);

    public static void instantiateEquippedItem() {

        //get weapon from hotbar
        instance.weaponPrefab = HotbarSelect.equippedItem.Item;

        //stop it colliding with the player
        instance.weaponPrefab.GetComponent<Rigidbody>().isKinematic = true;
        instance.weaponPrefab.GetComponent<Rigidbody>().useGravity = false;

        //instantiate and move to nice position next to player
        equippedObject = (GameObject)Instantiate((Object)instance.weaponPrefab, instance.player.transform, false);
        equippedObject.transform.rotation = instance.player.transform.rotation * niceEquipRotation;
        equippedObject.transform.localPosition = niceEquipPosition;
        instance.weaponPrefab.GetComponent<Rigidbody>().isKinematic = false;
        instance.weaponPrefab.GetComponent<Rigidbody>().useGravity = true;
    }

    private void Update() {
        if (equippedObject == null) { return; }

        if (Input.GetMouseButtonDown(0) && !UIToggler.inventoryOpen) {
            equippedObject.GetComponent<Use0002>().Use();
        }
    }

    public static void destroyEquippedItem() {
        Destroy(equippedObject);
        HotbarSelect.equippedItem = null;
    }

    private void Awake() {
        instance = this;
    }
}
