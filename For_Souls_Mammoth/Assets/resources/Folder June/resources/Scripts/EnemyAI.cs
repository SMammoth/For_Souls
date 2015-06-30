using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    [Header("Patrolling")]
    public float walkSpeed;
    public float detectionRange;

    [Header("Ranged Enemy")]
    public float shootingDelay;
    private bool canShoot = true;

    [Header("Melee Enemy")]
    public float hitDelay;
    public GameObject arcAttack;
    private Quaternion oldRotation;
    private bool waitPls;
    private bool canHit = true;

    [Header("Bomb Enemy")]
    public float throwDelay;
    private GameObject arcThrow;
    private bool canThrow = true;

    [Header("Physics")]
    public float pushbackPower;
    public float pushbackRadius;

    public string enemyType;
    private bool doAnimation;
    private GameObject target;
    private bool isPatrolling;

    // Use this for initialization
    public void Start()
    {
        isPatrolling = true;
        target = GameObject.FindWithTag("Player");

        if (enemyType == "Melee")
        {
            arcAttack = GameObject.Find("ArkAttackEnemy").gameObject;
            oldRotation = arcAttack.transform.rotation;
        }
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
                StartCoroutine(WaitForSecondsRanged(shootingDelay));
            }
        }

        if (enemyType == "Melee")
        {
            if (canHit)
            {
                //Enemy slaat erop los 
                ArcAttack(20, 300);
                StartCoroutine(WaitForSecondsMelee(hitDelay));
            }
        }

        if (enemyType == "Bomb")
        {
            Debug.Log(canThrow);
            if (canThrow)
            {
                ArcThrow(30000, 30000);
                StartCoroutine(WaitForSecondsBomb(throwDelay));
            }
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
            objectScale.x = -5f;
            transform.localScale = objectScale;
        }
        else if (checkCollisionLeft && checkCollisionLeft.transform.tag != "Player"
            || (transform.localPosition.x - target.transform.position.x <= 0 && !isPatrolling))
        {
            objectScale.x = 5f;
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

    //Melee arc strike
    void ArcAttack(float hitTime, float rotateTime)
    {
        doAnimation = true;

        if (arcAttack.transform.position.y >= gameObject.transform.FindChild("targetArkEnemy").transform.position.y - .1f && arcAttack.transform.position.y <= gameObject.transform.FindChild("targetArkEnemy").transform.position.y + .1f)
        {
            if (transform.localScale.x == 5f)
            {
                arcAttack.transform.position = new Vector2(gameObject.transform.position.x + 1, gameObject.transform.position.y + 1);
            }
            else if (transform.localScale.x == -5f)
            {
                arcAttack.transform.position = new Vector2(gameObject.transform.position.x - 1, gameObject.transform.position.y + 1);
            }
            arcAttack.transform.rotation = oldRotation;
            arcAttack.SetActive(false);
            doAnimation = false;
            canHit = false;
        }

        if (doAnimation)
        {
            arcAttack.SetActive(true);
            arcAttack.transform.position = Vector2.MoveTowards(arcAttack.transform.position, GameObject.Find("targetArkEnemy").transform.position, hitTime * Time.deltaTime);
            arcAttack.transform.rotation = Quaternion.RotateTowards(arcAttack.transform.rotation, GameObject.Find("targetArkEnemy").transform.rotation, rotateTime * Time.deltaTime);
        }
    }

    //Arc throw - bomb
    void ArcThrow(float arcSpeed, float verticalSpeed)
    {
        arcThrow = Instantiate(Resources.Load("Folder Dylan/resources/Prefabs/ArcThrow", typeof(GameObject)) as GameObject);
        arcThrow.transform.position = gameObject.transform.position;

        if (arcThrow != null)
        {
            if (transform.localScale.x == 5) //shooting to the right.
            {
                arcThrow.GetComponent<Rigidbody2D>().AddForce(Vector2.up * verticalSpeed * Time.deltaTime);
                arcThrow.GetComponent<Rigidbody2D>().AddForce(Vector2.right * arcSpeed * Time.deltaTime);
            }
            else if (transform.localScale.x == -5) //shooting to the left.
            {
                arcThrow.GetComponent<Rigidbody2D>().AddForce(Vector2.up * verticalSpeed * Time.deltaTime);
                arcThrow.GetComponent<Rigidbody2D>().AddForce(-Vector2.right * arcSpeed * Time.deltaTime);
            }
            Physics2D.IgnoreCollision(arcThrow.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>()); // ignore the player
            canThrow = false;
            Destroy(arcThrow, 5);
        }
    }

    public IEnumerator WaitForSecondsRanged(float delay)
    {
        yield return new WaitForSeconds(delay);
        canShoot = true;
    }

    public IEnumerator WaitForSecondsMelee(float delay)
    {
        yield return new WaitForSeconds(delay);
        canHit = true;
    }

    public IEnumerator WaitForSecondsBomb(float delay)
    {
        yield return new WaitForSeconds(delay);
        canThrow = true;
    }
}
