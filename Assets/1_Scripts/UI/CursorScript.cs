using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CursorScript : MonoBehaviour
{
    public bool hideCursor = false;

    private GameObject cursorImage;
    private GameObject player;

    Vector2 cursorPos;

    // Use this for initialization
    void Awake()
    {
        Cursor.visible = !hideCursor;
        player = GameObject.Find("Rotator");
        cursorImage = GameObject.Find("Cursor");
        StartCoroutine(UpdateCursor());
    }

    void OnValidate()
    {
        Cursor.visible = !hideCursor;
    }

    Vector3 mid = new Vector3(0.5f*Screen.width, 0.5f*Screen.height, 0);
    IEnumerator UpdateCursor()
    {
        while (true)
        {
            Vector3 cPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
            Vector3 dir = cPos - mid;
            
            cursorImage.transform.position = cPos;
            Quaternion rot = Quaternion.LookRotation(dir);
            //cursorImage.transform.rotation = Quaternion.Euler(0, 0, -player.transform.localEulerAngles.y);
            cursorImage.transform.rotation = rot;
            yield return new WaitForEndOfFrame();
        }
    }

}

