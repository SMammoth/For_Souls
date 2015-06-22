using UnityEngine;
using System.Collections;

public class NewBulletScript : MonoBehaviour {

    /// <summary>
    /// Destroy object when it collides
    /// </summary>
    /// <param name="col"></param>
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject)
        {
            Destroy(gameObject);
        }

        //Todo: when bullet hits enemy kill enemy/ do damage
    }
}
