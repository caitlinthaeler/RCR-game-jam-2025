using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class InventoryHandler : MonoBehaviour
{
    public static InventoryHandler Instance { get; private set; }
    public InventoryUI inventoryUI;
    public int numSlots;
    public List<ItemObject> items;


    void Start(){
        items = new List<ItemObject>();
        inventoryUI.UpdateInventoryUI(items);
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddItem(ItemObject item)
    {
        if (CanAddItem())
        {
            items.Add(item);
            inventoryUI.UpdateInventoryUI(items);
            Debug.Log($"Added {item.itemName} to inventory!");
        }
        else
        {
            Debug.Log("Inventory full!");
        }
    }

     public void RemoveItem(int i)
    {
        if (i < items.Count)
        {
            items.RemoveAt(i);
            inventoryUI.UpdateInventoryUI(items);
        }
    }

    public bool CanAddItem()
    {
        return items.Count < numSlots;
    }
}
