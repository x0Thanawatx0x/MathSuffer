using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [HideInInspector] public int cardID;
    [HideInInspector] public bool isFlipped = false;

    public Image cardImage; // ต้องลากใน Inspector

    private Sprite frontSprite;
    private Sprite backSprite;

    private Button button;
    private GameManager gameManager;

    void Awake()
    {
        // ✅ ใช้ Awake แทน Start (สำคัญมาก)
        button = GetComponent<Button>();
        gameManager = FindObjectOfType<GameManager>();

        if (cardImage == null)
            cardImage = GetComponent<Image>();
    }

    public void Setup(Sprite front, Sprite back)
    {
        frontSprite = front;
        backSprite = back;

        FlipDown();
        gameObject.SetActive(true);
    }

    void OnEnable()
    {
        if (button != null)
            button.onClick.AddListener(OnCardClicked);
    }

    void OnDisable()
    {
        if (button != null)
            button.onClick.RemoveListener(OnCardClicked);
    }

    void OnCardClicked()
    {
        if (isFlipped || gameManager.isChecking) return;

        FlipUp();
        gameManager.CardSelected(this);
    }

    public void FlipUp()
    {
        isFlipped = true;
        cardImage.sprite = frontSprite;
        button.interactable = false;
    }

    public void FlipDown()
    {
        isFlipped = false;

        if (cardImage != null)
            cardImage.sprite = backSprite;

        if (button != null)
            button.interactable = true;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}