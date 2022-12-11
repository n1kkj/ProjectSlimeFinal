using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA
{
    public class FootIK : MonoBehaviour
    {
        Animator animator;

        Vector3 rightFootPosition;
        Vector3 leftFootPosition;
        Vector3 rightFootRotation;
        Vector3 leftFootRotation;
        public float offset_y = 1;
        public float base_offset = 0.01f;
        public float feetOffset;
        LayerMask ignoreLayers;

        private void Start()
        {
            animator = GetComponent<Animator>();
            ignoreLayers = ~(1 << 2 | 1 << 6 | 1 << 9);
        }

        float rf_weight;
        float lf_weight;

        private void Update()
        {
            rf_weight = animator.GetFloat("IKLeftFootWeight");
            lf_weight = animator.GetFloat("IKRightFootWeight");

            if (!DetectFootPosition(animator.GetBoneTransform(HumanBodyBones.RightFoot), ref rightFootPosition, ref rightFootRotation))
            {
                rf_weight = 0;
            }

            if (!DetectFootPosition(animator.GetBoneTransform(HumanBodyBones.LeftFoot), ref leftFootPosition, ref leftFootRotation))
            {
                lf_weight = 0;
            }
        }

        private void OnAnimatorIK(int layerIndex)
        {

            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, rf_weight);
            animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootPosition);
            animator.SetIKRotation(AvatarIKGoal.RightFoot, Quaternion.LookRotation(transform.forward, rightFootRotation));

            

            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, lf_weight);
            animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootPosition);
            animator.SetIKRotation(AvatarIKGoal.LeftFoot, Quaternion.LookRotation(transform.forward, leftFootRotation));
        }

        bool DetectFootPosition(Transform originTrans, ref Vector3 position, ref Vector3 rotation)
        {
            Vector3 origin = originTrans.position;
            origin.y = transform.position.y + offset_y;
            Vector3 destination = originTrans.position;
            destination.y = transform.position.y + base_offset;
                ;
            RaycastHit hit;

            Debug.DrawLine(origin, destination);
            if (Physics.Linecast(origin, destination, out hit, ignoreLayers))
            {
                Debug.DrawLine(hit.point, hit.transform.right, Color.red);
                Debug.DrawLine(hit.point, hit.transform.forward, Color.green);
                Debug.DrawLine(hit.point, hit.normal, Color.blue);
                Debug.DrawLine(hit.point, hit.transform.up, Color.magenta);
                position = hit.point;
                position.y += feetOffset;
                rotation = hit.point;
                return true;
            }

            return false;
        }
    }
}

