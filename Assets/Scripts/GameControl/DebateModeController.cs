using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;
using System;

public class DebateModeController : MonoBehaviour
{
    public Flowchart flowchart;
    private DataController dataController;

    public Canvas ActionGUI;
    public Canvas TacticGUI;
    public Canvas FactGUI;
    public Canvas ThesesGUI;
    public Canvas OverviewGUI;
    public Canvas MainGUI;

    public bool DebugMode;
    public Text DebugText;

    private Debate currentDebate;
    
    // Start is called before the first frame update
    void Start()
    {
        dataController = FindObjectOfType<DataController>();
        if (dataController == null)
        {
            Debug.LogError("Cannot get reference of DataController. You should load the Persistent scene.");
        } else
        {
            // Get defined arguments and divided into player arguments and enemy arguments
            
        }
    }

    #region GameLogic

    /* Called by fungus block to initiate variables and gui for game */
    public void InitDebate()
    {
        UpdateDebugText();
        dataController.InitDebate();
        Debug.Log("Start the debate");
    }

    /* Create arguments for enemy and stack them in temporary arguments */
    public void GenerateEnemyArguments()
    {
        // Call the below method 1-3 times to get argument(s) for enemy
        // FOR TUTORIAL, only called once
        if (dataController.IsTutorial())
        {
            dataController.GenerateEnemyArgument();
        } else
        {
            dataController.GenerateEnemyArgument();
        }
        Debug.Log("Enemy argument loaded into temporary argument");
        // Make argument
        // MakeArgument();
    }

    public void StartRound()
    {
        Debug.Log("Play fungus start round block");
        flowchart.ExecuteBlock("StartRound");
    }

    public void EndRound()
    {
        // update debug info
        UpdateDebugText();

        Debug.Log("Check if there is winner");
        if (dataController.IsPlayerWin())
        {
            Debug.Log("player win");
            flowchart.ExecuteBlock("Win");
        } else if (dataController.IsPlayerLose())
        {
            Debug.Log("enemy win");
            flowchart.ExecuteBlock("Lose");
        } else
        {
            Debug.Log("Neither one wins or loses");
            UpdateRound();
        }
    }

    public void UpdateDebugText()
    {
        Debug.Log("Update debug panel");
        DebugText.text = "Player proponent=" +  dataController.IsPlayerProponent() + 
                         "\nPlayer confidence=" + dataController.GetPlayerMentalHealth() +
                         "\nPopularity=" + dataController.GetAudienceSupport() +
                         "\nPlayer convincingness=" + dataController.GetPlayerThesesHealth() +
                         "\nEnemy confidence=" + dataController.GetEnemyMentalHealth() +
                         "\nIs player round=" + dataController.IsPlayerRound();

    }

    public void UpdateRound()
    {
        Debug.Log("Update round.");
        dataController.UpdateRound();
        flowchart.SetIntegerVariable("round", dataController.GetCurrentRound());
        flowchart.SetBooleanVariable("isPlayerRound", dataController.IsPlayerRound());
        Debug.Log("Back to interaction block");
        flowchart.ExecuteBlock("Interaction");
    }

    /* Push an argument from temporary argument list, if empty, execute end round block */
    public void MakeArgument()
    {
        // flowchart.ExecuteBlock("DefaultArgument");
        Debug.Log("Make argument");
        while (dataController.tempArguments.Count != 0)
        {
            Debug.Log("Call datacontroller to apply temp argument");
            string blockToExecute = dataController.ApplyTempArgument();

            flowchart.ExecuteBlock(blockToExecute);
        }

        Debug.Log("Empty temporary argument, run fungus EndRound block");
        flowchart.ExecuteBlock("EndRound");

    }

    public void AddPlayerThesis(string thesisName)
    {
        Debug.Log("Add debater thesis to player");
        dataController.AddPlayerThesis(thesisName);
    }

    public void AddPlayerArgument(string arg)
    {
        //Debug.Log("Try to add argument '" + arg + "' into player argument list.");
        dataController.AddPlayerThesis(arg);
    }


    public void PlayAIMove()
    {

        flowchart.ExecuteBlock("OpponentMove1");
    } 

    public void MakePlayerRound()
    {
        
    }

    /*
     * Check if the player can fire the arguments.
     * First it ensures all arguments are responding to either a thesis or an enemy argument, and have a tactic attached to them
     * Second in tutorial mode, the argument that the player made is limited and fixed
     * Finally it checks if similar arguments are made previously
     */
    public bool ValidateArgument()
    {
        if (dataController.IsTutorial())
        {
            if (dataController.GetCurrentRound() == 0)
            {
                // Check if player is making "MobileAppProductivity" argument
            } else if (dataController.GetCurrentRound() == 1)
            {
                // Check if player is making "RefuteYouAreAddictedClaim" argument
            }
        }
        return false;
    }
    #endregion

    #region Setter

    public void SetPlayerSide(bool isProponent)
    {
        Debug.Log("Set player side: isProponent=" + isProponent);
        dataController.SetPlayerSide(isProponent);
    }

    public void UpdatePlayerConfidence(float newValue)
    {
        Debug.Log(newValue);
        dataController.UpdatePlayerConfidence(newValue);
        Debug.Log("New player confidence = " + newValue);
    }

    #endregion

    #region GUIControl
    public void InitFactMenu()
    {
        DebateFacts facts = dataController.GetDebateFacts();

        foreach (Fact fact in facts.availableFacts)
        {
            Debug.Log("Draw fact with name " + fact.factName);
        }
    }

    public void InitTacticMenu()
    {
        TacticData tactics = dataController.GetTacticData();

        foreach (Tactic tactic in tactics.tacitcs)
        {
            Debug.Log("Draw tactic with name " + tactic.tacticName);
        }
    }

    public void InitInfoMenu()
    {
        Audience audience = dataController.GetAudience();
        Debater enemy = dataController.GetEnemy();
        Debater player = dataController.GetPlayer();
    }

    public void DrawStatusPanel()
    {
        float playerMentalHealth = dataController.GetPlayerMentalHealth();
        float playerMaxMentalHealth = dataController.GetPlayerMaxMentalHealth();
        float enemyMentalHealth = dataController.GetEnemyMentalHealth();
        float enemyMaxMentalHealth = dataController.GetEnemyMaxMentalHealth();
        float playerThesisHealth = dataController.GetPlayerThesesHealth();
        float playerMaxThesisHealth = dataController.GetPlayerMaxThesesHealth();
        float enemyThesisHealth = dataController.GetEnemyThesesHealth();
        float enemyMaxThesisHealth = dataController.GetEnemyrMaxThesesHealth();
        float publicSupport = dataController.GetAudienceSupport();
    }

    public void DrawThesesMenu(bool showPlayer)
    {
        DebaterThesis[] debaterTheses = null;
        List<Argument> playerArguments = dataController.GetPlayerArguments();
        List<Argument> enemyArguments = null;

        if (showPlayer)
        {
            debaterTheses = dataController.GetPlayerTheses();
        } else
        {
            List<string> enemyArgumentsResponded = new List<string> ();
            foreach (Argument argument in playerArguments)
            {
                ArgumentInfo info = dataController.GetArgumentInfo(argument);
                if (info.name != "")
                {
                    enemyArgumentsResponded.Add(info.name);
                }
            }
            debaterTheses = dataController.GetEnemyTheses();
            enemyArguments = dataController.GetEnemyArguments();

            // display which enemy argument has been refuted
            foreach (Argument argument in enemyArguments)
            {
                ArgumentInfo info = dataController.GetArgumentInfo(argument);
                if (enemyArgumentsResponded.Contains(info.name))
                {
                    Debug.Log("Player already responded to argument " + info.name + ", should disable clickable property");
                }
            }
        }
        foreach (DebaterThesis thesis in debaterTheses)
        {
            Debug.Log("Draw debater thesis of " + (showPlayer ? "player" : "enemy"));
        }

        foreach (Argument argument in playerArguments)
        {
            ArgumentInfo info = dataController.GetArgumentInfo(argument);
            Debug.Log("Draw debater arguments: " + info.name);
        }

    }

    // Open interactive mode: player can use Consult button, in particular
    public void EnableInteraction()
    {
        Transform consultBtn = MainGUI.transform.Find("MainPanel/ButtonsContainer/ButtonsPanel/ConsultBtn");
        // in tutorial mode, consult button is always disabled
        if (consultBtn && !dataController.IsTutorial())
        {
            consultBtn.gameObject.GetComponent<Button>().interactable = true;
        }
        Transform actionBtn = MainGUI.transform.Find("MainPanel/ButtonsContainer/ButtonsPanel/ActionBtn");
        if (actionBtn)
        {
            actionBtn.gameObject.GetComponent<Button>().interactable = true;
        }
    }

    public void DisableInteraction()
    {
        Transform consultBtn = MainGUI.transform.Find("MainPanel/ButtonsContainer/ButtonsPanel/ConsultBtn");
        if (consultBtn)
        {
            consultBtn.gameObject.GetComponent<Button>().interactable = false;
        }
        Transform actionBtn = MainGUI.transform.Find("MainPanel/ButtonsContainer/ButtonsPanel/ActionBtn");
        if (actionBtn)
        {
            actionBtn.gameObject.GetComponent<Button>().interactable = false;
        }
    }

    public void DrawTemporaryArguments()
    {
        List<Argument> tempArguments = dataController.GetTemporaryArguments();
        foreach (Argument tempArg in tempArguments)
        {
            Debug.Log("Show temp argument info and index in UI");

            if (dataController.DoesPlayerMadeArgument(tempArg))
            {
                Debug.Log("WARNING: player already made that move. Should disable action button");
            }
        }

    }

    #endregion

    #region EventHandler
    /* Main menu */
    public void OnClickedToggleThesesBtn()
    {
        Debug.Log("Toggle thesis menu");
        ThesesGUI.gameObject.SetActive(!ThesesGUI.gameObject.activeSelf);
    }

    public void OnClickedToggleActionBtn()
    {
        Debug.Log("Toggle argument menu");
        ActionGUI.gameObject.SetActive(!ActionGUI.gameObject.activeSelf);
    }

    public void OnClickedToggleOverviewBtn()
    {
        Debug.Log("Toggle info menu");
        OverviewGUI.gameObject.SetActive(!OverviewGUI.gameObject.activeSelf);
    }

    public void OnClickedConserveBtn()
    {
        Debug.Log("Converse button is clicked");
    }

    /* Argument menu */

    public void OnClickedMakeArgumentsBtn()
    {
        Debug.Log("Make arguments");
        // Load items into temporary arguments

        // Make argument
        MakeArgument();

    }

    public void toggleFactMenu()
    {
        FactGUI.gameObject.SetActive(!FactGUI.gameObject.activeSelf);
    }

    public void toggleTacticMenu()
    {
        TacticGUI.gameObject.SetActive(!TacticGUI.gameObject.activeSelf);
    }

    public void OnClickedAddFactBtn()
    {
        Debug.Log("Add fact to active temporary argument list, close argument menu and toggle fact menu");
    }

    public void OnClickedAddTacticBtn()
    {
        Debug.Log("Add tactic to active temporary argument list, close argument menu and toggle tactic menu");
    }

    public void OnClickedAddResponseBtn()
    {
        Debug.Log("Add thesis/argument responded to active temporary argument list, close argument menu and toggle thesis menu");
    }

    public void OnClickedArgumentInfo()
    {
        Debug.Log("Change active temporary argument in UI");
    }

    /* Thesis menu */

    public void OnClickedToggleDebaterBtn(bool showPlayer)
    {
        DrawThesesMenu(!showPlayer);
    }

    public void OnClickedThesisBtn()
    {
        Debug.Log("Add this thesis to active temp argument menu, close thesis menu and toggle argument menu");
    }

    public void OnClickedArgumentBtn()
    {
        Debug.Log("Add this enemy  argument to active temp argument menu, close thesis menu and toggle argument menu");
    }

    /* Fact menu */
    public void OnClickedFactItem()
    {
        Debug.Log("Pass the description value of the button to the text panel");
    }

    public void OnClickedUseFactBtn()
    {
        Debug.Log("Add selected fact to active temporary argument, close fact menu and toggle argument menu");
    }

    /* Tactic menu */
    public void OnClickedTacticItem()
    {
        Debug.Log("Pass the description value of the button to the text panel");
    }

    public void OnClickedUseTacticBtn()
    {
        Debug.Log("Add selected tactic to active temporary argument, close tactic menu and toggle argument menu");
    }
    #endregion
}
