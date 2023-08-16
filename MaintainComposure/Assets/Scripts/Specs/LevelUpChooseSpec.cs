using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelUpChooseSpec : MonoBehaviour
{

    // LearnArt handles the learnable arts in the learning art panel


    #region VARIABLES


    [SerializeField] private TMP_Text specNameText;

    private LevelUpHandlerAttribute handlerAttribute;
    private SpecializationData specData;

    [SerializeField] private TMP_Text buttonText;


    #endregion


    #region SETUP


    // Sets choose spec name
    //----------------------------------------//
    public void SetupSpec(SpecializationData _specData, LevelUpHandlerAttribute _handlerAttribute, string _buttonText)
    //----------------------------------------//
    {
        handlerAttribute = _handlerAttribute;
        specData = _specData;

        specNameText.text = specData.specName;
        buttonText.text = _buttonText;

    } // END SetAttribute


    #endregion


    #region ON LEARN


    // Choose spec and give back to level up handler attribute
    //----------------------------------------//
    public void OnChooseButton()
    //----------------------------------------//
    {
        if (buttonText.text == "Choose")
        {
            handlerAttribute.OnChooseSpec(specData);
            FindObjectOfType<LevelUpHandler>().CloseChooseArtPanel();
        }
        else
        {
            handlerAttribute.OnBeginChooseArt();
        }

    } // END OnLearnButton


    #endregion


} // END LevelUpChooseArt.cs
