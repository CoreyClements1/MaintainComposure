using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpChooseArt : Art
{

    // LearnArt handles the learnable arts in the learning art panel


    #region VARIABLES


    [SerializeField] private Image buttonImg;
    [SerializeField] private TMP_Text buttonText;

    private LevelUpHandlerAttribute levelUpHandlerAttribute;


    #endregion


    #region SETUP


    // Sets attribute
    //----------------------------------------//
    public void SetAttribute(LevelUpHandlerAttribute _levelUpHandlerAttribute)
    //----------------------------------------//
    {
        levelUpHandlerAttribute = _levelUpHandlerAttribute;

    } // END SetAttribute


    #endregion


    #region COLORS


    // Sets up additional colors
    //----------------------------------------//
    public override void SetupAdditionalColors()
    //----------------------------------------//
    {
        buttonImg.color = ArtsManager.artTextColors[(int)artData.artType];

        buttonText.color = ArtsManager.artBgColors[(int)artData.artType];

    } // END SetupAdditionalColors


    #endregion


    #region ON LEARN


    // Choose art and give back to level up handler attribute
    //----------------------------------------//
    public void OnChooseButton()
    //----------------------------------------//
    {
        levelUpHandlerAttribute.OnChooseArt(artData);
        FindObjectOfType<LevelUpHandler>().CloseChooseArtPanel();

    } // END OnLearnButton


    #endregion


} // END LevelUpChooseArt.cs
