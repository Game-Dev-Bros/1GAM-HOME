using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
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
            Vector3 direction = (ray.GetPoint(distance) - origin).normalized;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction, transform.up), Time.deltaTime * 5);
        }
    }
}
