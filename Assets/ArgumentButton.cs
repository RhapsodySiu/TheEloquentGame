using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArgumentButton : MonoBehaviour
{
    public Button btn;
    public ActionListPanel actionlistpanel;
    public Sprite highlight;
    public Sprite dehighlight;


    public SwitchSubActionBtn sub1;
    public SwitchSubActionBtn sub2;
    public SwitchSubActionBtn sub3;
    private string respondName;
    private string tacticName;
    private string factName;

    public ArgumentInfo arg;
    public Thesis thesis;
    public Tactic tactic;
    public Fact fact;
    // Start is called before the first frame update
    void Start()
    {
        respondName = "None";
        tacticName = "None";
        factName = "None";
        actionlistpanel = Object.FindObjectOfType<ActionListPanel>();
        btn = transform.GetComponent<Button>();
        setArgumentText();
        btn.onClick.AddListener(Highlight);
        btn.onClick.AddListener(updateSubActionBtn);

    }

    // Update is called once per frame
    void Update()
    {
        setArgumentText();

    }

    public void setArgumentText(){
        Text txt = transform.Find("Text").GetComponent<Text>();
        txt.text = "Argument to respond: " + respondName + "\n Tactic: " + tacticName + "\n Fact: " + factName;
    }

    public void Highlight(){
        transform.GetComponent<Image>().sprite = highlight;
        btn.interactable = false;
        actionlistpanel.Dehighlight(btn.name);
    }

    public void Dehighlight(){
        transform.GetComponent<Image>().sprite = dehighlight;
        btn.interactable = true;
        Debug.LogError("dehighlighting");
    }

    public void updateSubActionBtn(){
        sub1.setText(respondName);
        sub2.setText(tacticName);
        sub3.setText(factName);
    }

    public void getTactic(Tactic tactic){
        this.tactic = tactic;
        this.tacticName = tactic.tacticName;
    }

    public void getFact(Fact fact){
        this.fact = fact;
        this.factName = fact.factName;
    }

    public void getThesis(Thesis thesis){
        this.thesis = thesis;
        this.arg = null;
        this.respondName = thesis.thesisName;
    }

    public void getArgumentInfo(ArgumentInfo argument){
        this.thesis = null;
        this.arg = argument;
        this.respondName = argument.ArgumentName;
    }
}
