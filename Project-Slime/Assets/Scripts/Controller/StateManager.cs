using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA
{
    public class StateManager : MonoBehaviour
    {
        [Header("Init")]
        public GameObject activeModel;
        public GameObject cam;

        [Header("Stats")]
        public Attributes attributes;
        public CharacterStats characterStats;
        public Vector3 curCheckpoint;
        public Transform firstChp;

        [Header("Inputs")]
        public float vertical;
        public float horizontal;
        public float moveAmount;
        public Vector3 moveDir;
        public bool rt, rb, lt, lb;
        public bool rollInput;
        public bool itemInput;

        [Header("Stats")]
        public float moveSpeed = 2f;
        public float runSpeed = 3.5f;
        public float rotateSpeed = 5;
        public float toGround = 0.3f;
        public float rollSpeed = 1;


        [Header("States")]
        public bool onGround;
        public bool run;
        public bool lockOn;
        public bool inAction;
        public bool canMove;
        public bool canRotate;
        public bool isTwoHanded;
        public bool usingItem;
        public bool isBlocking;
        public bool isLeftHand;
        public bool enabledSc;

        [Header("Other")]
        public EnemyTarget lockOnTarget;
        public Transform lockOnTransform;
        public AnimationCurve roll_curve;
        public GetDamagePlayer getDamage;
        public GameObject shield;
        public GameObject saveEnemy;

        List<Rigidbody> ragdollRigids = new List<Rigidbody>();
        List<Transform> ragdollRigidsT = new List<Transform>();
        List<Collider> ragdollColliders = new List<Collider>();


        [HideInInspector]
        public Animator anim;
        [HideInInspector]
        public Rigidbody rigid;
        [HideInInspector]
        public AnimatorHook a_hook;
        [HideInInspector]
        public ActionManager actionManager;
        [HideInInspector]
        public InventoryManager inventoryManager;


        [HideInInspector]
        public float delta;
        [SerializeField]
        public LayerMask ignoreLayers;
        public LayerMask ignoreCamLayers;

        [HideInInspector]
        public Action currentAction;

        float _actionDelay;
        float _itemUseDelay;

        public void Init()
        {
            enabledSc = true;
            SetupAnimator();
            rigid = GetComponent<Rigidbody>();
            
            rigid.angularDrag = 999;
            rigid.drag = 4;
            rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            curCheckpoint = firstChp.position + Vector3.forward * 2;

            inventoryManager = GetComponent<InventoryManager>();
            inventoryManager.Init(this);

            actionManager = GetComponent<ActionManager>();
            actionManager.Init(this);

            a_hook = activeModel.GetComponent<AnimatorHook>();
            if (a_hook == null || a_hook.states == null)
            {
                a_hook = activeModel.AddComponent<AnimatorHook>();
                a_hook.Init(this, null);
            }
            a_hook.Init(this, null);


            //gameObject.layer = 6;
            //ignoreLayers = ~(1 << 6);


            getDamage = GetComponent<GetDamagePlayer>();
            getDamage.Init(this);

            InitRagdoll();

            anim.SetBool(StaticStrings.onGround, true);
        }

        void InitRagdoll()
        {
            Rigidbody[] rigs = GetComponentsInChildren<Rigidbody>();

            for (int i = 0; i < rigs.Length; i++)
            {
                if (rigs[i] == rigid || rigs[i].gameObject.tag == "Shield")
                    continue;

                ragdollRigids.Add(rigs[i]);
                ragdollRigidsT.Add(rigs[i].transform);
                rigs[i].isKinematic = true;

                Collider col = rigs[i].gameObject.GetComponent<Collider>();
                rigs[i].gameObject.GetComponent<Collider>().isTrigger = true;
                ragdollColliders.Add(col);
            }
        }

        public void EnableRagdoll()
        {

            for (int i = 0; i < ragdollRigids.Count; i++)
            {
                ragdollRigids[i].isKinematic = false;
                ragdollColliders[i].isTrigger = false;
            }

            Collider controllerCollider = rigid.GetComponent<Collider>();


            controllerCollider.enabled = false;
            rigid.isKinematic = true;

            StartCoroutine("CloseAnimator");
        }

        public void DisableRagdoll()
        {

            for (int i = 0; i < ragdollRigids.Count; i++)
            {
                ragdollRigids[i].gameObject.transform.position = ragdollRigidsT[i].position;
                ragdollRigids[i].gameObject.transform.rotation = ragdollRigidsT[i].rotation;
                ragdollRigids[i].isKinematic = true;
                ragdollColliders[i].isTrigger = true;
            }

            Collider controllerCollider = rigid.GetComponent<Collider>();


            controllerCollider.enabled = true;
            rigid.isKinematic = false;
        }

        IEnumerator CloseAnimator()
        {
            yield return new WaitForEndOfFrame();
            anim.enabled = false;
            enabledSc = false;
        }

        public void CheckHealth()
        {
            if (characterStats.hp <= 0)
            {
                EnableRagdoll();

                StartCoroutine("Respawn");
            }
        }

        public void ChangeCheckpoint(Transform newChP)
        {
            curCheckpoint = newChP.position + Vector3.forward * 2;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (!enabledSc)
                return;
            if (other.gameObject.tag == "Campfire")
            {
                if (other.gameObject.transform.GetChild(0).gameObject.activeInHierarchy)
                {
                    other.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                    other.gameObject.transform.GetChild(1).gameObject.SetActive(true);
                    ChangeCheckpoint(other.gameObject.transform);
                    GameObject.Find("enemy_save").GetComponent<WriteToTheFile>().SaveTheProgress();
                }
                
            }
        }

        IEnumerator Respawn()
        {
            yield return new WaitForSeconds(4);
            DisableRagdoll();
            GameObject.Find("Gameplay").GetComponent<UI_gameplay>().bar = 0;
            GameObject.Find("Gameplay").GetComponent<UI_gameplay>().bar_rect.gameObject.SetActive(false);
            characterStats.xp = 0;
            GameObject.Find("Gameplay").GetComponent<UI_gameplay>().health_back.sizeDelta = new Vector2(characterStats.max_hp, 35);
            GameObject.Find("Gameplay").GetComponent<UI_gameplay>().damdage_moment.sizeDelta = new Vector2(characterStats.max_hp, 35);
            anim.enabled = true;
            enabledSc = true;
            transform.position = curCheckpoint;
            characterStats.hp = characterStats.max_hp;
            saveEnemy.GetComponent<ReadFromFile>().Read_f();
            //GameObject.Find("enemy_save").GetComponent<WriteToTheFile>().SaveTheProgress();
        }

        void SetupAnimator()
        {
            if (activeModel == null)
            {
                anim = GetComponentInChildren<Animator>();
                if (anim == null)
                    Debug.Log("No model found");
                else
                    activeModel = anim.gameObject;
            }

            if (anim == null)
                anim = activeModel.GetComponent<Animator>();

            anim.applyRootMotion = false;
        }

        public void FixedTick(float d)
        {
        
            if (!enabledSc)
                return;
            delta = d;

            isBlocking = false;
            usingItem = anim.GetBool(StaticStrings.interacting);
            DetectAction();
            DetectItemAction();
            inventoryManager.rightHandWeapon.weaponModel.SetActive(!usingItem);

            anim.SetBool(StaticStrings.blocking, isBlocking);
            anim.SetBool(StaticStrings.isLeft, isLeftHand);


            if (isBlocking && shield.activeInHierarchy == false)
            {
                EnableShield();
            }
            else if (!isBlocking && shield.activeInHierarchy == true)
                DisabelShield();

            if (inAction)
            {
                anim.applyRootMotion = true;

                _actionDelay += delta;
                if (_actionDelay > 0.3f)
                {

                    inAction = false;
                    _actionDelay = 0;
                }
                else
                    return;
            }

                
               

            canMove = anim.GetBool(StaticStrings.canMove);

            if (!canMove)
            {
                return;
            }

            // a_hook.rm_multi = 1;
            a_hook.CloseRoll();
            HandleRolls();

            anim.applyRootMotion = false;
            

            rigid.drag = (moveAmount > 0 || onGround == false) ? 1 : 4;

            float targetSpeed = moveSpeed;
            if (usingItem)
            {
                run = false;
                moveAmount = Mathf.Clamp(moveAmount, 0, 0.5f);
            }
                
            if (run)
                targetSpeed = runSpeed;
            

            if (onGround)
                rigid.velocity = moveDir * (targetSpeed * moveAmount);

            if (run)
                lockOn = false;


            Vector3 targetDir = (lockOn == false) ? moveDir
                : 
                (lockOnTransform != null) ?  lockOnTransform.transform.position - transform.position
                    : 
                    moveDir;

            targetDir.y = 0;
            if (targetDir == Vector3.zero)
                targetDir = transform.forward;
            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, delta * moveAmount * rotateSpeed);
            transform.rotation = targetRotation;

            anim.SetBool(StaticStrings.lockon, lockOn);

            if (!lockOn)
                HandleMovementAnimations();
            else
                HandleLockOnAnimations(moveDir);


        }

        public void EnableShield()
        {
            shield.SetActive(true);
        }

        public void DisabelShield()
        {
            shield.SetActive(false);
        }

        public void DetectItemAction()
        {
            if (canMove == false || usingItem || isBlocking)
            {
                return;
            }
                

            if (itemInput == false)
                return;

            ItemAction slot = actionManager.consumableItem;
            string targetAnim = slot.targetAnim;
            if (string.IsNullOrEmpty(targetAnim))
                return;

            //inventoryManager.curWeapon.weaponModel.SetActive(false);
            usingItem = true;
            anim.Play(targetAnim);
        }

        public void DetectAction()
        {
            if (canMove == false || usingItem || inAction)
                return;

            if (rb == false && rt == false && lt == false && lb == false)
                return;

            

            Action slot = actionManager.GetActionSlot(this);
            if (slot == null)
                return;

            switch (slot.type)
            {
                case ActionType.attack:

                    AttackAction(slot);
                    break;
                case ActionType.block:
                    BlockAction(slot);
                    break;
                case ActionType.spells:
                    break;
                case ActionType.parry:
                    ParryAction(slot);
                    break;
                default:
                    break;
            }
            //AttackAction(slot);
        }

        void AttackAction(Action slot)
        {
            string targetAnim = null;
            targetAnim = slot.targetAnim;

            if (string.IsNullOrEmpty(targetAnim))
                return;

            currentAction = slot;

            inAction = true;
            canMove = false;
            anim.SetBool(StaticStrings.mirror, slot.mirror);
            anim.CrossFade(targetAnim, 0.2f);
            //rigid.velocity = Vector3.zero;
        }

        void BlockAction(Action slot)
        {
            isBlocking = true;
            isLeftHand = slot.mirror;
        }

        void ParryAction(Action slot)
        {
            string targetAnim = null;
            targetAnim = slot.targetAnim;



            if (string.IsNullOrEmpty(targetAnim))
                return;

            inAction = true;
            canMove = false;
            anim.SetBool(StaticStrings.mirror, slot.mirror);
            anim.CrossFade(targetAnim, 0.2f);
            //rigid.velocity = Vector3.zero;
        }

        public void Tick(float d)
        {
            delta = d;
            onGround = OnGround();
            anim.SetBool(StaticStrings.onGround, onGround);
        }


        void HandleRolls()
        {
            if (!rollInput || usingItem)
                return;

            a_hook.rolling = true;
            float v = vertical;
            float h = horizontal;
            v = (moveAmount > 0.3f) ? 1 : 0;
            h = 0;

            /*if (!lockOn)
            {
                v = (moveAmount > 0.3f) ? 1 : 0;
                h = 0;
            }
            else
            {
                if (Mathf.Abs(v) < 0.3f)
                    v = 0;
                if (Mathf.Abs(h) < 0.3f)
                    h = 0;
            }*/

            if (v != 0)
            {
                Debug.Log("HandleRolling");
                if (moveDir == Vector3.zero)
                    moveDir = transform.forward;
                Quaternion targetRot = Quaternion.LookRotation(moveDir);
                transform.rotation = targetRot;

                a_hook.InitForRoll();
                a_hook.rm_multi = rollSpeed;
            }
            else
            {
                a_hook.InitForRoll();
                a_hook.rm_multi = 1.3f;
            }

            anim.SetFloat(StaticStrings.vertical, v);
            anim.SetFloat(StaticStrings.horizontal, h);

            if (v == 0)
                rigid.AddForce(-gameObject.transform.forward * 5, ForceMode.Impulse);
            else
            {
                rigid.AddForce(gameObject.transform.forward * 7, ForceMode.Impulse);
            }

            inAction = true;
            canMove = false;
            isBlocking = false;
            anim.applyRootMotion = true;
            Debug.Log("HHHHHH");
            anim.CrossFade(StaticStrings.Rolls, 0.1f);
        }

        void HandleMovementAnimations()
        {
            anim.SetBool(StaticStrings.run, run);
            anim.SetFloat(StaticStrings.vertical, moveAmount, 0.4f, delta);
        }

        void HandleLockOnAnimations(Vector3 moveDir)
        {
            Vector3 realtiveDir = transform.InverseTransformDirection(moveDir);
            float h = realtiveDir.x;
            float v = realtiveDir.z;

            anim.SetFloat(StaticStrings.vertical, v, 0.2f, delta);
            anim.SetFloat(StaticStrings.horizontal, h, 0.2f, delta);
        }

        public void Jump()
        {
            a_hook.jumping = true;
            inAction = true;
            canMove = false;
            isBlocking = false;
            anim.Play(StaticStrings.Jump_start);
            skipGroundCheck = true;

            Vector3 targetVel = transform.forward * runSpeed;
            targetVel.y = 5;
            rigid.velocity = targetVel;
        }

        bool skipGroundCheck;
        float skipTimer;
        bool prevGround;

        public bool OnGround()
        {
            if (skipGroundCheck)
            {
                skipTimer += delta;
                if (skipTimer > .2f)
                    skipGroundCheck = false;
                prevGround = false;
                return false;
            }
            canMove = true;
            skipTimer = 0;

            bool r = false;

            Vector3 origin = transform.position + (Vector3.up * toGround);
            Vector3 dir = -Vector3.up;
            float dis = toGround + 0.3f;

            RaycastHit hit;
            Debug.DrawRay(origin, dir * dis);
            if (Physics.Raycast(origin, dir, out hit, dis, ignoreLayers))
            {
                r = true;
                Vector3 targetPosition = hit.point;
                transform.position = targetPosition;
            }

            if (r && !prevGround)
            {
                Land();
            }

            prevGround = r;
            return r;
        }

        void Land()
        {
            Vector3 dir = moveDir;
            dir.y = 0;
            if (dir.magnitude == 0)
            {
                anim.Play(StaticStrings.Jump_land);
            }
            else
            {
                rollInput = true;
                if (moveDir == Vector3.zero)
                    moveDir = transform.forward;
                Quaternion targetRot = Quaternion.LookRotation(moveDir);
                transform.rotation = targetRot;

                HandleRolls();

                a_hook.InitForRoll();
                a_hook.rm_multi = rollSpeed;

                anim.SetFloat(StaticStrings.vertical, 1);
                anim.SetFloat(StaticStrings.horizontal, 0);

                anim.applyRootMotion = true;
                anim.CrossFade(StaticStrings.Rolls, 0.1f);
            }

            inAction = true;
            canMove = true;
            isBlocking = false;
            skipGroundCheck = false;
            a_hook.jumping = false;
        }

        public void HandleTwoHamded()
        {
            anim.SetBool("twoHanded", isTwoHanded);

            if (isTwoHanded)
                actionManager.UpdateActionsTwoHanded();
            else
                actionManager.UpdateActionsOneHanded();
        }

        
    }
}

