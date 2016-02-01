using UnityEngine;

public class CameraTracker : MonoBehaviour
{
    public GameObject objectToTrack;
    public Vector3 relativePosition;

	void Awake ()
    {
	    if(objectToTrack == null)
        {
            objectToTrack = GameObject.FindGameObjectWithTag("Player");
        }
	}
	
	void Update ()
    {
	    transform.up = objectToTrack.transform.up;

        transform.LookAt(objectToTrack.transform);
	}
}
