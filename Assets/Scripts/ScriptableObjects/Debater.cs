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
    public int thesisCount = 0;
    public int id;
    public bool isProponent;
    public string debaterName;
    [TextArea(5, 20)]
    public string description;
    public DebaterThesis[] thesisList = new DebaterThesis[MAX_ARGS];
    public int totalCurrentThesesHealth;
    public int totalMaxThesesHealth;


    public List<Argument> arguments = new List<Argument> ();

    public Trait[] traits;
    public float mentalMaxHealth = 100;
    public float mentalHealth = 100;
    public int actionPoint = 3;

    public float basePow = 1f;
    public float baseInfluence = 1f;
    public float baseRoast = 1f;

    public override string ToString()
    {
        return "Debater[name="+debaterName+",proponent="+isProponent+",argument made:"+arguments.Count+
                ",confidence="+mentalHealth+"/"+mentalMaxHealth+
                ",convincingness="+totalCurrentThesesHealth+"/"+totalMaxThesesHealth+
                ",thesis count="+thesisCount+"]";
    }

    public void InitDebater(bool clearThesis = false)
    {
        thesisCount = 0;
        // detach thesis from debater thesis if clearThesis = true. Otherwise, reset debater thesis
        if (clearThesis)
        {
            totalCurrentThesesHealth = 0;
            totalMaxThesesHealth = 0;
            foreach (DebaterThesis debaterThesis in thesisList)
            {
                if (debaterThesis != null)
                {
                    debaterThesis.thesis = null;
                }
            }
        } else
        {
            foreach (DebaterThesis debaterThesis in thesisList)
            {
                if (debaterThesis != null)
                {
                    debaterThesis.InitDebaterThesis();
                }
            }

            UpdateDebaterThesis();
        }
        mentalHealth = mentalMaxHealth;
        if (arguments.Count > 0)
        {
            arguments.Clear();
        }
        Debug.Log("Debater initialized: " + ToString());
    }

    public bool HasThesis(Thesis thesis)
    {
        foreach (DebaterThesis debaterThesis in thesisList)
        {
            if (debaterThesis.thesis == thesis)
            {
                return true;
            }
        }
        return false;
    }

    public void AddThesis(Thesis thesis)
    {
        // call debaterThesis's AddThesis method to register thesis for debater
        thesisList[thesisCount].AddThesis(thesis);
        thesisCount += 1;

        UpdateDebaterThesis();
    }

    // Called only when new thesis is added, update debater thesis health
    public void UpdateDebaterThesis()
    {
        totalCurrentThesesHealth = 0;
        totalMaxThesesHealth = 0;
        foreach (DebaterThesis debaterThesis in thesisList)
        {
            if (debaterThesis != null && debaterThesis.thesis != null)
            {
                totalMaxThesesHealth += debaterThesis.maxHealth;
                totalCurrentThesesHealth += debaterThesis.currentHealth;
                Debug.Log("Debater " + debaterName + " increases convincingness to " + totalCurrentThesesHealth);
            }
        }
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
