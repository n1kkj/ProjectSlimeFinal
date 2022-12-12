using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace SA
{
    public class EventBoss : MonoBehaviour
    {
        public UnityEvent boss;
        public InputHandler IP;
        public StateManager states;
        public GameObject boss_obj;
        public CameraManager camMan;
        public Rigidbody rb;


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                states.en_Checking = false;
                states.run = false;
                states.lockOn = false;
                states.anim.Play("Rage");
                //states.enabled = false;
                IP.enabled = false;
                camMan.enabled = false;
                //states.enabled = true;
                states.anim.CrossFade("Final", 12f);
                boss.Invoke();

            }
        }
    }

}

