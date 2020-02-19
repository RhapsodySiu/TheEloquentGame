using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Argument
{
    public string argumentRespondName;
    public string thesisRespondName;
    public string tacticName;
    public string factName;
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
