using UnityEngine;
using System.Collections;

public class TeleporterPlatformScript : MonoBehaviour
{

    private GameObject teleporterParent;
    private Component[] colliders2D;
    public float timeInactive;

    // Use this for initialization
    void Start()
    {
        teleporterParent = gameObject.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        //Teleport on trigger
        if (coll.gameObject.tag == "Player")
        {
            if (gameObject.name == "Entrance_1")
            {
                coll.gameObject.transform.position = gameObject.transform.parent.FindChild("Entrance_2").transform.position;
            }
            else
            {
                coll.gameObject.transform.position = gameObject.transform.parent.FindChild("Entrance_1").transform.position;
            }
            StartCoroutine(WaitForSeconds(timeInactive));
        }
    }

    //Puts time between teleports
    IEnumerator WaitForSeconds(float resetTimer)
    {
        colliders2D = teleporterParent.GetComponentsInChildren<Collider2D>();

        foreach (Collider2D collider2D in colliders2D)
        {
            collider2D.enabled = false;
        }

        yield return new WaitForSeconds(resetTimer);

        foreach (Collider2D collider2D in colliders2D)
        {
            collider2D.enabled = true;
        }
    }
}
