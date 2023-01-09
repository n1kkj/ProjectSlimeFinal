using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SA;

public class Weapon_anim_controller : MonoBehaviour
{
    public void Weapon_activate()
    {
        GameObject weapon = GameObject.Find("DamageCollider");
        weapon.GetComponent<BoxCollider>().enabled = true;
    }
    public void Weapon_disactivate()
    {
        GameObject weapon = GameObject.Find("DamageCollider");
        weapon.GetComponent<BoxCollider>().enabled = false;
    }
}
