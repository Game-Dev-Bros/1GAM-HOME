using UnityEngine;
using System.Collections;
using System;

public class EnemyShipScript : MonoBehaviour
{
    public float speed = 10;
    public int hp = 10;

    private bool _hasDied = false;
    private GameObject _planet;
    private bool _landed = false;
    
	void Awake ()
    {
        _planet = GameObject.FindGameObjectWithTag("Planet");
        StartCoroutine(LandShip());
	}

    IEnumerator LandShip()
    {
        while (true)
        {
            if (!_landed && _planet != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, _planet.transform.position, speed * Time.deltaTime);
            }
            yield return new WaitForEndOfFrame();
        }
    }

    void Update()
    {
        if(hp < 0 && !_hasDied)
        {
            _hasDied = true;
            Debug.Log("Im dead");
        }
    }

    void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Planet":
                _landed = true;
                break;
            case "PlayerProjectile":
                var ps = other.gameObject.GetComponent<ProjectileScript>();
                hp -= (int)ps.GetProjectileDamage();
                Destroy(other.gameObject);
                break;
            default:
                break;
        }
    }
}
