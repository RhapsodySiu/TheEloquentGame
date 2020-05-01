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

    public List<Debate> debateList = new List<Debate>();
    public int debateIdx = 0;

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

    public void Init()
    {
        debateIdx = 0;
    }

    public void IncrementDebateIdx()
    {
        debateIdx += 1;
    }

    public bool Test()
    {
        // Task 1: can add player argument
        // AddPlayerArgument("Yellow1");
        return true;
    }

    public void InitDebateStatus()
    {
        GetCurrentDebate().Init();
    }

    #region Getter
    public int GetCurrentRound()
    {
        return GetCurrentDebate().round;
    }

    public Debate GetCurrentDebate()
    {
        return debateList[debateIdx];
    }

    public bool IsTutorial()
    {
        return GetCurrentDebate().isTutorial;
    }

    public bool IsPlayerRound()
    {
        return GetCurrentDebate().isPlayerRound;
    }

    public bool IsPlayerWin()
    {
        return GetCurrentDebate().IsPlayerWin();
    }

    public bool IsPlayerLose()
    {
        return GetCurrentDebate().IsPlayerLose();
    }

    public bool IsArgumentInfoExists(Argument argument)
    {
        return GetCurrentDebate().definedArgumentInfoDict.ContainsKey(argument);
    }

    /* Check whether player made an argument
     * Useful when checking if player is doing repeated move.
     */
    public bool DoesPlayerMadeArgument(Argument argument)
    {
        return GetCurrentDebate().IsPlayerArgumentExist(argument);
    }

    public bool DoesEnemyMadeArgument(Argument argument)
    {
        return GetCurrentDebate().IsEnemyArgumentExist(argument);
    }

    public Debater GetPlayer()
    {
        return GetCurrentDebate().player;
    }

    public Debater GetEnemy()
    {
        return GetCurrentDebate().enemy;
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
        return GetCurrentDebate().debateFacts;
    }

    public Fact GetDebateFactByName(string factName)
    {
        return GetCurrentDebate().debateFactDict[factName];
    }

    public ArgumentInfo GetArgumentInfo(Argument argument)
    {
        ArgumentInfo value = new ArgumentInfo();

        if (GetCurrentDebate().definedArgumentInfoDict.TryGetValue(argument, out value))
        {
            return value;
        }
        else
        {
            // TODO: generate argument information if no info is found
            return value;
        }
    }

    /**
     * Get the argument effect for an argument
     * It searches the definedArgumentEffectDict of the Debate
     * If no result is found, the default arguemnt effect is returned
     */
    public ArgumentEffect GetArgumentEffect(Argument argument)
    {
        ArgumentEffect value;

        if (GetCurrentDebate().definedArgumentEffectDict.TryGetValue(argument, out value))
        {
            return value;
        }
        else
        {
            return GetCurrentDebate().GetDefaultArgumentEffect();
        }
    }

    public Dictionary<Argument, ArgumentInfo> GetDefinedArgumentInfoDict()
    {
        return GetCurrentDebate().definedArgumentInfoDict;
    }

    public Dictionary<Argument, ArgumentEffect> GetDefinedArgumentEffectDict()
    {
        return GetCurrentDebate().definedArgumentEffectDict;
    }

    public void GenerateEnemyArgument()
    {
        // Add the enemy argument to temporary list
        Tuple<ArgumentInfo, Argument, ArgumentEffect> result = GetCurrentDebate().GenerateEnemyArgument();
        if (result != null)
        {
            tempArguments.Add(result.Item2);
        }
    }

    public void AddPlayerTemporaryArgument(Argument playerArgument)
    {
        tempArguments.Add(playerArgument);
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
        return GetCurrentDebate().player.isProponent;
    }

    public List<Tuple<ArgumentInfo, Argument>> GetPlayerArguments()
    {
        return GetCurrentDebate().GetPlayerArgumentInfos();
    }

    public List<Tuple<ArgumentInfo, Argument>> GetEnemyArguments()
    {
        return GetCurrentDebate().GetEnemyArgumentInfos();
    }

    public DebaterThesis[] GetPlayerTheses()
    {
        return GetCurrentDebate().player.thesisList;
    }

    public float GetPlayerConfidence()
    {
        return GetCurrentDebate().player.mentalHealth;
    }

    public float GetPlayerMaxConfidence()
    {
        return GetCurrentDebate().player.mentalMaxHealth;
    }

    public float GetEnemyConfidence()
    {
        return GetCurrentDebate().enemy.mentalHealth;
    }

    public float GetEnemyMaxConfidence()
    {
        return GetCurrentDebate().enemy.mentalMaxHealth;
    }

    public float GetPlayerMaxThesesHealth()
    {
        return GetCurrentDebate().player.totalMaxThesesHealth;
    }

    public float GetPlayerThesesHealth()
    {
        return GetCurrentDebate().player.totalCurrentThesesHealth;
    }

    public float GetEnemyrMaxThesesHealth()
    {
        return GetCurrentDebate().enemy.totalMaxThesesHealth;
    }

    public float GetEnemyThesesHealth()
    {
        return GetCurrentDebate().enemy.totalCurrentThesesHealth;
    }

    public DebaterThesis[] GetEnemyTheses()
    {
        return GetCurrentDebate().enemy.thesisList;
    }


    public Audience GetAudience()
    {
        return GetCurrentDebate().audience;
    }

    public float GetAudienceSupport()
    {
        return GetCurrentDebate().audience.support;
    }

    public List<Argument> GetTemporaryArguments()
    {
        return tempArguments;
    }


    #endregion

    #region Setter
    public void InitDebate()
    {
        GetCurrentDebate().Init();
    }

    public void InitMainDebate()
    {
        debateIdx = 1;
        GetCurrentDebate().Init();
    }

    public void SetPlayerSide(bool isProponent)
    {
        try
        {
            GetCurrentDebate().SetPlayerSide(isProponent);
        } catch (System.Exception ex)
        {
            Debug.LogException(ex);
        }

    }

    public void AddPlayerThesis(string thesisName)
    {
        try
        {
            if (GetCurrentDebate().player.thesisCount >= MAX_THESES)
            {
                Debug.Log("Error in add player thesis: Player already have 3 or above theses");
                return;
            }

            Thesis thesisToAdd = null;
            foreach (Thesis thesis in GetCurrentDebate().thesisAvailable)
            {
                if (thesisName == thesis.thesisName)
                {
                    thesisToAdd = thesis;
                    break;
                }
            }

            if (thesisToAdd != null)
            {
                if (GetCurrentDebate().player.isProponent != thesisToAdd.isProponent)
                {
                    Debug.LogError("Error in add player thesis: Player stand conflicts with thesis stand");
                    return;
                }

                GetCurrentDebate().player.AddThesis(thesisToAdd);

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

        if (IsPlayerRound())
        {
            GetCurrentDebate().player.arguments.Add(argumentMade);
        } else
        {
            GetCurrentDebate().enemy.arguments.Add(argumentMade);
        }

        // search if the argument is defined
        ArgumentInfo argumentInfo = GetCurrentDebate().GetArgumentInfo(argumentMade);
        if (argumentInfo != null)
        {
            GetCurrentDebate().AddArgumentRecord(argumentMade, argumentInfo);
        } else
        {
            Debug.Log("Cannot get argumentInfo for " + argumentMade);
        }

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
        GetCurrentDebate().round += 1;
        GetCurrentDebate().isPlayerRound = !GetCurrentDebate().isPlayerRound;
    }

    /**
     * Apply argument effect to the debate status.
     * Also apply change to the fungus flowchart
     */
    public void UpdateDebateStat(ArgumentEffect effect)
    {
        blockToPlay = effect.conversationBlock;
        if (effect.toSelf)
        {
            GetCurrentDebate().player.UpdateStat(effect);
            Debug.Log("Player status updated");
        }
        else
        {
            GetCurrentDebate().enemy.UpdateStat(effect);
            Debug.Log("Enemy status updated");
        }

        GetCurrentDebate().audience.UpdateStat(effect);
        Debug.Log("Audience status updated");
    }

    // For adjusting value without argument
    public void UpdatePlayerConfidence(float newValue)
    {
        try
        {
            GetCurrentDebate().player.mentalHealth = newValue;
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
            debateList[debateIdx] = loadedDebate;
        } else
        {
            Debug.LogError("Cannot load game data!");
        }
    }

}
