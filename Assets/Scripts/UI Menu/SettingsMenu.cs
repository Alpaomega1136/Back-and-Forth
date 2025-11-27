using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private TextMeshProUGUI musicValueText;

    [Header("SFX")]
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TextMeshProUGUI sfxValueText;



    private float savedMusic;
    private float savedSfx;

    private float tempMusic;
    private float tempSfx;

    private void Awake()
    {
        savedMusic = PlayerPrefs.GetFloat("MusicVolume", 1f);
        savedSfx   = PlayerPrefs.GetFloat("SFXVolume",   1f);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        musicSlider.onValueChanged.AddListener(value =>
        {
            tempMusic = value;
            musicValueText.text = Mathf.RoundToInt(value * 100).ToString();
            BGMManager.Instance.SetMusicVolume(value);
        });

        sfxSlider.onValueChanged.AddListener(value =>
        {
            tempSfx = value;
            sfxValueText.text = Mathf.RoundToInt(value * 100).ToString();
            SoundManager.Instance.SetSFXVolume(value);
        });
    }

    // Update is called once per frame
    void Update() {}

    private void OnEnable()
    {
        tempMusic = savedMusic;
        tempSfx   = savedSfx;

        // Update sliders without triggering listeners
        musicSlider.SetValueWithoutNotify(tempMusic);
        sfxSlider.SetValueWithoutNotify(tempSfx);

        musicValueText.text = Mathf.RoundToInt(tempMusic * 100).ToString();
        sfxValueText.text   = Mathf.RoundToInt(tempSfx * 100).ToString();
    }

    public void ApplySettings()
    {
        savedMusic = tempMusic;
        savedSfx   = tempSfx;

        PlayerPrefs.SetFloat("MusicVolume", savedMusic);
        PlayerPrefs.SetFloat("SFXVolume",   savedSfx);
        PlayerPrefs.Save();

        Debug.Log("Settings applied!");
    }

    public void CancelSettings()
    {
        // Restore saved values
        tempMusic = savedMusic;
        tempSfx   = savedSfx;

        // Update UI
        musicSlider.SetValueWithoutNotify(savedMusic);
        sfxSlider.SetValueWithoutNotify(savedSfx);

        musicValueText.text = Mathf.RoundToInt(savedMusic * 100).ToString();
        sfxValueText.text   = Mathf.RoundToInt(savedSfx * 100).ToString();

        // Apply to managers
        BGMManager.Instance.SetMusicVolume(savedMusic);
        SoundManager.Instance.SetSFXVolume(savedSfx);

        Debug.Log("Settings canceled.");
    }

    private void OnDisable()
    {
        Debug.Log("Settings Panel closed!");
        // Save settings / cleanup here
    }
}
