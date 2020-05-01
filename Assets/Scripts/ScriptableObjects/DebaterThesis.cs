using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Debater thesis (testing)", menuName = "DebateGame/Create Debater thesis")]
[System.Serializable]
public class DebaterThesis : ScriptableObject
{
    public Thesis thesis;
    public int defense = 0;
    public float percentHealthBuff = 0f;
    public int absHealthBuff = 0;
    public int maxHealth;
    public int currentHealth;

    public DebaterThesis()
    {

    }

    public void AddThesis(Thesis t)
    {
        thesis = t;
        maxHealth = thesis.thesisMaxHealth;
        currentHealth = maxHealth;
        Debug.Log("current health = " + currentHealth.ToString());
    }

    public override string ToString()
    {
        return "Thesis: " + thesis.thesisName + "\n\thealth: " + currentHealth.ToString() + "/" + thesis.thesisMaxHealth.ToString() + "\n\tdefense: " + defense.ToString();
    }
}
