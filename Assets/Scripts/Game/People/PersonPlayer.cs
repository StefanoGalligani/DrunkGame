using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityStandardAssets.Characters.FirstPerson;

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

    public AudioMixer mixer;
    public AudioMixer musicMixer;
    public AudioClip soundDamage;
    public AudioSource dmgAudioSource;

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
        if (SettingsScript.sensitivity > 0.01f) {
            GetComponent<RigidbodyFirstPersonController>().mouseLook.XSensitivity = SettingsScript.sensitivity;
            GetComponent<RigidbodyFirstPersonController>().mouseLook.YSensitivity = SettingsScript.sensitivity;
        }
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
        if (health <=0) {
            death();
            dmgAudioSource.PlayOneShot(soundDeath[Random.Range(0,3)]);
        } else {
            muffleSound((float)health/maxHealth);
            StartCoroutine(damageUIRoutine());
            dmgAudioSource.PlayOneShot(soundDamage);
        }
    }

    private void death() {
        targetAlcoholLevel = 0;
        muffleSound(1);
        Destroy(GetComponent<PlayerAttack>());
        Destroy(GetComponent<RigidbodyFirstPersonController>());
        GetComponentInChildren<BeerTrigger>().removeFromGame();
        foreach(PunchScript p in GetComponentsInChildren<PunchScript>()) {
            Destroy(p.gameObject);
        }

        Transform body = transform.GetChild(1);
        Transform head = transform.GetChild(0);
        body.GetComponent<Rigidbody>().useGravity = true;
        body.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        body.GetComponent<CapsuleCollider>().enabled = true;
        body.gameObject.layer = LayerMask.NameToLayer("Default");
        body.SetParent(null);
        head.GetComponent<Rigidbody>().useGravity = true;
        head.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        head.gameObject.layer = LayerMask.NameToLayer("Default");
        head.SetParent(null);

        GetComponentInChildren<CamAttractor>().GameFinished();
        FindObjectOfType<PauseScript>().FinishGame(false);
        Destroy(damagedImg.gameObject);
        Destroy(lowHealthImg.gameObject);

        StartCoroutine(waitAndDie());
    }

    private IEnumerator waitAndDie() {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    public override void heal(float points) {
        health = Mathf.Min(health+points, maxHealth);
        changeAlpha(lowHealthImg, .9f - ((float)health/maxHealth));
        muffleSound((float)health/maxHealth);
        if (!SettingsScript.disableDoubleVision)
            targetAlcoholLevel = Mathf.Min(targetAlcoholLevel + alcoholPerBeer, maxAlcoholTime);
    }

    public override void dashAfterPunch(float magnitude) {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.drag = 0f;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.forward * magnitude + Vector3.up * 10, ForceMode.Impulse);
    }

    public override void push(float pushPower, bool actualHit, Vector3 direction) {
        if(!actualHit) pushPower /= 2;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.drag = 0f;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(direction * pushPower + Vector3.up * 10, ForceMode.Impulse);
    }

    private IEnumerator damageUIRoutine() {
        changeAlpha(damagedImg, 1);
        changeAlpha(lowHealthImg, Mathf.Max(0, 1 - ((float)health/maxHealth)));
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
        if (!i) return;
        Color c = i.color;
        c.a = Mathf.Max(0, a);
        i.color = c;
    }

    private void muffleSound(float healthPerc) {
        mixer.SetFloat("pitch", Mathf.Lerp(0.5f, 1, healthPerc));
        mixer.SetFloat("cutoff", Mathf.Lerp(500, 5000, healthPerc));
        musicMixer.SetFloat("cutoff", Mathf.Lerp(500, 5000, healthPerc));
    }
}
