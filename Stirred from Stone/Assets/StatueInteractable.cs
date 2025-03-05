using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class StatueInteractable : MonoBehaviour
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
            foreach (var item in InventoryHandler.Instance.items)
            {
                ItemObject itemObject = item;
                if (itemObject != null && itemObject.itemName == "Song Book")
                {
                    statueSatisfied = true;
                    Sing();
                    StatueManager.Instance.RevealStone();
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
