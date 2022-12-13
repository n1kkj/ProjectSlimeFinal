using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA
{
    public class CameraManager : MonoBehaviour
    {
        public bool enabledTurning = false;
        public bool en_turn = false;

        public bool lockon;
        public float followSpeed = 3;
        public float mouseSpeed = 2;
        public float controllerSpeed = 7f;

        public Transform target = null;
        public EnemyTarget lockonTarget = null;
        public Transform lockonTransform = null;

        
        public Transform pivot;
        
        public Transform camTrans;
        StateManager states;

        public float turnSmoothing = .2f;
        public float minAngle = -35f;
        public float maxAngle = 35f;

        public float defZ;
        float curZ;
        public float zSpeed = 5;

        float smoothX;
        float smoothY;
        float smoothXvelocity;
        float smoothYvelocity;
        public float lookAngle;
        public float tiltAngle;

        bool usedRightAxis;

        bool changeTargetLeft;
        bool changeTargetRight;

        public void Init(StateManager st)
        {

            lookAngle = 0;
            tiltAngle = 0;

            target = st.transform;
            states = st;

            camTrans = Camera.main.transform;
            pivot = camTrans.parent;

            curZ = defZ;

            StartCoroutine(stup());
        }

        IEnumerator stup()
        {
            yield return new WaitForSeconds(.1f);

            en_turn = true;
        }

        public void SetLockonTarget(GameObject Boss)
        {
            lockonTarget = Boss.GetComponent<EnemyTarget>();

        }

        public void Tick(float d)
        {
            if (enabledTurning && en_turn && states.en_Checking)
            {
                float h = Input.GetAxis("Mouse X");
                float v = Input.GetAxis("Mouse Y");

                float c_h = Input.GetAxis("RightAxis X");
                float c_v = Input.GetAxis("RightAxis Y");

                float targetSpeed = mouseSpeed;

                changeTargetLeft = Input.GetKeyUp(KeyCode.V);
                changeTargetRight = Input.GetKeyUp(KeyCode.B);

                if (lockonTarget != null)
                {
                    if (lockonTransform == null)
                    {
                        lockonTransform = lockonTarget.GetTarget();
                        states.lockOnTransform = lockonTransform;
                    }


                    /*if (Mathf.Abs(c_h) > 0f)
                    {
                        if (!usedRightAxis)
                        {
                            lockonTransform = lockonTarget.GetTarget(c_h > 0);
                            states.lockOnTransform = lockonTransform;
                            usedRightAxis = true;
                        }
                    }*/

                    if (changeTargetLeft || changeTargetRight)
                    {
                        lockonTransform = lockonTarget.GetTarget(changeTargetLeft);
                        states.lockOnTransform = lockonTransform;
                    }
                }

                if (usedRightAxis)
                {
                    if (Mathf.Abs(c_h) == 0f)
                    {
                        usedRightAxis = false;
                    }
                }

                if (c_h != 0 || c_v != 0)
                {
                    h = c_h;
                    v = -c_v;
                    targetSpeed = controllerSpeed;
                }

                FollowTarget(d);
                StartCoroutine(HandleRotationsTimer(d, v, h, targetSpeed));
                HandlePivotPosition();
            }
            
        }

        void FollowTarget(float d)
        {
            float speed = d * followSpeed;
            Vector3 targetPositon = Vector3.Lerp(transform.position, target.position, speed);
            transform.position = targetPositon;
        }    

        IEnumerator HandleRotationsTimer(float d, float v, float h, float targetSpeed)
        {
            yield return new WaitForEndOfFrame();
            HandleRotations(d, v, h, targetSpeed);

        }

        void HandleRotations(float d, float v, float h, float targetSpeed)
        {
            if (turnSmoothing > 0)
            {
                smoothX = Mathf.SmoothDamp(smoothX, h, ref smoothXvelocity, turnSmoothing);
                smoothY = Mathf.SmoothDamp(smoothY, v, ref smoothYvelocity, turnSmoothing);
            }
            else
            {
                smoothX = h;
                smoothY = v;
            }

            tiltAngle -= smoothY * targetSpeed;
            tiltAngle = Mathf.Clamp(tiltAngle, minAngle, maxAngle);

            if (tiltAngle != float.NaN && tiltAngle != 0)
                pivot.localRotation = Quaternion.Euler(tiltAngle, 0, 0);
            else
                pivot.localRotation = Quaternion.identity;
            


            if (lockon && lockonTarget != null)
            {
                Vector3 targetDir = lockonTransform.position - transform.position;
                targetDir.Normalize();
                //targetDir.y = 0;



                if (targetDir == Vector3.zero)
                    targetDir = transform.forward;
                Quaternion targetRot = Quaternion.LookRotation(targetDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, d * 9);
                lookAngle = transform.eulerAngles.y;
                return;
            }


            if (lookAngle == float.NaN && lookAngle != 0)
            { 
                lookAngle += smoothX * targetSpeed;
                Debug.Log(lookAngle);
                transform.rotation = Quaternion.Euler(0, lookAngle, 0);
            }
            else
            {
                lookAngle += smoothX * targetSpeed;
                if (lookAngle != float.NaN && lookAngle != 0 && Quaternion.Euler(0, lookAngle, 0) != null)
                {
                    transform.rotation = Quaternion.Euler(0, lookAngle, 0);
                }
                else lookAngle = smoothX * targetSpeed;
            }


            

            
        }

        void HandlePivotPosition()
        {
            float targerZ = defZ;

            CameraCollision(defZ, ref targerZ);


            curZ = Mathf.Lerp(curZ, targerZ, states.delta * zSpeed);
            Vector3 tp = Vector3.zero;
            tp.z = curZ;
            camTrans.localPosition = tp;
        }

        void CameraCollision(float targetZ, ref float actualZ)
        {
            float step = Mathf.Abs(actualZ);
            int stepCount = 2;
            float stepIncrement = step / stepCount;

            RaycastHit hit;
            Vector3 origin = pivot.position;
            Vector3 direction = -pivot.forward;

            Debug.DrawRay(origin, direction * step, Color.blue);
            if (Physics.Raycast(origin, direction, out hit, step))
            {
                //Debug.Log(hit.collider.gameObject.name);
                float distance = Vector3.Distance(hit.point, origin);
                actualZ = -(distance / 2);
            }
            else
            {
                for (int s = 0; s < stepCount + 1; s++)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        Vector3 dir = Vector3.zero;
                        Vector3 secondOrigin = origin + (direction * s) * stepIncrement;

                        switch (i)
                        {
                            case 0:
                                dir = camTrans.right;
                                break;
                            case 1:
                                dir = -camTrans.right;
                                break;
                            case 2:
                                dir = camTrans.up;
                                break;
                            case 3:
                                dir = -camTrans.up;
                                break;
                        }    

                        Debug.DrawRay(secondOrigin, dir * 0.2f, Color.red);
                        if (Physics.Raycast(secondOrigin, dir, out hit, 0.2f))
                        {
                            float distance = Vector3.Distance(secondOrigin, origin);
                            actualZ = -(distance / 2);
                            if (actualZ < 0.2f)
                                actualZ = 0;

                            return;
                        }
                    }
                }

            }
        }


        public static CameraManager singleton;

        private void Awake()
        {
            lookAngle = 0;
            tiltAngle = 0;


            singleton = this;
        }

        IEnumerator HHH()
        {
            yield return new WaitForSeconds(0.1f);

            enabledTurning = true;
        }
    }
}

