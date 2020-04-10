using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsListPanel : MonoBehaviour
{
    public List<Tactic> tacticsList;
    public GameObject tacticBtnPrefab;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Tactic tactic in tacticsList)
        {
            GameObject btn = Instantiate(tacticBtnPrefab);
            btn.GetComponent<TacticButton>().tacticDisplayed = tactic;
            btn.transform.parent = gameObject.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
