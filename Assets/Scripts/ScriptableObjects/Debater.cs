using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum Trait
{
    Confident, 
    Diffident,
    Demagogue,
    Superstar
}

[CreateAssetMenu(fileName = "new Debater", menuName = "DebateGame/Create new Debater")]
[System.Serializable]
public class Debater : ScriptableObject
{
    public const int MAX_ARGS = 3;
    public int thesis_count = 0;
    public int id;
    public bool isProponent;
    public string debaterName;
    [TextArea(10, 20)]
    public string description;
    public DebaterThesis[] thesisList = new DebaterThesis[MAX_ARGS];
    public int totalCurrentThesesHealth;
    public int totalMaxThesesHealth;

    private void Awake()
    {
        totalMaxThesesHealth = 0;

        foreach (DebaterThesis thesis in thesisList)
        {
            totalMaxThesesHealth += thesis.maxHealth;
            totalCurrentThesesHealth += thesis.currentHealth;
        }
    }

    public List<Argument> arguments = new List<Argument> ();

    public Trait[] traits;
    public int mentalMaxHealth = 100;
    public int mentalHealth = 100;
    public int actionPoint = 3;

    public float basePow = 1f;
    public float baseInfluence = 1f;
    public float baseRoast = 1f;

    public void InitDebater()
    {
        thesis_count = 0;
        mentalHealth = mentalMaxHealth;

        arguments.Clear();
    }

    public void UpdateStat(ArgumentEffect effect)
    {
        mentalHealth += effect.mentalHealthChange;
        baseRoast += effect.mentalAttBuff;
        basePow += effect.ArgAttBuff;
        baseInfluence += effect.influenceBuff;

        foreach (DebaterThesis thesis in thesisList)
        {
            if (thesis.thesis.thesisId == effect.targetThesisId)
            {
                thesis.absHealthBuff += effect.maxThesisHealthChange;
                thesis.currentHealth += effect.currentThesisHealthChange;
                thesis.defense += effect.defenseChange;
            }
        }
    }
}
