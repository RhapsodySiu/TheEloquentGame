using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FactButton : MonoBehaviour
{
    public Fact factDisplayed;
    private ActionListPanel actionlistpanel;
    private Component[] argumentbtn;
    private DebateModeController debateModeController;
    // Start is called before the first frame update
    void Start()
    {
        actionlistpanel = GameObject.FindObjectOfType<ActionListPanel>();
        argumentbtn = actionlistpanel.GetComponentsInChildren<ArgumentButton>();
        debateModeController = GameObject.FindObjectOfType<DebateModeController>();
        gameObject.GetComponentInChildren<Text>().text = factDisplayed.factName;
    }

    public void OnClicked()
    {
        foreach(ArgumentButton arg in argumentbtn)
        {
           if(!arg.IsInteractable())
            {
                arg.getFact(factDisplayed);
                arg.updateSubActionBtn();
                break;
            }
        }
        debateModeController.toggleFactMenu();

    }

    public void setFactText(){
        FactsMenu factMenu = gameObject.GetComponentInParent<FactsMenu>();
        factMenu.DisplayFactDetail(factDisplayed);
    }
}
