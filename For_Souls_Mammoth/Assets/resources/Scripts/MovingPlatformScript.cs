using UnityEngine;
using System.Collections;

public class MovingPlatformScript : MonoBehaviour 
{
	[Header("Main Speed")]
	public int Speed;

	[Header("Moving up and down")]
	public bool UpDown;
	public GameObject BorderUp;
	public GameObject BorderDown;

	[Header("Moving left and right")]
	public bool LeftRight;
	public GameObject BorderRight;
	public GameObject BorderLeft;

	bool MovingDown;
	bool MovingRight;

	// Use this for initialization
	void Start () 
	{
		MovingDown = true;
		MovingRight = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(UpDown)
		{
			if(MovingDown)
			{
				transform.Translate(Vector3.up * -Speed * Time.deltaTime);

				if(transform.position.y < BorderDown.transform.position.y)
				{
					MovingDown = false;
				}
			}
			else
			{
				transform.Translate(Vector3.up * Speed * Time.deltaTime);

				if(transform.position.y > BorderUp.transform.position.y)
				{
					MovingDown = true;
				}
			}
		}

		if(LeftRight)
		{
			if(MovingRight)
			{
				transform.Translate(Vector3.right * Speed * Time.deltaTime);
				
				if(transform.position.x > BorderRight.transform.position.x)
				{
					MovingRight = false;
				}
			}
			else
			{
				transform.Translate(Vector3.right * -Speed * Time.deltaTime);
				
				if(transform.position.x < BorderLeft.transform.position.x)
				{
					MovingRight = true;
				}
			}
		}
	}
}
