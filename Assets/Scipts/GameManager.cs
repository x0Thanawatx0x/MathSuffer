using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Cards")]
    public List<Card> cards;

    [Header("Sprites Reference")]
    public List<Sprite> frontSprites;   // รูปหน้า (ไม่ซ้ำ)
    public Sprite backSprite;            // รูปหลัง (รูปเดียว)

    public float flipBackDelay = 1f;
    [HideInInspector] public bool isChecking;

    private Card firstCard;
    private Card secondCard;

    void Start()
    {
        SetupCards();
        ShufflePositions();
    }

    void SetupCards()
    {
        if (cards.Count % 2 != 0 || frontSprites.Count < cards.Count / 2)
        {
            Debug.LogError("จำนวนการ์ดหรือรูปไม่ถูกต้อง");
            return;
        }

        List<int> ids = new List<int>();

        for (int i = 0; i < cards.Count / 2; i++)
        {
            ids.Add(i);
            ids.Add(i);
        }

        for (int i = 0; i < ids.Count; i++)
        {
            int rand = Random.Range(i, ids.Count);
            (ids[i], ids[rand]) = (ids[rand], ids[i]);
        }

        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].cardID = ids[i];
            cards[i].Setup(frontSprites[ids[i]], backSprite);
        }
    }

    public void CardSelected(Card card)
    {
        if (isChecking) return;

        if (firstCard == null)
        {
            firstCard = card;
        }
        else
        {
            secondCard = card;
            StartCoroutine(CheckMatch());
        }
    }

    IEnumerator CheckMatch()
    {
        isChecking = true;
        yield return new WaitForSeconds(0.3f);

        if (firstCard.cardID == secondCard.cardID)
        {
            firstCard.Hide();
            secondCard.Hide();
        }
        else
        {
            yield return new WaitForSeconds(flipBackDelay);
            firstCard.FlipDown();
            secondCard.FlipDown();
        }

        firstCard = null;
        secondCard = null;
        isChecking = false;
    }

    void ShufflePositions()
    {
        foreach (Card card in cards)
        {
            card.transform.SetSiblingIndex(Random.Range(0, cards.Count));
        }
    }
}
