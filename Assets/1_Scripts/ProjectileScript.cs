using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {

    private float range;
    private float damage;
    private float radius;

	// Use this for initialization
	void Awake () {
	}
	
    public void SetupProjectile(float ran, float dmg, float rad)
    {
        range = ran;
        damage = dmg;
        radius = rad;
    }

    public void Shoot()
    {
        StartCoroutine(CheckStatus());
    }

    IEnumerator CheckStatus()
    {
        
        while(Vector3.Magnitude(transform.position-transform.parent.position) < range)
        {
            yield return new WaitForSeconds(0.2f);
        }
        Destroy(gameObject);
    }

}
