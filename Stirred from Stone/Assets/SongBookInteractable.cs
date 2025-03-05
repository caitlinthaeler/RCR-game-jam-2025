using UnityEngine;

public class SongBookInteractable : MonoBehaviour
{
    public ItemObject itemObject;
    public string Name => itemObject.itemName;

    public void Interact()
    {
        //
    }

    public void Pickup()
    {
        InventoryHandler.Instance.AddItem(itemObject);
        Destroy(gameObject);
    }
}
