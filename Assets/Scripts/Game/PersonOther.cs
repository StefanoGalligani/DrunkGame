using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonOther : PersonScript
{
    List<GameObject> targets;
    Animator anim;

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
        rb.AddForce(direction * pushPower + Vector3.up * 10, ForceMode.Impulse);
        StartCoroutine(stopRigidbody(.2f));
    }

    private IEnumerator stopRigidbody(float time) {
        yield return new WaitForSeconds(time);
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0, 0, 0);
    }

    public GameObject getNextTarget() {
        if(targets[0] == null) targets.RemoveAt(0);
        anim.SetInteger("Targets", targets.Count);
        if (targets.Count > 0)
            return targets[0];
        return null;
    }

    public void harassed(GameObject person) {
        if (!targets.Contains(person))
            targets.Add(person);
        anim.SetInteger("Targets", targets.Count);
    }
}
