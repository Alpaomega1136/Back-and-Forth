using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Referensi UI")]
    [SerializeField] GameObject pauseMenuUI;

    private bool isPaused = false;

    void Start()
    {
        // Pastikan menu mati saat mulai
        if (pauseMenuUI != null) pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Update()
    {
        // --- PERBAIKAN UTAMA DISINI ---
        // Cek ke Manager: "Apakah Game Over?"
        // Jika GameEventManager belum siap ATAU sudah Game Over, tombol ESC dimatikan.
        if (GameEventManager.Instance == null || GameEventManager.Instance.isGameOver) 
        {
            return; // Stop, jangan baca input di bawahnya
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Home()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}