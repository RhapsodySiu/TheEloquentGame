using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArgumentInfoBtn : MonoBehaviour
{

    public ArgumentInfo argumentInfo;
    public Sprite enemyArgumentImage;
    public Sprite defaultArgumentImage;
    private bool interactable;
    // whether the argument is against another debater thesis
    private bool respondToOther;
    private DebateModeController debateModeController;
    private Button button;
    // Start is called before the first frame update
    void Start()
    {
        button = gameObject.GetComponent<Button>();
        debateModeController = FindObjectOfType<DebateModeController>();
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
        if (interactable)
        {
            debateModeController.OnClickedArgumentInfo(argumentInfo);
        } else
        {
            Debug.Log("Cannot add argument info as it is NOT interactable");
        }
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
            button.GetComponent<Image>().sprite = enemyArgumentImage;
        } else
        {
            button.GetComponent<Image>().sprite = defaultArgumentImage;
        }
    }

    public void SetInteractable(bool interactable)
    {
        this.interactable = interactable;
        button.interactable = this.interactable;
    }
}
