using UnityEngine;
using System.Collections;

public class EnemyMelee : EnemyAI {

    public void Awake()
    {
        enemyType = "Melee";
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
