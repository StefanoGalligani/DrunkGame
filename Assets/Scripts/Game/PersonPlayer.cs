using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonPlayer : PersonScript
{
    private void Start() {
        health = maxHealth;
    }
    
    public override void hit(int damage, bool head, GameObject attacker) {
        if (head) damage *= 2;
        health -= damage;
        if (health <=0) Debug.Log("Dead");
    }

    public override void heal(int points) {
        health+=points;
    }

    public override void dashAfterPunch(float magnitude) {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.drag = 0f;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.forward * magnitude + Vector3.up * 10, ForceMode.Impulse);
    }
}
