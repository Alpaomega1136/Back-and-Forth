using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource sfxSource;          // For sound effects
    public AudioClip gravity1, gravity2, gravity3;
    public AudioClip collision;

    private int lastPlayedGravityClip = -1;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);   // best for SFX
    }

    public void PlayGravitySFX()
    {
        int choice = Random.Range(0, 3);
        // Avoid repeating the same clip
        if (choice == lastPlayedGravityClip) choice = (choice + 1) % 3;
        
        lastPlayedGravityClip = choice;

        switch (choice)
        {
            case 0:
                PlaySFX(gravity1);
                break;
            case 1:
                PlaySFX(gravity2);
                break;
            case 2:
                PlaySFX(gravity3);
                break;
        }
    }
}