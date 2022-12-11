using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class Helper : MonoBehaviour
    {
        [Range(-1f, 1f)] 
        public float vertical;
        [Range(-1f, 1f)]
        public float horizontal;

        [Range(0f, 1f)]
        public float DistanceToGround;

        public bool playAnim;
        public string[] oh_attacks;
        public string[] th_attacks;


        public bool twoHanded;
        public bool enableRM;
        public bool useItem;
        public bool interacting;
        public bool lockon;

        Animator anim;

        public LayerMask layerMask;

        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            

            enableRM = !anim.GetBool("canMove");
            anim.applyRootMotion = enableRM;

            interacting = anim.GetBool("interacting");

            if (!lockon)
            {
                horizontal = 0;
                vertical = Mathf.Clamp01(vertical);
            }

            anim.SetBool("lockon", lockon);

            if (enableRM)
                return;

            if (useItem)
            {
                anim.Play("Gathering");
                useItem = false;
            }

            if (interacting)
            {
                playAnim = false;
                vertical = Mathf.Clamp(vertical, 0f, 0.5f);
            }

            anim.SetBool("two_handed", twoHanded);

            if (playAnim)
            {
                string targetAnim;

                if (!twoHanded)
                {
                    int r = Random.Range(0, oh_attacks.Length);
                    targetAnim = oh_attacks[r];

                    if (vertical > 0.5f)
                        targetAnim = "oh_attack_3";
                }
                else
                {
                    int r = Random.Range(0, th_attacks.Length);
                    targetAnim = th_attacks[r];
                }

                if (vertical > 0.5f)
                    targetAnim = "oh_attack_3";

                vertical = 0;
                Debug.Log(targetAnim);
                anim.CrossFade(targetAnim, 0.2f);
                //anim.SetBool("canMove", false);
                //enableRM = true;
                playAnim = false;
            }
            anim.SetFloat("vertical", vertical);
            anim.SetFloat("horizontal", horizontal);

        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (anim)
            {
                anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);
                Vector3 footPositionLeft = Vector3.zero;

                // Left foot
                RaycastHit hit;
                Ray ray = new Ray(anim.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
                if (Physics.Raycast(ray, out hit, DistanceToGround + 1f, layerMask))
                {
                    if (hit.transform.tag == "Walkable")
                    {
                        footPositionLeft = hit.point;
                        footPositionLeft.y += DistanceToGround;
                        anim.SetIKPosition(AvatarIKGoal.LeftFoot, footPositionLeft);
                        anim.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(transform.forward, hit.normal));

                    }
                }


                anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
                anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);
                Vector3 footPositionRight = Vector3.zero;

                ray = new Ray(anim.GetIKPosition(AvatarIKGoal.RightFoot) + Vector3.up, Vector3.down);
                if (Physics.Raycast(ray, out hit, DistanceToGround + 1f, layerMask))
                {
                    if (hit.transform.tag == "Walkable")
                    {
                        footPositionRight = hit.point;
                        footPositionRight.y += DistanceToGround;
                        anim.SetIKPosition(AvatarIKGoal.RightFoot, footPositionRight);
                        anim.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(transform.forward, hit.normal));

                    }
                }

            }
        }
    }

}
