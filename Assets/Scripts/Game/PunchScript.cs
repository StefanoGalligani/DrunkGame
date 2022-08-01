using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchScript : MonoBehaviour
{
    public float minDamage = 1;
    public float maxDamage = 3;
    public bool autoAlert;
    public float minDashPower = 4;
    public float maxDashPower = 15;
    public float minPushPower = 5;
    public float maxPushPower = 20;

    private bool isPunching;
    private float currentDamagePerc;

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
                other.transform.parent.GetComponent<PersonScript>().hit(Mathf.Lerp(minDamage, maxDamage, currentDamagePerc),
                    other.gameObject.name == "Head", transform.parent.parent.gameObject);
                other.transform.parent.GetComponent<PersonScript>().push(Mathf.Lerp(minPushPower, maxPushPower, currentDamagePerc)
                    , true, transform.parent.parent.forward);
            }
            if (other.gameObject.layer == LayerMask.NameToLayer("Punch")) {
                other.transform.parent.parent.GetComponent<PersonScript>().push(Mathf.Lerp(minPushPower, maxPushPower, currentDamagePerc)
                , false, transform.parent.parent.forward);
            }
            stopPunching();
        }
    }

    public void startPunching(float damagePerc) {
        if (!isPunching) {
            transform.parent.parent.GetComponent<PersonScript>().dashAfterPunch(Mathf.Lerp(minDashPower, maxDashPower, damagePerc));
            gameObject.layer = LayerMask.NameToLayer("MovingPunch");
            isPunching = true;
            currentDamagePerc = damagePerc;
            if (!canPunch()) {
                GetComponent<Animator>().SetBool("Blocked", true);
                PersonScript personColliding = findPersonColliding();
                if (personColliding != null) {
                    personColliding.hit(0, false, transform.parent.parent.gameObject);
                }
            } else {
                GetComponent<Animator>().SetBool("Blocked", false);
            }
            GetComponent<Animator>().SetBool("Punching", true);
        }
    }

    private bool canPunch() {
        Vector3 localCenter = GetComponent<SphereCollider>().center;
        Vector3 center = transform.position + new Vector3(localCenter.x * transform.localScale.x, localCenter.y * transform.localScale.y, localCenter.z * transform.localScale.z);
        float radius = GetComponent<SphereCollider>().radius * transform.localScale.z / 2;
        Collider[] overlaps = Physics.OverlapSphere(center, radius);

        foreach (Collider overlap in overlaps)
        {
            if (!transform.IsChildOf(overlap.transform) && !overlap.isTrigger) return false;
        }
        return true;
    }

    private PersonScript findPersonColliding() {
        Vector3 localCenter = GetComponent<SphereCollider>().center;
        Vector3 center = transform.position + new Vector3(localCenter.x * transform.localScale.x, localCenter.y * transform.localScale.y, localCenter.z * transform.localScale.z);
        float radius = GetComponent<SphereCollider>().radius * transform.localScale.z / 2;
        Collider[] overlaps = Physics.OverlapSphere(center, radius);

        foreach (Collider overlap in overlaps)
        {
            if (!transform.IsChildOf(overlap.transform) && !overlap.isTrigger) {
                if (overlap.gameObject.layer == LayerMask.NameToLayer("People"))
                    return overlap.transform.parent.GetComponent<PersonScript>();
                if (overlap.gameObject.layer == LayerMask.NameToLayer("Punch"))
                    return overlap.transform.parent.parent.GetComponent<PersonScript>();
            }
        }
        return null;
    }

    public void stopPunching() {
        gameObject.layer = LayerMask.NameToLayer("Punch");
        isPunching = false;
        GetComponent<Animator>().SetBool("Punching", false);
    }

    public void changePunchColor(float redPerc) {
        Color c = new Color(1, 1 - redPerc, 1 - redPerc, 1);

        foreach(Renderer r in GetComponentsInChildren<Renderer>()) {
            r.material.color = c;
        }
    }
}
