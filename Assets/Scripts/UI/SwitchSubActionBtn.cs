using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchSubActionBtn : MonoBehaviour
{
    // Start is called before the first frame update
    private Button btn;
    private DebateModeController controller;
    // private DataController data;
    void Start()
    {
        btn = gameObject.GetComponent<Button>();
        controller = Object.FindObjectOfType<DebateModeController>();
        // data = Object.FindObjectOfType<DataController>();
        if (btn == null)
        {
            Debug.LogError("SwitchSubActionBtn: cannot find button component");
        }
    }

    public void setText(string t)
    {
        Text txt = transform.Find("Text").GetComponent<Text>();
        txt.text = t;
    }


    public void setFieldText(Argument argument, string option)
    {
        switch (option)
        {
            case "THESES":
                if (argument.thesis != null) setText(argument.thesis.thesisName);
                else setText(argument.argument.ArgumentName);
                break;
            case "TACTIC":
                if (argument.tactic != null) setText(argument.tactic.tacticName);
                break;
            case "FACT":
                if (argument.fact != null) setText(argument.fact.factName);
                break;
        }
    }
    public void SetRedirectTo(PreviewSubActionPanel.ToggleMenuOption option)
    {
        switch (option)
        {
            case PreviewSubActionPanel.ToggleMenuOption.THESES:
                transform.GetComponent<Button>().onClick.AddListener(delegate { controller.OnClickedToggleThesesBtn(); });
                break;
            case PreviewSubActionPanel.ToggleMenuOption.TACTIC:
                transform.GetComponent<Button>().onClick.AddListener(delegate { controller.toggleTacticMenu(); });
                break;
            default:
                transform.GetComponent<Button>().onClick.AddListener(delegate { controller.toggleFactMenu(); });
                return;
        }
    }

}