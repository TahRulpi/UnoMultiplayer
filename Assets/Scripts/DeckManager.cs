using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{
    [System.Serializable]
    public class CardData
    {
        public string color;
        public string value;
        public Sprite sprite;
    }

    public GameObject cardPrefab;         // UNO Card Prefab
    public Transform deckParent;          // Where to place cards in the scene
    public List<CardData> allCards = new List<CardData>(); // List of all cards

    void Start()
    {
        GenerateDeck();
    }

    void GenerateDeck()
    {
        foreach (CardData data in allCards)
        {
            GameObject card = Instantiate(cardPrefab, deckParent);
            UNOCard unoCard = card.GetComponent<UNOCard>();
            unoCard.SetCard(data.color, data.value, data.sprite);
        }
    }
}
