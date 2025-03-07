using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public List<InventoryItem> items = new List<InventoryItem>();
    public int maxSlots = 20;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    public bool AddItem(ItemData itemData, int quantity)
    {
        if (itemData.isStackable)
        {
            InventoryItem existingItem = items.Find(i => i.itemData == itemData);
            if (existingItem != null)
            {
                existingItem.quantity += quantity;
                return true;
            }
        }

        if (items.Count < maxSlots)
        {
            items.Add(new InventoryItem(itemData, quantity));
            return true;
        }

        return false;
    }

    public void RemoveItem(ItemData itemData, int quantity)
    {
        InventoryItem item = items.Find(i => i.itemData == itemData);
        if (item != null)
        {
            item.quantity -= quantity;
            if (item.quantity <= 0) items.Remove(item);
        }
    }
}

[System.Serializable]
public class InventoryItem
{
    public ItemData itemData;
    public int quantity;

    public InventoryItem(ItemData data, int qty)
    {
        itemData = data;
        quantity = qty;
    }
}
