using UnityEngine;
using System.Collections;
using TMPro; // Untuk UI Text
using UnityEngine.SceneManagement; 

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager Instance;

    [Header("Referensi Utama")]
    public PlayerGravityController player;
    public Camera mainCamera;
    public GameObject gameOverPanel; 

    [Header("UI Skor")]
    public TextMeshProUGUI scoreText;      // Masukkan UI Score disini
    public TextMeshProUGUI highscoreText;  // Masukkan UI Highscore disini
    public TextMeshProUGUI finalScoreText; 

    [Header("Pengaturan Dunia")]
    public float baseMoveSpeed = 5f;    
    public float worldSpeedMultiplier = 1f; 
    public float worldDirection = 1f;       

    private float score;
    private bool isGameOver = false;

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
    }

    void Update()
    {
        if (isGameOver) return;

        // LOGIKA SKOR PINDAH KESINI (Hanya dihitung 1x, jadi aman)
        if (worldDirection > 0)
        {
            // Skor nambah berdasarkan kecepatan dunia saat ini
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

    // --- GAME OVER ---
    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        // Simpan Highscore
        float currentHighscore = PlayerPrefs.GetFloat("hiscore", 0);
        if (score > currentHighscore)
        {
            PlayerPrefs.SetFloat("hiscore", score);
            PlayerPrefs.Save();
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            if (finalScoreText != null) finalScoreText.text = "Score: " + Mathf.FloorToInt(score).ToString();
        }
        player.gameObject.SetActive(false);
    }

    void UpdateHighscoreUI()
    {
        if (highscoreText != null)
            highscoreText.text = "Best: " + Mathf.FloorToInt(PlayerPrefs.GetFloat("hiscore", 0)).ToString();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void BackToMenu()
    {
        Time.timeScale = 1f; // Pastikan waktu normal
        SceneManager.LoadScene("Main Menu"); // Sesuaikan nama scene menu kamu
    }

}