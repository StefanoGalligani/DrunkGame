using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonScript : MonoBehaviour
{
    public int maxHealth = 20;
    int health;
    Animator anim;
    List<GameObject> targets;

    private void Start() {
        health = maxHealth;
        anim = GetComponent<Animator>();
        targets = new List<GameObject>();
    }

    public void hit(int damage, bool head) {
        if (head) damage *= 2;
        health -= damage;
        anim.SetInteger("Health", anim.GetInteger("Health") - damage);
    }

    public void heal(int points) {
        health+=points;
    }

    public void harassed(GameObject person) {
        if (!targets.Contains(person))
            targets.Add(person);
        anim.SetInteger("Targets", targets.Count);
    }

    public GameObject getNextTarget() {
        if(targets[0] == null) targets.RemoveAt(0);
        anim.SetInteger("Targets", targets.Count);
        if (targets.Count > 0)
            return targets[0];
        return null;
    }
}
