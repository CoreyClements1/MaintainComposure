using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpecLevelDisplayHandler : MonoBehaviour
{

    // SpecLevelDisplayHandler handles displaying a specialization's level


    #region VARIABLES


    private LevelUpData levelUpData;

    [SerializeField] private SpecLevelAttributeDisplay specLevelAttributePrefab;

    [SerializeField] private TMP_Text levelNameText;
    [SerializeField] private Transform levelAttributesParent;


    #endregion


    #region SETUP


    // Sets up display
    //----------------------------------------//
    public void SetupDisplay(LevelUpData _levelUpData, int levelInSpec)
    //----------------------------------------//
    {
        levelUpData = _levelUpData;

        levelNameText.text = "Level " + (levelInSpec + 1) + ": " + levelUpData.levelUpName;

        foreach(LevelUpAspect aspect in levelUpData.levelUpAspects)
        {
            SpecLevelAttributeDisplay attributeDisplay = GameObject.Instantiate(specLevelAttributePrefab);
            attributeDisplay.transform.SetParent(levelAttributesParent);
            attributeDisplay.SetupDisplay(aspect);
        }

    } // END SetupDisplay


    #endregion


} // END SpecLevelDisplayHandler.cs
