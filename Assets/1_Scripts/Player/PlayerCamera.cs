using System.Collections;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private GameObject player;
    public float height;

	void Awake ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	void FixedUpdate ()
    {
        Vector3 targetPosition = player.transform.position + player.transform.up * height;
        transform.position = targetPosition;

        Quaternion targetRotation = player.transform.rotation * Quaternion.Euler(90, 0, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 3f);    
	}
}
