using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA
{
    public class GetDamageEnemy : MonoBehaviour
    {
        public EnemyStates eStates;
        public AIHandler aiHandler;
        public List<AudioSource> dmg = new List<AudioSource>();
        public AudioSource audio;
        public GameObject weapon;

        public void Init(EnemyStates eSt)
        {
            eStates = eSt;
            weapon = transform.root.transform.GetChild(0).GetChild(0).gameObject;
            Debug.Log(weapon);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 9 && other.gameObject.tag == "Player")
            {
                /*float seed = Random.Range(-0.7f, 0f);
                int rnd = Random.Range(0, dmg.Count);*/

                //dmg[rnd].pitch = seed

                gameObject.transform.GetChild(0).GetComponent<WeaponHook>().CloseDamageCollidersWeapon();
                other.GetComponent<DamageCollider>().enabled = false;
                aiHandler.stunned = true;
                aiHandler.s_t = 0;
                eStates.characterStats.hp -= other.gameObject.GetComponent<DamageCollider>().damage;
                aiHandler.aiState = AIHandler.AIState.close;
                Debug.Log(aiHandler.aiState);
                eStates.anim.Play(StaticStrings.damage2);
                eStates.CheckHealth(weapon);
                audio.Play();
            }
        }
    }
}

