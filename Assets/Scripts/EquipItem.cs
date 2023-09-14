using System.Collections;
using System.Collections.Generic;
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
        instance.weaponPrefab = HotbarSelect.equippedItem.Item;
        instance.weaponPrefab.GetComponent<Rigidbody>().isKinematic = true;
        instance.weaponPrefab.GetComponent<Rigidbody>().useGravity = false;
        //equippedObject = Instantiate(instance.weaponPrefab, new Vector3(0.3f, 0.7f, 0.2f), instance.player.transform.rotation * Quaternion.Euler(0f, 90f, 30f), instance.playerCamera.transform);

        equippedObject = (GameObject)Instantiate((Object)instance.weaponPrefab, instance.player.transform, false);
        equippedObject.transform.rotation = instance.player.transform.rotation * niceEquipRotation;
        equippedObject.transform.localPosition = niceEquipPosition;
    }

    public static void destroyEquippedItem() {
        Destroy(equippedObject);
        HotbarSelect.equippedItem = null;
    }

    private void Awake() {
        instance = this;
    }
}
