using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Transform person = animator.transform;
        Transform head = person.GetChild(0);
        Transform body = person.GetChild(1);

        foreach (PunchScript punch in person.GetComponentsInChildren<PunchScript>())
        {
            Destroy(punch.gameObject);
        }

        head.gameObject.layer = LayerMask.NameToLayer("Default");
        head.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        head.GetComponent<Rigidbody>().useGravity = true;
        head.SetParent(null);

        body.gameObject.layer = LayerMask.NameToLayer("Default");
        body.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        body.GetComponent<Rigidbody>().useGravity = true;
        body.SetParent(null);

        Destroy(person.gameObject);
    }

}
