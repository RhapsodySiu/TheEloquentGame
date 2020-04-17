using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Argument Info", menuName = "DebateGame/Create Argument Info")]
[System.Serializable]
public class ArgumentInfo : ScriptableObject
{
    public string ArgumentName = "";

    public bool IsProponentArgument = false;

    [Tooltip("The information text to display in GUI")]
    [TextArea(10, 10)]
    public string description;
    [TextArea(10, 10)]
    public string cn_description;
    [TextArea(10, 10)]
    public string zh_description;

    public override string ToString()
    {
        return "[" + ArgumentName + ", isProponentArgument=" + IsProponentArgument + "]";
    }

    public override bool Equals(object other)
    {
        if (other == null) return false;
        ArgumentInfo otherArgumentInfo = other as ArgumentInfo;

        return ArgumentName == otherArgumentInfo.ArgumentName && IsProponentArgument == otherArgumentInfo.IsProponentArgument;
    }

    public override int GetHashCode()
    {
        return ArgumentName.GetHashCode();
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
