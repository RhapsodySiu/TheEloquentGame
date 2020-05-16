using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Argument", menuName = "DebateGame/Create Argument")]
[System.Serializable]
public class Argument : ScriptableObject
{
    // deprecated
    [Header("Deprecated")]
    public string argumentRespondName;
    [Header("Deprecated")]
    public string thesisRespondName;
    [Header("Deprecated")]
    public string tacticName;
    [Header("Deprecated")]
    public string factName;

    public Fact fact;
    public Tactic tactic;
    [Header("Either thesis is null or argument is null")]
    public Thesis thesis;
    public ArgumentInfo argument;

    public override string ToString()
    {
        if (thesis != null)
        {
            return "Thesis to respond: " + thesis.thesisName + "\n Tactic: " + (tactic == null ? "Invariant" : tactic.tacticName) + "\n Fact: " + (fact == null ? "None" : fact.factName) ;
        }
        return "Argument to respond: " + argument.ArgumentName + "\n Tactic: " + (tactic == null ? "Invariant" : tactic.tacticName) + "\n Fact: " + (fact == null ? "None" : fact.factName) ;
    }
}

public class ArgumentComparer : IEqualityComparer<Argument>
{
    public bool Equals(Argument x, Argument y)
    {
        return (x.argumentRespondName == y.argumentRespondName &&
                x.thesisRespondName == y.thesisRespondName &&
                x.tacticName == y.tacticName &&
                x.factName == y.factName);
    }

    public int GetHashCode(Argument x)
    {
        return x.argumentRespondName.GetHashCode() + x.thesisRespondName.GetHashCode() + x.tacticName.GetHashCode() + x.factName.GetHashCode();
    }
}
