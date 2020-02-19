using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IdeologySpectrum
{
    /* Usally, -1.0 indicates extreme left-wing, 1.0 indicates extreme right-wing
     * It refers to the wiki definition:  
     * the left-wing is characterized by an emphasis on "ideas such as freedom, equality, fraternity, rights, progress, reform and internationalism" 
     * while the right-wing is characterized by an emphasis on "notions such as authority, hierarchy, order, duty, tradition,.
     * 
     * For ideas that cannot be categorized into these two groups, addtional comment is given.
     */

    [Header("幸福觀：道德價值vs物質利益 Metaphysical vs Material")]
    [Range(-1f, 1f)]
    public float eudaimoniaView;

    [Header("人生觀：沙文主義vs犬儒主義 Passionate vs Phlegmatic")]
    [Range(-1f, 1f)]
    public float lifePassionView;

    [Header("信仰觀：人文vs宗教 Humanism vs Religion")]
    [Range(-1f, 1f)]
    public float religionView;

    [Header("娛樂觀：享樂vs禁欲 Hedonism vs Asceticism")]
    [Range(-1f, 1f)]
    public float desireView;

    [Header("現實觀：理想vs現實 Idealism vs Realism")]
    [Range(-1f, 1f)]
    public float realityView;

    [Header("經濟觀：共產vs資本 Communism vs Capitalism")]
    [Range(-1f, 1f)]
    public float economyView;

    [Header("國家觀：世界主義vs愛國主義 Cosmopolitanism vs Nationalism")]
    [Range(-1f, 1f)]
    public float nationView;

    [Header("政治觀：民粹vs獨裁 Despotism vs Popularism")]
    [Range(-1f, 1f)]
    public float politicView;

    [Header("世界觀：全球主義vs孤立主義 Globalism vs Isolationism")]
    [Range(-1f, 1f)]
    public float worldView;

    [Header("社會觀：社會主義vs達爾文主義 Socialism vs Social Darwinism")]
    [Range(-1f, 1f)]
    public float societyView;

    [Header("性別觀：母權vs父權 Matriarchy vs Patriarchy")]
    [Range(-1f, 1f)]
    public float genderView;

    [Header("種族觀：平等主義vs種族主義 Equalism vs Racism")]
    [Range(-1f, 1f)]
    public float raceView;

    [Header("社會風氣：進步vs保守 Progressivism vs Conservatism")]
    [Range(-1f, 1f)]
    public float conductView;

    [Header("自我觀：個人vs集體 Individualism vs Collectivism")]
    [Range(-1f, 1f)]
    public float idView;

    [Header("階級：完全平等vs極不平等 Equality vs Inequality")]
    [Range(-1f, 1f)]
    public float hierarchyView;

    [Header("外交觀：和平主義vs軍國主義 Pacifism vs Militarism")]
    [Range(-1f, 1f)]
    public float diplomacyView;

    [Header("自然觀：科學主義vs神秘主義 Scientism vs Mysticism")]
    [Range(-1f, 1f)]
    public float natureView;

    [Header("知識觀：理性主義vs經驗主義 Rationalism vs Empiricism")]
    [Range(-1f, 1f)]
    public float knowledgeView;

    [Header("處事態度：理性vs感性 Rational vs Emotional")]
    [Range(-1f, 1f)]
    public float altitude;
}
