using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public PunchScript punchLeft;
    public PunchScript punchRight;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0)) {
            punchLeft.startPunching();
        }
        if(Input.GetKeyDown(KeyCode.Mouse1)) {
            punchRight.startPunching();
        }
    }
}
