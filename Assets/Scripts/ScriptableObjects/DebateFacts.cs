using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Debate Facts", menuName = "DebateGame/Create Debate Facts data")]
public class DebateFacts : ScriptableObject
{
    public Fact[] availableFacts;
}
