using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chase_behaviour : StateMachineBehaviour
{
    float timer;
    NavMeshAgent agent;
    Transform player;
    float Attack_range = 2;
    float Chase_range = 10;
    GameObject weapon;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = 4;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        weapon = GameObject.Find("DamageCollider");
        weapon.GetComponent<BoxCollider>().enabled = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(player.position);
        float distance = Vector3.Distance(animator.transform.position, player.position);
        if (distance < Attack_range)
        {
            animator.SetBool("Is_attacking1", true);
        }

        if (distance > Chase_range) animator.SetBool("Is_chasing", false);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
        agent.speed = 2;
    }

}
