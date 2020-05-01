using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Debate Data", menuName = "DebateGame/Create Debate Data asset")]
public class DebateData : ScriptableObject
{
    public Debate debate;

    /*
     * How to use scriptable object
     * 1. declare scriptable object in the target script
     * public DebateData m_Debate
     * 
     * 2. load the data through resource:
     * m_Debate = (DebateData) Resources.Load("New Debate Data", typeof(DebateData))
     */
}
