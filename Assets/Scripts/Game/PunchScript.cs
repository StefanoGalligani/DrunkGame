using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchScript : MonoBehaviour
{
    public int damage = 1;
    private bool isPunching;

    private void Start() {
        isPunching = false;
    }

    private void OnCollisionEnter(Collision other) {
        if (isPunching) {
            if (other.gameObject.layer == LayerMask.NameToLayer("People")) {
                other.transform.parent.GetComponent<PersonScript>().hit(damage, true);
                Debug.Log("HIT!");
            }
        }
    }

    public void startPunching() {
        if (!isPunching) {
            isPunching = true;
            GetComponent<Animator>().SetBool("Punching", true);
        }
    }

    public void stopPunching() {
        isPunching = false;
        GetComponent<Animator>().SetBool("Punching", false);
    }
}
