using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class TacticsMenu : MonoBehaviour
{
    public Text tacticNameDisplay;
    public Text tacticDescriptionDisplay;
    public void DisplayTacticDetail(Tactic tactic)
    {
        // TODO: Highlight selected button
        tacticNameDisplay.text = Regex.Replace(tactic.tacticName, "(\\B[A-Z])", " $1");
        tacticDescriptionDisplay.text = tactic.description;
    }
}
