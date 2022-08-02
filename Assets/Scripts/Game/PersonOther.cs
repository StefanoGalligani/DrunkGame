using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonOther : PersonScript
{
    public AttacksScriptableObject attackInfo;
    List<GameObject> targets;
    Animator anim;

    [HideInInspector]
    public float attackWaitTimer;
    [HideInInspector]
    public float attackChargeTimer;
    [HideInInspector]
    public int attackIndex = -1;
    [HideInInspector]
    public bool charging = false;

    bool beingPushed = false;

    private void Start() {
        health = maxHealth;
        anim = GetComponent<Animator>();
        targets = new List<GameObject>();
    }

    public override void hit(float damage, bool head, GameObject attacker) {
        if (head) damage *= 2;
        health -= damage;
        anim.SetFloat("Health", health);
        harassed(attacker);
    }

    public override void heal(float points) {
        health = Mathf.Min(health+points, maxHealth);
        anim.SetFloat("Health", health);
    }

    public override void dashAfterPunch(float magnitude) {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.forward * magnitude + Vector3.up * 10, ForceMode.Impulse);
        StartCoroutine(stopRigidbody(.2f));
    }

    public override void push(float pushPower, bool actualHit, Vector3 direction) {
        if(!actualHit) pushPower /= 2;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(direction * pushPower/2 + Vector3.up * 10, ForceMode.Impulse);
        StartCoroutine(stopRigidbody(.2f*2));
        beingPushed = true;
    }

    private IEnumerator stopRigidbody(float time) {
        yield return new WaitForSeconds(time);
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0, 0, 0);
        beingPushed = false;
    }

    public GameObject getNextTarget() {
        if (targets.Count == 0) return null;
        if(targets[0] == null) targets.RemoveAt(0);
        anim.SetInteger("Targets", targets.Count);
        if (targets.Count > 0)
            return targets[0];
        return null;
    }

    public void harassed(GameObject person) {
        if (!targets.Contains(person))
            targets.Insert(0, person);
        anim.SetInteger("Targets", targets.Count);
    }

    private void OnTriggerEnter(Collider other) {
        if (!beingPushed || other.transform.IsChildOf(transform)) return;
        if (other.transform.parent && other.transform.parent.GetComponent<PersonOther>()) {
            other.transform.parent.GetComponent<PersonOther>().harassed(gameObject);
        }
    }
}
