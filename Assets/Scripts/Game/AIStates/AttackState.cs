using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : StateMachineBehaviour
{
    PersonOther person;
    float timer = .5f;
    Transform target;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<NavMeshAgent>().isStopped = true;
        person = animator.GetComponent<PersonOther>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timer <= 0) {
            target = person.getNextTarget().transform;
            timer = 1;
            if (target != null) {
                person.GetComponentInChildren<PunchScript>().startPunching();
            }
        }
        if (target != null) {
            animator.transform.LookAt(target.position);
            float distance = Vector3.Distance(person.transform.position, target.position);
            if (distance < 1.4f) {
                person.transform.position += -person.transform.forward * Time.deltaTime;
                person.GetComponent<Rigidbody>().AddForce(-person.transform.forward, ForceMode.Force);
            }
            animator.SetFloat("DistanceFromTarget", distance);
        }
        timer -= Time.deltaTime;
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
