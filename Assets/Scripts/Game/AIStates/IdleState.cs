using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : StateMachineBehaviour
{
    NavMeshAgent agent;
    PersonOther person;
    bool atPosition = true;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        person = animator.GetComponent<PersonOther>();
        person.attackWaitTimer = 0;
        person.attackChargeTimer = 0;
        person.attackIndex = -1;
        person.charging = false;
        person.GetComponentsInChildren<PunchScript>()[0].changePunchColor(0);
        person.GetComponentsInChildren<PunchScript>()[1].changePunchColor(0);

        atPosition = false;
        agent = animator.GetComponent<NavMeshAgent>();
        agent.isStopped = false;
        agent.SetDestination(person.idlePosition);
        
        foreach (PunchScript punch in person.GetComponentsInChildren<PunchScript>())
        {
            punch.SetAlerted(false);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!atPosition) {
            if ((person.transform.position - person.idlePosition).sqrMagnitude < 1) {
                atPosition = true;
                agent.isStopped = true;
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
