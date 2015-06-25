using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    //hit-attack (Dylan's code kopiëren)
    //

    [Header("Patrolling")]
    public float walkSpeed;
    public float detectionRange;

    [Header("Attacking")]
    public float shootingDelay;
    private GameObject target;
    private bool canShoot = true;

    [Header("Physics")]
    public float pushbackPower;
    public float pushbackRadius;

    public string enemyType;
    private bool isPatrolling;

    // Use this for initialization
    public void Start()
    {
        isPatrolling = true;
        target = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    public void Update()
    {
        FlipSprite();
        CheckPatrol();

        if (isPatrolling)
        {
            EnemyPatrol();
        }
        else if (!isPatrolling)
        {
            EnemyAttack();
        }
    }

    public void EnemyPatrol()
    {
        //Enemy beweegt tijdens zijn patrol (duh)
        gameObject.transform.Translate(new Vector3(walkSpeed * transform.localScale.x * Time.deltaTime, 0, 0));
    }

    public void EnemyAttack()
    {
        transform.Translate(new Vector3(walkSpeed * transform.localScale.x * Time.deltaTime, 0, 0));

        if (enemyType == "Ranged")
        {
            if (canShoot)
            {
                //Schiet bullets met een bepaalde shooting rate
                Instantiate(Resources.Load("Prefabs/Bullet", typeof(GameObject)), gameObject.transform.position, Quaternion.identity);
                canShoot = false;
                StartCoroutine(WaitForSeconds(shootingDelay));
            }
        }

        if (enemyType == "Mine")
        {

        }
    }

    public bool CheckPatrol()
    {
        //Check of speler zichtbaar is
        RaycastHit2D checkRay = Physics2D.Raycast(transform.position, target.transform.position - transform.position);

        //Patrol wel of niet
        if (Vector3.Distance(transform.position, target.transform.position) > detectionRange
            || (Vector3.Distance(transform.position, target.transform.position) <= detectionRange && checkRay.collider.tag != "Player"))
        {
            return isPatrolling = true;
        }
        else
        {
            return isPatrolling = false;
        }
    }

    public void FlipSprite()
    {
        //Check collision left and right
        RaycastHit2D checkCollisionRight = Physics2D.Raycast(transform.position, Vector2.right, 2);
        RaycastHit2D checkCollisionLeft = Physics2D.Raycast(transform.position, Vector2.right, -2);
        Vector3 objectScale = transform.localScale;

        //Flip speed direction
        if (checkCollisionRight && checkCollisionRight.transform.tag != "Player" 
            || (transform.localPosition.x - target.transform.position.x > 0 && !isPatrolling))
        {
            objectScale.x = -1;
            transform.localScale = objectScale;
        }
        else if (checkCollisionLeft && checkCollisionLeft.transform.tag != "Player" 
            || (transform.localPosition.x - target.transform.position.x <= 0 && !isPatrolling))
        {
            objectScale.x = 1;
            transform.localScale = objectScale;
        }
    }

    public void AddExplosionForce(Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        var bodyDirection = (body.transform.position - explosionPosition);
        float calc = 1 - (bodyDirection.magnitude / explosionRadius);
        if (calc <= 0)
        {
            calc = 0;
        }
        body.AddForce(bodyDirection.normalized * explosionForce * calc);
    }

    public void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.transform.tag == "Player")
        {
            AddExplosionForce(coll.transform.GetComponent<Rigidbody2D>(), pushbackPower * 100, gameObject.transform.position, pushbackRadius);
        }
    }

    public IEnumerator WaitForSeconds(float shootingDelay)
    {
        yield return new WaitForSeconds(shootingDelay);
        canShoot = true;
    }
}
