using UnityEngine;
using System.Collections;

public class EnemyRanged : EnemyAI {

    public void Awake()
    {
        enemyType = "Ranged";
    }

	// Use this for initialization
	public void Start () {
        base.Start();
	}
	
	// Update is called once per frame
	void Update () {
        base.Update();
	}
}
