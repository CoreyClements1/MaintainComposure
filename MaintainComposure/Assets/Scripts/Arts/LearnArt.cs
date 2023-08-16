using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LearnArt : Art
{

    // LearnArt handles the learnable arts in the learning art panel


    #region VARIABLES


    [SerializeField] private Image buttonImg;
    [SerializeField] private TMP_Text buttonText;


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


    // Learn art through arts manager
    //----------------------------------------//
    public void OnLearnButton()
    //----------------------------------------//
    {
        OnPointerExit(null);
        ArtsManager.Instance.LearnArt(artData);
        // TODO

    } // END OnLearnButton


    #endregion


} // END LearnArt.cs
