using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fungus;
using System;
using TMPro;

public class DebateModeController : MonoBehaviour
{
    public Audience defaultAudience;
    public Flowchart flowchart;
    private DataController dataController;

    public Canvas ActionGUI;
    public Canvas TacticGUI;
    public Canvas FactGUI;
    public Canvas ThesesGUI;
    public Canvas OverviewGUI;
    public Canvas MainGUI;
    public Canvas AlertDialog;

    public AudioSource fireArgumentSound;
    //public AudioSource updateStatusSound;

    // public StatusPanel statusPanel;
    // public AudiencePanel audiencePanel;
    public bool DebugMode;
    public Text DebugText;

    public List<Argument> PlayerTemporaryArgumentList = new List<Argument>();

    private ThesisPanel[] thesisPanels;
    public ActionListPanel actionListPanel;
    private Component[] argumentBtns;
    private bool editMode;

    public ConvincingnessScript convincingness;
    public PopularityScript popularity;
    // TODO: use another component
    public ConvincingnessScript confidence;

    private Debate currentDebate;

    // Start is called before the first frame update
    void Start()
    {
        dataController = FindObjectOfType<DataController>();
        if (actionListPanel == null)
        {
            Debug.LogError("Cannot find reference to action list panel");
        }
        // EMPTY TEMPORARY ARGUMENTS
        foreach (Argument arg in PlayerTemporaryArgumentList)
        {
            arg.argument = null;
            arg.thesis = null;
            arg.fact = null;
            arg.tactic = null;
            // Debug.Log("Empty temporary argument");
        }
        if (dataController == null)
        {
            Debug.LogError("Cannot get reference of DataController. You should load the Persistent scene.");
        } else
        {
            // Get defined arguments and divided into player arguments and enemy arguments

        }
        thesisPanels = ThesesGUI.gameObject.GetComponentsInChildren<ThesisPanel>();
        if (thesisPanels.Length != 3)
        {
            Debug.LogError("Thesis panel list length does not match");
        }
    }

    #region GameLogic

    /* Called by fungus block to initiate variables and gui for game */
    public void InitDebate()
    {
        // UpdateDebugText();
        if (dataController != null)
        {
            dataController.InitDebate();
            // update isProponent in flowchart menu
            flowchart.SetBooleanVariable("isProponent", dataController.IsPlayerProponent());
            Debug.Log("Update flowchart variable isProponent=" + dataController.IsPlayerProponent());
        } else
        {
            Debug.LogWarning("Cannot find reference of dataController.");
        }
    }

        /**
     * Called by fungus block as a shortcut to prepare main debate.
     */
    public void InitMainDebate()
    {
        dataController.InitMainDebate();
        flowchart.SetBooleanVariable("isProponent", dataController.IsPlayerProponent());
        Debug.Log("Set Flowchart isProponent=" + dataController.IsPlayerProponent());
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
        //Debug.Log("Enemy argument loaded into temporary argument");
        // Make argument
        // MakeArgument();
    }

    public void LoadPlayerArguments()
    {
        foreach (Argument argument in PlayerTemporaryArgumentList)
        {
            if (argument.argument != null || argument.thesis != null)
            {
                dataController.AddPlayerTemporaryArgument(argument);
                Debug.Log("Load " + argument + " to temporary argument list");
            }
        }
    }

    public void StartRound()
    {
        DrawStatusPanel();
        //Debug.Log("Play fungus start round block");
        flowchart.ExecuteBlock("StartRound");
    }

    public void EndRound()
    {
        // update debug info
        UpdateDebugText();
        UpdateFlowChartStatus();
        DrawStatusPanel();

        //Debug.Log("Check if there is winner");
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
        // Debug.Log("Update debug panel");
        DebugText.text = "Player proponent=" +  dataController.IsPlayerProponent() +
                         "\nPlayer confidence=" + dataController.GetPlayerConfidence() +
                         "\nPopularity=" + dataController.GetAudienceSupport() +
                         "\nPlayer convincingness=" + dataController.GetPlayerThesesHealth() +
                         "\nEnemy confidence=" + dataController.GetEnemyConfidence() +
                         "\nIs player round=" + dataController.IsPlayerRound();

    }


    public void UpdateRound()
    {
        //Debug.Log("Update round.");
        UpdateFlowChartStatus();
        dataController.UpdateRound();
        flowchart.SetIntegerVariable("round", dataController.GetCurrentRound());
        Debug.Log("Current round=" + dataController.GetCurrentRound());
        flowchart.SetBooleanVariable("isPlayerRound", dataController.IsPlayerRound());
        // Debug.Log("Back to interaction block");
        flowchart.ExecuteBlock("Interaction");
    }

    /**
     * Update game related status variables in the flowchart
     */
    public void UpdateFlowChartStatus()
    {
        flowchart.SetFloatVariable("support", dataController.GetAudienceSupport());
        flowchart.SetFloatVariable("health", dataController.GetPlayerFloatThesesHealth());
        flowchart.SetFloatVariable("confidence", dataController.GetPlayerConfidence());
        flowchart.SetFloatVariable("enemyHealth", dataController.GetEnemyFloatThesesHealth());
        flowchart.SetFloatVariable("enemyConfidence", dataController.GetEnemyConfidence());
        flowchart.SetIntegerVariable("round", dataController.GetCurrentRound());
        flowchart.SetBooleanVariable("isPlayerRound", dataController.IsPlayerRound());
        // Debug.Log("Flowchart stat updated");
    }

    /* Push an argument from temporary argument list, if empty, execute end round block */
    public void MakeArgument()
    {
        // flowchart.ExecuteBlock("DefaultArgument");
        // Debug.Log("Make argument");
        
        while (dataController.tempArguments.Count != 0)
        {
            Debug.Log("Call datacontroller to apply temp argument");
            string blockToExecute = dataController.ApplyTempArgument();

            flowchart.ExecuteBlock(blockToExecute);

            // Update stat after applying an argument
            UpdateFlowChartStatus();
        }
        foreach (Argument arg in PlayerTemporaryArgumentList)
        {
            arg.argument = null;
            arg.thesis = null;
            arg.fact = null;
            arg.tactic = null;
            // Debug.Log("Empty temporary argument");
        }

        //Debug.Log("Empty temporary argument, run fungus EndRound block");
        flowchart.ExecuteBlock("EndRound");

    }

    /**
     * Called by fungus blocked to add player selected thesis into record
     */
    public void AddPlayerThesis(string thesisName)
    {
        //Debug.Log("Add debater thesis to player");
        dataController.AddPlayerThesis(thesisName);
        UpdateDebugText();
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
    /**
     * Finish the current debate. Increment debate index in dataController
     */
    public void EndDebate()
    {
        Debug.Log("Debate is declared finished.");
        dataController.IncrementDebateIdx();
    }

    public void SetPlayerSide(bool isProponent)
    {
        Debug.Log("Set player side: isProponent=" + isProponent);
        if (dataController != null)
        {
            dataController.SetPlayerSide(isProponent);
        } else
        {
            Debug.LogWarning("SetPlayerSide: Reference of DataController not found");
        }
        
    }

    /**
     * Utility to update player confidence in fungus block
     */
    public void UpdatePlayerConfidence(float newValue)
    {
        //Debug.Log(newValue);
        dataController.UpdatePlayerConfidence(newValue);
        UpdateFlowChartStatus();
        DrawStatusPanel();
        Debug.Log("New player confidence = " + newValue);
    }

    /**
     *  Utility to update support in fungus block
     */
    public void UpdateSupport(float newValue)
    {
        //Debug.Log(newValue);
        dataController.UpdateSupport(newValue);
        UpdateFlowChartStatus();
        DrawStatusPanel();
        Debug.Log("New support = " + newValue);
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

    /**
     * Update status panel figures
     */
    public void DrawStatusPanel()
    {
        float playerConfidence = dataController.GetPlayerConfidence();
        float playerMaxConfidence = dataController.GetPlayerMaxConfidence();
        // float enemyMentalHealth = dataController.GetEnemyMentalHealth();
        // float enemyMaxMentalHealth = dataController.GetEnemyMaxMentalHealth();
        float playerThesisHealth = dataController.GetPlayerThesesHealth();
        float playerMaxThesisHealth = dataController.GetPlayerMaxThesesHealth();
        // float enemyThesisHealth = dataController.GetEnemyThesesHealth();
        // float enemyMaxThesisHealth = dataController.GetEnemyrMaxThesesHealth();
        float publicSupport = dataController.GetAudienceSupport();

        confidence.targetValue = playerConfidence / playerMaxConfidence;
        popularity.targetValue = publicSupport;
        convincingness.targetValue = playerThesisHealth / playerMaxThesisHealth;
        // statusPanel = FindObjectOfType<StatusPanel>();
        // statusPanel.setConfidence(playerMentalHealth);
        // statusPanel.setMaxConfidence(playerMaxMentalHealth);
        // statusPanel.setPopularity(playerThesisHealth);
        // statusPanel.setMaxPopularity(playerMaxThesisHealth);
        // statusPanel.setConvincingness(publicSupport);

    }

    public void DrawThesesMenu(bool showPlayer)
    {
        DebaterThesis[] debaterTheses = null;
        List<Tuple<ArgumentInfo, Argument>> playerArguments = new List<Tuple<ArgumentInfo, Argument>>();
        List<Tuple<ArgumentInfo, Argument>> enemyArguments = new List<Tuple<ArgumentInfo, Argument>>();
        if (dataController.GetPlayerArguments() != null)
        {
            playerArguments = dataController.GetPlayerArguments();
            Debug.Log("Arguments player made=" + playerArguments.Count);
        }
        
        if (dataController.GetEnemyArguments() != null)
        {
            enemyArguments = dataController.GetEnemyArguments();
            //Debug.Log("Arguments enemy made=" + enemyArguments.Count);
        }
        // Debug.Log("Enemy argument: " + enemyArguments[0].Item1 + " " + enemyArguments[0].Item2);

        if (showPlayer)
        {
            Debug.Log("Thesis menu[show player=True]");
            // get arguments and theses related to player
            debaterTheses = dataController.GetPlayerTheses();
        } else
        {
            Debug.Log("Thesis menu[show player=False]");
            // get arguments and theses related to enemy
            debaterTheses = dataController.GetEnemyTheses();
        }
        // Debug.Log("Length of theses = " + debaterTheses.Length);

        int i = 0;
        foreach (DebaterThesis debaterThesis in debaterTheses)
        {
            if (debaterThesis != null && debaterThesis.thesis != null)
            {
                //Debug.Log("Updating thesis '" + debaterThesis.thesis.thesisName + "'");
                // set thesis content
                thesisPanels[i].SetDebaterThesis(debaterThesis);
                thesisPanels[i].ClearArguments();

                // set argument content
                int j = 0;
                List<Tuple<ArgumentInfo, Argument>> matchArguments = playerArguments.FindAll(x => x.Item2.thesis == debaterThesis.thesis);
                //x => x.Item2.thesis != null && (x.Item2.thesis.thesisName == debaterThesis.thesis.thesisName)
                if (matchArguments == null)
                {
                    Debug.Log("Null return for playerArguments");
                }
                foreach (Tuple<ArgumentInfo, Argument> matchArgument in matchArguments)
                {
                    // cannot respond to own arguments
                    thesisPanels[i].AddArgument(matchArgument.Item1, false, !showPlayer);
                    j += 1;
                    Debug.Log("- Add argument '" + matchArgument.Item1.ArgumentName + "' into thesis, interactable = false, respondToOther="+ !showPlayer);
                }

                matchArguments = enemyArguments.FindAll(x => x.Item2.thesis == debaterThesis.thesis);
                if (matchArguments == null)
                {
                    Debug.Log("Null return for enemyArguments");
                }
                foreach (Tuple<ArgumentInfo, Argument> matchArgument in matchArguments)
                {
                    // check if the argument is interactable (unresponded enemy argument)
                    bool interactable = !playerArguments.Exists(x => x.Item1 == matchArgument.Item1);
                    if (!editMode)
                    {
                        interactable = false;
                    }
                    thesisPanels[i].AddArgument(matchArgument.Item1, interactable, showPlayer);
                    j += 1;
                    //Debug.Log("- Add argument '" + matchArgument.Item1.ArgumentName + "' into thesis, interactable = " + interactable + ", respondToOther=" + showPlayer);
                }
                //Debug.Log("Total argument added to this thesis = " + j);
            }
            thesisPanels[i].UpdateArgumentListDisplay();
            i += 1;
        }
    }

    public void DrawOverviewMenu()
    {
        //audiencePanel = FindObjectOfType<AudiencePanel>();
        //audiencePanel.setIdeologySlider(defaultAudience);
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

        /**
         * Open Theses menu through Action menu
         * ThesisInfoBtn and ArgumentInfoBtn are interactable in this method
         */
    public void OnClickedToggleThesesBtn()
    {
        //Debug.Log("Toggle thesis menu from action menu, enable interaction");
        ThesesGUI.gameObject.SetActive(!ThesesGUI.gameObject.activeSelf);
        if (ThesesGUI.gameObject.activeSelf)
        {
            editMode = true;
            DrawThesesMenu(true);
            foreach (ThesisPanel thesisPanel in thesisPanels)
            {
                thesisPanel.EnableInteraction();
            }
        }
    }

    /**
     * Simply toggle thesis menu without handling interaction
     */
    public void ToggleThesesMenu()
    {
        editMode = false;
        DrawThesesMenu(true);
        ThesesGUI.gameObject.SetActive(!ThesesGUI.gameObject.activeSelf);
    }

    public void OnClickedToggleActionBtn()
    {
        //Debug.Log("Toggle argument menu");
        ActionGUI.gameObject.SetActive(!ActionGUI.gameObject.activeSelf);
        if (ActionGUI.gameObject.activeSelf)
        {
            ClearActionMenuArguments();
        }
    }

    public void OnClickedToggleOverviewBtn()
    {
        //Debug.Log("Toggle info menu");
        OverviewGUI.gameObject.SetActive(!OverviewGUI.gameObject.activeSelf);
        DrawOverviewMenu();
    }

    public void OnClickedConserveBtn()
    {
        Debug.Log("Converse button is clicked");
    }

    /* Argument menu */

    public void OnClickedMakeArgumentsBtn()
    {
        
        // Load items into temporary arguments
        if (argumentBtns == null)
        {
            argumentBtns = actionListPanel.GetComponentsInChildren<ArgumentButton>();
        }
        int i = 0;
        foreach (ArgumentButton argumentButton in argumentBtns)
        {
            if (argumentButton.thesis != null || argumentButton.arg != null)
            {
                // only argument bar with respond to field is added to argument list.
                PlayerTemporaryArgumentList[i].tactic = argumentButton.tactic;
                PlayerTemporaryArgumentList[i].fact = argumentButton.fact;
                PlayerTemporaryArgumentList[i].argument = argumentButton.arg;
                PlayerTemporaryArgumentList[i].thesis = argumentButton.thesis;
                i += 1;
            }
        }
        Debug.Log("Make arguments="+i);
        if (i == 0)
        {
            // no action if no concrete argument
            toggleAlertDialog("No valid argument is made");
            return;
        } else if (i > 1)
        {
            // TODO: support multiple arguments
            // For now, an alert is shown if more than 1 argument is made.
            toggleAlertDialog("Multiple arguments are not supported in current level.");
            return;
        }

        // validate argument in tutorial mode
        if (dataController.IsTutorial())
        {
            // TODO: No hardcode alert
            if (dataController.GetCurrentRound() == 1)
            {
               if (PlayerTemporaryArgumentList[0].thesis == null || PlayerTemporaryArgumentList[0].thesis.thesisName != "SmartphoneIsBeneficialToTeenagerDevelopment")
                {
                    toggleAlertDialog("Please make argument on your own thesis");
                    return;
                }
                if (PlayerTemporaryArgumentList[0].fact == null || PlayerTemporaryArgumentList[0].fact.factName != "MobileProductivity")
                {
                    toggleAlertDialog("Please select the correct fact");
                    return;
                }
            }
            else if (dataController.GetCurrentRound() == 3)
            {
                if (PlayerTemporaryArgumentList[0].argument == null || PlayerTemporaryArgumentList[0].argument.ArgumentName != "YouAreAddictedAsWell")
                {
                    toggleAlertDialog("Please make argument on enemy argument against your thesis");
                    return;
                }
                if (PlayerTemporaryArgumentList[0].tactic == null || PlayerTemporaryArgumentList[0].tactic.tacticName != "GuiltByAssociation")
                {
                    toggleAlertDialog("Please select the tactic that the enemy argument used");
                    return;
                }
                if (PlayerTemporaryArgumentList[0].fact != null)
                {
                    toggleAlertDialog("Fact is not needed for this argument");
                    return;
                }
            }
        }

        LoadPlayerArguments();
        if (fireArgumentSound != null)
        {
            fireArgumentSound.Play();
        }
        // close action menu
        OnClickedToggleActionBtn();
        // Make argument
        MakeArgument();

    }

    public void ClearActionMenuArguments()
    {
        //Debug.Log("Action list panel" + actionListPanel);
        if (argumentBtns == null)
        {
            argumentBtns = actionListPanel.GetComponentsInChildren<ArgumentButton>();
        }
        foreach (ArgumentButton argumentButton in argumentBtns)
        {
            argumentButton.Clear();
        }
    }

    public void toggleAlertDialog(string alertText = "")
    {
        if (AlertDialog != null)
        {
            
            AlertDialog.gameObject.SetActive(!AlertDialog.gameObject.activeSelf);
            TextMeshProUGUI warningText = AlertDialog.gameObject.GetComponentInChildren<TextMeshProUGUI>();

            if (warningText != null)
            {
                warningText.text = alertText;
            } else
            {
                Debug.LogError("Cannot find TextMeshProUGUI component in AlertDialog");
            }
        }
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

    public void OnClickedArgumentInfo(ArgumentInfo argumentInfo)
    {
        if (argumentBtns == null)
        {
            argumentBtns = actionListPanel.GetComponentsInChildren<ArgumentButton>();
        }
        foreach (ArgumentButton arg in argumentBtns)
        {
            if (!arg.IsInteractable())
            {
                arg.getArgumentInfo(argumentInfo);
                arg.updateSubActionBtn();
            }
        }
        Debug.Log("Change active temporary argument in UI");
        ToggleThesesMenu();
    }

    /* Thesis menu */

    public void OnClickedToggleDebaterBtn(bool showPlayer)
    {
        DrawThesesMenu(!showPlayer);
    }

    public void OnClickedThesisBtn(Thesis thesis)
    {
        if (actionListPanel == null)
        {
            actionListPanel = GameObject.FindObjectOfType<ActionListPanel>();
        }
        if (argumentBtns == null)
        {
            argumentBtns = actionListPanel.GetComponentsInChildren<ArgumentButton>();
        }
        foreach (ArgumentButton arg in argumentBtns)
        {
            if (!arg.IsInteractable())
            {
                arg.getThesis(thesis);
                arg.updateSubActionBtn();
            }
        }
        //Debug.Log("Add this thesis to active temp argument menu, close thesis menu and toggle argument menu");
        ToggleThesesMenu();
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
