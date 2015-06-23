using UnityEngine;
using System.Collections;

public class EnemyMines : EnemyAI {

	// Use this for initialization
	public void Start () {
        base.Start();
        enemyType = "Mine";
	}
	
	// Update is called once per frame
	public void Update () {
        base.Update();
	}
}
