using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TacticButton : MonoBehaviour
{
    private ActionListPanel actionlistpanel;
    public Tactic tacticDisplayed;
    public Component[] argumentbtn;
    // Start is called before the first frame update
    void Start()
    {
        actionlistpanel = Object.FindObjectOfType<ActionListPanel>();
        argumentbtn = actionlistpanel.GetComponentsInChildren<ArgumentButton>();
        gameObject.GetComponentInChildren<Text>().text = tacticDisplayed.tacticType + ": " + tacticDisplayed.tacticName;
    }

    public void OnClicked()
    {
        foreach(ArgumentButton arg in argumentbtn){
           if(arg.btn.interactable == false){
            arg.getTactic(tacticDisplayed);
            arg.updateSubActionBtn();
            }
        }

        //update argument with
    }

    public void setTacticText(){
        TacticsMenu tacticMenu = gameObject.GetComponentInParent<TacticsMenu>();
        tacticMenu.DisplayTacticDetail(tacticDisplayed);
    }
}
