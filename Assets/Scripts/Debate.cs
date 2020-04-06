using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Debate", menuName = "DebateGame/Create Debate Data")]
[System.Serializable]
public class Debate : ScriptableObject
{
    public Debater player;
    public Debater enemy;

    public bool isPlayerRound;

    public Audience audience;

    public DebateFacts debateFacts;
    public Dictionary<string, Fact> debateFactDict = new Dictionary<string, Fact>();

    public Thesis[] thesisAvailable;
    public Dictionary<Argument, ArgumentEffect> definedArgumentEffectDict = new Dictionary<Argument, ArgumentEffect>();
    public Dictionary<Argument, ArgumentInfo> definedArgumentInfoDict = new Dictionary<Argument, ArgumentInfo>();
    
    public DefinedArgumentEffects definedEffectAsset;
    public DefinedArgumentInfos definedInfoAsset;
    

    private ArgumentEffect defalutInvalidArgumentEffect;
    
    public int maxRound;
    public int round;

    public void Init()
    {
        player.mentalHealth = player.mentalMaxHealth;
        enemy.mentalHealth = enemy.mentalMaxHealth;
        player.thesis_count = 0;
        enemy.thesis_count = 0;
        audience.support = 0f;
        PopulateDebateFactDict();
        PopulateDefinedArgumentEffectDict();
        
    }

    public void PopulateDebateFactDict()
    {
        debateFactDict.Clear();
        if (debateFacts != null)
        {
            foreach (Fact fact in debateFacts.availableFacts)
            {
                debateFactDict.Add(fact.factName, fact);
            }
        } else
        {
            Debug.Log("Unable to popuolate fact dict: debateFacts is null");
        }
    }

    public void PopulateDefinedArgumentEffectDict()
    {
        definedArgumentEffectDict.Clear();
        if (definedEffectAsset != null)
        {
            foreach (DefinedArgumentEffect definedArgument in definedEffectAsset.definedArguments)
            {
                definedArgumentEffectDict.Add(definedArgument.argument, definedArgument.argumentEffect);
            }
        } else
        {
            Debug.LogError("Unable to populate argument effect dict: definedEffectAsset for debate is null!");
        }
    }

    public void PopulateDefinedArgumentInfoDict()
    {
        definedArgumentInfoDict.Clear();
        if (definedInfoAsset != null)
        {
            foreach (DefinedArgumentInfo definedArgumentInfo in definedInfoAsset.definedArgumentInfos)
            {
                definedArgumentInfoDict.Add(definedArgumentInfo.argument, definedArgumentInfo.argumentInfo);
            }
        } else
        {
            Debug.LogError("Unable to populate argument info dict: DefinedArgumentInfoAsset is null");
        }
    }

    public void SetDefaultEnemyThesis()
    {
        if (player.isProponent)
        {
            // set enemy as opponent
            enemy.isProponent = false;
        } else
        {
            // set enemy as proponent
            enemy.isProponent = true;
        }
    }

    public bool IsPlayerLose()
    {
        // Check audience support first
        if (player.isProponent && audience.support <= -1f)
        {
            return true;
        } else if (!player.isProponent && audience.support >= 1f)
        {
            return true;
        }

        // Check mental health
        if (player.mentalHealth <= 0)
        {
            return true;
        }

        // Check thesis health
        if (player.totalCurrentThesesHealth <= 0)
        {
            return true;
        }

        return false;
    }

    public bool IsPlayerWin()
    {
        // Check audience support first
        if (player.isProponent && audience.support >= 1f)
        {
            Debug.Log("Audience is in favor of proponent, player wins");
            return true;
        } else if (!player.isProponent && audience.support <= -1f)
        {
            Debug.Log("Audience is in favor of opponent, player wins");
            return true;
        }

        // Check enemy mental health
        if (enemy.mentalHealth <= 0)
        {
            Debug.Log("Enemy mental health reaches zero, player wins");
            return true;
        }

        if (enemy.totalCurrentThesesHealth <= 0)
        {
            Debug.Log("Enemy thesis health is zero, player wins");
            return true;
        }
        return false;
    }
}
