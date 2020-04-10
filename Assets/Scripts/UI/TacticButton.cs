using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TacticButton : MonoBehaviour
{
    public Tactic tacticDisplayed;
    public bool isSelected;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponentInChildren<Text>().text = tacticDisplayed.tacticType + ": " + tacticDisplayed.tacticName;
    }

    public void OnClicked()
    {
        TacticsMenu tacticMenu = gameObject.GetComponentInParent<TacticsMenu>();
        tacticMenu.DisplayTacticDetail(tacticDisplayed);
    }
}
