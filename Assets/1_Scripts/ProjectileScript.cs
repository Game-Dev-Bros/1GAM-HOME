using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {

    private float range;
    private GameObject parentGun;

	// Use this for initialization
	void Awake () {    
        //StartCoroutine(CheckStatus());
	}
	
    public void SetRange(float v)
    {
        range = v;
    }

    public void SetParent(GameObject p)
    {
        parentGun = p;
    }

    public void Shoot()
    {
        StartCoroutine(CheckStatus());
    }

    IEnumerator CheckStatus()
    {
        while(Vector3.Magnitude(transform.position-parentGun.transform.position) < range)
        {
            yield return new WaitForSeconds(0.2f);
        }
        Destroy(gameObject);
    }

}
