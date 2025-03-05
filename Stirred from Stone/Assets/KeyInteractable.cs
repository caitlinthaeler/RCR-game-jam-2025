using UnityEngine;

public class KeyInteractable : MonoBehaviour, ICollectable
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
        //DialogueManager.Instance.AddActionDialogue("Oh, it looks like someone dropped this key while they were down here. I'll return it to the church later.");
        Destroy(gameObject);
    }
}
