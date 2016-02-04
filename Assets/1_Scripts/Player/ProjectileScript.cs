﻿using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {

    private float _range;
    private float _damage;
    private float _radius;
    private float _impactForce;
    private GameObject _player;

    // Use this for initialization
    void Awake () {
        _player = GameObject.FindWithTag("Player");
	}
	
    public void SetupProjectile(/*GunScript.GunType t,*/ float ran, float dmg, float rad, float imp)
    {
        //ChangeBulletType(t);
        _range = ran;
        _damage = dmg;
        _radius = rad;
        _impactForce = imp;
    }

    public void Shoot()
    {
        StartCoroutine(CheckStatus());
    }

    public float GetProjectileDamage()
    {
        return _damage;
    }
    
    public float GetProjectileImpactForce()
    {
        return _impactForce;
    }

    IEnumerator CheckStatus()
    {        
        while(Vector3.Magnitude(transform.position-_player.transform.position) < _range)
        {
            yield return new WaitForSeconds(0.2f);
        }
        Destroy(gameObject);
    }

}
