using UnityEngine;
using System.Collections;

public class BombScript : MonoBehaviour {

    public float explosionPower;
    public float explosionRadius;

    GameObject particleGameObject;
    GameObject upgraded;

    Player1_Class player;
	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("Player").GetComponent<Player1_Class>();
       /* upgraded = GameObject.Find("Upgraded");
        upgraded.SetActive(false);*/
	}
	
	// Update is called once per frame
	void Update () {

	}

    //Add force to rigidbody (explosion effect)
    public static void AddExplosionForce(Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        var bodyDirection = (body.transform.position - explosionPosition);
        float calc = 1 - (bodyDirection.magnitude / explosionRadius);
        if (calc <= 0)
        {
            calc = 0;
        }
        body.AddForce(bodyDirection.normalized * explosionForce * calc);
    }

    public virtual void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.transform.tag == "Player")
        {
            particleGameObject = Instantiate(Resources.Load("Prefabs/Particles/Particle_Death", typeof(GameObject)), gameObject.transform.position, Quaternion.identity) as GameObject;
           AddExplosionForce(coll.transform.GetComponent<Rigidbody2D>(), explosionPower * 100, gameObject.transform.position, explosionRadius);
           Destroy(gameObject);
           Destroy(particleGameObject, .001f);
        }
        if (coll.gameObject.tag == "Enemy")
        {
            coll.gameObject.GetComponent<Animator>().SetTrigger(coll.gameObject.GetComponent<EnemyAI>().death);
            particleGameObject = Instantiate(Resources.Load("Prefabs/Particles/Particle_Death", typeof(GameObject)), gameObject.transform.position, Quaternion.identity) as GameObject;
            AddExplosionForce(coll.transform.GetComponent<Rigidbody2D>(), explosionPower * 50, gameObject.transform.position, explosionRadius);
            Destroy(gameObject, 0.5f);
            Destroy(particleGameObject, .001f);
            if (coll.gameObject.GetComponent<EnemyAI>().enemyType == "Ranged")
            {
                GameObject shootUpgrade = Instantiate(Resources.Load("Folder Dylan/resources/Prefabs/shootUpgrade", typeof(GameObject)) as GameObject);
                shootUpgrade.name = "shootUpgrade";
                shootUpgrade.transform.position = coll.gameObject.transform.position;
            }
            else if (coll.gameObject.GetComponent<EnemyAI>().enemyType == "Mine")
            {
                GameObject arcThrow = Instantiate(Resources.Load("Folder Dylan/resources/Prefabs/FireballPlayer", typeof(GameObject)) as GameObject);
                arcThrow.name = "ArcThrowBomb";
                arcThrow.transform.position = coll.gameObject.transform.position;
            }
            else if (coll.gameObject.GetComponent<EnemyAI>().enemyType == "Melee")
            {
                GameObject arcAttack = Instantiate(Resources.Load("Folder Dylan/resources/Prefabs/arcPunchUpgrade", typeof(GameObject)) as GameObject);
                arcAttack.name = "arcPunchUpgrade";
                arcAttack.transform.position = coll.gameObject.transform.position;
            }
            Destroy(coll.gameObject);
        }
        else if (coll.gameObject.tag == "Boss")
        {
            BossScript.health -= BossScript.doDamage;
            Destroy(gameObject);
        }
    }
}
