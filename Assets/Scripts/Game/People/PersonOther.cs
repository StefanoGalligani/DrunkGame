using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonOther : PersonScript
{
    public AttacksScriptableObject attackInfo;
    public AudioSource walkAudioSource;
    public AudioClip[] footstepSounds;
    public MeshRenderer headRenderer;
    public MeshRenderer bodyRenderer;
    public Texture[] faceTextures1;
    public Texture[] faceTextures2;
    public Texture[] clothTextures;
    public GameObject[] hairObjects;
    public Material[] hairMats;

    [HideInInspector]
    public Vector3 idlePosition;
    List<GameObject> targets;
    Animator anim;

    [HideInInspector]
    public float attackWaitTimer;
    [HideInInspector]
    public float attackChargeTimer;
    [HideInInspector]
    public int attackIndex = -1;
    [HideInInspector]
    public bool charging = false;

    bool beingPushed = false;
    private int faceIndex;
    private bool previousAngry = false;
    

    private void Start() {
        health = maxHealth;
        anim = GetComponent<Animator>();
        targets = new List<GameObject>();
        idlePosition = transform.position;
        InitiateTextures();
    }

    private void InitiateTextures() {
        faceIndex = Random.Range(0,2);
        changeFace(false);
        bodyRenderer.material.mainTexture = clothTextures[Random.Range(0, clothTextures.Length)];
        if (hairObjects.Length > 0) {
            int toDelete = Random.Range(0,3);
            if (toDelete < 2) {
                Destroy(hairObjects[toDelete]);
                hairObjects[1 - toDelete].GetComponent<MeshRenderer>().material = hairMats[Random.Range(0,hairMats.Length)];
            } else {
                Destroy(hairObjects[0]);
                Destroy(hairObjects[1]);
            }
        }
    }

    public void changeFace(bool isAngry) {
        previousAngry = isAngry;
        int nextIndex = 0;
        if (isAngry) nextIndex += 1;
        if (health <= maxHealth/2) nextIndex += 2;
        if (health <= 0.1f) nextIndex = 4;

        if (faceIndex == 0)
            headRenderer.material.mainTexture = faceTextures1[nextIndex];
        else
            headRenderer.material.mainTexture = faceTextures2[nextIndex];
    }

    public override void hit(float damage, bool head, GameObject attacker) {
        changeFace(previousAngry);
        if (head) damage *= 2;
        health -= damage;
        anim.SetFloat("Health", health);
        harassed(attacker);
    }

    public override void heal(float points) {
        health = Mathf.Min(health+points, maxHealth);
        anim.SetFloat("Health", health);
    }

    public override void dashAfterPunch(float magnitude) {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.forward * magnitude + Vector3.up * 10, ForceMode.Impulse);
        StartCoroutine(stopRigidbody(.2f));
        beingPushed = true;
    }

    public override void push(float pushPower, bool actualHit, Vector3 direction) {
        if(!actualHit) pushPower /= 2;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(direction * pushPower/2 + Vector3.up * 10, ForceMode.Impulse);
        StartCoroutine(stopRigidbody(.2f*2));
        beingPushed = true;
    }

    public bool isBeingPushed() {
        return beingPushed;
    }

    private IEnumerator stopRigidbody(float time) {
        yield return new WaitForSeconds(time);
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0, 0, 0);
        beingPushed = false;
    }

    public GameObject getNextTarget() {
        if (targets.Count == 0) return null;
        if(targets[0] == null) targets.RemoveAt(0);
        anim.SetInteger("Targets", targets.Count);
        if (targets.Count > 0)
            return targets[0];
        return null;
    }

    public void harassed(GameObject person) {
        if (!targets.Contains(person)){
            if (Random.Range(0f,1f) < 0.5f || (gameObject.tag == "Enemy" && person.tag == "Enemy"))
                targets.Add(person);
            else
                targets.Insert(0, person);
        }
        anim.SetInteger("Targets", targets.Count);
    }

    private void OnTriggerEnter(Collider other) {
        if (!beingPushed || other.transform.IsChildOf(transform)) return;
        if (other.transform.parent && other.transform.parent.GetComponent<PersonOther>()) {
            other.transform.parent.GetComponent<PersonOther>().harassed(gameObject);
        }
    }

    public void chasePlayer() {
        StartCoroutine(addPlayerToTargets());
    }

    private IEnumerator addPlayerToTargets() {
        yield return new WaitForSeconds(0.3f);
        PersonPlayer player = FindObjectOfType<PersonPlayer>();
        if (player) {
            targets.Add(player.gameObject);
            anim.SetInteger("Targets", targets.Count);
        }
    }
}
