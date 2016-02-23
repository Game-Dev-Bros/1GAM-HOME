using System.Collections;
using UnityEngine;

public class EnemyShipDebris : MonoBehaviour
{
    public float fallingSpeed = 3;
    private bool _hasExploded = false;
    private new Rigidbody rigidbody;

    public float freeHeight;

	public float timeToSelfDestruct;
	public float distanceToSelfDestruct;

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
        if(!_hasExploded)
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
			StartCoroutine(DestroySelf());
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

	IEnumerator DestroySelf()
	{
		float runningTime = 0;

		distanceToSelfDestruct += transform.position.magnitude; // add the distance from the origin

		while(true)
		{
			if(timeToSelfDestruct == 0 && distanceToSelfDestruct == 0)
			{
				break;
			}

			if(timeToSelfDestruct > 0)
			{
				runningTime += Time.deltaTime;

				if(runningTime > timeToSelfDestruct)
				{
					break;
				}
			}

			if (distanceToSelfDestruct > 0)
			{
				if(transform.position.magnitude > distanceToSelfDestruct)
				{
					break;
				}
			}

			yield return null;
		}

		StartCoroutine(DestroyNow());
	}

	private bool _isDestroying;
	public IEnumerator DestroyNow(float duration = 3)
	{
		if(_isDestroying)
		{
			yield break;
		}

		_isDestroying = true;

		float _scaler = transform.localScale.x;
		while(transform.localScale.x > 0.01)
		{
			transform.localScale -= Vector3.one * _scaler * Time.deltaTime / duration;
			yield return new WaitForEndOfFrame();
		}

		Destroy(gameObject);
	}
}
