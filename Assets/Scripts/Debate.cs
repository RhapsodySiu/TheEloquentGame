﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "new Debate", menuName = "DebateGame/Create Debate Data")]
[System.Serializable]
public class Debate : ScriptableObject
{
    public Debater player;
    public Debater enemy;

    public Debater enemyProponent;
    public Debater enemyOpponent;

    public bool isProponent;

    public bool isPlayerRound;
    public bool isTutorial;

    public Audience audience;

    public DebateFacts debateFacts;
    public Dictionary<string, Fact> debateFactDict = new Dictionary<string, Fact>();

    public Thesis[] thesisAvailable;
    public Dictionary<Argument, ArgumentEffect> definedArgumentEffectDict = new Dictionary<Argument, ArgumentEffect>();
    public Dictionary<Argument, ArgumentInfo> definedArgumentInfoDict = new Dictionary<Argument, ArgumentInfo>();

    public DefinedArgumentEffects definedEffectAsset;
    public DefinedArgumentInfos definedInfoAsset;

    // Arguments that player/enemy made
    private List<Tuple<ArgumentInfo, Argument>> playerArgumentInfoHistory = new List<Tuple<ArgumentInfo, Argument>>();
    private List<Tuple<ArgumentInfo, Argument>> enemyArgumentInfoHistory = new List<Tuple<ArgumentInfo, Argument>>();

    // Arguments that player/enemy can make
    private List<(ArgumentInfo, Argument)> availableEnemyArguments = new List<(ArgumentInfo, Argument)>();
    private List<(ArgumentInfo, Argument)> availablePlayerArguments = new List<(ArgumentInfo, Argument)>();

    public ArgumentEffect defalutInvalidArgumentEffect;

    // for random utility
    private System.Random rng = new System.Random();

    public int maxRound;
    public int round;

    /**
     * Init debate AFTER player side confirmed
     */
    public void Init()
    {
        Debug.Log("InitDebate player isProponent=" + player.isProponent);
        try
        {
            round = 0;
            isPlayerRound = false;
            // player.isProponent = isProponent;
            player.InitDebater(true);
            if (!isTutorial)
            {
                enemy = player.isProponent ? enemyOpponent : enemyProponent;
                // SetPlayerSide(isProponent);
                //Debug.Log("Set player isProponent=" + isProponent);
                Debug.Log("Set " + enemy + " to enemy");
            }
            enemy.InitDebater(false);
            audience.support = 0f;
            playerArgumentInfoHistory.Clear();
            enemyArgumentInfoHistory.Clear();
            availableEnemyArguments.Clear();
            availablePlayerArguments.Clear();
            InitDefinedArguments();
            PopulateDefinedArgumentEffectDict();
            PopulateDefinedArgumentInfoDict();
        } catch (System.Exception ex)
        {
            Debug.LogError("Error during initializing debate");
            Debug.LogException(ex);
        }
    }

    public void InitDefinedArguments()
    {
        availableEnemyArguments.Clear();
        availablePlayerArguments.Clear();
        foreach (DefinedArgumentInfo definedArgumentInfo in definedInfoAsset.definedArgumentInfos)
        {
            ArgumentInfo argumentInfo = definedArgumentInfo.argumentInfo;
            Argument argument = definedArgumentInfo.argument;

            if (argumentInfo != null && argument != null)
            {
                if (argumentInfo.IsProponentArgument == player.isProponent)
                {
                    // Debug.Log("Add " + argumentInfo + " to player available move");
                    availablePlayerArguments.Add((argumentInfo, argument));
                }
                else
                {
                    // Debug.Log("Add " + argumentInfo + " to enemy available move");
                    availableEnemyArguments.Add((argumentInfo, argument));
                }
            }
        }
    }

    public void PopulateDefinedArgumentEffectDict()
    {
        definedArgumentEffectDict.Clear();
        if (definedEffectAsset != null)
        {
            foreach (DefinedArgumentEffect definedArgumentEffect in definedEffectAsset.definedArguments)
            {
                if (definedArgumentEffect.argument != null && definedArgumentEffect.argumentEffect != null)
                {
                    definedArgumentEffectDict.Add(definedArgumentEffect.argument, definedArgumentEffect.argumentEffect);
                }
            }
        } else
        {
            Debug.LogError("Unable to populate argument effect dict: definedEffectAsset for debate is null!");
        }
    }

    // Create dictionary from the asset for better lookup
    public void PopulateDefinedArgumentInfoDict()
    {
        definedArgumentInfoDict.Clear();
        if (definedInfoAsset != null)
        {
            foreach (DefinedArgumentInfo definedArgumentInfo in definedInfoAsset.definedArgumentInfos)
            {
                if (definedArgumentInfo.argument != null && definedArgumentInfo.argumentInfo != null)
                {
                    definedArgumentInfoDict.Add(definedArgumentInfo.argument, definedArgumentInfo.argumentInfo);
                }
            }
        } else
        {
            Debug.LogError("Unable to populate argument info dict: DefinedArgumentInfoAsset is null");
        }
    }

    public void AddArgumentRecord(Argument argumentMade,ArgumentInfo argumentInfo)
    {
        if (argumentInfo.IsProponentArgument == player.isProponent)
        {
            Debug.Log("Add argument info '" + argumentInfo.ArgumentName + "' to player history, previous lengt="+ playerArgumentInfoHistory.Count); 
            playerArgumentInfoHistory.Add(new Tuple<ArgumentInfo, Argument>(argumentInfo, argumentMade));
        } else
        {
            Debug.Log("Add argument info '" + argumentInfo.ArgumentName + "' to enemy history, previous length=" + enemyArgumentInfoHistory.Count);
            enemyArgumentInfoHistory.Add(new Tuple<ArgumentInfo, Argument>(argumentInfo, argumentMade));
        }
    }

    public void SetPlayerSide(bool isProponent)
    {
        player.isProponent = isProponent;
        this.isProponent = isProponent;
        Debug.Log("Set player isProponent=" + isProponent);
    }

    /*
     * Draw random argument from availableEnemyArguments.
     * The argument drawn must fulfill two condition 1) Its ArgumentInfo's never in enemy history;
     * 2) The argument/thesis to which it responds exists.
     * If the ArgumentInfo is already in enemy history, remove the item.
     * TODO: Use better datatype eg Dictionary to manage items
    */
    public Tuple<ArgumentInfo, Argument, ArgumentEffect> GenerateEnemyArgument()
    {
        try
        {
            ArgumentInfo argumentInfo = null;
            Argument argument = null;
            ArgumentEffect argumentEffect;

            Debug.Log("Current enemy argument remains = " + availableEnemyArguments.Count);
            if (isTutorial)
            {
                // HARD CODE enemy ai for tutorial
                // In tutorial, MobileAppsCauseAddiction argument goes first, following by YouAreAddictedAsWell
                if (availableEnemyArguments.Count <= 1)
                {
                    argumentInfo = availableEnemyArguments[0].Item1;
                    argument = availableEnemyArguments[0].Item2;
                    availableEnemyArguments.Clear();
                }
                else
                {
                    foreach ((ArgumentInfo, Argument) argumentTuple in availableEnemyArguments)
                    {
                        if (argumentTuple.Item1.ArgumentName == "MobileAppsCauseAddiction")
                        {
                            argumentInfo = argumentTuple.Item1;
                            argument = argumentTuple.Item2;
                            break;
                        }
                    }
                    availableEnemyArguments.RemoveAll(t => t.Item1 == argumentInfo);

                }

                Debug.Log("GenerateEnemyArgument: get arugment info = " + argumentInfo);
                Debug.Log("GenerateEnemyArgument: get arugment = " + argument);

                definedArgumentEffectDict.TryGetValue(argument, out argumentEffect);
                Debug.Log("GenerateEnemyArgument: get effect = " + argumentEffect);
                return new Tuple<ArgumentInfo, Argument, ArgumentEffect>(argumentInfo, argument, argumentEffect);
            }

            List<int> values = Enumerable.Range(0, availableEnemyArguments.Count).ToList();
            // shuffle the list
            int n = values.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                int value = values[k];
                values[k] = values[n];
                values[n] = value;
            }

            bool foundArgument = false;
            foreach (int i in values)
            {
                argumentInfo = availableEnemyArguments[i].Item1;
                argument = availableEnemyArguments[i].Item2;
                // Check if the argument/thesis to which it responds exists.
                if (argument.thesis != null && (player.HasThesis(argument.thesis) || enemy.HasThesis(argument.thesis)))
                {
                    //Debug.Log("Found corresponding thesis info, candidate selected");
                    foundArgument = true;
                    break;
                }
                else if (argument.argument != null && playerArgumentInfoHistory.Any(argumentTuple => argumentTuple.Item1 == argument.argument))
                {
                    //Debug.Log("Found corresponding argument info, candidate selected");
                    foundArgument = true;
                    break;
                }
            }

            if (!foundArgument)
            {
                Debug.LogError("Enemy argument exhausted. Should increase available argument size or handle properly");
            }

            if (argumentInfo != null && foundArgument)
            {
                Debug.Log("Argument selected: " + argumentInfo);
                // if suitable argument is found, remove all available arguments having the same argument info
                //Debug.Log("Removed relative argument infos");
                availableEnemyArguments.RemoveAll(t => t.Item1 == argumentInfo);
                // update enemy history and enemy argument record
                Tuple<ArgumentInfo, Argument> history = new Tuple<ArgumentInfo, Argument>(argumentInfo, argument);
                // enemyArgumentInfoHistory.Add(history);
                // enemy.arguments.Add(argument);

                definedArgumentEffectDict.TryGetValue(argument, out argumentEffect);
                return new Tuple<ArgumentInfo, Argument, ArgumentEffect>(argumentInfo, argument, argumentEffect);
            }
        } catch (System.Exception ex)
        {
            Debug.LogError("Error exists during generation of enemy argument:");
            Debug.LogException(ex);
        }
        return null;
    }

    public bool IsPlayerLose()
    {
        // Check audience support first
        if (player.isProponent && audience.support <= -1f)
        {
            Debug.Log("Player loses as proponent");
            return true;
        } else if (!player.isProponent && audience.support >= 1f)
        {
            Debug.Log("Player loses as opponent");
            return true;
        }

        // Check mental health
        if (player.mentalHealth <= 0)
        {
            Debug.Log("Player breakdown");
            return true;
        }

        // Check thesis health
        if (player.totalCurrentThesesHealth <= 0)
        {
            Debug.Log("Player loses convincingness");
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

    /**
     * Given an argument, check if the corresponding argument info exists
     * 
     */
    public (ArgumentInfo, ArgumentEffect) GetArgumentResult(Argument argument)
    {
        ArgumentInfo argumentInfo = null;
        ArgumentEffect argumentEffect = null;
        Debug.Log("Get argument result for " + argument + ", playerRound=" + isPlayerRound + ", definedArgumentInfoDict length=" + definedArgumentInfoDict.Count);
        foreach (KeyValuePair<Argument, ArgumentInfo> entry in definedArgumentInfoDict)
        {
            bool respondMatched = false;
            bool tacticMatched = false;
            bool factMatched = false;
            bool standMatched = false;
            // Since enemy AI often fails to match move, revamp the search method

            // checking argument
            if (entry.Key.argument != null && argument.argument != null)
            {
                if (entry.Key.argument.ArgumentName == argument.argument.ArgumentName) respondMatched = true;
            }

            // checking thesis
            if (entry.Key.thesis != null && argument.thesis != null)
            {
                if (entry.Key.thesis.thesisName == argument.thesis.thesisName) respondMatched = true;
            }

            if (entry.Value.TacticInvariant) tacticMatched = true;
            else if (entry.Key.tactic == null && argument.tactic == null) tacticMatched = true;
            else if (entry.Key.tactic != null && argument.tactic != null && entry.Key.tactic.tacticName == argument.tactic.tacticName) tacticMatched = true;

            // if the entry fact is null and the argument fact should be null or None
            if (entry.Key.fact == null)
            {
                if (argument.fact == null || argument.fact.factName == "None") factMatched = true;
            }
            else if (entry.Key.fact.factName == "None")
            {
                // if the entry fact is None, the argument fact should be null or None
                if (argument.fact == null || argument.fact.factName == "None") factMatched = true;
            }
            else
            {
                // finally, if fact is neither none nor null, compare fact name directly
                if (argument.fact != null && argument.fact.factName == entry.Key.fact.factName) factMatched = true;
            }

            if (isPlayerRound)
            {
                if (entry.Value.IsProponentArgument == player.isProponent) standMatched = true;
            }
            else
            {
                if (entry.Value.IsProponentArgument == enemy.isProponent) standMatched = true;
            }

            // debug last problemm....
            //if (!isPlayerRound) Debug.Log("Compare respond tactic fact stand of " + entry.Value.ArgumentName + " = " + respondMatched + "," + tacticMatched + "," + factMatched + "," + standMatched);
            if (!respondMatched || !tacticMatched || !factMatched || !standMatched) continue;

            // if tactic invariant, return this result
            argumentInfo = entry.Value;
            argumentEffect = GetArgumentEffect(argument, entry.Value.TacticInvariant);
            break;
        }
        if (argumentEffect == null)
        {
            argumentEffect = GetDefaultArgumentEffect();
        }
        Debug.Log("Query=" + argument + ", info=" + argumentInfo + ", effect=" + argumentEffect);
        return (argumentInfo, argumentEffect);
    }

    public ArgumentEffect GetArgumentEffect(Argument argument, bool tacticInvariant)
    {

        ArgumentEffect argumentEffect = null;
        foreach (KeyValuePair<Argument, ArgumentEffect> entry in definedArgumentEffectDict)
        {
            if (entry.Key.argument == argument.argument && entry.Key.thesis == argument.thesis)
            {
                bool factMatched = false;
                // if the entry fact is null and the argument fact should be null or None
                if (entry.Key.fact == null)
                {
                    if (argument.fact == null || argument.fact.factName == "None") factMatched = true;
                }
                else if (entry.Key.fact.factName == "None")
                {
                    // if the entry fact is None, the argument fact should be null or None
                    if (argument.fact == null || argument.fact.factName == "None") factMatched = true;
                }
                else
                {
                    // finally, if fact is neither none nor null, compare fact name directly
                    if (argument.fact != null && argument.fact.factName == entry.Key.fact.factName) factMatched = true;
                }

                if (!factMatched) continue;
                // if tactic invariant, return this result
                if (tacticInvariant)
                {
                    argumentEffect = entry.Value;
                    break;
                }
                else
                {
                    if (entry.Key.tactic == argument.tactic)
                    {
                        argumentEffect = entry.Value;
                        break;
                    }
                }
            }
        }
        if (argumentEffect == null)
        {
            argumentEffect = GetDefaultArgumentEffect();
        }
        return argumentEffect;
    }

    public bool IsPlayerArgumentExist(Argument argument)
    {
        ArgumentInfo argumentInfo;
        if (definedArgumentInfoDict.TryGetValue(argument, out argumentInfo))
        {
            if (playerArgumentInfoHistory.Any(t => t.Item1 == argumentInfo))
            {
                return true;
            }
        }

        return false;
    }


    public bool IsEnemyArgumentExist(Argument argument)
    {
        ArgumentInfo argumentInfo;
        if (definedArgumentInfoDict.TryGetValue(argument, out argumentInfo))
        {
            if (enemyArgumentInfoHistory.Any(t => t.Item1 == argumentInfo))
            {
                return true;
            }
        }

        return false;
    }

    /**
     * Get the defined argument infos made by player so far
     */
    public List<Tuple<ArgumentInfo, Argument>> GetPlayerArgumentInfos()
    {
        return playerArgumentInfoHistory;
    }

    /**
     * Get the defined argument infos made by enemy so far
     */
    public List<Tuple<ArgumentInfo, Argument>> GetEnemyArgumentInfos()
    {
        return enemyArgumentInfoHistory;
    }

    /**
     * Get the default argument effect for the debate
     * Usually indicates a fail argument
     */
    public ArgumentEffect GetDefaultArgumentEffect()
    {
        return defalutInvalidArgumentEffect;
    }
}
