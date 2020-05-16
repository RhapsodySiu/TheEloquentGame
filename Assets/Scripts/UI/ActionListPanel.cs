using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ActionListPanel : MonoBehaviour
{
    public ArgumentButton Arg1;
    public ArgumentButton Arg2;
    public ArgumentButton Arg3;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Dehighlight(string name){
        switch(name){
            case "Arg1":
                {
                    Arg2.Dehighlight();
                    Arg3.Dehighlight();
                    // Debug.Log("dehighlighting 2 and 3");
                 }
                 break;
            case "Arg2":
                {
                    Arg3.Dehighlight();
                    Arg1.Dehighlight();
                    // Debug.Log("dehighlighting 1 and 3");
                 }
                 break;
            case "Arg3":
                {
                    Arg2.Dehighlight();
                    Arg1.Dehighlight();
                    // Debug.Log("dehighlighting 1 and 2");
                 }
                 break;
        }

    }
}
