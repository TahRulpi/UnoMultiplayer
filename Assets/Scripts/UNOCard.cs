using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening; // Keep this if you want an animated flip later

public class UNOCard : MonoBehaviour
{
    public string cardColor;
    public string cardValue;

    [Header("Face Elements")]
    public Image cardImage; // The main image/sprite of the card face
    public TextMeshProUGUI cardText; // The number/value text

    [Header("Back Element")]
    public Image cardBackImage; // Drag your card back image component here in the Inspector

    // Assign card data and update UI
    public void SetCard(string color, string value, Sprite sprite)
    {
        cardColor = color;
        cardValue = value;

        if (cardImage != null) cardImage.sprite = sprite;
        if (cardText != null) cardText.text = value;

        // Ensure face starts visible (if not explicitly hidden)
        if (cardImage != null) cardImage.enabled = true;
        if (cardBackImage != null) cardBackImage.enabled = false;
    }

    // Method to show the card back (for other players)
    public void ShowBack()
    {
        if (cardImage != null) cardImage.enabled = false;
        if (cardText != null) cardText.enabled = false;
        if (cardBackImage != null) cardBackImage.enabled = true;
    }

    // Method to show the card face (for local player/reveal)
    public void ShowFace()
    {
        if (cardImage != null) cardImage.enabled = true;
        if (cardText != null) cardText.enabled = true;
        if (cardBackImage != null) cardBackImage.enabled = false;
    }
}