using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class GameController : MonoBehaviour
{
    private DataController dataController;
    public Flowchart flowchart;
    // Start is called before the first frame update
    void Start()
    {
        dataController = FindObjectOfType<DataController>();
    }

    
}
