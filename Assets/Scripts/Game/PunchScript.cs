using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchScript : MonoBehaviour
{
    private bool isPunching;

    private void Start() {
        isPunching = false;
    }

    private void OnCollisionEnter(Collision other) {
        if (isPunching)
            Debug.Log("HIT!");
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
