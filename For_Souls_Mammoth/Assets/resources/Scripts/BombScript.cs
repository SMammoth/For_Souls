﻿using UnityEngine;
using System.Collections;

public class BombScript : MonoBehaviour {

    public float explosionPower;
    public float explosionRadius;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

    //Add force to rigidbody (explosion effect)
    public static void AddExplosionForce(Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        var bodyDirection = (body.transform.position - explosionPosition);
        float calc = 1 - (bodyDirection.magnitude / explosionRadius);
        if (calc <= 0)
        {
            calc = 0;
        }
        body.AddForce(bodyDirection.normalized * explosionForce * calc);
    }

    public virtual void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.transform.tag == "Player")
        {
           Instantiate(Resources.Load("Prefabs/Particles/Particle_Death", typeof(GameObject)), gameObject.transform.position, Quaternion.identity);
           AddExplosionForce(coll.transform.GetComponent<Rigidbody2D>(), explosionPower * 100, gameObject.transform.position, explosionRadius);
           Destroy(gameObject);
        }
        else if (coll.gameObject)
        {
            Instantiate(Resources.Load("Prefabs/Particles/Particle_Death", typeof(GameObject)), gameObject.transform.position, Quaternion.identity);
            AddExplosionForce(coll.transform.GetComponent<Rigidbody2D>(), explosionPower * 50, gameObject.transform.position, explosionRadius);
            Destroy(gameObject);
        }
    }
}
