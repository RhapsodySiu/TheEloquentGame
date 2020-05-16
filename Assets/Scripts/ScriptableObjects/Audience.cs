using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Audience", menuName = "DebateGame/Create Audience")]
[System.Serializable]
public class Audience : ScriptableObject
{
    public int audienceId;
    public string audienceName;
    [Range(-1f, 1f)]
    public float support;

    public IdeologySpectrum ideology;

    public void UpdateStat(ArgumentEffect effect)
    {
        Debug.Log("Audience support change=" + effect.supportChange);
        support += effect.supportChange;
    }
}
