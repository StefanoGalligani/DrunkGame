using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PersonScript : MonoBehaviour
{
    public int maxHealth = 20;
    protected int health;

    public abstract void hit(int damage, bool head, GameObject attacker);

    public abstract void heal(int points);
}
