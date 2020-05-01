using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class FactsMenu : MonoBehaviour
{
    public Text factNameDisplay;
    public Text factDescriptionDisplay;
    public void DisplayFactDetail(Fact fact)
    {
        // TODO: Highlight selected button
       // factNameDisplay.text = Regex.Replace(fact.factName, "(\\B[A-Z])", " $1");
        factNameDisplay.text = fact.factName;
        factDescriptionDisplay.text = fact.description;
    }
}
