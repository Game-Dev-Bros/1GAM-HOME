using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
	void Start()
	{
		GameObject.Find("Highscore").GetComponent<Text>().text = Constants.Strings.HIGHSCORE_PREFIX + PlayerPrefs.GetInt(Constants.Prefs.HIGHSCORE);
	}

    public void StartGame()
    {
        SceneManager.LoadScene(Constants.Levels.GAME);
    }

	void Update()
	{
		if(Application.isEditor && Input.GetKeyDown(KeyCode.P))
		{
			PlayerPrefs.DeleteAll();
			GameObject.Find("Highscore").GetComponent<Text>().text = Constants.Strings.HIGHSCORE_PREFIX + PlayerPrefs.GetInt(Constants.Prefs.HIGHSCORE);
		}
	}
}
