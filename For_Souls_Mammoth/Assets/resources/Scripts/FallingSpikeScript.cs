using UnityEngine;
using System.Collections;

public class FallingSpikeScript : MonoBehaviour 
{
	[Header("FallinSpeed")]
	public float FallSpeed;
		
	// Use this for initialization
	void Start () 
	{
		gameObject.GetComponentInChildren<Rigidbody2D> ().gravityScale = FallSpeed;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnTriggerEnter2D(Collider2D coll) 
	{
		if (coll.tag == "Player")
		{
			gameObject.GetComponentInChildren<Rigidbody2D>().isKinematic = false;
		}	
	}
}
