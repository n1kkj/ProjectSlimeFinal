using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA
{
    public class DamageCollider : MonoBehaviour
    {
        public int damage;
        public int add_damage;
        GetDamagePlayer g;

        void Init(StateManager st)
        {

        }

        void Start()
        {
            g = FindObjectOfType<GetDamagePlayer>();
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject.tag == "Player")
                g.Damage(damage);
        }
    }
}

