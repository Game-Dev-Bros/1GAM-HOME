using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlanetGravity planetGravity;

    private new Rigidbody rigidbody;

    public float movementSpeed = 5; // meters per second

    private Vector3 surfaceNormal = Vector3.zero;

    void Awake()
    {
        planetGravity = GameObject.FindObjectOfType<PlanetGravity>();

        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        ApplyMovement();
        AlignPlayerToSurface();
    }

    void ApplyMovement()
    {
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

    void AlignPlayerToSurface()
    {
        Ray ray = new Ray(transform.position, -transform.up);

        var hits = Physics.RaycastAll(ray, transform.localScale.y);

        foreach(RaycastHit hit in hits)
        {
            if(hit.transform.tag == "Planet")
            {
                surfaceNormal = hit.normal;

                Quaternion targetRotation = Quaternion.FromToRotation(transform.up, surfaceNormal) * transform.rotation;

                transform.position = hit.point;
                transform.rotation = targetRotation;

                break;
            }
        }
    }
}
