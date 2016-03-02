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

	private bool _ended;
	public bool ended
	{
		get
		{
			return _ended;
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
		if(_ended)
		{
			return;
		}

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

	public void SetEnded(bool ended)
	{
		_ended  = ended;

		HUDMessageScript hudMessage = GameObject.Find("HUDMessageCenter").GetComponent<HUDMessageScript>();

		if (_ended)
		{
			_paused = true;
			Time.timeScale = 0;
			hudMessage.ShowMessage(Constants.Strings.GAME_OVER, 10000);
			SceneManager.LoadScene(Constants.Levels.LOSE_MENU, LoadSceneMode.Additive);
		}
		else
		{
			hudMessage.ShowMessage("", 0);
			Time.timeScale = 1;
			SceneManager.UnloadScene(Constants.Levels.LOSE_MENU);
		}
	}
}
