using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class StatueInteractable : MonoBehaviour, IInteractable
{
    public string Name => "Singing Statue";
    
    private bool statueSatisfied = false;
    public AudioSource audioSource;
    public AudioClip statueSinging;

    

    public void Interact()
    {
        Debug.Log("Interacting with the Statue.");
        if (!statueSatisfied)
        {
            for (int i = 0; i < InventoryHandler.Instance.items.Count; i++)
            {
                ItemObject itemObject = InventoryHandler.Instance.items[i];
                if (itemObject != null && itemObject.itemName == "Song Book")
                {
                    InventoryHandler.Instance.RemoveItem(i);
                    statueSatisfied = true;
                    StatueManager.Instance.RevealStone();
                    Sing();
                    return;
                }
            }
            DialogueManager.Instance.AddActionDialogue("The statue...");
        }
    }

    public void Sing()
    {
        if (audioSource && statueSinging)
        {
            audioSource.PlayOneShot(statueSinging);
        }
    }

    

}
