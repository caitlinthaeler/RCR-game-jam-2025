using UnityEngine;

public class GameManager : MonoBehaviour
{

    public StoneManager stoneManager;
    public BellManager bellManager;
    public BellTowerDoorManager bellTowerDoorManager;
    public DialogueManager dialogueManager;
    public string NarraratorName = "Narrarator";

    // Start is called once before the first execution of Update after the MonoBehaviour is created

     private void OnEnable()
    {
        bellManager.OnBellsRungInCorrectOrder += BellsEvent;
        bellTowerDoorManager.OnBellTowerDoorUnlocked += BellTowerDoorOpenEvent;
        // windowsill
        // book
        // crypt
        // graveyard
        stoneManager.OnStonesSealed += WinGame;
    }

    private void OnDisable()
    {
        bellManager.OnBellsRungInCorrectOrder -= BellsEvent;
        bellTowerDoorManager.OnBellTowerDoorUnlocked -= BellTowerDoorOpenEvent;
       stoneManager.OnStonesSealed -= WinGame;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // stone
    void BookEvent()
    {
        dialogueManager.AddDialogue(NarraratorName, "Those are some eerie noises coming from that statue... oh look! A stone has been revealed.");
    }

    // stone
    void GraveyardEvent()
    {
        dialogueManager.AddDialogue(NarraratorName, "");
    }

    // stone
    void WindowsillEvent()
    {
        dialogueManager.AddDialogue(NarraratorName, "You have found the windowsill");
    }

    void KeyFoundEvent()
    {
        dialogueManager.AddDialogue(NarraratorName, "You have found a key. Someone must have left it down here");
    }

    // stone
    void CryptOpenEvent()
    {
        dialogueManager.AddDialogue(NarraratorName, "You have opened the crypt");
    }

    void BellTowerDoorOpenEvent()
    {
        dialogueManager.AddDialogue(NarraratorName, "You have unlocked the door to the bell tower");
    }

    void BellsEvent()
    {
        dialogueManager.AddDialogue(NarraratorName, "You have rung the bells in a particular order. It seems like the draft has disappeared in the main hall");
    }

    // stone
    void CandlesEvent()
    {
        dialogueManager.AddDialogue(NarraratorName, "You have lit all the candles. The room is now illuminated, and a mysterious box has appeared");
    }

    void WinGame()
    {
        dialogueManager.AddDialogue(NarraratorName, "You have brought back all the stones. The curse has been lifted. You have won the game!");
    }
}
