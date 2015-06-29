using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BossScript : MonoBehaviour {

    Vector2 OriginalPos;
    Vector2 CurrentPos;
    bool MovingUp, movingLeft;
    private GameObject prefabBullet, bullet;
    public float bulletTimer = 3, speedBullet = 2000, prevBulletTimer;

    public bool canShoot = false;

    private Image bossHP;

    public static float doDamage = Random.Range(1, 5), health = 100;

	// Use this for initialization
	void Start () {
        OriginalPos = transform.position;
        CurrentPos = transform.position;

        prefabBullet = Resources.Load("Folder Dylan/resources/Prefabs/New Bullet", typeof(GameObject)) as GameObject;
        MovingUp = false;
        movingLeft = true;
        prevBulletTimer = bulletTimer;
        bossHP = GameObject.Find("BossHealth").GetComponent<Image>();
        bossHP.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 1.5f);

	}
	
	// Update is called once per frame
	void Update () {
        BossHealth();
        Behaviour();
        Shooting(speedBullet);

        if (Vector2.Distance(gameObject.transform.position, GameObject.FindWithTag("Player").transform.position) < 30)
        {
            canShoot = true;
        }
        else
        {
            canShoot = false;
        }

	}

    void Behaviour()
    {
        CurrentPos = transform.position;

        if (CurrentPos.y >= OriginalPos.y + .5f)
        {
            MovingUp = false;
        }
        if (CurrentPos.y <= OriginalPos.y - .5f)
        {
            MovingUp = true;
        }
        if (CurrentPos.x >= OriginalPos.x + .5f)
        {
            movingLeft = false;
        }
        if (CurrentPos.x <= OriginalPos.x -.5f)
        {
            movingLeft = true;
        }

        if (MovingUp)
        {
            transform.Translate(0, (Time.deltaTime / 1.4f), 0);
        }
        else if (!MovingUp)
        {
            transform.Translate(0, (-Time.deltaTime / 1.4f), 0);
        }

        if (movingLeft)
        {
            transform.Translate((Time.deltaTime / 1.2f), 0, 0);
        }
        else if (!movingLeft)
        {
            transform.Translate( (-Time.deltaTime / 1.2f),0, 0);
        }
    }

    void Shooting(float bulletSpeed)
    {
        if (canShoot)
        {
            bulletTimer -= Time.deltaTime;
            if (bulletTimer < 1)
            {
                bullet = Instantiate(prefabBullet, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - .2f), Quaternion.identity) as GameObject;
                Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
                bullet.GetComponent<Rigidbody2D>().AddForce(-Vector2.right * bulletSpeed * Time.deltaTime);
                bulletTimer = prevBulletTimer;
            }
            Destroy(bullet, 3);
        }
    }

    void BossHealth()
    {
        bossHP.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 1.5f);

        if (health < 1)
        {
            Destroy(gameObject);
        }
        bossHP.fillAmount = (health / 100);
    }
}
