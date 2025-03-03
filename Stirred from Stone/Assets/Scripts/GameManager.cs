using UnityEngine;

public class GameManager : MonoBehaviour
{

    public StoneManager stoneManager;
    public BellManager bellManager;
    public DialogueManager dialogueManager;
    public string NarraratorName = "Narrarator";

    // Start is called once before the first execution of Update after the MonoBehaviour is created

     private void OnEnable()
    {
        bellManager.OnBellsRungInCorrectOrder += BellsEvent;
        // windowsill
        // book
        // crypt
        // graveyard
        stoneManager.OnStonesSealed += WinGame;
    }

    private void OnDisable()
    {
    bellManager.OnBellsRungInCorrectOrder -= BellsEvent;
       stoneManager.OnStonesSealed -= WinGame;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void BellsEvent()
    {
        dialogueManager.AddDialogue(NarraratorName, "You have rung the bells in a particular order. It seems like the draft has disappeared in the main hall");
    }

    void WinGame()
    {
        dialogueManager.AddDialogue(NarraratorName, "You have brought back all the stones. The curse has been lifted. You have won the game!");
    }
}
