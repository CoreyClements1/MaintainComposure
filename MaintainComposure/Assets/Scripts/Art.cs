using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Art : MonoBehaviour
{

    // Art manages an art under the arts panel


    #region VARIABLES


    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text typeText;
    [SerializeField] private Image bgImg;
    [SerializeField] private Toggle favToggle;
    [SerializeField] private Image favBg;
    [SerializeField] private Image favCheck;

    private ArtData artData;


    #endregion


    #region SETUP


    // Sets up art
    //----------------------------------------//
    public void SetupArt(ArtData data)
    //----------------------------------------//
    {
        artData = data;

        nameText.text = artData.artName;

        string type = "";

        // Complexity
        switch (artData.artComplexity)
        {
            case ArtsManager.ArtComplexity.Basic:
                // None
                break;
            case ArtsManager.ArtComplexity.Advanced:
                type += "Advanced ";
                break;
            case ArtsManager.ArtComplexity.Expert:
                type += "Expert ";
                break;
        }

        // Type
        switch (artData.artType)
        {
            case ArtsManager.ArtType.Fundamental:
                type += "Fundamental ";
                break;
            case ArtsManager.ArtType.Combat:
                type += "Combat ";
                break;
            case ArtsManager.ArtType.Collaborative:
                type += "Collaborative ";
                break;
            case ArtsManager.ArtType.Reactionary:
                type += "Reactionary ";
                break;
            case ArtsManager.ArtType.Recovery:
                type += "Recovery ";
                break;
            case ArtsManager.ArtType.Stealth:
                type += "Stealth ";
                break;
            case ArtsManager.ArtType.Utility:
                type += "Utility ";
                break;
        }

        type += "Art";

        // Passive
        if (artData.passive)
        {
            type += " (passive)";
        }

        typeText.text = type;

        // Favorite
        if (artData.favorited)
        {
            favToggle.isOn = true;
        }
        else
        {
            favToggle.isOn = false;
        }

        // Color
        nameText.color = ArtsManager.artTextColors[(int)artData.artType];
        typeText.color = ArtsManager.artTextColors[(int)artData.artType];

        favBg.color = ArtsManager.artTextColors[(int)artData.artType];
        favCheck.color = ArtsManager.artTextColors[(int)artData.artType];

        bgImg.color = ArtsManager.artBgColors[(int)artData.artType];

    } // END SetupArt


    #endregion


    #region FAV TOGGLE


    // OnFavToggle, save art as favorite
    //----------------------------------------//
    public void OnFavToggle()
    //----------------------------------------//
    {
        artData.favorited = favToggle.isOn;

    } // END OnFavToggle


    #endregion


} // END Art.cs


[System.Serializable]
public class ArtData
{

    // ArtData contains data for an art


    #region VARIABLES


    public string artName;
    public ArtsManager.ArtType artType;
    public ArtsManager.ArtComplexity artComplexity;
    public int actCost;
    public string artRange;
    public bool passive;
    public bool specializationSpecific;
    public bool favorited;
    public string artDescription;


    #endregion


    #region CONSTRUCTORS


    // Constructor, takes all elements
    public ArtData(string _artName, ArtsManager.ArtType _artType, ArtsManager.ArtComplexity _artComplexity, int _actCost, string _artRange, bool _passive, bool _specializationSpecific, bool _favorited, string _artDescription)
    {
        artName = _artName;
        artType = _artType;
        artComplexity = _artComplexity;
        actCost = _actCost;
        artRange = _artRange;
        passive = _passive;
        specializationSpecific = _specializationSpecific;
        favorited = _favorited;
        artDescription = _artDescription;

    } // END Constructor


    #endregion


} // END ArtData.cs
