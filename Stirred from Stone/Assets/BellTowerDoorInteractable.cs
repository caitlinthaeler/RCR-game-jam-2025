using UnityEngine;
using System;

public class BellTowerDoorInteractable : MonoBehaviour, IInteractable
{
    
    public string Name => "Bell Tower Door";
    private Animator animator;
    private bool isOpen = false;
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    public void Interact()
    {
        Debug.Log("Interacting with Bell Tower Door.");
        if (!isOpen)
        {
            foreach (var item in InventoryHandler.Instance.items)
            {
                ItemObject itemObject = item;
                if (itemObject != null && itemObject.itemName == "Key")
                {
                    UnlockDoor();
                    break;
                }
            }
            DialogueManager.Instance.AddActionDialogue("This door is locked");
        }
    }

    public void UnlockDoor()
    {
        Debug.Log("Unlocking Bell Tower Door.");
        isOpen = true;
        animator.SetBool("isOpen", isOpen);
        BellTowerDoorManager.Instance.DoorUnlocked();
    }
}
