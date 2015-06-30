using UnityEngine;
using System.Collections;

public class NewBulletScript : MonoBehaviour {

    Player1_Class player;
    public bool shooting = false;
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player1_Class>();
    }

    void Update()
    {
        if (shooting)
        {
            if (player.transform.localScale.x == 5)
            {
                transform.localScale = new Vector3(5, 5, 5);
            }
            else if (player.transform.localScale.x == -5)
            {
                transform.localScale = new Vector3(-5, 5, 5);
            }
        }
    }

    /// <summary>
    /// Destroy object when it collides
    /// </summary>
    /// <param name="col"></param>
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<Animator>().SetTrigger(col.gameObject.GetComponent<EnemyAI>().death);
            if(col.gameObject.GetComponent<EnemyAI>().enemyType == "Ranged"){
              GameObject shootUpgrade =  Instantiate(Resources.Load("Folder Dylan/resources/Prefabs/shootUpgrade", typeof(GameObject)) as GameObject);
              shootUpgrade.name = "shootUpgrade";
              shootUpgrade.transform.position = col.gameObject.transform.position;
            }
            else if (col.gameObject.GetComponent<EnemyAI>().enemyType == "Melee")
            {
                GameObject arcThrow = Instantiate(Resources.Load("Folder Dylan/resources/Prefabs/arcPunchUpgrade", typeof(GameObject)) as GameObject);
                arcThrow.name = "arcPunchUpgrade";
                arcThrow.transform.position = col.gameObject.transform.position;
            }
            else if (col.gameObject.GetComponent<EnemyAI>().enemyType == "Bomb")
            {
                GameObject arcAttack = Instantiate(Resources.Load("Folder Dylan/resources/Prefabs/ArcThrowBomb", typeof(GameObject)) as GameObject);
                arcAttack.name = "ArcThrowBomb";
                arcAttack.transform.position = col.gameObject.transform.position;
            }
            player.defauttAttack.SetActive(false);
            col.gameObject.GetComponent<EnemyAI>().enabled = false;
            col.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            Destroy(col.gameObject, 3);
            if (gameObject.name == "New Bullet")
            {
                Destroy(gameObject);
            }

        }
        else if (col.gameObject.tag == "Boss")
        {
            BossScript.health -= BossScript.doDamage;
            if (gameObject.name != "DefaultAttack" || gameObject.name != "ArcUpgradeAttack")
            {
                Destroy(gameObject);
            }
        }
        else if (col.gameObject.tag != "Enemy" && col.gameObject.tag != "Boss")
        {
            if (gameObject.name == "New Bullet")
            {
                Destroy(gameObject);
            }
        }

        //Todo: when bullet hits enemy kill enemy/ do damage
    }
}
