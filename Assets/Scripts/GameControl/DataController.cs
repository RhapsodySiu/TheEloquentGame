using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using Fungus;
using System;

public class DataController : MonoBehaviour
{
    public const string ARGUMENT_ASSET_PATH = "Assets/GameData/Arguments (debug only)/";
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
        debateIdx = 0;
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

    //public ArgumentInfo GetArgumentInfo(Argument argument)
    //{
    //    ArgumentInfo value = GetCurrentDebate().GetArgumentInfo(argument);
    //    return value;
    //}

    /**
     * Get the argument effect for an argument
     * It searches the definedArgumentEffectDict of the Debate
     * If no result is found, the default arguemnt effect is returned
     */
    //public ArgumentEffect GetArgumentEffect(Argument argument)
    //{
    //    return GetCurrentDebate().GetDefaultArgumentEffect();
    //}

    public Dictionary<Argument, ArgumentInfo> GetDefinedArgumentInfoDict()
    {
        return GetCurrentDebate().definedArgumentInfoDict;
    }

    public Dictionary<Argument, ArgumentEffect> GetDefinedArgumentEffectDict()
    {
        return GetCurrentDebate().definedArgumentEffectDict;
    }

    /**
     * Generate temporary arguments and store it in temporary arguments list of data controller
     */
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

    public float GetPlayerFloatThesesHealth()
    {
        if (GetPlayerMaxThesesHealth() > 0.0f)
        {
            return GetPlayerThesesHealth() / GetPlayerMaxThesesHealth();
        }
        return 0f;
    }

    public float GetEnemyFloatThesesHealth()
    {
        if (GetEnemyMaxThesesHealth() > 0.0f)
        {
            return GetEnemyThesesHealth() / GetEnemyMaxThesesHealth();
        }
        return 0f;
    }

    public float GetPlayerMaxThesesHealth()
    {
        return GetCurrentDebate().player.totalMaxThesesHealth;
    }

    public float GetPlayerThesesHealth()
    {
        return GetCurrentDebate().player.totalCurrentThesesHealth;
    }

    public float GetEnemyMaxThesesHealth()
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
        SetPlayerSide(false);
        GetCurrentDebate().Init();
    }

    public void InitMainDebate()
    {
        debateIdx = 1;
        GetCurrentDebate().Init();
        GetCurrentDebate().isPlayerRound = false;
    }

    /**
     * Since only main debate allows select side, the current debate is hardcode
     */
    public void SetPlayerSide(bool isProponent)
    {
        try
        {
            // TODO: avoid hardcode debateidx
            // debateIdx = 1;
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

        if (argumentMade == null)
        {
            Debug.LogError("Cannot find argument from temporary argument list");
            return "DefaultArgumentBlock";
        } else
        {
            //Debug.Log("Argument made:");
            //Debug.Log(argumentMade);
        }
        tempArguments.RemoveAt(0);

        if (IsPlayerRound())
        {
            //Debug.Log("Argument made by player:" + argumentMade);
            GetCurrentDebate().player.arguments.Add(argumentMade);
        } else
        {
            GetCurrentDebate().enemy.arguments.Add(argumentMade);
        }

        // search if the argument is defined
        (ArgumentInfo, ArgumentEffect) query = GetCurrentDebate().GetArgumentResult(argumentMade);
        //Debug.Log("ArgumentEffect returned = " + query.Item2);
        if (query.Item1 != null)
        {
            GetCurrentDebate().AddArgumentRecord(argumentMade, query.Item1);
        } else
        {
            Debug.Log("Cannot get argumentInfo for " + argumentMade);
        }
;
        if (query.Item2 != null)
        {
            UpdateDebateStat(query.Item2);
        } else
        {
            Debug.LogError("Received null argument effect during applying temporary argument, return default effect");
            ArgumentEffect effect = GetCurrentDebate().GetDefaultArgumentEffect();
            UpdateDebateStat(effect);
            return effect.conversationBlock;
        }

        Debug.Log("Applied debate effect, return conversation block " + query.Item2.conversationBlock);
        return query.Item2.conversationBlock;
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
        if (effect.toSelf && IsPlayerRound() || !effect.toSelf && !IsPlayerRound())
        {
            GetCurrentDebate().player.UpdateStat(effect);
            //Debug.Log("Player status updated");
        }
        else
        {
            GetCurrentDebate().enemy.UpdateStat(effect);
            //Debug.Log("Enemy status updated");
        }

        GetCurrentDebate().audience.UpdateStat(effect);
        //Debug.Log("Audience status updated");
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

    public void UpdateSupport(float newValue)
    {
        try
        {
            GetCurrentDebate().audience.support = newValue;
        }
        catch (System.Exception ex)
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

    //[MenuItem("GameObject/Load Arguments from arguments.json")]
    //static void CreateArgumentFromJSON()
    //{
    //    string argumentPath = Application.dataPath + "/" + "arguments.json";

    //    if (!System.IO.File.Exists(argumentPath))
    //    {
    //        return;
    //    }

    //    string contents = System.IO.File.ReadAllText(argumentPath);
    //    JsonWrapper wrapper = JsonUtility.FromJson<JsonWrapper>(contents);
    //    ArgumentData argumentData = wrapper.argumentData;

    //    foreach (SerializedArgument serializedArgument in argumentData.serializedArguments)
    //    {
    //        string argumentAssetFilename = serializedArgument.argumentName;
    //        Debug.Log("Saving " + argumentAssetFilename);
    //        try
    //        {
    //            if (serializedArgument.fact != "" && serializedArgument.fact != null)
    //            {
    //                argumentAssetFilename += "_By" + serializedArgument.fact;
    //            }
    //            if (serializedArgument.toArgument && serializedArgument.tactic != null)
    //            {

    //                argumentAssetFilename += "_Against" + serializedArgument.tactic;
    //            }
    //            argumentAssetFilename += "_To" + serializedArgument.respondTo + ".asset";

    //            Argument argumentBase = ScriptableObject.CreateInstance<Argument>();
    //            argumentBase = ScriptableObject.CreateInstance<Argument>();

    //            Fact factBase = (Fact)AssetDatabase.LoadAssetAtPath("Assets/GameData/Facts/main/" + serializedArgument.fact + ".asset", typeof(Fact));
    //            argumentBase.fact = factBase;

    //            if (serializedArgument.tactic != null && serializedArgument.tactic != "")
    //            {
    //                Tactic tacticBase = (Tactic)AssetDatabase.LoadAssetAtPath("Assets/GameData/Tactics/" + serializedArgument.tactic + ".asset", typeof(Tactic));
    //                argumentBase.tactic = tacticBase;
    //            }

    //            if (serializedArgument.toArgument)
    //            {
    //                ArgumentInfo argumentInfoBase = (ArgumentInfo)AssetDatabase.LoadAssetAtPath("Assets/GameData/DefinedArgumentInfos/main/" + serializedArgument.respondTo + ".asset", typeof(ArgumentInfo));
    //                argumentBase.argument = argumentInfoBase;
    //            }
    //            else
    //            {
    //                Thesis thesisBase = (Thesis)AssetDatabase.LoadAssetAtPath("Assets/GameData/Theses/" + serializedArgument.respondTo + ".asset", typeof(Thesis));
    //                argumentBase.thesis = thesisBase;
    //            }

    //            // Create asset if not exist
    //            if (!System.IO.File.Exists(Application.dataPath + "/GameData/Arguments (debug only)/main/" + argumentAssetFilename ))
    //            {
    //                AssetDatabase.CreateAsset(argumentBase, ARGUMENT_ASSET_PATH + "main/" + argumentAssetFilename);
    //            }
                
    //        }
    //        catch (Exception ex)
    //        {
    //            Debug.LogError("Error when trying create asset for " + argumentAssetFilename);
    //            Debug.LogException(ex);
    //            throw;
    //        }
    //    }
        
    //    AssetDatabase.SaveAssets();
    //    AssetDatabase.Refresh();
    //    EditorUtility.FocusProjectWindow();
    //    Debug.Log("Create assets successfully.");

    //}

    /**
     * Create defined argument info set from arguments for the main debate
     */
    //[MenuItem("GameObject/Create ArgumentInfo_Effect set by name")]
    //static void PopulateArgumentInfoEffectSet()
    //{
    //    DirectoryInfo dir = new DirectoryInfo(ARGUMENT_ASSET_PATH + "main/");
    //    FileInfo[] argumentAssets = dir.GetFiles("*.asset");

    //    // create generated defined argument info/effect set object
    //    DefinedArgumentInfos definedArgumentInfoSet = ScriptableObject.CreateInstance<DefinedArgumentInfos>();
    //    DefinedArgumentEffects definedArgumentEffectSet = ScriptableObject.CreateInstance<DefinedArgumentEffects>();
    //    List<DefinedArgumentInfo> definedArgumentInfos = new List<DefinedArgumentInfo>();
    //    List<DefinedArgumentEffect> definedArgumentEffects = new List<DefinedArgumentEffect>();
    //    foreach (FileInfo argumentAsset in argumentAssets)
    //    {
    //        string filename = argumentAsset.Name;
    //        try
    //        {
                
    //            Argument argument = (Argument)AssetDatabase.LoadAssetAtPath(ARGUMENT_ASSET_PATH + "main/" + filename, typeof(Argument));

    //            string argumentName = filename.Split(new string[] { "_By" }, StringSplitOptions.None)[0];
    //            // Try to load corresponding argument effect and info in their folders by argumentName
    //            ArgumentInfo argumentInfo = (ArgumentInfo)AssetDatabase.LoadAssetAtPath("Assets/GameData/DefinedArgumentInfos/main/" + argumentName + ".asset", typeof(ArgumentInfo));
    //            ArgumentEffect argumentEffect = (ArgumentEffect)AssetDatabase.LoadAssetAtPath("Assets/GameData/DefinedArgumentEffects/main/" + argumentName + ".asset", typeof(ArgumentEffect));
    //            DefinedArgumentInfo definedArgumentInfo;
    //            DefinedArgumentEffect definedArgumentEffect;
    //            definedArgumentInfo.argument = argument;
    //            definedArgumentInfo.argumentInfo = argumentInfo;
    //            definedArgumentEffect.argument = argument;
    //            definedArgumentEffect.argumentEffect = argumentEffect;

    //            definedArgumentInfos.Add(definedArgumentInfo);
    //            definedArgumentEffects.Add(definedArgumentEffect);
    //        }
    //        catch (Exception ex)
    //        {
    //            Debug.LogError("Error when handling argument " + filename);
    //            Debug.LogException(ex);
    //        }

    //    }

    //    // save the generated assets
    //    definedArgumentInfoSet.definedArgumentInfos = definedArgumentInfos.ToArray();
    //    definedArgumentEffectSet.definedArguments = definedArgumentEffects.ToArray();

    //    AssetDatabase.CreateAsset(definedArgumentInfoSet, "Assets/generated_main_defined_argument_infos.asset");
    //    AssetDatabase.CreateAsset(definedArgumentEffectSet, "Assets/generated_main_defined_argument_effects.asset");
    //    AssetDatabase.SaveAssets();
    //    AssetDatabase.Refresh();
    //    EditorUtility.FocusProjectWindow();
    //    Debug.Log("Create assets successfully.");
    //}

    //[MenuItem("GameObject/Create sample argument data")]
    //static void CreateSampleJsonData()
    //{
    //    SerializedArgument serializedArgument = new SerializedArgument();
    //    serializedArgument.argumentName = "Argument name";
    //    serializedArgument.fact = "Fact name";
    //    serializedArgument.tactic = "Tactic name";
    //    serializedArgument.toArgument = false;
    //    JsonWrapper jsonWrapper = new JsonWrapper(); 
    //    ArgumentData argumentData = new ArgumentData();
    //    argumentData.serializedArguments.Add(serializedArgument);
    //    jsonWrapper.argumentData = argumentData;

    //    string contents = JsonUtility.ToJson(jsonWrapper, true);
    //    System.IO.File.WriteAllText(Application.dataPath + "/test.json", contents);
    //}
}
