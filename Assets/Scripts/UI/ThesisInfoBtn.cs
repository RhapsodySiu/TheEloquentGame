using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThesisInfoBtn : MonoBehaviour
{
    public Thesis thesis;
    public bool interactable;

    private Button button;
    private DebateModeController debateModeController;

    // Start is called before the first frame update
    void Start()
    {
        button = gameObject.GetComponent<Button>();
        debateModeController = GameObject.FindObjectOfType<DebateModeController>();
        if (thesis != null)
        {
            gameObject.GetComponentInChildren<Text>().text = thesis.GetDescription("en");
        }
    }

    void Update()
    {
        button.interactable = interactable;
    }

    public void SetThesisInfo(DebaterThesis debaterThesis)
    {
        thesis = debaterThesis.thesis;
        // gameObject.GetComponentInChildren<Text>().text = thesis.GetDescription("en") + "\n" + debaterThesis.currentHealth + "/" + debaterThesis.maxHealth;
        gameObject.GetComponentInChildren<Text>().text = thesis.GetDescription("en");
        // Debug.Log("Set thesis text successfully");
    }

    public void OnClick()
    {
        // only toggle action menu when interactable
        if (interactable && thesis != null)
        {
            Debug.Log("Ask debateModeController to add thesis to action menu");
            debateModeController.OnClickedThesisBtn(thesis);
        } else
        {
            Debug.Log("Not interactable or null thesis, ignore click");
        }
    }
}
