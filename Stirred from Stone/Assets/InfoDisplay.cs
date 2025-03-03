using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI title;
    [SerializeField]
    private TextMeshProUGUI description;
    [SerializeField]
    private Image iconImage;
    [SerializeField]
    private Button exitButton;
    public string Title => title.text;
    public string Description => description.text;
    public Sprite Icon => iconImage.sprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        exitButton.onClick.AddListener(OnExitButtonClick);
    }

    private void OnExitButtonClick()
    {
        UIManager.Instance.ExitInfoCard(); // Call ExitInfoCard from UIManager
    }

    public void SetInfo(string titleText, string descriptionText, Sprite icon=null)
    {
        title.text = titleText;
        description.text = descriptionText;
        if (icon != null)
        {
            iconImage.gameObject.SetActive(true);
            iconImage.sprite = icon;
        } else {
            iconImage.gameObject.SetActive(false);
        }
        
    }
}
