using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerSpawner : MonoBehaviour
{
    public GameObject beer;

    public void respawn() {
        foreach (Transform t in transform) {
            bool canSpawn = true;
            Collider[] colliders = Physics.OverlapBox(t.position, new Vector3(1,1,1));
            foreach (Collider c in colliders) {
                if (c.gameObject.GetComponent<Beer>()) canSpawn = false;
            }
            if (canSpawn)
                GameObject.Instantiate(beer, t.position, Quaternion.identity);
        }

    }
}
