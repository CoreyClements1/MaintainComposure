using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpecLevelAttributeDisplay : MonoBehaviour
{

    // SpecLevelAttributeDisplay displays a specialization level attribute


    #region VARIABLES


    private LevelUpAspect aspectToDisplay;

    [SerializeField] private TMP_Text textDisplay;


    #endregion


    #region SETUP


    // Sets up display
    //----------------------------------------//
    public void SetupDisplay(LevelUpAspect _aspectToDisplay)
    //----------------------------------------//
    {
        aspectToDisplay = _aspectToDisplay;

        string displayString = "";

        switch(aspectToDisplay.levelUpType)
        {
            case LevelUpAspect.LevelUpType.SkillIncrease:
                switch(aspectToDisplay.assocIncNumber)
                {
                    case -1:
                        displayString = "Skill increase: +1 to a Skill of your choice";
                        break;
                    case 0:
                        displayString = "Skill increase: +1 to Strength Skill";
                        break;
                    case 1:
                        displayString = "Skill increase: +1 to Dexterity Skill";
                        break;
                    case 2:
                        displayString = "Skill increase: +1 to Intelligence Skill";
                        break;
                    case 3:
                        displayString = "Skill increase: +1 to Senses Skill";
                        break;
                    case 4:
                        displayString = "Skill increase: +1 to Charisma Skill";
                        break;
                }
                break;
            case LevelUpAspect.LevelUpType.AppBonus:
                if (aspectToDisplay.assocString == "Any")
                {
                    displayString = "App. Bonus: +1 Application Bonus to any Application of your choice";
                }
                else
                {
                    displayString = "App. Bonus: +1 " + aspectToDisplay.assocString + " Application Bonus";
                }
                break;
            case LevelUpAspect.LevelUpType.MaxComposureIncrease:
                displayString = "Composure increase: +" + aspectToDisplay.assocIncNumber + aspectToDisplay.assocDie.ToString() + " Maximum Composure";
                break;
            case LevelUpAspect.LevelUpType.SpeedIncrease:
                displayString = "Speed increase: +" + aspectToDisplay.assocIncNumber + " meters movement speed";
                break;
            case LevelUpAspect.LevelUpType.ThresholdIncrease:
                displayString = "Threshold increase: +" + aspectToDisplay.assocIncNumber + " Composure Threshold";
                break;
            case LevelUpAspect.LevelUpType.ActIncrease:
                displayString = "Act increase: +1 Maximum Acts";
                break;
            case LevelUpAspect.LevelUpType.DefianceIncrease:
                displayString = "Defiance increase: +1 Defiance";
                break;
            case LevelUpAspect.LevelUpType.LearnSpecificArt:
                string c1 = "";
                switch(aspectToDisplay.assocIncNumber)
                {
                    case 0:
                        c1 = "";
                        break;
                    case 1:
                        c1 = "Advanced ";
                        break;
                    case 2:
                        c1 = "Expert ";
                        break;
                }
                displayString = "Learn Art: Learn the \"" + aspectToDisplay.assocString + "\" " + c1 + aspectToDisplay.assocArtType.ToString() + " Art";
                break;
            case LevelUpAspect.LevelUpType.LearnArtFromList:
                displayString = "Learn Art: Learn a \"" + aspectToDisplay.assocString + "\" Art";
                break;
            case LevelUpAspect.LevelUpType.LearnArtOfType:
                string c2 = "";
                switch (aspectToDisplay.assocIncNumber)
                {
                    case 0:
                        c2 = "";
                        break;
                    case 1:
                        c2 = "Advanced ";
                        break;
                    case 2:
                        c2 = "Expert ";
                        break;
                }
                displayString = "Learn Art: Learn any " + c2 + aspectToDisplay.assocArtType.ToString() + " Art of your choice";
                break;
            case LevelUpAspect.LevelUpType.LearnAnyArt:
                string c3 = "";
                switch (aspectToDisplay.assocIncNumber)
                {
                    case 0:
                        c3 = "";
                        break;
                    case 1:
                        c3 = "Advanced ";
                        break;
                    case 2:
                        c3 = "Expert ";
                        break;
                }
                displayString = "Learn Art: Learn any " + c3 + " Art of your choice";
                break;
            case LevelUpAspect.LevelUpType.UnlockSpec:
                if (aspectToDisplay.assocString == "Any")
                {
                    displayString = "Unlock Specialization: Unlock access to any " + (aspectToDisplay.assocIncNumber + 1) + "-Star Specialization of your choice";
                }
                else
                {
                    displayString = "Unlock Specialization: Unlock access to one of the following specializations: " + aspectToDisplay.assocString;
                }
                break;
            case LevelUpAspect.LevelUpType.Other:
                displayString = aspectToDisplay.assocString;
                break;
            case LevelUpAspect.LevelUpType.InitialSetup:
                displayString = "Skill Setup: Gain 40 points to distribute to skills";
                break;
        }

        textDisplay.text = displayString;

    } // END SetupDisplay


    #endregion


} // END SpecLevelAttributeDisplay.cs
