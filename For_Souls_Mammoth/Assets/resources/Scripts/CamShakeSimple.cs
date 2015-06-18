using UnityEngine;
using System.Collections;

public class CamShakeSimple : MonoBehaviour 
{
	public string[] Collidables; //add the tags of shakable objects
	
	Vector3 originalCameraPosition;
	
	public float shakeAmt;
	
	Camera mainCamera;
	
	void Start()
	{
		mainCamera = Camera.main;
		originalCameraPosition = mainCamera.transform.position;
	}
	
	public void Shake () 
	{
		InvokeRepeating("CameraShake", 0, .01f);
		Invoke("StopShaking", 0.3f);
	}
	
	void CameraShake()
	{
		if(shakeAmt>0) 
		{
			float quakeAmt = Random.value * shakeAmt * 2 - shakeAmt;
			Vector3 pp = mainCamera.transform.position;
			pp.x += quakeAmt; // can also add to x and/or z
			//pp.y += quakeAmt;
			mainCamera.transform.position = pp;
		}
	}
	
	void StopShaking()
	{
		Vector3 newPos = originalCameraPosition;
		CancelInvoke("CameraShake");
		mainCamera.transform.position = newPos;
	}
	
	
}