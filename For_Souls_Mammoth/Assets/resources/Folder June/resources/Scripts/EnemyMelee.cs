using UnityEngine;
using System.Collections;

public class EnemyMelee : EnemyAI {

	// Use this for initialization
	public void Start () {
        base.Start();
        enemyType = "Melee";
	}
	
	// Update is called once per frame
	public void Update () {
        base.Update();
	}
}
