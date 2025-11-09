using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
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

    [Header("Deck Setup")]
    public GameObject cardPrefab;
    public RectTransform deckParent; // The deck center UI position
    public List<CardData> allCards = new List<CardData>();

    [Header("Players")]
    public RectTransform[] playerAreas; // 4 player areas (UI)
    public int cardsPerPlayer = 7;

    [Header("Animation")]
    public float dealDelay = 0.15f;
    public float moveDuration = 0.4f;
    public float cardSpacing = 55f; // space between cards
    public float cardFanAngle = 6f; // optional fan angle

    private List<List<GameObject>> playerHands = new List<List<GameObject>>();

    void Start()
    {
        Shuffle(allCards);
        StartCoroutine(DistributeCardsAnimated());
    }

    void Shuffle(List<CardData> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            CardData temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }

    IEnumerator DistributeCardsAnimated()
    {
        playerHands.Clear();
        for (int i = 0; i < playerAreas.Length; i++)
            playerHands.Add(new List<GameObject>());

        int totalPlayers = playerAreas.Length;
        int totalCardsToDeal = totalPlayers * cardsPerPlayer;

        for (int i = 0; i < totalCardsToDeal && i < allCards.Count; i++)
        {
            int playerIndex = i % totalPlayers;
            CardData data = allCards[i];

            // --- Determine Player Layout ---
            RectTransform targetArea = playerAreas[playerIndex];
            int cardCount = playerHands[playerIndex].Count;

            // 0: Bottom, 1: Left, 2: Top, 3: Right
            bool isVertical = (playerIndex == 1 || playerIndex == 3);
            float baseRotation = 0f;

            if (playerIndex == 1) baseRotation = 90f;  // Left player
            else if (playerIndex == 2) baseRotation = 180f; // Top player
            else if (playerIndex == 3) baseRotation = -90f; // Right player
            // --- End Player Layout ---

            // Create the card at deck center
            GameObject card = Instantiate(cardPrefab, deckParent.parent);
            RectTransform cardRect = card.GetComponent<RectTransform>();
            cardRect.anchoredPosition = deckParent.anchoredPosition;

            UNOCard unoCard = card.GetComponent<UNOCard>();
            unoCard.SetCard(data.color, data.value, data.sprite);




            playerHands[playerIndex].Add(card);

            // Calculate the final target LOCAL position (relative to targetArea center)
            // *** FIX 1: Pass 'isVertical' to the positioning function ***
            Vector2 localTargetPos = GetCardTargetPosition(targetArea, cardCount, isVertical);

            // Calculate card fan angle relative to the base hand rotation
            float fanAngle = (cardCount - 1 - ((float)cardsPerPlayer - 1) / 2f) * cardFanAngle;
            float finalRotationZ = baseRotation + fanAngle;


            // --- DOTWEEN.FROM() LOGIC for smooth path ---

            // 1. Change the card's parent immediately (no worldPositionStays: false)
            cardRect.SetParent(targetArea, false);

            // 2. Calculate the Deck position *relative to the TargetArea*
            Vector2 deckLocalPos;
            Camera canvasCam = targetArea.GetComponentInParent<Canvas>().worldCamera;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                targetArea,
                RectTransformUtility.WorldToScreenPoint(canvasCam, deckParent.position),
                canvasCam,
                out deckLocalPos);

            // 3. Animate TO the final position, starting FROM the calculated Deck position (deckLocalPos)
            cardRect.DOAnchorPos(localTargetPos, moveDuration)
                .SetEase(Ease.OutCubic)
                .From(deckLocalPos);

            // Animate rotation
            cardRect.DORotate(new Vector3(0, 0, finalRotationZ), moveDuration);
            // --- END DOTWEEN.FROM() LOGIC ---

            yield return new WaitForSeconds(dealDelay);
        }

        Debug.Log("?? Smooth dealing complete!");
    }

    // Function to position cards neatly
    // *** FIX 2: Update function signature to accept 'isVertical' ***
    Vector2 GetCardTargetPosition(RectTransform area, int cardCount, bool isVertical)
    {
        // Total width/height of the hand (N cards require N-1 spacing units)
        float totalSpacingUnits = cardsPerPlayer - 1;
        float totalHandSize = totalSpacingUnits * cardSpacing;

        // Start offset from the center
        float startOffset = -totalHandSize / 2f;

        // Final position is Start position + accumulated spacing
        // (cardCount - 1) is the 0-based index.
        float pos = startOffset + (cardCount - 1) * cardSpacing;

        if (isVertical)
        {
            // For side players, space vertically (Y axis)
            return new Vector2(0, pos);
        }
        else
        {
            // For top/bottom players, space horizontally (X axis)
            return new Vector2(pos, 0);
        }
    }
}