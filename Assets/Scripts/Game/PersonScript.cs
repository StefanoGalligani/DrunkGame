using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PersonScript : MonoBehaviour
{
    public float maxHealth = 20;
    protected float health;

    public abstract void hit(float damage, bool head, GameObject attacker);

    public abstract void heal(float points);

    public abstract void dashAfterPunch(float magnitude);

    public abstract void push(float pushPower, bool actualHit, Vector3 direction);
}
