using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Fact", menuName = "DebateGame/Create Fact")]
[System.Serializable]
public class Fact : ScriptableObject
{
    public int factId;
    public string factName;
    [Tooltip("The information text to display")]
    [TextArea(20, 20)]
    public string description;
    [TextArea(20, 20)]
    public string cn_description;
    [TextArea(20, 20)]
    public string zh_description;

    public string GetDescription(string language)
    {
        switch(language)
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
