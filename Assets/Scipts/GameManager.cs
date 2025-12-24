using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Cards")]
    public List<Card> cards;

    [Header("Sprites Reference")]
    public List<Sprite> frontSprites;
    public Sprite backSprite;

    public float flipBackDelay = 1f;
    [HideInInspector] public bool isChecking;

    private Card firstCard;
    private Card secondCard;

    // ================== TIME ==================
    [Header("Time")]
    public float timeLimit = 120f;
    private float currentTime;
    public TextMeshProUGUI timeText;

    // ================== PANELS ==================
    [Header("Panels")]
    public GameObject gameOverPanel;
    public GameObject winPanel;

    private int matchedPairs = 0;
    private int totalPairs;
    private bool gameEnd = false;

    // ================== SOUND ==================
    [Header("Sound")]
    public AudioSource bgmSource;
    public AudioClip normalMusic;
    public AudioClip warningMusic;

    private bool isWarningPlaying = false;
    // ==========================================

    void Start()
    {
        SetupCards();
        ShufflePositions();

        currentTime = timeLimit;
        totalPairs = cards.Count / 2;

        gameOverPanel.SetActive(false);
        winPanel.SetActive(false);

        // 🔊 เล่นเพลงปกติ
        if (bgmSource != null && normalMusic != null)
        {
            bgmSource.clip = normalMusic;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    void Update()
    {
        if (gameEnd) return;

        currentTime -= Time.deltaTime;
        UpdateTimeUI();

        // ⚠ เหลือ 6 วินาที → เปลี่ยนเพลง
        if (currentTime <= 6f && !isWarningPlaying)
        {
            isWarningPlaying = true;

            if (bgmSource != null && warningMusic != null)
            {
                bgmSource.clip = warningMusic;
                bgmSource.loop = true;
                bgmSource.Play();
            }
        }

        if (currentTime <= 0)
        {
            GameOver();
        }
    }

    void UpdateTimeUI()
    {
        int min = Mathf.FloorToInt(currentTime / 60);
        int sec = Mathf.FloorToInt(currentTime % 60);
        timeText.text = $"{min:00}:{sec:00}";
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

            matchedPairs++;
            if (matchedPairs >= totalPairs)
            {
                Win();
            }
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

    void GameOver()
    {
        gameEnd = true;

        if (bgmSource != null)
            bgmSource.Stop();

        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    void Win()
    {
        gameEnd = true;

        if (bgmSource != null)
            bgmSource.Stop();

        winPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}
