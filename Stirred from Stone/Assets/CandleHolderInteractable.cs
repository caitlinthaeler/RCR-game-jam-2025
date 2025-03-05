using UnityEngine;

public class CandleHolderInteractable : MonoBehaviour, ICollectable
{
    public Transform playerHoldPoint;
    private Transform candle;
    private bool isHeld = false;
    public ItemObject itemObject;
    public string Name => itemObject.itemName;

    private void Start()
    {
        candle = transform.parent; // Get the parent of this object
    }

    public void Interact()
    {
        //
    }

    public void Pickup()
    {
        InventoryHandler.Instance.AddItem(itemObject);
        isHeld = true;
        candle.SetParent(playerHoldPoint); // Attach to player's hold point
        candle.localPosition = Vector3.zero; // Align position
        candle.localRotation = Quaternion.identity; // Reset rotation
    }
}
