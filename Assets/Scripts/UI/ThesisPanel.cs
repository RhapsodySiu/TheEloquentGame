using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ThesisPanel : MonoBehaviour
{
    public struct ArgumentInfoBtnData
    {
        public ArgumentInfo argumentInfo;
        public bool interactable;
        public bool respondToOther;
    }

    public DebaterThesis debaterThesis;

    // arguments data related to the given thesis, bool represents whether the argument is from other debater
    public List<ArgumentInfoBtnData> argumentInfoBtnDatas = new List<ArgumentInfoBtnData>();

    private ThesisInfoBtn thesisInfoBtn;

    private ThesisArgumentsPanel thesisArgumentsPanel;

    public GameObject argumentInfoBtnPrefab;

    // Start is called before the first frame update
    void Start()
    {
        argumentInfoBtnDatas.Clear();
        thesisInfoBtn = gameObject.GetComponentInChildren<ThesisInfoBtn>();
        thesisArgumentsPanel = gameObject.GetComponentInChildren<ThesisArgumentsPanel>();

        if (thesisInfoBtn == null || thesisArgumentsPanel == null)
        {
            Debug.LogError("Cannot find reference of thesis info btn or thesis arguments panel in the children");
        }

        if (argumentInfoBtnPrefab == null)
        {
            Debug.LogError("Reference of argumentInfoBtn Prefab not found.");
        }

        if (debaterThesis != null)
        {
            thesisInfoBtn.SetThesisInfo(debaterThesis);
        }

        UpdateArgumentListDisplay();
    }

    /**
     * Decide which button(s) is/are interactable.
     * Thesis info buttons are always interactable (at least in this demo)
     * For arguments that are not yet responded, they are interactable
     * Player cannot respond to their own arguments
     */
    public void EnableInteraction()
    {
        if (thesisInfoBtn == null)
        {
            thesisInfoBtn = gameObject.GetComponentInChildren<ThesisInfoBtn>();
        }
        
        thesisInfoBtn.interactable = true;

        thesisArgumentsPanel.UpdateArgumentInteraction();
    }

    public void DisableInteraction()
    {
        if (thesisInfoBtn == null)
        {
            thesisInfoBtn = gameObject.GetComponentInChildren<ThesisInfoBtn>();
        }
        thesisInfoBtn.interactable = true;

        thesisArgumentsPanel.DisableInteraction();
    }

    public void SetDebaterThesis(DebaterThesis debaterThesis)
    {
        this.debaterThesis = debaterThesis;
        if (thesisInfoBtn == null)
        {
            thesisInfoBtn = gameObject.GetComponentInChildren<ThesisInfoBtn>();
        }

        thesisInfoBtn.SetThesisInfo(debaterThesis);
    }

    public void AddArgument(ArgumentInfo argumentInfo, bool interactable, bool respondtToOther)
    {
        // Check if no duplicate argument info, if already exist, return
        //foreach (ArgumentInfoBtnData data in argumentInfoBtnDatas)
        //{
        //    if (data.argumentInfo.ArgumentName == argumentInfo.ArgumentName)
        //    {
        //        Debug.Log("Encounter repeat argument info, skip drawing argument info butotn");
        //        return;
        //    }
        //}
        ArgumentInfoBtnData argumentInfoBtnData;
        argumentInfoBtnData.argumentInfo = argumentInfo;
        argumentInfoBtnData.interactable = interactable;
        argumentInfoBtnData.respondToOther = respondtToOther;
        argumentInfoBtnDatas.Add(argumentInfoBtnData);
        // UpdateArgumentListDisplay();
    }

    //public void DeleteArgument(ArgumentInfo argumentInfo)
    //{
    //    bool found = false;
    //    int idxToRemove = 0;
    //    for (int i = 0; i < argumentInfoBtnDatas.Count; ++i)
    //    {
    //        if (argumentInfoBtnDatas[i].argumentInfo == argumentInfo)
    //        {
    //            found = true;
    //            idxToRemove = i;
    //            break;
    //        }
    //    }
    //    if (found)
    //    {
    //        argumentInfoBtnDatas.RemoveAt(idxToRemove);
    //        UpdateArgumentListDisplay();
    //    }
    //}

    /**
     * Update the display of argument list
     * Destroy all objects within argument panel and create new buttons from argumentInfoList information
     * Should use pooling for better performance
     */
    public void UpdateArgumentListDisplay()
    {
        Debug.Log("Argument(s) to add = " + argumentInfoBtnDatas.Count);
        foreach ( ArgumentInfoBtnData argumentInfoBtnData in argumentInfoBtnDatas)
        {
            GameObject btn = Instantiate(argumentInfoBtnPrefab);
            btn.GetComponent<ArgumentInfoBtn>().SetArgumentInfo(argumentInfoBtnData.argumentInfo);
            btn.GetComponent<ArgumentInfoBtn>().SetRespondToOther(argumentInfoBtnData.respondToOther);
            btn.GetComponent<ArgumentInfoBtn>().SetInteractable(argumentInfoBtnData.interactable);
            btn.transform.SetParent(thesisArgumentsPanel.transform);
            btn.transform.localPosition = Vector3.zero;
            btn.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }

    }

    public void ClearArguments()
    {
        if (thesisArgumentsPanel == null)
        {
            thesisArgumentsPanel = gameObject.GetComponentInChildren<ThesisArgumentsPanel>();
        }
        Debug.Log("Original argument size = " + argumentInfoBtnDatas.Count);
        argumentInfoBtnDatas.Clear();
        int i = 0;
        foreach (Transform child in thesisArgumentsPanel.transform)
        {
            i += 1;
            GameObject.Destroy(child.gameObject);
        }
        Debug.Log("Destroy child game object = " + i);
    }
}
