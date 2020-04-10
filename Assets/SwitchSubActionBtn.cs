using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchSubActionBtn : MonoBehaviour
{
    // Start is called before the first frame update
    private Button btn;
    private DebateModeController controller;
    void Start()
    {
        controller = Object.FindObjectOfType<DebateModeController>();
        btn = transform.GetComponent<Button>();
        if (btn == null)
        {
            Debug.LogError("SwitchSubActoinBtn: cannot find button component");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
