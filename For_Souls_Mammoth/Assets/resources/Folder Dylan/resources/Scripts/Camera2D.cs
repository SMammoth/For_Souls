using UnityEngine;
using System.Collections;

public class Camera2D : MonoBehaviour {

	public Transform player;
    private Player1_Class playerObject;

	public Vector2 margin, smoothing;

    public float cameraXPos = 8;

	//public BoxCollider2D bounds;
	//private Vector3 _min, _max;

	public bool isFollowing { get; set;} 

	public void Start(){
	//	_min = bounds.bounds.min; // minimum of the level
	//	_max = bounds.bounds.max; // maximun of the level
        playerObject = player.gameObject.GetComponent<Player1_Class>();
		isFollowing = true;
	}

	// Camera stays within the level and will not be outside that box
	// Camera will follow the player
	public void Update(){
		var x = transform.position.x;
		var y = transform.position.y;

		if (isFollowing) {
			if(Mathf.Abs(x - player.position.x) > margin.x)
				x = Mathf.Lerp(x, player.position.x, smoothing.x * Time.deltaTime);

			if(Mathf.Abs(y - player.position.y) > margin.y)
				y = Mathf.Lerp(y, player.position.y, smoothing.y * Time.deltaTime);
		}
        
		var cameraHalfWidth = GetComponent<Camera>().orthographicSize * ((float)Screen.width / Screen.height); // calculate the half width of the main camera screen

		// calculates that the main camera stays in the collisionBox 
	//	x = Mathf.Clamp (x, _min.x + cameraHalfWidth, _max.x - cameraHalfWidth);
	//	y = Mathf.Clamp (y, _min.y + GetComponent<Camera>().orthographicSize, _max.y - GetComponent<Camera>().orthographicSize);

		transform.position = new Vector3 (x, y, transform.position.z);
	}
}
