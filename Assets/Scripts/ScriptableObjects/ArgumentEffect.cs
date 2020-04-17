using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Argument Effect", menuName = "DebateGame/Create Argument Effect data")]
[System.Serializable]
public class ArgumentEffect : ScriptableObject
{
    public bool toSelf;
    public float mentalHealthChange;
    public float supportChange;
    public float ArgAttBuff;
    public float mentalAttBuff;
    public float influenceBuff;

    public int targetThesisId;
    public int maxThesisHealthChange;
    public int currentThesisHealthChange;
    public int defenseChange;

    public string conversationBlock;

    public override string ToString()
    {
        return "ArgumentEffect[toSelf="+toSelf+", conversationBlock="+conversationBlock+"]";
    }
}
