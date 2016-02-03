using System.Collections;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private GameObject player;

    public float cameraHeight = 10;
    public float maxDistanceAdjustment = 15;
    public float maxDegreeAdjustment = 5;

    public bool adjustToMouse = true;

    public bool useSlerp = true;
    public float slerpMultiplier = 3;

	void Awake ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	void Update ()
    {
        Vector3 targetPosition = player.transform.position + player.transform.up * cameraHeight;

        Vector3 origin = player.transform.position + player.transform.up * player.transform.localScale.y / 2;
        Plane plane = new Plane(player.transform.up, origin);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        Quaternion targetRotation = player.transform.rotation;

        if(adjustToMouse)
        {
            float distance;
            if(plane.Raycast(ray, out distance))
            {
                Vector3 difference = (ray.GetPoint(distance) - transform.position);
                Vector3 direction = difference.normalized;

                Vector2 eulerAdjustement = difference;
                eulerAdjustement.x = Mathf.Max(-maxDistanceAdjustment, Mathf.Min(eulerAdjustement.x, maxDistanceAdjustment)) / maxDistanceAdjustment;
                eulerAdjustement.y = -Mathf.Max(-maxDistanceAdjustment, Mathf.Min(eulerAdjustement.y, maxDistanceAdjustment)) / maxDistanceAdjustment;
            
                eulerAdjustement *=  maxDegreeAdjustment;

                targetRotation *= Quaternion.Euler(eulerAdjustement.y, 0, eulerAdjustement.x); 
            }
        }

        targetRotation *= Quaternion.Euler(90, 0, 0);

        float slerpT = (useSlerp ? (Time.deltaTime * slerpMultiplier) : 1);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, slerpT);

        transform.position = targetPosition;
	}
}
