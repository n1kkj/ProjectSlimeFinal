using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurseHeal : MonoBehaviour
{
    [SerializeField] private float _rad;
    [SerializeField] private float _timer;
    private float deltaTime = 0;
    private Vector3 dis;
    public bool allive;
    void OnTriggerEnter(Collider other)
    {
        deltaTime = Time.deltaTime;
    }


    void Update()
    {
        dis = gameObject.transform.position - GameObject.Find("Controller").transform.position;
        if (dis.magnitude <= _rad)
        {
            if (deltaTime >= _timer)
                {
                Destroy(gameObject.transform.parent.gameObject);
                Destroy(gameObject.transform.parent.GetChild(0).gameObject);
                Destroy(gameObject.transform.parent.GetChild(1).gameObject);
                GameObject.Find("Gameplay").GetComponent<UI_gameplay>().bar = 0;
                //GameObject.Find("Gameplay").GetComponent<UI_gameplay>().damage_coll.damage = 50;

            }
            deltaTime += Time.deltaTime;
        }
        else
        {
            deltaTime = 0;
        }
    }
}
