using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerHealth : MonoBehaviour {

    public List<GameObject> healthPoints;
    private bool canSubtract = true;
    private bool bla = true;
    public float cooldownTime;
    private Animator animator;

	// Use this for initialization
	void Start () {
        animator = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        CheckDeath();
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.transform.tag == "Enemy" && canSubtract)
        {
            animator.SetTrigger("Hit");
            if (healthPoints.Count != 0)
            {
                Destroy(healthPoints[healthPoints.Count - 1]);
                healthPoints.RemoveAt(healthPoints.Count - 1);
            }
            canSubtract = false;
            StartCoroutine(WaitForSeconds(cooldownTime));
        }
    }

    void CheckDeath()
    {
        if (healthPoints.Count <= 0)
        {
            if (bla)
            {
                animator.SetBool("Idle", false);
                animator.SetTrigger("Dead");
                GetComponent<BoxCollider2D>().enabled = false;
                GetComponent<Player1_Class>().enabled = false;
                Debug.Log("U ded bro");
                bla = false;
            }
        }
    }

    IEnumerator WaitForSeconds(float cooldownTime)
    {
        yield return new WaitForSeconds(cooldownTime);
        canSubtract = true;
    }
}
