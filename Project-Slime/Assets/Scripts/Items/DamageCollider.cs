using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA
{
    public class DamageCollider : MonoBehaviour
    {
        /*StateManager states;*/
        public int damage;
        public int add_damage = 0;

        public void Init(StateManager st)
        {
            //states = st;

        }

        /*private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Sword");
            *//*if (other.transform.GetComponentInParent<EnemyStates>() != null)
            {
                EnemyStates eStates = other.transform.GetComponentInParent<EnemyStates>();

                if (eStates == null)
                    return;

                eStates.DoDamage(states.currentAction);
            }*//*
            
        }*/
    }
}

