using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelOptionsScript : MonoBehaviour 
{
	public string LevelName;

	[Header("VISUAL EFFECTS")]

	[Header("Camera Rotation")]

	public bool RotateCam;

	public bool RightTurn;
	public bool LeftTurn;
	public int TurnSpeed;
	[Space(10)]

	public bool TurnedCam;
	public int TurnedCamDegrees;
	[Space(10)]

	public bool ShakyCam = false;
	public float ShakeAmt = 0;
	public bool ShakeUp;
	public bool ShakeDown;
	
	[Header("Zoom and Follow Player")]
	public bool ZoomAndFollow;
	public bool ZoomedOut;
	public bool FlashLight;

	[Header("Image Effects")]
	public bool GrayScale; 	//is for pro
	public bool Noise;		//is for pro

	[Header("Player Effects")]
	public bool SpriteFlip;
	public bool BouncyPlayer;

	[Header("GAMEPLAY SETTINGS")]
	[Header("Platform Effector Settings")]
	public bool SideFriction;
	public bool SideBounce;
	public float SideAngle;

	PlatformEffector2D[] Platforms;

	Camera MainCamera;
	Vector3 PlayerPos;

	GameObject[] Players;



	void Start () 
	{
		MainCamera = Camera.main;
		
		Players = GameObject.FindGameObjectsWithTag ("Player");

		Platforms = gameObject.GetComponentsInChildren<PlatformEffector2D> ();

		GameObject.FindWithTag ("UI_Text_LevelName").GetComponent<Text> ().text = "LEVEL: " + LevelName;
		
		foreach(PlatformEffector2D Platform in Platforms)
		{
			Platform.useSideFriction = SideFriction;
			Platform.useSideBounce = SideBounce;
			Platform.sideAngleVariance = SideAngle;
		}

		if(TurnedCam)
		{
			MainCamera.transform.rotation *= Quaternion.Euler(0, 0, TurnedCamDegrees);
		}

		if(ZoomAndFollow)
		{
			foreach(GameObject Player in Players)
			{
				MainCamera.transform.parent = Player.transform;
				MainCamera.transform.localPosition = new Vector3(0, 0, -10);
				MainCamera.orthographicSize = 4;
			}
		}
		
		if(ZoomedOut)
		{
			MainCamera.orthographicSize = 60;
		}

		if(FlashLight)
		{
			GameObject.Find("Sun").SetActive(false);

			foreach(GameObject Player in Players)
			{
				Player.GetComponentInChildren<Light>().enabled = true;
			} 

		}

		if(GrayScale)
		{
			MainCamera.GetComponent<GrayscaleEffect>().enabled = true;
		}

		if(Noise)
		{
			MainCamera.GetComponent<NoiseEffect>().enabled = true;
		}

		if(SpriteFlip)
		{ 
			foreach(GameObject Player in Players)
			{
				Player.transform.rotation *= Quaternion.Euler(0, 180, 0);
			}
		}

		if(BouncyPlayer)
		{
			foreach(GameObject Player in Players)
			{
				Player.GetComponent<CircleCollider2D>().sharedMaterial = (PhysicsMaterial2D)Resources.Load("Materials/Bouncy");
				Player.GetComponent<PolygonCollider2D>().sharedMaterial = (PhysicsMaterial2D)Resources.Load("Materials/Bouncy");
			} 
		}
	}
	
	void Update () 
	{
		if(RotateCam)
		{
			if(RightTurn)
			{
				//rotate right * speedturn
				MainCamera.transform.Rotate(Vector3.forward, TurnSpeed * Time.deltaTime);
			}
			else if(LeftTurn)
			{
				//rotate left * speedturn
				MainCamera.transform.Rotate(Vector3.forward, -TurnSpeed * Time.deltaTime);
			}
		}
	}
}
