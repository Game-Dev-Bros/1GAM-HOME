using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public void ResumeGame()
    {
		FindObjectOfType<PauseManager>().SetPause(false);
    }

    public void GoToMainMenu()
    {
		Time.timeScale = 1;
        SceneManager.LoadScene(Constants.Levels.MAIN_MENU);
    }
}
