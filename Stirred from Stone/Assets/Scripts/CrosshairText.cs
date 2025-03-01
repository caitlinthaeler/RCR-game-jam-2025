using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CrosshairText : MonoBehaviour
{
    public string crosshairText;
    public TextMeshProUGUI textBox;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        crosshairText = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText(string text)
    {
        crosshairText = text;
        textBox.text = text;
    }
}
