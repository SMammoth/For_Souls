using UnityEngine;
using System.Collections;

public class EnemyBomb : EnemyAI {

	// Use this for initialization
	void Start () {
        base.Start();
        enemyType = "Bomb";
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
	}
}
