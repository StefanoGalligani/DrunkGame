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

    public override void hit(int damage, bool head, GameObject attacker) {
        if (head) damage *= 2;
        health -= damage;
        anim.SetInteger("Health", health);
        harassed(attacker);
    }

    public override void heal(int points) {
        health+=points;
        anim.SetInteger("Health", health);
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
