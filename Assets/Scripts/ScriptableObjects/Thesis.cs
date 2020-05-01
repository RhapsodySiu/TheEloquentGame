using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Thesis", menuName = "DebateGame/Create Thesis")]
[System.Serializable]
public class Thesis : ScriptableObject
{
    public int thesisId;
    public string thesisName;
    public bool isProponent;
    public int thesisMaxHealth;

    [TextArea(5, 20)]
    public string description;
    [TextArea(5, 20)]
    public string cn_description;
    [TextArea(5, 20)]
    public string zh_description;

    public override string ToString()
    {
        return "Thesis: " + thesisName + "\n\tMax health: " + thesisMaxHealth.ToString();
    }
    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        Thesis objAsThesis = obj as Thesis;
        if (objAsThesis == null) return false;
        else return Equals(objAsThesis);
    }

    public override int GetHashCode()
    {
        return thesisName.GetHashCode();
    }
    public bool Equals(Thesis other)
    {
        if (other == null) return false;
        return (this.thesisName == other.thesisName && this.thesisId == other.thesisId);
    }

    public string GetDescription(string language)
    {
        switch (language)
        {
            case "CN":
                return cn_description;
            case "ZH":
                return zh_description;
            default:
                return description;
        }
    }
}
