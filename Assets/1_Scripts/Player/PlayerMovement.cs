using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlanetGravity planetGravity;

    private new Rigidbody rigidbody;

    public float movementSpeed = 5; // meters per second

    private bool isAirborne = true; // notice me flying, senpai! ಥ_ಥ
    private Vector3 surfaceNormal = Vector3.zero;

    void Awake()
    {
        planetGravity = GameObject.FindObjectOfType<PlanetGravity>();

        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        ApplyMovement();
    }

    void ApplyMovement()
    {
        if(isAirborne)
        {
            return;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 transformDiff = Vector3.zero;
        transformDiff += vertical * transform.forward;
        transformDiff += horizontal * transform.right;
        if(transformDiff.magnitude > 1)
        {
            transformDiff.Normalize();
        }

        Vector3 targetPosition = transform.position + transformDiff * movementSpeed * Time.deltaTime;
        transform.position = targetPosition;
    }

    void FixedUpdate()
    {
        ApplyGravity();
    }

    void ApplyGravity()
    {
        Vector3 gravitationalForce = Vector3.zero;

        if(isAirborne)
        {
            gravitationalForce = planetGravity.GetAtmosphericalGravity(transform.position);
        }
        else
        {
            gravitationalForce = planetGravity.GetSurfaceGravity(surfaceNormal);
        }

        rigidbody.AddForce(gravitationalForce);
    }

    void OnCollisionStay(Collision collision)
    {
        if(collision.transform.tag == planetGravity.tag)
        {
            isAirborne = false;
            surfaceNormal = Vector3.Lerp(surfaceNormal, collision.contacts[0].normal, Time.deltaTime);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if(collision.transform.tag == planetGravity.tag)
        {
            isAirborne = true;
        }
    }
}
