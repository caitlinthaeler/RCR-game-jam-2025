using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }
    public string dialogueText;
    public string speakerName;
    public TextMeshProUGUI textBox;
    private void Awake()
    {
        Instance = this;
    }
    // St
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogueText = "";
        speakerName= "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddDialogue(string speaker, string dialogue)
    {
        dialogueText = dialogue;
        speakerName = speaker;
        textBox.text = $"{speaker}: '{dialogue}'";
    }

    public void AddActionDialogue(string dialogue)
    {
        dialogueText = dialogue;
        textBox.text = dialogue;
    }
}
