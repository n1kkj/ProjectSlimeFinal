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


        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                states.run = false;
                states.lockOn = true;
                states.lockOnTarget = boss_obj.GetComponent<EnemyTarget>();
                states.lockOnTransform = states.lockOnTarget.GetTarget();
                camMan.lockonTarget = states.lockOnTarget;
                camMan.lockonTransform = states.lockOnTransform;
                camMan.lockon = states.lockOn;
                IP.enabled = false;
                states.enabled = false;
                boss.Invoke();
            }
        }
    }

}

