using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public List<Image> slotImages; // List of Image components (your inventory slots)
    
    // Update the inventory UI with item sprites
    public void UpdateInventoryUI(List<ItemObject> itemObjects)
    {
        for (int i = 0; i < slotImages.Count; i++)
        {
            if (i < itemObjects.Count)
            {
                slotImages[i].sprite = itemObjects[i].itemIcon; // Set the sprite of the slot
                slotImages[i].enabled = true; // Show the sprite in the UI slot
            }
            else
            {
                slotImages[i].enabled = false; // Hide the slot if no item is assigned
            }
        }
    }
}
