using UnityEngine;
using System.Collections;

public class TurretScript : MonoBehaviour {

    private bool canShoot;
    public float instantiateSpeed;
    public float radius;

	// Use this for initialization
	void Start () {
        InvokeRepeating("SpawnBullets", 0, instantiateSpeed);
	}
	
	// Update is called once per frame
	void Update () {
        CanShootBullets(gameObject.transform.position, radius);
	}

    void CanShootBullets(Vector3 center, float radius)
    {
        Collider2D player = Physics2D.OverlapCircle(center, this.radius, 7 << LayerMask.NameToLayer("Player"));

        if (player != null)
        {
            canShoot = true;
        }
        else if (player == null)
        {
            canShoot = false;
        }
    }

    void SpawnBullets()
    {
        if (canShoot)
        {
            Instantiate(Resources.Load("Prefabs/Bullet", typeof(GameObject)), gameObject.transform.position, Quaternion.identity);
        }
    }
}
