using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager Instance;

    [Header("Referensi Utama")]
    public PlayerGravityController player;
    public Camera mainCamera;
    public GameObject gameOverPanel;

    [Header("UI Skor")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highscoreText;
    public TextMeshProUGUI finalScoreText;

    [Header("Pengaturan Dunia")]
    public float baseMoveSpeed = 5f;
    public float worldSpeedMultiplier = 1f;
    public float worldDirection = 1f;

    private float score;
    
    // UBAH JADI PUBLIC (Supaya PauseMenu bisa baca)
    public bool isGameOver = false; 

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (player == null) player = FindFirstObjectByType<PlayerGravityController>();
        if (mainCamera == null) mainCamera = Camera.main;

        UpdateHighscoreUI();
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        
        // Pastikan waktu berjalan normal saat game mulai
        Time.timeScale = 1f;
        isGameOver = false;
    }

    void Update()
    {
        if (isGameOver) return;

        if (worldDirection > 0)
        {
            float currentSpeed = baseMoveSpeed * worldSpeedMultiplier;
            score += currentSpeed * Time.deltaTime;
        }

        if (scoreText != null)
            scoreText.text = Mathf.FloorToInt(score).ToString();
    }

    // --- FUNGSI EVENT ---
    public void TriggerZeroGravity(float duration) { StartCoroutine(ZeroGravityRoutine(duration)); }
    IEnumerator ZeroGravityRoutine(float duration)
    {
        player.SetZeroGravity(true);
        yield return new WaitForSeconds(duration);
        player.SetZeroGravity(false);
    }

    public void TriggerSpeedChange(float multiplier, float duration) { StartCoroutine(SpeedChangeRoutine(multiplier, duration)); }
    IEnumerator SpeedChangeRoutine(float multiplier, float duration)
    {
        worldSpeedMultiplier = multiplier;
        yield return new WaitForSeconds(duration);
        worldSpeedMultiplier = 1f;
    }

    public void TriggerReverseDirection(float duration) { StartCoroutine(ReverseRoutine(duration)); }
    IEnumerator ReverseRoutine(float duration)
    {
        worldDirection = -1f;
        yield return new WaitForSeconds(duration);
        worldDirection = 1f;
    }

    public void TriggerScreenRotate(float duration) { StartCoroutine(ScreenRotateRoutine(duration)); }
    IEnumerator ScreenRotateRoutine(float duration)
    {
        mainCamera.transform.rotation = Quaternion.Euler(0, 0, 180);
        yield return new WaitForSeconds(duration);
        mainCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void TriggerForcedFlip() { player.TriggerForcedFlip(); }

    // --- PERBAIKAN LOGIKA GAME OVER ---
    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log("Game Over: Time Stopped");

        // 1. HENTIKAN WAKTU (Ini yang bikin lantai berhenti gerak)
        Time.timeScale = 0f; 

        // 2. Simpan Highscore
        float currentHighscore = PlayerPrefs.GetFloat("hiscore", 0);
        if (score > currentHighscore)
        {
            PlayerPrefs.SetFloat("hiscore", score);
            PlayerPrefs.Save();
        }

        // 3. Tampilkan Panel
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            if (finalScoreText != null) finalScoreText.text = "Score: " + Mathf.FloorToInt(score).ToString();
        }
        
        // 4. Matikan Player
        if (player != null) player.gameObject.SetActive(false);
    }

    void UpdateHighscoreUI()
    {
        if (highscoreText != null)
            highscoreText.text = "Best: " + Mathf.FloorToInt(PlayerPrefs.GetFloat("hiscore", 0)).ToString();
    }

    // Tombol Retry
    public void RestartGame()
    {
        Time.timeScale = 1f; // Kembalikan waktu normal sebelum reload
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Tombol Menu
    public void BackToMenu()
    {
        Time.timeScale = 1f; // Kembalikan waktu normal sebelum pindah
        SceneManager.LoadScene("Main Menu");
    }
}