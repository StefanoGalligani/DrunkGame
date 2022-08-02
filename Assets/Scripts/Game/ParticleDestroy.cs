using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroy : MonoBehaviour
{
    public float timeToLive = .6f;

    void Update()
    {
        timeToLive -= Time.deltaTime;
        if (timeToLive <= 0) {
            Destroy(this.gameObject);
        }        
    }
}
