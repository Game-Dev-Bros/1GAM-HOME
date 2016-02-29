using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDScoreScript : MonoBehaviour {

    private Text _scoreText;
    private int _score;

	private Text _highscoreText;
	private int _highscore;

	// Use this for initialization
	void Awake () {
        _score = 0;
        _scoreText = GameObject.Find("HUDScore").GetComponent<Text>();

		_highscoreText = GameObject.Find("Highscore").GetComponent<Text>();

		_highscore = PlayerPrefs.GetInt(Constants.Prefs.HIGHSCORE);
		_highscoreText.text = Constants.Strings.HIGHSCORE_PREFIX + _highscore;
	}

    void SetString()
    {
        _scoreText.text = "score\n"+_score.ToString();

		if(_score > _highscore)
		{
			_highscore = _score;
			PlayerPrefs.SetInt(Constants.Prefs.HIGHSCORE, _highscore);
			_highscoreText.text = Constants.Strings.HIGHSCORE_PREFIX + _highscore;
		}
    }

    public void ResetScore()
    {
        _score = 0;
        SetString();   
    }

    public void SetScore(int val)
    {
        _score = val;
        SetString();
    }

    public void IncreaseScoreBy(int val)
    {
        _score += val;
        SetString();
    }

    public void DecreaseScoreBy(int val)
    {
        _score -= val;
        SetString();
    }

    public int GetScore()
    {
        return _score;
    }
}