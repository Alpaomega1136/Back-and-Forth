using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    public AudioSource audioSource;
    public AudioClip gameMusic;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Load saved volumes on startup
        float music = PlayerPrefs.GetFloat("MusicVolume", 1f);
        SetMusicVolume(music);
    }

    void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        if (gameMusic != null)
        {
            audioSource.clip = gameMusic;
            audioSource.loop = true;
        }
    }

    // Play
    public void PlayMusic()
    {
        if (audioSource.clip == null && gameMusic != null)
            audioSource.clip = gameMusic;

        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    // PAUSE (keeps current time)
    public void PauseMusic()
    {
        if (audioSource.isPlaying)
            audioSource.Pause();
    }

    // RESUME (continue from paused time)
    public void ResumeMusic()
    {
        audioSource.UnPause();
    }

    // STOP (stops and resets to start)
    public void StopMusic()
    {
        if (audioSource.isPlaying || audioSource.time > 0f)
            audioSource.Stop(); // Stop() resets playback position to 0
    }

    public void SetMusicVolume(float value)
    {
        audioSource.volume = value;
    }
}