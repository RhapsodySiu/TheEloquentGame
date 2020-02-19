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

    [TextArea(20, 20)]
    public string description;
    [TextArea(20, 20)]
    public string cn_description;
    [TextArea(20, 20)]
    public string zh_description;

    public override string ToString()
    {
        return "Thesis: " + thesisName + "\n\tMax health: " + thesisMaxHealth.ToString();
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
