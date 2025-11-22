using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Referensi UI")]
    [SerializeField] GameObject pauseMenuUI; // Masukkan Panel Pause disini

    // Status apakah game sedang pause atau tidak
    private bool isPaused = false;

    void Update()
    {
        // Jika tombol ESC ditekan
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true); // Munculkan menu
        Time.timeScale = 0f;         // Waktu beku
        isPaused = true;
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false); // Sembunyikan menu
        Time.timeScale = 1f;          // Waktu jalan
        isPaused = false;
    }

    public void Home()
    {
        Time.timeScale = 1f; // PENTING: Waktu harus normal sebelum pindah
        SceneManager.LoadScene("Main Menu"); // Pastikan nama scene pakai SPASI
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}