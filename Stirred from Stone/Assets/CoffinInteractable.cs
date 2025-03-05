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
        else
        {
            CloseCoffin();
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
    public void CloseCoffin()
    {
        isOpen = false;
        if (audioSource && coffinOpeningSound != null)
        {
            audioSource.PlayOneShot(coffinOpeningSound);
        }
        if (movementAnimator)
        {
            movementAnimator.ReverseMovement(OnRevealComplete);
        }
    }

    public void OnRevealComplete()
    {
        DialogueManager.Instance.AddActionDialogue("Oh great, it's a skeleton");
    }

    public void OnCloseComplete()
    {
        DialogueManager.Instance.AddActionDialogue("I'll close this just in case...");
    }
}
