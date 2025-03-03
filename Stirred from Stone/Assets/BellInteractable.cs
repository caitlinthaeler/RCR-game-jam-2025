using UnityEngine;
using System;
using System.Collections;

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
        PlaySoundAndExecute(() => BellManager.Instance.RingBell(bellObject.BellNumber));
    }

    public void PlaySoundAndExecute(Action callback)
    {
        if (bellObject.audioFile != null)
        {
            audioSource.PlayOneShot(bellObject.audioFile);
            StartCoroutine(WaitForSound(callback, bellObject.audioFile.length));
        }
        else
        {
            Debug.LogWarning("No audio file assigned.");
            callback?.Invoke(); // Execute callback immediately if no sound
        }
    }

    private IEnumerator WaitForSound(Action callback, float duration)
    {
        yield return new WaitForSeconds(duration);
        callback?.Invoke(); // Execute the action after the sound finishes
    }
}
