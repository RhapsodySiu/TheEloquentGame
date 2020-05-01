using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusPanel : MonoBehaviour
{
    public Speedometer confidence;
    public Speedometer popularity;
    public Speedometer convincingness;

    // Update is called once per frame
    public void setConfidence(float i){
        confidence.settargetValue(i);
    }

    public void setMaxConfidence(float i){
        confidence.setMaxValue(i);
    }

    public void setPopularity(float i){
        popularity.settargetValue(i);
    }

    public void setMaxPopularity(float i){
        popularity.setMaxValue(i);
    }
    public void setConvincingness(float i){
        convincingness.settargetValue(i);
    }

    public void setMaxConvincingness(float i){
        convincingness.setMaxValue(i);
    }
}
