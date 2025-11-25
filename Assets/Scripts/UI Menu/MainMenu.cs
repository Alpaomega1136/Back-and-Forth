using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
   public void PlayGame()
   {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(1);

        // Pilihan lainnya taruh event listener di asyncLoad daripada busy waiting
        // tapi by nature of async bisa aja listener ditambahin setelah load selesai idk ga tau tech Unity
        while (!asyncLoad.isDone) {}
        BGMManager.Instance?.PlayMusic();
   }
   public void QuitGame()
    {
        Application.Quit();
    }
}
