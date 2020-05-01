using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum TacticType
{
    Logical,
    Informal,
    Distraction,
    AdHominem,
    Other
};

[CreateAssetMenu(fileName = "new Tactic", menuName = "DebateGame/Create Tactic")]
[System.Serializable]
public class Tactic : ScriptableObject
{
    public int tacticId;
    public TacticType tacticType;
    public string tacticName;
    [Tooltip("The information text to display")]
    [TextArea(20, 20)]
    public string description;
    [TextArea(20, 20)]
    public string zh_description;
    [TextArea(20, 20)]
    public string cn_description;

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

