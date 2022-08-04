using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamAttractor : MonoBehaviour
{
    Camera[] cams;
    Quaternion startAngle;
    Vector3 startPos;
    float currentPerc;

    public void GameFinished() {
        transform.SetParent(null);

        cams = FindObjectsOfType<Camera>();
        cams[0].transform.SetParent(transform);
        cams[1].transform.SetParent(transform);
        
        startAngle = cams[0].transform.localRotation;
        startPos = cams[0].transform.position;
        currentPerc = 1;
        
        StartCoroutine(attractCameras());
    }

    private IEnumerator attractCameras() {
        while (true) {
            if (currentPerc < 0.01f) currentPerc = 0;
            currentPerc -= Time.deltaTime;

            cams[0].transform.localRotation = Quaternion.Slerp(startAngle, Quaternion.identity, 1-currentPerc);
            cams[1].transform.localRotation = Quaternion.Slerp(startAngle, Quaternion.identity, 1-currentPerc);


            cams[0].transform.position = transform.position + (startPos-transform.position)*currentPerc;
            cams[1].transform.position = transform.position + (startPos-transform.position)*currentPerc;

            if (currentPerc < 0.01f) break;
            yield return null;
        }
    }
}
