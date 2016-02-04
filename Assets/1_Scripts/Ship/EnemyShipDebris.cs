using UnityEngine;

public class EnemyShipDebris : MonoBehaviour
{
    public void AddExplosionForce(Vector3 collisionPoint, float impactForce)
    {
        foreach(Transform child in transform)
        {
            child.GetComponent<Rigidbody>().AddExplosionForce(impactForce, collisionPoint, 0.1f);
        }
    }
}
