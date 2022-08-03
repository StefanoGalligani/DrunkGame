using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObjects/AttacksScriptableObject", order = 1)]
public class AttacksScriptableObject : ScriptableObject
{
    public PunchAction[] actions;
}
