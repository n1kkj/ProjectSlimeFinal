using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA
{
    public class GetDamagePlayer : MonoBehaviour
    {
        public StateManager states;
        public List<AudioSource> sourcesn = new List<AudioSource>();

        public void Init(StateManager st)
        {
            states = st;
        }

        public void Damage(int damage)
        {
            List<AudioSource> sources = sourcesn;
            int ind = Random.Range(0, sources.Count);
            sources[ind].Play();
            states.characterStats.hp -= damage;
            states.anim.Play(StaticStrings.damage2);
            states.CheckHealth();
        }
    }
}


