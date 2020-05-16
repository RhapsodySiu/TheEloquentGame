using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArgumentButton : MonoBehaviour
{
    private Button btn;
    private ActionListPanel actionlistpanel;
    public Sprite highlight;
    public Sprite dehighlight;

    public SwitchSubActionBtn sub1;
    public SwitchSubActionBtn sub2;
    public SwitchSubActionBtn sub3;
    private Text txt;
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
        txt = transform.Find("Text").GetComponent<Text>();
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

    public void setArgumentText()
    {
        string newText = "";
        if (respondName != "None" && respondName != null)
        {
            newText += "Argument to respond: " + respondName;
        }
        if (tacticName != "None" && tacticName != null)
        {
            newText += "\n Tactic: " + tacticName;
        }
        if (factName != "None" && factName != null)
        {
            newText += "\n Fact: " + factName;
        }
        txt.text = newText;
    }

    public void Highlight(){
        transform.GetComponent<Image>().sprite = highlight;
        btn.interactable = false;
        actionlistpanel.Dehighlight(btn.name);
    }

    public void Dehighlight(){
        transform.GetComponent<Image>().sprite = dehighlight;
        if (btn == null)
        {
            btn = transform.GetComponent<Button>();
        }
        btn.interactable = true;
        // Debug.Log("dehighlighting");
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

    public bool IsInteractable()
    {
        if (btn == null)
        {
            btn = transform.GetComponent<Button>();
        }
        return this.btn.interactable;
    }

    public void Clear()
    {
        respondName = "None";
        tacticName = "None";
        factName = "None";
        tactic = null;
        fact = null;
        arg = null;
        thesis = null;
        updateSubActionBtn();
    }
}
