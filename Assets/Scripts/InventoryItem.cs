using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class InventoryItem : ScriptableObject
{
    public GameObject Item;

    public Sprite UIImage;

    public string ID;

    public bool Stackable;

    public MonoScript Use;
}
