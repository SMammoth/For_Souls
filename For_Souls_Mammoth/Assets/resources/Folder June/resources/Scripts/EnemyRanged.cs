using UnityEngine;
using System.Collections;

public class EnemyRanged : EnemyAI {

	// Use this for initialization
	public void Start () {
        base.Start();
        enemyType = "Ranged";
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
	}
}
