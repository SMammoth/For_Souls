using UnityEngine;
using System.Collections;

public class NewBulletScript : MonoBehaviour {

    Player1_Class player;
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player1_Class>();
    }

    /// <summary>
    /// Destroy object when it collides
    /// </summary>
    /// <param name="col"></param>
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            if(col.gameObject.GetComponent<EnemyAI>().enemyType == "Ranged"){
              GameObject shootUpgrade =  Instantiate(Resources.Load("Folder Dylan/resources/Prefabs/shootUpgrade", typeof(GameObject)) as GameObject);
              shootUpgrade.name = "shootUpgrade";
              shootUpgrade.transform.position = col.gameObject.transform.position;
            }
            else if (col.gameObject.GetComponent<EnemyAI>().enemyType == "Melee")
            {
                GameObject arcThrow = Instantiate(Resources.Load("Folder Dylan/resources/Prefabs/ArcThrowBomb", typeof(GameObject)) as GameObject);
                arcThrow.name = "ArcThrowBomb";
                arcThrow.transform.position = col.gameObject.transform.position;
            }
            player.defauttAttack.SetActive(false);
            Destroy(col.gameObject);
            if (gameObject.name == "New Bullet")
            {
                Destroy(gameObject);
            }

        }
        else if (col.gameObject.tag == "Boss")
        {
            BossScript.health -= BossScript.doDamage;
            Destroy(gameObject);
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
