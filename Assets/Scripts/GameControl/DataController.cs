using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fungus;
using System;

public class DataController : MonoBehaviour
{

    public const int MAX_THESES = 3;

    public TacticData tacticData;
    private Dictionary<string, Tactic> tacticDict = new Dictionary<string, Tactic>();

    public List<Argument> tempArguments = new List<Argument>();

    public Debate debate;

    private string gameDataFileName = "data.json";

    public string debugScene;
    public string blockToPlay;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (tacticData != null)
        {
            foreach (Tactic tactic in tacticData.tacitcs)
            {
                tacticDict.Add(tactic.tacticName, tactic);
            }
        }

        if (Test())
        {
            SceneManager.LoadScene(debugScene);
        }
    }

    public bool Test()
    {
        // Task 1: can add player argument
        // AddPlayerArgument("Yellow1");
        return true;
    }

    public void InitDebateStatus()
    {
        debate.Init();
    }

    #region Getter
    public int GetCurrentRound()
    {
        return debate.round;
    }

    public bool IsTutorial()
    {
        return debate.isTutorial;
    }

    public bool IsPlayerRound()
    {
        return debate.isPlayerRound;
    }

    public bool IsPlayerWin()
    {
        return debate.IsPlayerWin();
    }

    public bool IsPlayerLose()
    {
        return debate.IsPlayerLose();
    }

    public bool IsArgumentInfoExists(Argument argument)
    {
        return debate.definedArgumentInfoDict.ContainsKey(argument);
    }

    /* Check whether player made an argument 
     * Useful when checking if player is doing repeated move.
     */
    public bool DoesPlayerMadeArgument(Argument argument)
    {
        return debate.IsPlayerArgumentExist(argument);
    }

    public bool DoesEnemyMadeArgument(Argument argument)
    {
        return debate.IsEnemyArgumentExist(argument);
    }

    public Debater GetPlayer()
    {
        return debate.player;
    }

    public Debater GetEnemy()
    {
        return debate.enemy;
    }

    public void GetPlayerStatus()
    {

    }

    public void GetDialogue()
    {

    }

    public void GetEffect()
    {

    } 

    public DebateFacts GetDebateFacts()
    {
        return debate.debateFacts;
    }

    public Fact GetDebateFactByName(string factName)
    {
        return debate.debateFactDict[factName];
    }

    public ArgumentInfo GetArgumentInfo(Argument argument)
    {
        ArgumentInfo value = new ArgumentInfo();

        if (debate.definedArgumentInfoDict.TryGetValue(argument, out value))
        {
            return value;
        }
        else
        {
            // TODO: generate argument information if no info is found
            return value;
        }
    }

    public ArgumentEffect GetArgumentEffect(Argument argument)
    {
        ArgumentEffect value;

        if (debate.definedArgumentEffectDict.TryGetValue(argument, out value))
        {
            return value;
        }
        else
        {
            return null;
        }
    }

    public Dictionary<Argument, ArgumentInfo> GetDefinedArgumentInfoDict()
    {
        return debate.definedArgumentInfoDict;
    }

    public Dictionary<Argument, ArgumentEffect> GetDefinedArgumentEffectDict()
    {
        return debate.definedArgumentEffectDict;
    }

    public void GenerateEnemyArgument()
    {
        // Add the enemy argument to temporary list
        Tuple<ArgumentInfo, Argument, ArgumentEffect> result = debate.GenerateEnemyArgument();
        if (result != null)
        {
            tempArguments.Add(result.Item2);
        }
    }

    public TacticData GetTacticData()
    {
        return tacticData;
    }

    public Tactic GetTacticByName(string tacticName)
    {
        return tacticDict[tacticName];
    }

    public bool IsPlayerProponent()
    {
        return debate.player.isProponent;
    }

    public List<Argument> GetPlayerArguments()
    {
        return debate.player.arguments;
    }

    public List<Argument> GetEnemyArguments()
    {
        return debate.enemy.arguments;
    }

    public DebaterThesis[] GetPlayerTheses()
    {
        return debate.player.thesisList;
    }

    public float GetPlayerMentalHealth()
    {
        return debate.player.mentalHealth;
    }

    public float GetPlayerMaxMentalHealth()
    {
        return debate.player.mentalMaxHealth;
    }

    public float GetEnemyMentalHealth()
    {
        return debate.enemy.mentalHealth;
    }

    public float GetEnemyMaxMentalHealth()
    {
        return debate.enemy.mentalMaxHealth;
    }

    public float GetPlayerMaxThesesHealth()
    {
        return debate.player.totalMaxThesesHealth;
    }

    public float GetPlayerThesesHealth()
    {
        return debate.player.totalCurrentThesesHealth;
    }

    public float GetEnemyrMaxThesesHealth()
    {
        return debate.enemy.totalMaxThesesHealth;
    }

    public float GetEnemyThesesHealth()
    {
        return debate.enemy.totalCurrentThesesHealth;
    }

    public DebaterThesis[] GetEnemyTheses()
    {
        return debate.enemy.thesisList;
    }


    public Audience GetAudience()
    {
        return debate.audience;
    }

    public float GetAudienceSupport()
    {
        return debate.audience.support;
    }

    public List<Argument> GetTemporaryArguments()
    {
        return tempArguments;
    }


    #endregion

    #region Setter
    public void InitDebate()
    {
        debate.Init();
    }

    public void SetPlayerSide(bool isProponent)
    {
        try
        {
            debate.SetPlayerSide(isProponent);
        } catch (System.Exception ex)
        {
            Debug.LogException(ex);
        }
        
    }

    public void AddPlayerThesis(string thesisName)
    {
        try
        {
            if (debate.player.thesisCount >= MAX_THESES)
            {
                Debug.Log("Error in add player thesis: Player already have 3 or above theses");
                return;
            }

            Thesis thesisToAdd = null;
            foreach (Thesis thesis in debate.thesisAvailable)
            {
                if (thesisName == thesis.thesisName)
                {
                    thesisToAdd = thesis;
                    break;
                }
            }

            if (thesisToAdd != null)
            {
                if (debate.player.isProponent != thesisToAdd.isProponent)
                {
                    Debug.LogError("Error in add player thesis: Player stand conflicts with thesis stand");
                    return;
                }

                debate.player.AddThesis(thesisToAdd);

            }
            else
            {
                Debug.LogError("Error in add player thesis: thesis with name '" + thesisName + "' not found.");
            }
        } catch (System.Exception ex)
        {
            Debug.Log("Error during adding player thesis:");
            Debug.LogException(ex);
        }
    }

    // TODO: revamp to make logic clear
    public string ApplyTempArgument()
    {
        // If no temporary arguments, indicate no argument made by returning false
        if (tempArguments.Count == 0)
        {
            return null;
        }
        Argument argumentMade = tempArguments[0];
        tempArguments.RemoveAt(0);

        // update player argument list
        debate.player.arguments.Add(argumentMade);

        ArgumentEffect effect = GetArgumentEffect(argumentMade);
        if (effect != null)
        {
            UpdateDebateStat(effect);
        } else
        {
            Debug.LogError("Received null argument effect during applying temporary argument");
        }

        Debug.Log("Applied debate effect, return conversation block " + effect.conversationBlock);
        return effect.conversationBlock;
    }

    // increment round count and toggle player round
    public void UpdateRound()
    {
        debate.round += 1;
        debate.isPlayerRound = !debate.isPlayerRound;
    }

    public void UpdateDebateStat(ArgumentEffect effect)
    {
        blockToPlay = effect.conversationBlock;
        if (effect.toSelf)
        {
            debate.player.UpdateStat(effect);
            Debug.Log("Player status updated");
        }
        else
        {
            debate.enemy.UpdateStat(effect);
            Debug.Log("Enemy status updated");
        }

        debate.audience.UpdateStat(effect);
        Debug.Log("Audience status updated");
    }

    // For adjusting value without argument
    public void UpdatePlayerConfidence(float newValue)
    {
        try
        {
            debate.player.mentalHealth = newValue;
        } catch (System.Exception ex)
        {
            Debug.LogException(ex);
        }
        
    }

    
    #endregion


    private void LoadGameData()
    {
        string filepath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);

        if (File.Exists(filepath))
        {
            string dataAsJson = File.ReadAllText(filepath);
            Debate loadedDebate = JsonUtility.FromJson<Debate> (dataAsJson);
            debate = loadedDebate;
        } else
        {
            Debug.LogError("Cannot load game data!");
        }
    }

}
