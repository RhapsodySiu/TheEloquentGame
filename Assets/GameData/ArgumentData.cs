using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SerializedArgument
{
    public string argumentName;
    public string fact;
    public string tactic;
    public string respondTo;
    public bool toArgument;
}

[System.Serializable]
public class ArgumentData
{
    public List<SerializedArgument> serializedArguments = new List<SerializedArgument>();

}
