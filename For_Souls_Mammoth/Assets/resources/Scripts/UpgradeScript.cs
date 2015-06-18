using UnityEngine;
using System.Collections;

public class UpgradeScript : MonoBehaviour 
{
	Vector2 OriginalPos;
	Vector2 CurrentPos;

	bool MovingUp;

	// Use this for initialization
	void Start () 
	{
		OriginalPos = transform.position;
		CurrentPos = transform.position;


		MovingUp = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		CurrentPos = transform.position;

		if(CurrentPos.y >= OriginalPos.y + .15f)
		{
			MovingUp = false;
		}
		if(CurrentPos.y <= OriginalPos.y - .15f)
		{
			MovingUp = true;
		}

		if(MovingUp)
		{
			transform.Translate(0, (Time.deltaTime / 2), 0);
		}
		else if(!MovingUp)
		{
			transform.Translate(0, (-Time.deltaTime / 2), 0);
		}
	}
}
