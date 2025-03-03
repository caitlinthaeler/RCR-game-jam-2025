using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CrosshairText : MonoBehaviour
{
    
    public TextMeshProUGUI textBox;
    public string currentText => textBox.text;
    private Coroutine fadeCoroutine;
    private IInteractable lastInteractedObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textBox.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText(string text)
    {
        textBox.text = text;
    }

    public void DisplayText(IInteractable interactable)
    {
        SetText(interactable.Name);
        textBox.alpha = 1f; // Ensure text is fully visible

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine); // Stop any existing fade coroutine
        }

        lastInteractedObject = interactable; // Store the last interacted object
        fadeCoroutine = StartCoroutine(FadeTextAfterDelay(2f)); // Start fade after 2 seconds
    }

    private IEnumerator FadeTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // If the player has interacted with a new object, don't fade
        if (textBox.text != lastInteractedObject.Name)
        {
            yield break;
        }

        // Gradually fade out text
        float duration = 1f;
        float startAlpha = textBox.alpha;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            textBox.alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        textBox.alpha = 0f; // Ensure it's fully invisible
    }
}
