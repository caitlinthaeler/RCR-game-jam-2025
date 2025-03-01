using UnityEngine;

public class StoneInteractable : MonoBehaviour, IInteractable
{
    public string Name => "Stone";
    private bool returned = false;
    public ItemObject itemObject;

    public StoneInteractable(bool Returned)
    {
        returned = Returned;
    }

    public void Interact()
    {
        if (!returned)
        {
            if (InventoryHandler.Instance.CanAddItem())
            {
                InventoryHandler.Instance.AddItem(itemObject);
                returned = true;
                Destroy(gameObject);
                return;
            }
        }
        Debug.Log("This stone has been returned to the right place.");
    }
}
