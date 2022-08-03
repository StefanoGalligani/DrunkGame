using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : StateMachineBehaviour
{
    NavMeshAgent agent;
    PersonOther person;
    float timer = -1;
    Transform target;

    private float m_StepCycle = 0;
    private float m_NextStep = 0;
    private float m_StepInterval = 1;
    
    public AudioSource audioSource;
    public AudioClip[] footstepSounds;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        person = animator.GetComponent<PersonOther>();
        agent.isStopped = false;
        
        audioSource = person.walkAudioSource;
        footstepSounds = person.footstepSounds;

        foreach (PunchScript punch in person.GetComponentsInChildren<PunchScript>())
        {
            punch.SetAlerted(true);
        }
        
        person.changeFace(true);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timer <= 0) {
            GameObject t = person.getNextTarget(); 
            target = t ? t.transform : null;
            timer = 1;
        }
        if (target != null) {
            agent.SetDestination(target.position);
            animator.transform.LookAt(target.position);
            animator.SetFloat("DistanceFromTarget", Vector3.Distance(person.transform.position, target.position));
        }
        timer -= Time.deltaTime;
        ProgressStepCycle();
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
}
