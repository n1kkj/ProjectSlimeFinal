using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA
{
    public class DefendPlayer : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 9 && gameObject.activeInHierarchy)
            {
                Animator anim = other.gameObject.GetComponentInParent<Animator>();
                if (anim != null)
                {
                    Debug.Log(other.gameObject.GetComponentInParent<Rigidbody>());
                    Rigidbody rigid = other.gameObject.transform.root.GetComponent<Rigidbody>();
                    rigid.isKinematic = false;
                    rigid.AddForce(transform.forward * 7, ForceMode.Impulse);
                    anim.Play(StaticStrings.damage1);
                }
            }
        }

    }
}

