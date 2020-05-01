using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DefinedArgumentEffect
{
    public Argument argument;
    public ArgumentEffect argumentEffect;
}

[CreateAssetMenu(fileName = "new Defined argument set", menuName = "DebateGame/Create defiend argument set")]
public class DefinedArgumentEffects : ScriptableObject
{

    public DefinedArgumentEffect[] definedArguments;
}
