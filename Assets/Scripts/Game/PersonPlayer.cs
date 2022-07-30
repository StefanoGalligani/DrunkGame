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
}
