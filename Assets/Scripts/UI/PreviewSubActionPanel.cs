using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Show the subaction made by player and redirect them to specific menu when clicked
public class PreviewSubActionPanel : MonoBehaviour
{
    // Start is called before the first frame update
    private SwitchSubActionBtn displayBtn;
    public enum ToggleMenuOption
    {
        THESES,
        TACTIC,
        FACT
    }
    public ToggleMenuOption selectFrom;

    void Start()
    {
        displayBtn = GetComponentInChildren<SwitchSubActionBtn>();
        if (displayBtn == null)
        {
            Debug.LogError("PreviewSubActionPanel: Cannot find SwitchSubActionBtn child component");
        } else
        {
            displayBtn.SetRedirectTo(selectFrom);
        }
        //Debug.Log("Listener added");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
