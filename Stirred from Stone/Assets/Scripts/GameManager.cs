using UnityEngine;

public class GameManager : MonoBehaviour
{

    public StoneManager stoneManager;
    public DialogueManager dialogueManager;
    public string NarraratorName = "Narrarator";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void WinGame()
    {
        //remove barrier
        Debug.Log("The curse has been sealed away once more!");
        dialogueManager.AddDialogue(NarraratorName, "The curse has been sealed away once more!");
    }
}
