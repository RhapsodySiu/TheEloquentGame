using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TacticButton : MonoBehaviour
{
    public Tactic tacticDisplayed;
    private ActionListPanel actionlistpanel;
    private Component[] argumentbtn;
    private DebateModeController debateModeController;
    // Start is called before the first frame update
    void Start()
    {
        actionlistpanel = Object.FindObjectOfType<ActionListPanel>();
        argumentbtn = actionlistpanel.GetComponentsInChildren<ArgumentButton>();
        debateModeController = GameObject.FindObjectOfType<DebateModeController>();
        gameObject.GetComponentInChildren<Text>().text = tacticDisplayed.tacticType + ": " + tacticDisplayed.tacticName;
    }

    public void OnClicked()
    {
        foreach(ArgumentButton arg in argumentbtn)
        {
           if(!arg.IsInteractable())
            {
                arg.getTactic(tacticDisplayed);
                arg.updateSubActionBtn();
                break;
            }

        }

        debateModeController.toggleTacticMenu();
    }

    public void setTacticText(){
        TacticsMenu tacticMenu = gameObject.GetComponentInParent<TacticsMenu>();
        tacticMenu.DisplayTacticDetail(tacticDisplayed);
    }
}
