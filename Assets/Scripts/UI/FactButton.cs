using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FactButton : MonoBehaviour
{
    private ActionListPanel actionlistpanel;
    public Fact factDisplayed;
    public Component[] argumentbtn;
    // Start is called before the first frame update
    void Start()
    {
        actionlistpanel = GameObject.FindObjectOfType<ActionListPanel>();
        argumentbtn = actionlistpanel.GetComponentsInChildren<ArgumentButton>();
        gameObject.GetComponentInChildren<Text>().text = factDisplayed.factName;
    }

    public void OnClicked()
    {
        foreach(ArgumentButton arg in argumentbtn){
           if(arg.btn.interactable == false){
            arg.getFact(factDisplayed);
            arg.updateSubActionBtn();
            }
        }
    }

    public void setFactText(){
        FactsMenu factMenu = gameObject.GetComponentInParent<FactsMenu>();
        factMenu.DisplayFactDetail(factDisplayed);
    }
}
