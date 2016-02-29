using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
	private bool _paused;
	public bool paused
	{
		get
		{
			return _paused;
		}
	}
	void Update()
	{
		if(Input.GetButtonDown("Cancel"))
		{
			SetPause(!_paused);
		}
	}

	public void SetPause(bool paused)
	{
		_paused  = paused;

		if(_paused)
		{
			Time.timeScale = 0;
			SceneManager.LoadScene(Constants.Levels.PAUSE_MENU, LoadSceneMode.Additive);
		}
		else
		{
			Time.timeScale = 1;
			SceneManager.UnloadScene(Constants.Levels.PAUSE_MENU);
		}
	}
}
