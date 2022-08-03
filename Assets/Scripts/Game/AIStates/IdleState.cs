using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : StateMachineBehaviour
{
    NavMeshAgent agent;
    PersonOther person;
    bool atPosition = true;

    private float m_StepCycle = 0;
    private float m_NextStep = 0;
    private float m_StepInterval = 1;

    public AudioSource audioSource;
    public AudioClip[] footstepSounds;

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
        
        audioSource = person.walkAudioSource;
        footstepSounds = person.footstepSounds;

        atPosition = false;
        agent = animator.GetComponent<NavMeshAgent>();
        agent.isStopped = false;
        agent.SetDestination(person.idlePosition);
        
        foreach (PunchScript punch in person.GetComponentsInChildren<PunchScript>())
        {
            punch.SetAlerted(false);
        }

        person.changeFace(false);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!atPosition) {
            ProgressStepCycle();
            if ((person.transform.position - person.idlePosition).sqrMagnitude < 1) {
                atPosition = true;
                agent.isStopped = true;
            }
        }
    }
    
    private void ProgressStepCycle()
    {
        if (!person.isBeingPushed())
        {
            m_StepCycle += agent.speed * Time.deltaTime;
        }

        if (!(m_StepCycle > m_NextStep))
        {
            return;
        }

        m_NextStep = m_StepCycle + m_StepInterval;

        PlayFootStepAudio();
    }

    private void PlayFootStepAudio()
    {
        // pick & play a random footstep sound from the array,
        // excluding sound at index 0
        int n = UnityEngine.Random.Range(1, footstepSounds.Length);
        audioSource.clip = footstepSounds[n];
        audioSource.Play();
        // move picked sound to index 0 so it's not picked next time
        footstepSounds[n] = footstepSounds[0];
        footstepSounds[0] = audioSource.clip;
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
