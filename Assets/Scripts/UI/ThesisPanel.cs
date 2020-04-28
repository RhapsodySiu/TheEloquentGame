﻿using System.Collections;
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetDebaterThesis(DebaterThesis debaterThesis)
    {
        this.debaterThesis = debaterThesis;
        thesisInfoBtn.SetThesisInfo(debaterThesis);
    }

    public void AddArgument(ArgumentInfo argumentInfo, bool interactable, bool respondtToOther)
    {
        ArgumentInfoBtnData argumentInfoBtnData;
        argumentInfoBtnData.argumentInfo = argumentInfo;
        argumentInfoBtnData.interactable = interactable;
        argumentInfoBtnData.respondToOther = respondtToOther;
        argumentInfoBtnDatas.Add(argumentInfoBtnData);
        UpdateArgumentListDisplay();
    }

    public void DeleteArgument(ArgumentInfo argumentInfo)
    {
        bool found = false;
        int idxToRemove = 0;
        for (int i = 0; i < argumentInfoBtnDatas.Count; ++i)
        {
            if (argumentInfoBtnDatas[i].argumentInfo == argumentInfo)
            {
                found = true;
                idxToRemove = i;
                break;
            }
        }
        if (found)
        {
            argumentInfoBtnDatas.RemoveAt(idxToRemove);
            UpdateArgumentListDisplay();
        }
    }

    /**
     * Update the display of argument list
     * Destroy all objects within argument panel and create new buttons from argumentInfoList information
     * Should use pooling for better performance
     */
    public void UpdateArgumentListDisplay()
    {
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
        foreach (Transform child in thesisArgumentsPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}