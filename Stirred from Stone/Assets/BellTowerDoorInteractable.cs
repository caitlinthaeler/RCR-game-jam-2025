using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class BellTowerDoorInteractable : MonoBehaviour, IInteractable
{
    
    public string Name => "Bell Tower Door";
    private Animator doorAnimator;
    private bool isOpen = false;
    public AudioSource audioSource;
    public AudioClip doorUnlockingSound;
    public AudioClip doorCreakingSound;
    
    void Start()
    {
        doorAnimator = GetComponent<Animator>();
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
                    return;
                }
            }
            DialogueManager.Instance.AddActionDialogue("This door is locked");
        }
    }

    public void UnlockDoor()
    {
        Debug.Log("Unlocking Bell Tower Door.");
        
        BellTowerDoorManager.Instance.DoorUnlocked();
        if (doorUnlockingSound != null)
        {
            audioSource.PlayOneShot(doorUnlockingSound);
            StartCoroutine(WaitForSoundThenOpenDoor(doorUnlockingSound.length));
        } else {
            OpenDoor();
        }
    }

    public IEnumerator WaitForSoundThenOpenDoor(float duration)
    {
        yield return new WaitForSeconds(duration);
        OpenDoor();
    }

    public void OpenDoor()
    {
        isOpen = true;
        if (doorAnimator)
        {
            doorAnimator.SetBool("isOpen", isOpen);
        }
        if (doorCreakingSound != null)
        {
            audioSource.PlayOneShot(doorCreakingSound);
        }
        BellTowerDoorManager.Instance.DoorUnlocked();
    }
}
