using UnityEngine;

public class PlayerRotation : MonoBehaviour {

    public float maxDistanceAjustment = 15;
    public float cameraOffsetAmount = 0.5f;

    void Start()
    {
        cameraOffsetAmount = Mathf.Clamp(cameraOffsetAmount, 0f, 1f);
    }

    void Update()
    {
        ApplyRotation();
    }

    void ApplyRotation()
    {
        Vector3 origin = transform.position + transform.up * transform.localScale.y / 2;
        Plane plane = new Plane(transform.up, origin);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float distance;
        if(plane.Raycast(ray, out distance))
        {
            Debug.Log("Distance: " + distance);
            Debug.Log("Ray: " + ray.GetPoint(distance).ToString());

            Vector3 direction = (ray.GetPoint(distance) - origin).normalized;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction, transform.up), Time.deltaTime * 5);

            Vector3 halfWay;
            if (distance < maxDistanceAjustment)
                halfWay = (ray.GetPoint(distance) - transform.position) * cameraOffsetAmount;
            else
                halfWay = (ray.GetPoint(maxDistanceAjustment) - transform.position) * cameraOffsetAmount;
            Camera.main.transform.LookAt(halfWay);
        }
    }

}
