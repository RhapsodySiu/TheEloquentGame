using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudiencePanel : MonoBehaviour
{
    public Slider Eudaimonia;
    public Slider LifePassion;
    public Slider Religion;
    public Slider Desire;
    public Slider Reality;
    public Slider Economy;
    public Slider Nation;
    public Slider Politic;
    public Slider World;
    public Slider Society;
    public Slider Gender;
    public Slider Race;
    public Slider Conduct;
    public Slider Id;
    public Slider Hierarchy;
    public Slider Diplomacy;
    public Slider Nature;
    public Slider Knowledge;
    public Slider Altitude;

    public void setIdeologySlider(Audience audience){
        Eudaimonia.value = audience.ideology.eudaimoniaView;
        LifePassion.value = audience.ideology.lifePassionView;
        Religion.value = audience.ideology.religionView;
        Desire.value = audience.ideology.desireView;
        Reality.value = audience.ideology.realityView;
        Economy.value = audience.ideology.economyView;
        Nation.value = audience.ideology.nationView;
        Politic.value = audience.ideology.politicView;
        World.value = audience.ideology.worldView;
        Society.value = audience.ideology.societyView;
        Gender.value = audience.ideology.genderView;
        Race.value = audience.ideology.raceView;
        Conduct.value = audience.ideology.conductView;
        Id.value = audience.ideology.idView;
        Hierarchy.value = audience.ideology.hierarchyView;
        Diplomacy.value = audience.ideology.diplomacyView;
        Nature.value = audience.ideology.natureView;
        Knowledge.value = audience.ideology.knowledgeView;
        Altitude.value = audience.ideology.altitude;
    }
}
