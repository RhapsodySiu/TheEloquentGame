using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThesisInfoBtn : MonoBehaviour
{
    public Thesis thesis;
    public bool interactable;

    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = gameObject.GetComponent<Button>();
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
        gameObject.GetComponentInChildren<Text>().text = thesis.GetDescription("en") + "\n" + debaterThesis.currentHealth + "/" + debaterThesis.maxHealth;
        Debug.Log("Set thesis text successfully");
    }

    public void OnClick()
    {
        // If clicked, set the thesis in action menu to the thesis this btn holds.
    }
}
