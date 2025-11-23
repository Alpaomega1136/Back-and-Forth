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

    public void PlayGravityChangeSFX()
    {
        // Randomize 1-3 clips
        int choice = Random.Range(1, 4);

        if (choice == lastPlayedGravityClip && lastPlayedGravityClip != -1)
        {
            choice = (choice % 3) + 1;
        }

        lastPlayedGravityClip = choice;
        switch (choice)
        {
            case 1:
                PlaySFX(gravity1);
                break;
            case 2:
                PlaySFX(gravity2);
                break;
            case 3:
                PlaySFX(gravity3);
                break;
        }
    }
}
