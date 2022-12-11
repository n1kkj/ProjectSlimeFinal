using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventBoss : MonoBehaviour
{
    public UnityEvent boss;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            boss.Invoke();
    }
}
