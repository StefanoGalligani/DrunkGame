using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchScript : MonoBehaviour
{
    public int damage = 1;
    public bool autoAlert;
    private bool isPunching;
    private List<GameObject> obstacles;

    private void Start() {
        SetAlerted(autoAlert);
        obstacles = new List<GameObject>();
        isPunching = false;
    }

    public void SetAlerted(bool a) {
        GetComponent<Animator>().SetBool("Alerted", a);
    }

    private void OnCollisionEnter(Collision other) {
        if (transform.IsChildOf(other.transform)) return;
        if (isPunching) {
            if (other.gameObject.layer == LayerMask.NameToLayer("People")) {
                if (other.transform.parent.GetComponent<PersonScript>()) {
                    other.transform.parent.GetComponent<PersonScript>().hit(damage, true);
                    other.transform.parent.GetComponent<PersonScript>().harassed(transform.parent.parent.gameObject);
                }
            }
            stopPunching();
        } else if (!obstacles.Contains(other.gameObject)){
            obstacles.Add(other.gameObject);
        }
    }

    private IEnumerator suddenStop() {
        yield return new WaitForEndOfFrame();
        stopPunching();
    }

    private void OnCollisionExit(Collision other) {
        if (transform.IsChildOf(other.transform)) return;
        obstacles.Remove(other.gameObject);
    }

    public void startPunching() {
        if (!isPunching) {
            gameObject.layer = LayerMask.NameToLayer("MovingPunch");
            isPunching = true;
            GetComponent<Animator>().SetBool("Punching", true);
            
            if (obstacles.Count > 0)
                StartCoroutine(suddenStop());
        }
    }

    public void stopPunching() {
        gameObject.layer = LayerMask.NameToLayer("Punch");
        isPunching = false;
        GetComponent<Animator>().SetBool("Punching", false);
    }
}
