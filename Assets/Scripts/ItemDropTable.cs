using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropTable : MonoBehaviour
{
    public List<InventoryItem> allItems;

    //add more lists to make drop tables



    public static ItemDropTable i;
    private void Start() {
        i = GetComponent<ItemDropTable>();
    }
}
