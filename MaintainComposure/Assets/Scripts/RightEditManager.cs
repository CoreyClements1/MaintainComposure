using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RightEditManager : MonoBehaviour
{

    // RightEditManager handles the right panel's edit section


    #region VARIABLES


    [SerializeField] private TMP_InputField charNameField;


    #endregion


    #region GENERAL


    // Adds to acts
    //----------------------------------------//
    public void AddToActs(int numToAdd)
    //----------------------------------------//
    {
        GeneralAreaManager.Instance.AddToMaxActs(numToAdd);

    } // END AddToActs


    // Adds to composure
    //----------------------------------------//
    public void AddToComposure(int numToAdd)
    //----------------------------------------//
    {
        GeneralAreaManager.Instance.AddToMaxComposure(numToAdd);

    } // END AddToComposure


    // Adds to defiances
    //----------------------------------------//
    public void AddToDefiances(int numToAdd)
    //----------------------------------------//
    {
        GeneralAreaManager.Instance.AddToDefiances(numToAdd);

    } // END AddToDefiances


    // Adds to threshold
    //----------------------------------------//
    public void AddToThreshold(int numToAdd)
    //----------------------------------------//
    {
        GeneralAreaManager.Instance.AddToThreshold(numToAdd);

    } // END AddToThreshold


    // Adds to speed
    //----------------------------------------//
    public void AddToSpeed(int numToAdd)
    //----------------------------------------//
    {
        GeneralAreaManager.Instance.AddToSpeedBonus(numToAdd);

    } // END AddToSpeed


    #endregion


    #region SKILL / APP


    // Adds to a skill
    //----------------------------------------//
    public void AddToSkill(string skill)
    //----------------------------------------//
    {
        SkillsManager.Instance.AddToSkill(skill, 1);

    } // END AddToSkill


    // Subtracts from a skill
    //----------------------------------------//
    public void SubtractFromSkill(string skill)
    //----------------------------------------//
    {
        SkillsManager.Instance.AddToSkill(skill, -1);

    } // END SubtractFromSkill


    // Adds to an application
    //----------------------------------------//
    public void AddToApplication(string app)
    //----------------------------------------//
    {
        SkillsManager.Instance.AddToApp(app, 1);

    } // END AddToApplication


    // Subtracts from an application
    //----------------------------------------//
    public void SubtractFromApplication(string app)
    //----------------------------------------//
    {
        SkillsManager.Instance.AddToApp(app, -1);

    } // END SubtractFromApplication


    #endregion


    #region CHAR NAME


    // OnCharFieldChange
    //----------------------------------------//
    public void OnCharFieldChange()
    //----------------------------------------//
    {
        HeaderAndSaveManager.Instance.ChangeCharName(charNameField.text);

    } // END OnCharFieldChange


    // Update character name field to character name
    //----------------------------------------//
    public void UpdateCharName(string charName)
    //----------------------------------------//
    {
        charNameField.text = charName;

    } // END UpdateCharName


    #endregion


} // END RightEditManager.cs
