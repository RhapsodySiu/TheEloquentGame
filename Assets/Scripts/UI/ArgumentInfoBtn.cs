using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArgumentInfoBtn : MonoBehaviour
{

    public ArgumentInfo argumentInfo;
    private bool interactable;
    // whether the argument is against another debater thesis
    private bool respondToOther;

    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        button = gameObject.GetComponent<Button>();
        if (argumentInfo != null)
        {
            gameObject.GetComponentInChildren<Text>().text = argumentInfo.GetDescription("en");
        }
    }

    // Update is called once per frame
    void Update()
    {
        button.interactable = interactable;
    }

    public void SetArgumentInfo(ArgumentInfo argumentInfo)
    {
        this.argumentInfo = argumentInfo;
        gameObject.GetComponentInChildren<Text>().text = argumentInfo.GetDescription("en");
    }

    public void OnClick()
    {
        // when clicked, pass the argumentInfo it holds to action menu argument field and close menu
    }

    public void SetRespondToOther(bool respondToOther)
    {
        this.respondToOther = respondToOther;
        if (button == null)
        {
            button = gameObject.GetComponent<Button>();
        }
        if (respondToOther)
        {
            button.image.color = new Color(1.0f, 0.95f, 0.95f);
        } else
        {
            button.image.color = Color.white;
        }
    }

    public void SetInteractable(bool interactable)
    {
        this.interactable = interactable;
        button.interactable = this.interactable;
    }
}
