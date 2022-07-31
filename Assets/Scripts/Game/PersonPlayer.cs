using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonPlayer : PersonScript
{
    public Image damagedImg;
    public Image lowHealthImg;
    public float maxAlcoholTime = 20;
    public float maxAngle = 30;
    public float oscillationSpeed = 1;
    public float oscillationMagnitudeMultiplier = .75f;
    public float alcoholPerBeer = 5;
    public float syncSpeed = 3;

    private float targetAlcoholLevel = 0;
    private float currentAlcoholLevel = 0;
    private float sineCounter = 0;
    private Transform cam1;
    private Transform cam2;

    private void Start() {
        health = maxHealth;
        changeAlpha(damagedImg, 0);
        changeAlpha(lowHealthImg, 0);
        Camera[] cams = GetComponentsInChildren<Camera>();
        cam1 = cams[0].transform;
        cam2 = cams[1].transform;
    }

    private void Update() {
        if (targetAlcoholLevel < 0.1f && currentAlcoholLevel < 0.1f) return;
        sineCounter += Time.deltaTime * oscillationSpeed;
        if (sineCounter >= 2*Mathf.PI) sineCounter -= 2*Mathf.PI;

        float actualTarget = targetAlcoholLevel + targetAlcoholLevel * oscillationMagnitudeMultiplier * Mathf.Sin(sineCounter);
        currentAlcoholLevel += (actualTarget - currentAlcoholLevel) * Time.deltaTime * syncSpeed;

        targetAlcoholLevel -= Time.deltaTime;
        tiltCameras();
    }

    private void tiltCameras() {
        if (targetAlcoholLevel < 0.1f && currentAlcoholLevel < 0.1f) currentAlcoholLevel = 0;
        float targetRot = maxAngle * currentAlcoholLevel/maxAlcoholTime;
        cam1.localRotation = Quaternion.Euler(0, -targetRot, 0);
        cam2.localRotation = Quaternion.Euler(0, targetRot, 0);
    }
    
    public override void hit(float damage, bool head, GameObject attacker) {
        if (head) damage *= 2;
        health -= damage;
        if (health <=0) Debug.Log("Dead");
        StartCoroutine(damageUIRoutine());
    }

    public override void heal(float points) {
        health = Mathf.Min(health+points, maxHealth);
        changeAlpha(lowHealthImg, .9f - ((float)health/maxHealth));
        targetAlcoholLevel = Mathf.Min(targetAlcoholLevel + alcoholPerBeer, maxAlcoholTime);
    }

    public override void dashAfterPunch(float magnitude) {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.drag = 0f;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.forward * magnitude + Vector3.up * 10, ForceMode.Impulse);
    }

    public override void push(float pushPower, bool actualHit) {
        if(!actualHit) pushPower /= 2;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.drag = 0f;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(-transform.forward * pushPower + Vector3.up * 10, ForceMode.Impulse);
    }

    private IEnumerator damageUIRoutine() {
        changeAlpha(damagedImg, 1);
        changeAlpha(lowHealthImg, .9f - ((float)health/maxHealth));
        float counter = 1;
        float speed = 2;
        while (counter > 0) {
            changeAlpha(damagedImg, counter*counter);
            counter -= Time.deltaTime*speed;
            yield return null;
        }
        changeAlpha(damagedImg, 0);
    }

    private void changeAlpha(Image i, float a) {
        Color c = i.color;
        c.a = a;
        i.color = c;
    }
}
