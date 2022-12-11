using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnemies : MonoBehaviour
{
    public bool emptyZone = true;
    public GameObject campfire;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Contains("enemy"))
            emptyZone = false;
    }

    private void FixedUpdate()
    {
        emptyZone = true;

        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        yield return new WaitForEndOfFrame();
        if (!emptyZone)
            campfire.SetActive(true);
    }
}
