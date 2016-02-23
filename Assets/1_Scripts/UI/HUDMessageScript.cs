using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDMessageScript : MonoBehaviour {

    public float fadeInLength = 0f;
    public float showLength = 3f;
    public float fadeOutLength = 1f;

    private Text _message;
    private float currAnimLength = 0f;
    private float currStep = 0f;
    private Color _color, _clear;

	// Use this for initialization
	void Awake () {
        _message = GetComponent<Text>();
        _message.enabled = false;
        _clear = new Color(_message.color.r, _message.color.g, _message.color.b, 0);
        _color = new Color(_message.color.r, _message.color.g, _message.color.b, 1);
        //ShowMessage("Hello from the other side!");
    }
	
    public void ShowMessage(string m)
    {
        _message.color = _clear;
        _message.text = m;
        StartCoroutine(Animate());
    }

    //TODO: doesnt fade out when fade in == 0 && fade out > 0
    IEnumerator Animate(int steps = 60)
    {
        _message.enabled = true;

        currAnimLength = 0f;
        currStep = 0f;
        if (fadeInLength > 0)
        {
            float step = fadeInLength / (float)steps;
            for (int s = 0; s < steps; s++)
            {
                _message.color = Color.Lerp(_message.color, _color, currStep);
                currStep += step;
                yield return new WaitForEndOfFrame();
            }
        }
        else _message.color = _color;

        yield return new WaitForSeconds(showLength);

        currAnimLength = 0f;
        currStep = 0f;
        if (fadeOutLength > 0)
        {
            float step = fadeInLength / (float)steps;
            for (int s = 0; s < steps; s++)
            {
                _message.color = Color.Lerp(_message.color, _clear, currStep);
                currStep += step;
                yield return new WaitForEndOfFrame();
            }
        }
        else _message.color = _clear;

        _message.enabled = false;
    }


}
