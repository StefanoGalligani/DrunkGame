using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : StateMachineBehaviour
{
    PersonOther person;
    float targetRefreshTimer = .5f;
    Transform target;
    
    AttacksScriptableObject attackInfo;
    float attackWaitTimer;
    float attackChargeTimer;
    int attackIndex = 0;
    bool charging = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<NavMeshAgent>().isStopped = true;
        person = animator.GetComponent<PersonOther>();

        attackInfo = person.attackInfo;
        attackWaitTimer = person.attackWaitTimer;
        attackChargeTimer = person.attackChargeTimer;
        attackIndex = person.attackIndex;
        charging = person.charging;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (targetRefreshTimer <= 0) {
            GameObject t = person.getNextTarget(); 
            target = t ? t.transform : null;
            targetRefreshTimer = 1;
        }
        if (target != null) {
            animator.transform.LookAt(target.position);
            float distance = Vector3.Distance(person.transform.position, target.position);
            if (distance < 1.2f) {
                person.transform.position += -person.transform.forward * Time.deltaTime;
                person.GetComponent<Rigidbody>().AddForce(-person.transform.forward, ForceMode.Force);
            }
            animator.SetFloat("DistanceFromTarget", distance);

            attackLogic();
        }
        targetRefreshTimer -= Time.deltaTime;
    }

    private void attackLogic() {
        if (attackIndex == -1) {
            attackIndex = 0;
            attackWaitTimer = attackInfo.actions[0].waitTime;
        }

        attackWaitTimer -= Time.deltaTime;
        if (attackWaitTimer <= 0) {
            charging = true;
            attackChargeTimer = attackInfo.actions[attackIndex].chargeTime;
            attackIndex = (attackIndex+1) % attackInfo.actions.Length;
            attackWaitTimer = attackInfo.actions[attackIndex].waitTime;
        }
        
        if (charging) {
            attackChargeTimer -= Time.deltaTime;
            int index = (attackIndex-1+attackInfo.actions.Length) % attackInfo.actions.Length;
            if (attackChargeTimer <= 0) {
                charging = false;
                if (attackInfo.actions[index].punchIndex < 2) {
                    person.GetComponentsInChildren<PunchScript>()[attackInfo.actions[index].punchIndex]
                        .startPunching(attackInfo.actions[index].chargeTime / 3);
                    person.GetComponentsInChildren<PunchScript>()[attackInfo.actions[index].punchIndex]
                        .changePunchColor(0);
                } else {
                    person.GetComponentsInChildren<PunchScript>()[0].startPunching(attackInfo.actions[index].chargeTime / 3);
                    person.GetComponentsInChildren<PunchScript>()[1].startPunching(attackInfo.actions[index].chargeTime / 3);
                    person.GetComponentsInChildren<PunchScript>()[0].changePunchColor(0);
                    person.GetComponentsInChildren<PunchScript>()[1].changePunchColor(0);
                }
            } else {
                if (attackInfo.actions[index].punchIndex < 2) {
                    person.GetComponentsInChildren<PunchScript>()[attackInfo.actions[index].punchIndex]
                        .changePunchColor((attackInfo.actions[index].chargeTime - attackChargeTimer) / 3);
                } else {
                    person.GetComponentsInChildren<PunchScript>()[0]
                        .changePunchColor((attackInfo.actions[index].chargeTime - attackChargeTimer) / 3);
                    person.GetComponentsInChildren<PunchScript>()[1]
                        .changePunchColor((attackInfo.actions[index].chargeTime - attackChargeTimer) / 3);
                }
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        person.attackWaitTimer = attackWaitTimer;
        person.attackChargeTimer = attackChargeTimer;
        person.attackIndex = attackIndex;
        person.charging = charging;
    }
}
