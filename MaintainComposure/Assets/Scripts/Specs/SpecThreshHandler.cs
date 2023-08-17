using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SpecThreshHandler : MonoBehaviour
{

    // SpecThreshHandler handles specialization threshold display


    #region VARIABLES


    [SerializeField] private TMP_Text levelText;


    #endregion


    #region SETUP


    // Sets level text
    //----------------------------------------//
    public void SetLevel(int level)
    //----------------------------------------//
    {
        levelText.text = "Threshold: Reach character level " + level + " before continuing";

    } // END SetLevel


    #endregion


} // END SpecThreshHandler.cs
