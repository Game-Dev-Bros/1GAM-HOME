using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {

    private float range;
    private float damage;
    private float radius;
    private TrailRenderer trail;

    const float PistolStartWidth = 0.5f;
    const float PistolEndWidth = 0.3f;
    const float MachinegunStartWidth = 0.3f;
    const float MachinegunEndWidth = 0.2f;


    // Use this for initialization
    void Awake () {
        trail = GetComponentInChildren<TrailRenderer>();
	}
	
    public void SetupProjectile(/*GunScript.GunType t,*/ float ran, float dmg, float rad)
    {
        //ChangeBulletType(t);
        range = ran;
        damage = dmg;
        radius = rad;
    }

    void ChangeBulletType(GunScript.GunType t)
    {
        switch (t)
        {
            case GunScript.GunType.Pistol:
                trail.startWidth = PistolStartWidth;
                trail.endWidth = PistolEndWidth;
                break;
            case GunScript.GunType.Machinegun:
                trail.startWidth = MachinegunStartWidth;
                trail.endWidth = MachinegunEndWidth;
                break;
            case GunScript.GunType.Rocketlauncher:
                break;
            default:
                break;
        }
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
