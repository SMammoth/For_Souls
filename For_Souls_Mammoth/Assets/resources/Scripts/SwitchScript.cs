using UnityEngine;
using System.Collections;

public class SwitchScript : MonoBehaviour 
{
	[Header("Visuals")]
	public Sprite ClosedDoor;
	public Sprite OpenDoor;

	public Sprite SwitchOFF;
	public Sprite SwitchON;

	GameObject EndLevel;

	// Use this for initialization
	void Awake () 
	{
		EndLevel = GameObject.FindWithTag("EndLevel");

		//Door stats
		EndLevel.GetComponent<PolygonCollider2D> ().enabled = false;
		EndLevel.GetComponent<Animator> ().SetBool ("Open", false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	void OnTriggerEnter2D(Collider2D coll) 
	{
		if (coll.tag == "Player")
		{
			if(EndLevel.GetComponent<PolygonCollider2D> ().enabled == false)
			{
				//Switch stats
				gameObject.GetComponent<Animator> ().SetTrigger ("Turn_On");

				//Door stats
				EndLevel.GetComponent<PolygonCollider2D> ().enabled = true;
				EndLevel.GetComponent<Animator> ().SetBool ("Open", true);
			}
		}	
	}
}
