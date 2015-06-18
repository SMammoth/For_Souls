using UnityEngine;
using System.Collections;

public class MusicObjectScript : MonoBehaviour 
{
	GameObject[] musicObject;

	// Use this for initialization
	void Start () 
	{
		musicObject = GameObject.FindGameObjectsWithTag ("Music");
		if (musicObject.Length == 1 ) {
			GetComponent<AudioSource>().Play ();
		} else {
			for(int i = 1; i < musicObject.Length; i++){
				Destroy(musicObject[i]);
			}
		}

		DontDestroyOnLoad(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
