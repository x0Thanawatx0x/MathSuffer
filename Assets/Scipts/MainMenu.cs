using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [Header("Scene")]
    public string gameSceneName = "GameScene";

    [Header("UI Panels")]
    public GameObject optionPanel;

    [Header("Fade")]
    public Image fadeImage;
    public float fadeDuration = 1f;

    [Header("Audio")]
    public AudioMixer audioMixer;
    public Slider volumeSlider;

    [Header("Button Sound")]
    public AudioClip clickSound;
    public float playDelay = 1f;

    private const string VOLUME_KEY = "MasterVolume";

    CanvasGroup fadeCanvasGroup;

    void Start()
    {
        if (optionPanel != null)
            optionPanel.SetActive(false);

        // === Fade Setup ===
        if (fadeImage != null)
        {
            fadeCanvasGroup = fadeImage.GetComponent<CanvasGroup>();
            if (fadeCanvasGroup == null)
                fadeCanvasGroup = fadeImage.gameObject.AddComponent<CanvasGroup>();

            fadeCanvasGroup.blocksRaycasts = false;
            fadeImage.gameObject.SetActive(true);

            // เริ่มจากดำ แล้ว Fade In
            SetFadeAlpha(1f);
            StartCoroutine(FadeIn());
        }

        // === Load Volume ===
        float volume = PlayerPrefs.GetFloat(VOLUME_KEY, 0.75f);
        if (volumeSlider != null)
        {
            volumeSlider.value = volume;
            SetVolume(volume);
        }
    }

    // ================== MENU ==================

    public void PlayGame()
    {
        StartCoroutine(PlayFadeAndLoad());
    }

    IEnumerator PlayFadeAndLoad()
    {
        // 🔊 เล่นเสียงปุ่ม
        if (clickSound != null)
        {
            GameObject temp = new GameObject("TempAudio");
            AudioSource source = temp.AddComponent<AudioSource>();
            source.clip = clickSound;
            source.volume = 1f;
            source.ignoreListenerPause = true;
            source.Play();
            Destroy(temp, clickSound.length);
        }

        // ⏱ รอให้เสียงเริ่ม
        yield return new WaitForSecondsRealtime(playDelay);

        // 🌑 Fade Out
        yield return StartCoroutine(FadeOut());

        // ▶ โหลดฉาก
        SceneManager.LoadScene(gameSceneName);
    }

    // ================== FADE ==================

    IEnumerator FadeOut()
    {
        fadeCanvasGroup.blocksRaycasts = true;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            SetFadeAlpha(Mathf.Lerp(0f, 1f, t / fadeDuration));
            yield return null;
        }
        SetFadeAlpha(1f);
    }

    IEnumerator FadeIn()
    {
        fadeCanvasGroup.blocksRaycasts = false;

        float t = fadeDuration;
        while (t > 0)
        {
            t -= Time.unscaledDeltaTime;
            SetFadeAlpha(Mathf.Lerp(0f, 1f, t / fadeDuration));
            yield return null;
        }
        SetFadeAlpha(0f);
    }

    void SetFadeAlpha(float a)
    {
        if (fadeImage == null) return;
        Color c = fadeImage.color;
        c.a = a;
        fadeImage.color = c;
    }

    // ================== OPTION PANEL ==================

    public void OpenOption()
    {
        optionPanel.SetActive(true);
    }

    public void CloseOption()
    {
        optionPanel.SetActive(false);
    }

    // ================== AUDIO ==================

    public void SetVolume(float value)
    {
        audioMixer.SetFloat(
            "MasterVolume",
            Mathf.Log10(Mathf.Clamp(value, 0.001f, 1f)) * 20
        );
        PlayerPrefs.SetFloat(VOLUME_KEY, value);
        PlayerPrefs.Save();
    }
}
