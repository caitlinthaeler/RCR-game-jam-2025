using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    public InventoryUI inventoryUI;
    public InfoDisplay infoDisplay;

    void Awake()
    {
        Instance = this;
        infoDisplay.gameObject.SetActive(false);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        infoDisplay.gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && infoDisplay.gameObject.activeSelf)
        {
            ExitInfoCard();
        }
    }

    public void DisplayInfoCard(string title, string description, Sprite icon)
    {
        infoDisplay.SetInfo(title, description, icon);
        infoDisplay.gameObject.SetActive(true);
    }

    public void ExitInfoCard()
    {
        infoDisplay.gameObject.SetActive(false);
    }
}
