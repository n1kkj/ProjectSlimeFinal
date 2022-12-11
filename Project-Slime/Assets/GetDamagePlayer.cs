using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA
{
    public class GetDamagePlayer : MonoBehaviour
    {
        public StateManager states;
        public List<AudioSource> sources = new List<AudioSource>();

        public void Init(StateManager st)
        {
            states = st;
        }

        private void OnTriggerEnter(Collider other)
        {
            DamageCollider damag = other.gameObject.GetComponent<DamageCollider>();

            if (damag != null && other.gameObject.tag == "Enemy")
            {
                int ind = Random.Range(0, sources.Count);
                sources[ind].Play();

                gameObject.transform.GetChild(0).GetComponent<WeaponHook>().CloseDamageCollidersWeapon();
                Debug.Log(other.gameObject.name);
                states.characterStats.hp -= damag.damage;
                states.anim.Play(StaticStrings.damage2);
                states.CheckHealth();
            }
        }
    }
}


