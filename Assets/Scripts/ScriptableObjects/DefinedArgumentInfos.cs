using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DefinedArgumentInfo
{
    public Argument argument;
    public ArgumentInfo argumentInfo;
}

[CreateAssetMenu(fileName = "new Defined argument info set", menuName = "DebateGame/Create defined argument info set")]
public class DefinedArgumentInfos : ScriptableObject
{
    public DefinedArgumentInfo[] definedArgumentInfos;
}
