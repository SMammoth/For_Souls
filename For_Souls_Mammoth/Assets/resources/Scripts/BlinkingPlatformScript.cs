using UnityEngine;
using System.Collections;

public class BlinkingPlatformScript : MonoBehaviour 
{
	[Header("BlinkSpeed")]
	public float Speed;

	public bool StartInvisible;

	bool Invisible;
	
	// Use this for initialization
	void Start () 
	{
		if(StartInvisible)
		{
			Invisible = true;
		}
		else
		{
			Invisible = false;
		}

		StartCoroutine (Blinking ());
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	IEnumerator Blinking()
	{
		if(Invisible)
		{
			GetComponent<Renderer>().enabled = true;
			GetComponent<PolygonCollider2D>().enabled = true;

		//	gameObject.GetComponentsInChildren<collider2D>().enabled = true;

			Invisible = false;
		}
		else
		{
			GetComponent<Renderer>().enabled = false;
            GetComponent<PolygonCollider2D>().enabled = false;
			Invisible = true;
		}

		yield return new WaitForSeconds (Speed);

		StartCoroutine (Blinking ());
	}
}
