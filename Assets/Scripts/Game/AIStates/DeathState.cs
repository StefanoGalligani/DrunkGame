using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : StateMachineBehaviour
{
    float timeToDestroy = .5f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Dead", true);
        Transform person = animator.transform;
        Transform head = person.GetChild(0);
        Transform body = person.GetChild(1);

        foreach (PunchScript punch in person.GetComponentsInChildren<PunchScript>())
        {
            Destroy(punch.gameObject);
        }

        head.gameObject.layer = LayerMask.NameToLayer("DeadPeople");
        head.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        head.GetComponent<Rigidbody>().useGravity = true;
        head.SetParent(null);
        head.GetComponent<Rigidbody>().AddForce(Vector3.up * 800 - animator.transform.forward * 1200);

        body.gameObject.layer = LayerMask.NameToLayer("DeadPeople");
        body.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        body.GetComponent<Rigidbody>().useGravity = true;
        body.SetParent(null);
        body.GetComponent<Rigidbody>().AddForce(-animator.transform.forward * 200 + animator.transform.right * 80);

        if (person.GetComponent<PersonOther>().musician) {
            Beer b = person.GetComponentInChildren<Beer>();
            b.gameObject.layer = LayerMask.NameToLayer("Beer");
            b.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            b.GetComponent<Rigidbody>().useGravity = true;
            b.transform.SetParent(null);
            person.GetComponent<Musician>().musicSource.Stop();
        }

        animator.GetComponent<AudioSource>().PlayOneShot(animator.GetComponent<PersonOther>().soundDeath[Random.Range(0,3)]);
        animator.GetComponent<PersonOther>().changeFace(false);
        Destroy(animator.GetComponent<PersonOther>());
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timeToDestroy -= Time.deltaTime;
        if (timeToDestroy <= 0) {
            Destroy(animator.gameObject);
        }
    }

}
