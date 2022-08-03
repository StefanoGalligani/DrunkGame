using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public PunchScript[] punches;
    public float maxHoldTime = 3;
    public float cooldownTime = 1.5f;

    private bool[] holding = {false, false};
    private float[] holdTime = {0f, 0f};
    private KeyCode[] punchKeys = {KeyCode.Mouse0, KeyCode.Mouse1};

    private void Start() {
        changePunchColor(0, 0);
        changePunchColor(1, 0);
    }

    private void Update()
    {
        checkPunchControls(0);
        checkPunchControls(1);
    }

    private void checkPunchControls(int i) {
        if (!holding[i] && holdTime[i] < 0.1f) {
            holdTime[i] = 0;
            checkInitiatePunch(i);
        }
        
        if (holding[i]) {
            holdTime[i] = Mathf.Min(holdTime[i] + Time.deltaTime, maxHoldTime);
            checkReleasePunch(i);
        } else if (holdTime[i] >= 0.1f) {
            holdTime[i] -= Time.deltaTime * maxHoldTime / cooldownTime;
        }

        changePunchColor(i, holdTime[i]/maxHoldTime);
    }

    private void checkReleasePunch(int i) {
        if(Input.GetKeyUp(punchKeys[i])) {
            if (holdTime[i] < 0.1f) holdTime[i] = 0;
            punches[i].startPunching(holdTime[i]/maxHoldTime);
            holding[i] = false;
        }
    }

    private void checkInitiatePunch(int i) {
        if(Input.GetKey(punchKeys[i])) {
            holding[i] = true;
        }
    }

    private void changePunchColor(int i, float redPerc) {
        GetComponentsInChildren<PunchScript>()[i].changePunchColor(redPerc);
    }
}
