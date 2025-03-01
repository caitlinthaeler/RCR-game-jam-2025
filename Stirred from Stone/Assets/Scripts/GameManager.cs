using UnityEngine;

public class GameManager : MonoBehaviour
{

    public StoneManager stoneManager;
    public DialogueManager dialogueManager;
    public string NarraratorName = "Narrarator";

    // Start is called once before the first execution of Update after the MonoBehaviour is created

     private void OnEnable()
    {
        stoneManager.OnStonesSealed += WinGame;
    }

    private void OnDisable()
    {
       stoneManager.OnStonesSealed -= WinGame;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void WinGame()
    {
        dialogueManager.AddDialogue(NarraratorName, "You have brought back all the stones. The curse has been lifted. You have won the game!");
    }
}
