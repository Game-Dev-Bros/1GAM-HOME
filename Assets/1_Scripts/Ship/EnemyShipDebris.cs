using UnityEngine;

public class EnemyShipDebris : MonoBehaviour
{
    public float fallingSpeed = 3;
    private bool _hasExploded = false;
    private bool _isStatic = false;
    private new Rigidbody rigidbody;

    public float freeHeight;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        ApplyGravity();
    }

    void ApplyGravity()
    {
        if(!_hasExploded || _isStatic)
        {
            return;
        }

        Ray ray = new Ray(transform.position, (Vector3.zero - transform.position).normalized);

        RaycastHit[] hits = Physics.RaycastAll(ray, 50);

        foreach(RaycastHit hit in hits)
        {
            if(hit.transform.tag == "Planet")
            {
                float distanceToSurface = (hit.point - transform.position).magnitude;
                float deltaDistance = Mathf.Min(distanceToSurface, fallingSpeed * Time.deltaTime);

                if(distanceToSurface < freeHeight)
                {
                    rigidbody.velocity -= hit.normal * deltaDistance;
                }
                break;
            }
        }

    }

    public void AddExplosionForce(float impactForce, Vector3 collisionPoint, float explosionRadius)
    {
        if(gameObject.activeSelf)
        {
            rigidbody.AddExplosionForce(impactForce, collisionPoint, explosionRadius);
            _hasExploded = true;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Enemy":
                Destroy(gameObject);
                break;

            case "PlayerProjectile":
                Destroy(other.gameObject);
                break;

            default:
                break;
        }
    }
}
