using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDScoreScript : MonoBehaviour {

    private Text _scoreText;
    private int _score;

	// Use this for initialization
	void Awake () {
        _score = 0;
        _scoreText = GameObject.Find("HUDScore").GetComponent<Text>();
	}

    void SetString()
    {
        _scoreText.text = "score\n"+_score.ToString();
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