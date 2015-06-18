using UnityEngine;
using System.Collections;

public class PlatformManager : MonoBehaviour 
{
	[Header("Platform Effector Settings")]
	public bool SideFriction;
	public bool SideBounce;
	public float SideAngle;

	PlatformEffector2D[] Platforms;


	// Use this for initialization
	void Start () 
	{
		Platforms = gameObject.GetComponentsInChildren<PlatformEffector2D> ();

		foreach(PlatformEffector2D Platform in Platforms)
		{
			Platform.useSideFriction = SideFriction;
			Platform.useSideBounce = SideBounce;
			Platform.sideAngleVariance = SideAngle;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{

	}
}
