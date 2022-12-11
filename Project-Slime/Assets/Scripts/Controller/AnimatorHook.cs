using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class AnimatorHook : MonoBehaviour
    {

        Animator anim;
        public StateManager states;
        public EnemyStates eStates;
        Rigidbody rigid;
        

        public float rm_multi;
        public bool rolling;
        public float roll_t;
        float delta;
        public bool jumping;
        AnimationCurve roll_curve;

        public void Init(StateManager st, EnemyStates eSt)
        {
            rolling = false;

            states = st;
            eStates = eSt;
            if (st != null)
            { 
                anim = st.anim; 
                rigid = st.rigid;
                roll_curve = states.roll_curve;
                delta = st.delta;
            }
                
            if (eSt != null)
            {
                anim = eSt.anim;
                rigid = eSt.rigid;
                delta = eSt.delta;
            }

            //Debug.Log(states + " --------------- " + eStates);
        }

        public void InitForRoll()
        {
            //Debug.Log("initting");
            rolling = true;
            roll_t = 0;
        }

        /*public void OpenRotationControl()
        {
            if (states)
            {
                states.canRotate = true;
            }

            if (eStates)
            {
                eStates.rotateToTarget = true;
            }
            
        }

        public void CloseRotationControl()
        {
            if (states)
            {
                states.canRotate = false;
            }

            if (eStates)
            {
                eStates.rotateToTarget = false;
            }

        }*/

        public void CloseRoll()
        {
            //Debug.Log("CloseingDf");
            if (rolling == false)
                return;
            rm_multi = 1;
            rolling = false;
        }

        private void OnAnimatorMove()
        {

            if (states == null && eStates == null)
            {
               // Debug.Log("COMING OUT" + 95);
                return;
            }
                

            if (rigid == null)
            {
               // Debug.Log("COMING OUT" + 103);
                return;
            }

            if (jumping)
            {
              //  Debug.Log("COMING OUT" + 109);
                return;
            }

            if (states != null)
            {
                if (states.canMove)
                    return;
               // Debug.Log("COMING OUT" + 117);
                delta = states.delta;
                roll_t += delta;
            }

            if (eStates != null)
            {
                if (eStates.canMove)
                    return;
               // Debug.Log("COMING OUT" + 125);
                delta = eStates.delta;
            }

            rigid.drag = 0;

            if (rm_multi == 0)
                rm_multi = 1;

            rolling = true;
           // Debug.Log("COMING OUT");
            if (rolling == false)
            {
                Vector3 delta2 = anim.deltaPosition;
                delta2.y = 0;

                Vector3 v = Vector3.zero;

                if (delta != 0)
                    v = (delta2 * rm_multi) / delta;
                else
                    return;

                /*if (states) <---------------------------- 38 part
                {

                }*/
                if (eStates)
                {
                    eStates.agent.velocity = v;
                }
                else
                {
                    rigid.velocity = v;
                }
                
            }
            else
            {
               // Debug.Log("IT IS WHAT IT IS");
                roll_t += delta / 0.6f;
                if (roll_t > 1)
                    roll_t = 1;

                if (states == null)
                {
                    //Debug.Log("OUT");
                    return;
                }

                //Debug.Log("Rolling");
                anim.applyRootMotion = true;
                float zValue = roll_curve.Evaluate(roll_t) * 10;
                Vector3 v1 = Vector3.forward * zValue;
                Vector3 relative = transform.TransformDirection(v1);
                Vector3 v2 = (relative * rm_multi);
                rigid.velocity = v2;
            }

           
        }

        public void OpenDamageColliders()
        {
            if (states == null)
                return;
            states.inventoryManager.OpenAllDamageColliders();
        }

        public void CloseDamageColliders()
        {
            if (states == null)
                return;

            states.inventoryManager.CloseAllDamageColliders();
        }

    }
}

