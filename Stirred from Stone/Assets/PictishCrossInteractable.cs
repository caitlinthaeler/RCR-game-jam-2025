using UnityEngine;

public class PictishCrossInteractable : MonoBehaviour, IInteractable
{
    public ItemObject itemObject;
    public string Name => itemObject.itemName;

    public void Interact()
    {
        Debug.Log("Supposed to display info card for Pictish Cross.");
        UIManager.Instance.DisplayInfoCard(itemObject.itemName, itemObject.itemDescription, itemObject.itemIcon);
    }
}