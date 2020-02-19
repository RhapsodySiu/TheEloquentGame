using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fungus;

public class DataController : MonoBehaviour
{

    public const int MAX_THESES = 3;

    public TacticData tacticData;
    private Dictionary<string, Tactic> tacticDict = new Dictionary<string, Tactic>();

    public List<Argument> tempArguments = new List<Argument>();

    public Dictionary<string, int> argumentDict = new Dictionary<string, int> ();

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

    public bool IsPlayerRound()
    {
        return debate.isPlayerRound;
    }

    public bool IsArgumentInfoExists(Argument argument)
    {
        return debate.definedArgumentInfoDict.ContainsKey(argument);
    }

    public bool DoesPlayerMadeArgument(Argument argument)
    {
        return debate.player.arguments.Contains(argument);
    }

    public bool DoesEnemyMadeArgument(Argument argument)
    {
        return debate.enemy.arguments.Contains(argument);
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
        ArgumentEffect value = new ArgumentEffect();

        if (debate.definedArgumentEffectDict.TryGetValue(argument, out value))
        {
            return value;
        }
        else
        {
            // TODO: set default argument effect(failure, no effect...)
            return value;
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

    public int GetPlayerMentalHealth()
    {
        return debate.player.mentalHealth;
    }

    public int GetPlayerMaxMentalHealth()
    {
        return debate.player.mentalMaxHealth;
    }

    public int GetEnemyMentalHealth()
    {
        return debate.enemy.mentalHealth;
    }

    public int GetEnemyMaxMentalHealth()
    {
        return debate.enemy.mentalMaxHealth;
    }

    public int GetPlayerMaxThesesHealth()
    {
        return debate.player.totalMaxThesesHealth;
    }

    public int GetPlayerThesesHealth()
    {
        return debate.player.totalCurrentThesesHealth;
    }

    public int GetEnemyrMaxThesesHealth()
    {
        return debate.enemy.totalMaxThesesHealth;
    }

    public int GetEnemyThesesHealth()
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

    public void AddPlayerThesis(string thesisName)
    {
        if (debate.player.thesis_count >= MAX_THESES)
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

            debate.player.thesisList[debate.player.thesis_count] = new DebaterThesis(thesisToAdd);
            debate.player.thesis_count += 1;

        } else
        {
            Debug.LogError("Error in add player thesis: thesis with name '" + thesisName + "' not found.");
        }
    }

    // TODO: revamp to make logic clear
    public bool ApplyTempArgument()
    {
        // If no temporary arguments, indicate no argument made by returning false
        if (tempArguments.Count == 0)
        {
            return false;
        }
        Argument argumentMade = tempArguments[0];
        tempArguments.RemoveAt(0);

        // update player argument list
        debate.player.arguments.Add(argumentMade);

        ArgumentEffect effect = GetArgumentEffect(argumentMade);
        UpdateDebateStat(effect);

        return true;
    }

    public void UpdateDebateStat(ArgumentEffect effect)
    {
        blockToPlay = effect.conversationBlock;
        if (effect.toSelf)
        {
            debate.player.UpdateStat(effect);
        }
        else
        {
            debate.audience.UpdateStat(effect);
        }

        debate.audience.UpdateStat(effect);
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
