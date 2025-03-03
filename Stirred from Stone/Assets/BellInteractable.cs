using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BellInteractable : MonoBehaviour, IInteractable
{
    public BellObject bellObject;
    public string Name => bellObject.itemName;
    private AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }
    public void Interact()
    {
        Debug.Log("Interacting with Bell.");
        BellManager.Instance.RingBell(bellObject.BellNumber);

        PlaySound();
    }

    public void PlaySound()
    {
        if (bellObject.audioFile != null)
        {
            audioSource.PlayOneShot(bellObject.audioFile);
        }
        
    }
}
