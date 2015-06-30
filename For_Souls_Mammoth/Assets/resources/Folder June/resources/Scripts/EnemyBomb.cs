using UnityEngine;
using System.Collections;

public class EnemyBomb : EnemyAI {

    public void Awake()
    {
        enemyType = "Bomb";
    }

	// Use this for initialization
	public void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	public void Update () {
        base.Update();
	}
}
