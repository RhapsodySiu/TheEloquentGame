using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class StoryController : MonoBehaviour
{
    private DataController dataController;
    // public Flowchart flowchart;
    // Start is called before the first frame update
    void Start()
    {
        dataController = FindObjectOfType<DataController>();
    }

    /**
     * Utility function to set player side in dataController.debateList[debateIdx] in story mode
     */
    public void SetPlayerSide(bool isProponent)
    {
        dataController.SetPlayerSide(isProponent);
        Debug.Log("Set player proponent = " + isProponent);
    }
    
}
