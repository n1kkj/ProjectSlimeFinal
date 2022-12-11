using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


namespace SA
{
    public class EnemyStates : MonoBehaviour
    {
        [Header("Stats")]


        [Header("Values")]
        public float delta;
        public float horizontal;
        public float vertical;

        public CharacterStats characterStats;

        [Header("States")]
        public bool isInvinvcible;
        public bool canMove;
        public bool isDead;
        public bool hasDestination;
        public Vector3 targetDestination;
        public Vector3 dirToTarget;
        public bool rotateToTarget;


        //references
        public Animator anim;
        EnemyTarget enTarget;
        AnimatorHook a_hook;
        public Rigidbody rigid;
        public NavMeshAgent agent;
        public GetDamageEnemy getDamage;

        public LayerMask ignoreLayers;

        List<Rigidbody> ragdollRigids = new List<Rigidbody>();
        List<Collider> ragdollColliders = new List<Collider>();

       


        public void Init()
        {
            anim = GetComponentInChildren<Animator>();
            enTarget = GetComponent<EnemyTarget>();
            enTarget.Init(this);

            rigid = GetComponentInChildren<Rigidbody>();
            agent = GetComponent<NavMeshAgent>();
            rigid.isKinematic = true;

            a_hook = anim.GetComponent<AnimatorHook>();
            //Debug.Log(a_hook);
            if (a_hook == null)
            {
                Debug.Log("ENEMY INIT");
                a_hook = anim.gameObject.AddComponent<AnimatorHook>();
                a_hook.Init(null, this);
            }
            a_hook.Init(null, this);

            getDamage = GetComponent<GetDamageEnemy>();
            getDamage.Init(this);
            InitRagdoll();
            ignoreLayers = ~(1 << 9);
        }

        void InitRagdoll()
        {
            Rigidbody[] rigs = GetComponentsInChildren<Rigidbody>();

            for (int i = 0; i < rigs.Length; i++)
            {
                if (rigs[i] == rigid)
                    continue;

                ragdollRigids.Add(rigs[i]);
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

            /*anim.enabled = false;
            this.enabled = false;*/
            StartCoroutine("CloseAnimator");
        }

        IEnumerator CloseAnimator()
        {
            yield return new WaitForEndOfFrame();

            anim.enabled = false;
            this.enabled = false;
        }

        public void CheckHealth(AudioSource audi, GameObject weapon = null)
        {
            if (characterStats.hp <= 0)
            {
                audi.Play();
                Debug.Log(weapon);
                //weapon.SetActive(false);
                gameObject.GetComponent<AIHandler>().enabled = false;
                EnableRagdoll();
                GameObject.Find("Controller").GetComponent<StateManager>().characterStats.xp += 10;
                StartCoroutine(DestroyEnemy());
            }
        }

        IEnumerator DestroyEnemy()
        {
            Debug.Log("Started");
            yield return new WaitForSeconds(4f);
            Debug.Log("Destroyed");
            Destroy(gameObject);
        }

        public void Tick(float d)
        {
            delta = d;
            canMove = anim.GetBool(StaticStrings.canMove);

            if (rotateToTarget)
            {
                LookTowardsTarget();
            }

            if (characterStats.hp <= 0)
            {
                if (!isDead)
                {
                    isDead = true;
                    EnableRagdoll();
                }
            }

            if (isInvinvcible)
            {
                isInvinvcible = !canMove;
            }

            if (canMove)
            {
                anim.applyRootMotion = false;
                rigid.isKinematic = true;

                MovementAnimation();
            }
            else
            {
                if (anim.applyRootMotion == false)
                    anim.applyRootMotion = true;
            }
                
        }

        public void MovementAnimation()
        {
            float square = agent.desiredVelocity.sqrMagnitude;
            float v = Mathf.Clamp(square, 0, .5f);
            anim.SetFloat(StaticStrings.vertical, v, 0.5f, delta);

            /*Vector3 desired = agent.desiredVelocity;
            Vector3 relative = transform.InverseTransformDirection(desired);

            float v = relative.z;
            float h = relative.x;

            v = Mathf.Clamp(v, -.5f, .5f);
            h = Mathf.Clamp(h, -.5f, .5f);

            anim.SetFloat(StaticStrings.horizontal, h, 0.2f, delta);
            anim.SetFloat(StaticStrings.vertical, v, 0.2f, delta);*/
        }

        void LookTowardsTarget()
        {
            Vector3 dir = dirToTarget;
            dir.y = 0;

            if (dir == Vector3.zero)
                dir = transform.forward;

            Quaternion targerRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targerRotation, delta * 5);
        }

        public void SetDestination(Vector3 d)
        {
            if (!hasDestination)
            {
                hasDestination = true;
                agent.isStopped = false;
                agent.SetDestination(d);
                targetDestination = d;
            }

             
        }

        public void DoDamage(Action a)
        {
            if (isInvinvcible)
                return;

            int damage = StatsCalculations.CalculateBaseDamage(a.weaponStats, characterStats);

            characterStats.poise += damage;
            characterStats.hp -= damage;

            if (canMove || characterStats.poise > 100) // May be harmful
            {
                if (a.overrideDamageAnim)
                    anim.Play(a.damageAnim);
                else
                {
                    int ran = Random.Range(0, 100);
                    string tA = (ran > 50) ? StaticStrings.damage1 : StaticStrings.damage2;
                    anim.Play(tA);
                }
            }

            Debug.Log("Damage is " + damage + " Poise is " + characterStats.poise);
            

            isInvinvcible = true;
            anim.applyRootMotion = true;
            anim.SetBool(StaticStrings.canMove, false);
        }
    }
}

