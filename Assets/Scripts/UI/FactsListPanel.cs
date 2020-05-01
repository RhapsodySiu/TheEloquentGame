using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactsListPanel : MonoBehaviour
{
    public List<Fact> factsList;
    public GameObject factBtnPrefab;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Fact fact in factsList)
        {
            GameObject btn = Instantiate(factBtnPrefab);
            btn.GetComponent<FactButton>().factDisplayed = fact;
            btn.transform.parent = gameObject.transform;
            Debug.Log("make fact button");

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
