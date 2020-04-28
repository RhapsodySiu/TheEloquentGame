using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchThesisStandBtn : MonoBehaviour
{

    public Sprite enemyNormal;
    public Sprite enemyHighlight;
    public Sprite playerNormal;
    public Sprite playerHighlight;

    public bool isPlayerSide = false;

    private Button button;
    private SpriteState spriteState;
    private DebateModeController debateModeController;

    // Start is called before the first frame update
    void Start()
    {
        button = gameObject.GetComponent<Button>();
        spriteState = button.spriteState;
        debateModeController = FindObjectOfType<DebateModeController>();

        if (button == null)
        {
            Debug.LogError("Cannot find button component");
        }
        if (enemyNormal == null || enemyHighlight == null || playerNormal == null || playerHighlight == null)
        {
            Debug.LogError("Cannot find reference of some sprites");
        }
        if (debateModeController == null)
        {
            Debug.Log("Cannot find reference of debate mode controller");
        }
        ToggleButton();
    }

    public void ToggleButton()
    {
        debateModeController.OnClickedToggleDebaterBtn(isPlayerSide);
        isPlayerSide = !isPlayerSide;
        if (isPlayerSide)
        {
            button.image.sprite = playerNormal;
            spriteState.highlightedSprite = playerHighlight;
        } else
        {
            button.image.sprite = enemyNormal;
            spriteState.highlightedSprite = enemyHighlight;
        }

        button.spriteState = spriteState;
        Debug.Log("Switch thesis side");
    }
}
