using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Tatic Set Data", menuName = "DebateGame/Create Tactic Set")]
[System.Serializable]
public class TacticData : ScriptableObject
{
    public Tactic[] tacitcs;
}
