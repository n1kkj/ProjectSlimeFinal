using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol_behaviour : StateMachineBehaviour
{
    float timer;
    List<Transform> points = new List<Transform>();
    NavMeshAgent agent;
    int next_point = 1;
    Transform player;
    float Chase_range = 10; 
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        Transform pointsObgect = GameObject.FindGameObjectWithTag("patrol").transform;
        foreach (Transform t in pointsObgect) points.Add(t);

        agent = animator.GetComponent<NavMeshAgent>();
        agent.SetDestination(points[0].position);

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (next_point > points.Count) next_point = 0;
                agent.SetDestination(points[next_point].position);
                next_point += 1;
            }

            timer += Time.deltaTime;
            if (timer > 10) animator.SetBool("Is_patroling", false);

            float distance = Vector3.Distance(animator.transform.position, player.position);
        if (distance < Chase_range) animator.SetBool("Is_chasing", true);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
    }

}
