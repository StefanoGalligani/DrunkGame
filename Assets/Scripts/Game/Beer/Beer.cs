using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beer : MonoBehaviour
{
    public GameObject liquid;

    public void drank(Vector3 origin) {
        gameObject.layer = LayerMask.NameToLayer("Default");
        Destroy(liquid);
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().AddForce((transform.position - origin).normalized * 5 + Vector3.up * 2, ForceMode.Impulse);
        StartCoroutine(destroyAfterTime(5));
    }

    private IEnumerator destroyAfterTime(float time) {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
