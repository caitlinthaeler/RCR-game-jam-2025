using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class CoffinInteractable : MonoBehaviour, IInteractable
{
    
    public string Name => "Coffin";
    public MovementAnimator movementAnimator;
    private bool isOpen = false;
    public AudioSource audioSource;
    public AudioClip coffinOpeningSound;
    
    public void Interact()
    {
        Debug.Log("Interacting with Coffin.");
        if (!isOpen)
        {
            OpenCoffin();
            
        }
    }
    public void OpenCoffin()
    {
        isOpen = true;
        if (audioSource && coffinOpeningSound != null)
        {
            audioSource.PlayOneShot(coffinOpeningSound);
        }
        if (movementAnimator)
        {
            movementAnimator.StartMovement(OnRevealComplete);
        }
    }

    public void OnRevealComplete()
    {
        DialogueManager.Instance.AddActionDialogue("Oh great, it's a skeleton");
    }
}
