using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchScript : MonoBehaviour
{
    public int damage = 1;
    public bool autoAlert;
    private bool isPunching;

    private void Start() {
        SetAlerted(autoAlert);
        isPunching = false;
    }

    public void SetAlerted(bool a) {
        GetComponent<Animator>().SetBool("Alerted", a);
    }

    private void OnCollisionEnter(Collision other) {
        if (transform.IsChildOf(other.transform)) return;
        if (isPunching) {
            if (other.gameObject.layer == LayerMask.NameToLayer("People")) {
                other.transform.parent.GetComponent<PersonScript>().hit(damage, other.gameObject.name == "Head", transform.parent.parent.gameObject);
            }
            stopPunching();
        }
    }

    private IEnumerator suddenStop() {
        yield return new WaitForSeconds(0.015f);
        stopPunching();
    }

    private bool canPunch() {
        Vector3 localCenter = GetComponent<SphereCollider>().center;
        Vector3 center = transform.position + new Vector3(localCenter.x * transform.localScale.x, localCenter.y * transform.localScale.y, localCenter.z * transform.localScale.z);
        float radius = GetComponent<SphereCollider>().radius * transform.localScale.z;
        Collider[] overlaps = Physics.OverlapSphere(center, radius);

        foreach (Collider overlap in overlaps)
        {
            if (!transform.IsChildOf(overlap.transform)) return false;
        }
        return true;
    }

    public void startPunching() {
        if (!isPunching) {
            transform.parent.parent.GetComponent<Rigidbody>().AddForce(new Vector3(10, 10, 0), ForceMode.Impulse);
            gameObject.layer = LayerMask.NameToLayer("MovingPunch");
            isPunching = true;
            GetComponent<Animator>().SetBool("Punching", true);
            
            if (!canPunch())
                StartCoroutine(suddenStop());
        }
    }

    public void stopPunching() {
        gameObject.layer = LayerMask.NameToLayer("Punch");
        isPunching = false;
        GetComponent<Animator>().SetBool("Punching", false);
    }
}
