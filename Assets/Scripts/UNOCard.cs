using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UNOCard : MonoBehaviour
{
    public string cardColor;
    public string cardValue;

    public Image cardImage;
    public TextMeshProUGUI cardText;

    // Assign card data and update UI
    public void SetCard(string color, string value, Sprite sprite)
    {
        cardColor = color;
        cardValue = value;

        if (cardImage != null) cardImage.sprite = sprite;
        if (cardText != null) cardText.text = value;
    }
}
