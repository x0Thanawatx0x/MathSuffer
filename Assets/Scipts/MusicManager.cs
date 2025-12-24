using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Audio")]
    public AudioSource audioSource;

    void Awake()
    {
        // กันซ้อน
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // ตั้งค่า AudioSource
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        // เล่นครั้งเดียวเท่านั้น
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
