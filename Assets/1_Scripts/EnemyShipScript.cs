using UnityEngine;
using System.Collections;
using System;

public class EnemyShipScript : MonoBehaviour {

    public float speed = 10;

    private GameObject planet;
    private bool landed = false;
    
	// Use this for initialization
	void Awake () {
        planet = GameObject.Find("Planet");
        //StartCoroutine(Kamikaze());
	}

    private IEnumerator Kamikaze(int steps = 60)
    {
        var radiusVector = (transform.position - planet.transform.position).normalized*planet.transform.localScale.x;
        var finaVector = ((planet.transform.position - transform.position) + radiusVector);

        var step = (finaVector.magnitude * speed) / steps;
        var pos = 0f;
        for (int i = 0; i < steps; i++)
        {

            transform.position = Vector3.Lerp(transform.position, planet.transform.position, pos);
            pos += step;
            Debug.Log("moverino");
            //transform.position = Vector3.MoveTowards(transform.position, planet.transform.position, step);
            yield return null;
        }

    }

    // Update is called once per frame
    void Update () {
        if(!landed)
            transform.position = Vector3.MoveTowards(transform.position, planet.transform.position, speed*Time.deltaTime);
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            landed = true;
            transform.gameObject.isStatic = true;
        }

    }

}
