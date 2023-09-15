using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InventoryItemData : MonoBehaviour
{
    public GameObject Item;

    public Sprite UIImage;

    public string ID;

    public bool Stackable;

    public MonoScript Use;
}
