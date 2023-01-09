using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle_behaviour : StateMachineBehaviour
{
    float timer;
    Transform player;
    float Chase_range = 10;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        Transform pointsObgect = GameObject.FindGameObjectWithTag("patrol").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        if (timer > 5) animator.SetBool("Is_patroling", true);

        float distance = Vector3.Distance(animator.transform.position, player.position);
        if (distance < Chase_range) animator.SetBool("Is_chasing", true);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }

}
