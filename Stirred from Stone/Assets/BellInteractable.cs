using UnityEngine;

public class BellInteractable : MonoBehaviour, IInteractable
{
    public BellObject bellObject;
    public string Name => bellObject.itemName;
    public AudioSource audioSource;
    public void Interact()
    {
        BellManager.Instance.RingBell(bellObject.BellNumber);

        PlaySound();
    }

    public void PlaySound()
    {
        audioSource.PlayOneShot(bellObject.audioFile);
    }
}
