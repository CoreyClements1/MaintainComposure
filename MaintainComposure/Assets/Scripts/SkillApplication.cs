using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillApplication : MonoBehaviour
{

    // SkillApplication handles everything related to a given application


    #region VARIABLES


    public string applicationName;
    [SerializeField] private TMP_Text additionText;

    [System.NonSerialized]
    public int applicationBonuses;
    public int applicationModifier { get; private set; }
    private int skillScore;



    #endregion


    #region NUMBERS


    // Updates the skill score
    //----------------------------------------//
    public void UpdateSkillScore(int _skillScore)
    //----------------------------------------//
    {
        skillScore = _skillScore;
        UpdateText();

    } // END UpdateSkillScore


    // Adds bonuses to the application bonuses
    //----------------------------------------//
    public void AddBonuses(int bonusesToAdd)
    //----------------------------------------//
    {
        applicationBonuses += bonusesToAdd;
        UpdateText();

    } // END AddBonuses


    #endregion


    #region TEXT


    // Updates text to match numbers
    //----------------------------------------//
    public void UpdateText()
    //----------------------------------------//
    {
        string displayText = "";

        applicationModifier = Mathf.FloorToInt((skillScore - 10) / 2f) + applicationBonuses;

        if (applicationModifier >= 0)
        {
            displayText = "+" + applicationModifier;
        }
        else
        {
            displayText = "" + applicationModifier;
        }

        additionText.text = displayText;

        if (applicationName == "Run")
        {
            GeneralAreaManager.Instance.UpdateSpeedFromRun(applicationModifier);
        }

    } // END UpdateText


    #endregion


} // END SkillApplication.cs
