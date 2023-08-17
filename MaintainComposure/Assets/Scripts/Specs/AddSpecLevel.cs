using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AddSpecLevel : MonoBehaviour
{

    // AddSpecLevel handles spec level in the add spec panel


    #region VARIABLES


    [SerializeField] private TMP_Text levelNumText;
    [SerializeField] private TMP_InputField levelNameField;
    [SerializeField] private AddSpecAttribute attributePrefab;
    [SerializeField] private Transform contentParent;

    private LevelUpData levelUpData;
    private List<AddSpecAttribute> attributes = new List<AddSpecAttribute>();


    #endregion


    #region SETUP


    // Sets up level from data
    //----------------------------------------//
    public void SetupLevel(LevelUpData setupLevelUpData, int levelNum)
    //----------------------------------------//
    {
        levelUpData = setupLevelUpData;
        levelNumText.text = "Level " + (levelNum + 1);
        levelNameField.text = levelUpData.levelUpName;

        for (int i = 0; i < levelUpData.levelUpAspects.Length; i++)
        {
            AddAttributeFromData(levelUpData.levelUpAspects[i]);
        }

    } // END SetupLevel


    // Sets up level from scratch
    //----------------------------------------//
    public void SetupLevelFromScratch(int levelNum)
    //----------------------------------------//
    {
        levelUpData = new LevelUpData();
        levelNumText.text = "Level " + (levelNum + 1);
        levelNameField.text = levelUpData.levelUpName;

        AddNewAttribute();

    } // END SetupLevelFromScratch


    #endregion


    #region VAL CHANGE


    // OnValChange, update data
    //----------------------------------------//
    public void OnValChange()
    //----------------------------------------//
    {
        levelUpData.levelUpName = levelNameField.text;

    } // END OnValChange


    #endregion


    #region ADD / REMOVE


    // On button, add new attribute
    //----------------------------------------//
    public void OnAddButton()
    //----------------------------------------//
    {
        AddNewAttribute();

    } // END OnAddButton


    // Adds new attribute in list
    //----------------------------------------//
    private void AddNewAttribute()
    //----------------------------------------//
    {
        AddSpecAttribute newAttribute = GameObject.Instantiate(attributePrefab);
        newAttribute.transform.SetParent(contentParent.transform);

        newAttribute.SetupAttributeFromScratch(this);

        attributes.Add(newAttribute);

        FindObjectOfType<NewSpecHandler>().RefreshScroll();

    } // END AddNewAttribute


    // Adds new attribute in list from data
    //----------------------------------------//
    private void AddAttributeFromData(LevelUpAspect aspectData)
    //----------------------------------------//
    {
        AddSpecAttribute newAttribute = GameObject.Instantiate(attributePrefab);
        newAttribute.transform.SetParent(contentParent.transform);

        newAttribute.SetupAttribute(aspectData, this);

        attributes.Add(newAttribute);

        FindObjectOfType<NewSpecHandler>().RefreshScroll();

    } // END AddAttributeFromData


    // Removes attribute from list
    //----------------------------------------//
    public void OnRemoveAttribute(AddSpecAttribute attributeToRemove)
    //----------------------------------------//
    {
        attributes.Remove(attributeToRemove);

    } // END OnRemoveAttribute


    #endregion


    #region GET


    // Gets data
    //----------------------------------------//
    public LevelUpData GetData()
    //----------------------------------------//
    {
        List<LevelUpAspect> levelUpAspectList = new List<LevelUpAspect>();

        foreach(AddSpecAttribute addSpecAttribute in attributes)
        {
            levelUpAspectList.Add(addSpecAttribute.levelUpAspect);
        }

        levelUpData.levelUpAspects = levelUpAspectList.ToArray();

        return levelUpData;

    } // END GetData


    #endregion


} // END AddSpecLevel.cs
