using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
   public void PlayGame()
   {
        SceneManager.LoadSceneAsync(1);
        BGMManager.Instance.PlayMusic();

   }
   public void QuitGame()
    {
        Application.Quit();
    }
}
