using UnityEngine;

public class PlanetGravity : MonoBehaviour
{
    public float gravitationalMagnitude = -9.81f; // same as the damn Earthlings are used too!
    public float radius;
    private Vector3 center;

    void Awake()
    {
        if(radius == 0)
        {
            radius = transform.localScale.x / 2;
        }

        center = transform.position;
    }

    public Vector3 GetAtmosphericalNormal(Vector3 position)
    {
        return (position - center).normalized;
    }

    public Vector3 GetAtmosphericalGravity(Vector3 position)
    {
        return GetAtmosphericalNormal(position) * gravitationalMagnitude;
    }

    public Vector3 GetSurfaceGravity(Vector3 surfaceNormal)
    {
        return surfaceNormal * gravitationalMagnitude;
    }
}
