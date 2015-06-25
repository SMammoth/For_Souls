using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{
    private GameObject[] players;
    private GameObject closestPlayer = null;
    private GameObject closestBullet;

    private GameObject target;

    private Vector3 targetPos;
    private Vector3 posDifference;
    private Vector3 position;

    private float currentDistance;
    private float distance;
    public float bulletSpeed;

    // Use this for initialization
    public virtual void Start()
    {
        //Find position for bullet to go to
        target = FindClosestPlayer();
        targetPos = target.transform.position;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, bulletSpeed * Time.deltaTime);
        }

        if (transform.position == targetPos)
        {
            Destroy(gameObject);
        }
    }

    //Bullet finds closest player
    GameObject FindClosestPlayer()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        distance = Mathf.Infinity;
        position = transform.position;

        foreach (GameObject player in players)
        {
            posDifference = player.transform.position - position;
            currentDistance = posDifference.sqrMagnitude;

            if (currentDistance < distance)
            {
                closestPlayer = player;
                distance = currentDistance;
            }
        }

        return closestPlayer;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.transform.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
