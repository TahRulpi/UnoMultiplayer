using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class CardDistributor : MonoBehaviour
{
    public List<Transform> deckPositions; // Assign 4 deck positions in Inspector
    public GameObject cardPrefab;
    public int cardsToDistribute = 12;
    public float delayBetweenCards = 0.2f;

    private void Start()
    {
        DistributeCards();
    }

    void DistributeCards()
    {
        int deckCount = deckPositions.Count;

        for (int i = 0; i < cardsToDistribute; i++)
        {
            GameObject card = Instantiate(cardPrefab, transform.position, Quaternion.identity);

            int deckIndex = i % deckCount;
            Transform targetDeck = deckPositions[deckIndex];

            // Set rotation direction (left deck cards tilt left, right ones tilt right)
            float rotateZ = 0f;
            if (deckIndex == 0 || deckIndex == 1) rotateZ = -25f;
            else if (deckIndex == 2 || deckIndex == 3) rotateZ = 25f;

            // Animate card movement and rotation
            Sequence s = DOTween.Sequence();
            s.AppendInterval(i * delayBetweenCards); // delay for each card
            s.Append(card.transform.DOMove(targetDeck.position, 0.6f).SetEase(Ease.OutQuad));
            s.Join(card.transform.DORotate(new Vector3(0, 0, rotateZ), 0.5f));
            s.Join(card.transform.DOScale(Vector3.one * 1.05f, 0.5f));
            s.Append(card.transform.DOScale(Vector3.one, 0.2f));
        }
    }
}
