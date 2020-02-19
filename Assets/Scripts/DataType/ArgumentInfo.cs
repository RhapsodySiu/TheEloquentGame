using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ArgumentInfo
{
    public string name;

    [Tooltip("The information text to display in GUI")]
    [TextArea(20, 20)]
    public string description;
    [TextArea(20, 20)]
    public string cn_description;
    [TextArea(20, 20)]
    public string zh_description;

    public ArgumentInfo()
    {
        name = "";
        description = "";
        cn_description = "";
        zh_description = "";
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
