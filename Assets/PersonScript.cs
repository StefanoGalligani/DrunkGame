using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonScript : MonoBehaviour
{
    public int maxHealth = 20;
    int health;

    private void Start() {
        health = maxHealth;
    }

    public void hit(int damage, bool head) {
        health -= damage;
        if (head) health -= damage;
        if (health <= 0) death();
    }

    public void heal(int points) {
        health+=points;
    }

    private void death() {
        Transform head = transform.GetChild(0);
        Transform body = transform.GetChild(1);

        foreach (PunchScript punch in GetComponentsInChildren<PunchScript>())
        {
            Destroy(punch.gameObject);
        }

        head.gameObject.layer = LayerMask.NameToLayer("Default");
        head.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        head.GetComponent<Rigidbody>().useGravity = true;
        head.SetParent(null);

        body.gameObject.layer = LayerMask.NameToLayer("Default");
        body.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        body.GetComponent<Rigidbody>().useGravity = true;
        body.SetParent(null);

        Destroy(this.gameObject);
    }
}
