using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpecDisplayHandler : MonoBehaviour
{

    // SpecDisplayHandler handles a specialization display


    #region VARIABLES


    [SerializeField] private TMP_Text specNameText;

    private SpecializationData specData;
    private int playerLevel;


    #endregion


    #region SETUP


    // Sets up the specialization to match data
    //----------------------------------------//
    public void SetupSpec(SpecializationData data)
    //----------------------------------------//
    {
        specData = data;
        specNameText.text = "(" + (data.starLevel + 1) + "-Star) " + data.specName;

    } // END SetupSpec


    // Sets up the specialization to match data
    //----------------------------------------//
    public void SetupSpec(SpecializationData data, int _playerLevel)
    //----------------------------------------//
    {
        specData = data;
        playerLevel = _playerLevel;
        specNameText.text = "(" + (data.starLevel + 1) + "-Star) " + data.specName + " (Level " + playerLevel + ")";

    } // END SetupSpec


    #endregion


    #region BUTTONS


    // Views specialization
    //----------------------------------------//
    public void OnViewButton()
    //----------------------------------------//
    {
        SpecializeManager.Instance.DisplaySpecInLevelUpPanel(specData, true, playerLevel);

    } // END OnViewButton


    // Edits specialization
    //----------------------------------------//
    public void OnEditButton()
    //----------------------------------------//
    {
        SpecializeManager.Instance.BeginEditSpec(specData);

    } // END OnEditButton


    // Deletes specialization
    //----------------------------------------//
    public void OnDeleteButton()
    //----------------------------------------//
    {
        SpecializeManager.Instance.RemoveRegistrySpec(specData);

    } // END OnDeleteButton


    #endregion


} // END SpecDisplayHandler
