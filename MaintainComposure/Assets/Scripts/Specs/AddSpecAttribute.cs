using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AddSpecAttribute : MonoBehaviour
{

    // AddSpecAttribute handles a spec attribute in the add spec panel


    #region VARIABLES


    [SerializeField] private TMP_Dropdown attributeType;
    [SerializeField] private TMP_Dropdown dropdown1;
    [SerializeField] private TMP_Dropdown dropdown2;
    [SerializeField] private TMP_InputField stringField1;
    [SerializeField] private TMP_InputField stringField2;

    public LevelUpAspect levelUpAspect { get; private set; }
    private bool holdValUpdates = false;
    private AddSpecLevel owningLevel;


    #endregion


    #region SETUP


    // Sets up attribute from level up aspect
    //----------------------------------------//
    public void SetupAttribute(LevelUpAspect setupLevelUpAspect, AddSpecLevel _owningLevel)
    //----------------------------------------//
    {
        levelUpAspect = setupLevelUpAspect;

        holdValUpdates = true;
        attributeType.value = (int)levelUpAspect.levelUpType;
        OnAttributeTypeChange(true, false);
        SetDisplayToData();
        holdValUpdates = false;

        owningLevel = _owningLevel;

    } // END SetupAttribute


    // Sets up attribute from scratch
    //----------------------------------------//
    public void SetupAttributeFromScratch(AddSpecLevel _owningLevel)
    //----------------------------------------//
    {
        levelUpAspect = new LevelUpAspect(LevelUpAspect.LevelUpType.SkillIncrease);

        holdValUpdates = true;
        attributeType.value = (int)levelUpAspect.levelUpType;
        OnAttributeTypeChange(true, true);
        SetDisplayToData();
        holdValUpdates = false;

        owningLevel = _owningLevel;

    } // END SetupAttributeFromScratch


    #endregion


    #region DISPLAY


    // Sets the displayed to stored data
    //----------------------------------------//
    private void SetDisplayToData()
    //----------------------------------------//
    {
        attributeType.value = (int)levelUpAspect.levelUpType;

        switch (attributeType.value)
        {
            case 0: // Skill increase
                dropdown1.value = levelUpAspect.assocIncNumber + 1;
                break;
            case 1: // App bonus
                switch(levelUpAspect.assocString)
                {
                    case "Any": dropdown1.value = 0; break;
                    case "Strength": dropdown1.value = 1; break;
                    case "Dexterity": dropdown1.value = 2; break;
                    case "Intelligence": dropdown1.value = 3; break;
                    case "Senses": dropdown1.value = 4; break;
                    case "Charisma": dropdown1.value = 5; break;
                    case "Force": dropdown1.value = 6; break;
                    case "Smash": dropdown1.value = 7; break;
                    case "Throw": dropdown1.value = 8; break;
                    case "Wrestle": dropdown1.value = 9; break;
                    case "Draw": dropdown1.value = 10; break;
                    case "Jump": dropdown1.value = 11; break;
                    case "Run": dropdown1.value = 12; break;
                    case "Sneak": dropdown1.value = 13; break;
                    case "Analyze": dropdown1.value = 14; break;
                    case "Deduce": dropdown1.value = 15; break;
                    case "Know": dropdown1.value = 16; break;
                    case "Learn": dropdown1.value = 17; break;
                    case "Feel": dropdown1.value = 18; break;
                    case "Hear": dropdown1.value = 19; break;
                    case "React": dropdown1.value = 20; break;
                    case "See": dropdown1.value = 21; break;
                    case "Charm": dropdown1.value = 22; break;
                    case "Glean": dropdown1.value = 23; break;
                    case "Manipulate": dropdown1.value = 24; break;
                    case "Persuade": dropdown1.value = 25; break;
                }
                break;
            case 2: // Max comp inc
                dropdown1.value = levelUpAspect.assocIncNumber - 1;
                dropdown2.value = (int)levelUpAspect.assocDie;
                break;
            case 3: // Speed inc
                dropdown1.value = levelUpAspect.assocIncNumber - 1;
                break;
            case 4: // Thresh inc
                dropdown1.value = levelUpAspect.assocIncNumber - 1;
                break;
            case 5: // App inc
                // N/A
                break;
            case 6: // Defiance inc
                // N/A
                break;
            case 7: // Learn specific art
                stringField1.text = levelUpAspect.assocString;
                break;
            case 8: // Learn art from list
                stringField2.text = levelUpAspect.assocString;
                stringField1.text = levelUpAspect.assocString2;
                break;
            case 9: // Learn art of type
                dropdown1.value = levelUpAspect.assocIncNumber;
                dropdown2.value = (int)levelUpAspect.assocArtType;
                break;
            case 10: // Learn any art
                dropdown1.value = levelUpAspect.assocIncNumber;
                break;
            case 11: // Unlock spec
                dropdown1.value = levelUpAspect.assocIncNumber;
                stringField2.text = levelUpAspect.assocString;
                break;
            case 12: // Other
                stringField1.text = levelUpAspect.assocString;
                break;
            case 13: // Initial Setup
                // N/A
                break;
        }

    } // END SetDisplayToData


    // Sets the data to the displayed
    //----------------------------------------//
    private void SetDataToDisplay()
    //----------------------------------------//
    {
        levelUpAspect.levelUpType = (LevelUpAspect.LevelUpType)Enum.ToObject(typeof(LevelUpAspect.LevelUpType), attributeType.value); 
        
        switch(attributeType.value)
        {
            case 0: // Skill increase
                levelUpAspect.assocIncNumber = dropdown1.value - 1;
                break;
            case 1: // App bonus
                levelUpAspect.assocString = dropdown1.options[dropdown1.value].text;
                break;
            case 2: // Max comp inc
                levelUpAspect.assocIncNumber = dropdown2.value + 1;
                levelUpAspect.assocDie = (InvItemsManager.DamageDie)Enum.ToObject(typeof(InvItemsManager.DamageDie), dropdown1.value);
                break;
            case 3: // Speed inc
                levelUpAspect.assocIncNumber = dropdown1.value + 1;
                break;
            case 4: // Thresh inc
                levelUpAspect.assocIncNumber = dropdown1.value + 1;
                break;
            case 5: // App inc
                // N/A
                break;
            case 6: // Defiance inc
                // N/A
                break;
            case 7: // Learn specific art
                levelUpAspect.assocString = stringField1.text;
                break;
            case 8: // Learn art from list
                levelUpAspect.assocString = stringField2.text;
                levelUpAspect.assocString2 = stringField1.text;
                break;
            case 9: // Learn art of type
                levelUpAspect.assocIncNumber = dropdown1.value;
                levelUpAspect.assocArtType = (ArtsManager.ArtType)Enum.ToObject(typeof(ArtsManager.ArtType), dropdown2.value);
                break;
            case 10: // Learn any art
                levelUpAspect.assocIncNumber = dropdown1.value;
                break;
            case 11: // Unlock spec
                levelUpAspect.assocIncNumber = dropdown1.value;
                levelUpAspect.assocString = stringField2.text;
                break;
            case 12: // Other
                levelUpAspect.assocString = stringField1.text;
                break;
            case 13: // Initial Setup
                // N/A
                break;
        }

    } // END SetDataToDisplay


    #endregion


    #region VAL CHANGE


    // On attribute type change, display accordingly
    //----------------------------------------//
    public void OnAttributeTypeChange(bool overrideHold, bool updateData)
    //----------------------------------------//
    {
        if (!overrideHold && holdValUpdates)
        {
            return;
        }

        List<TMP_Dropdown.OptionData> optionsList1 = new List<TMP_Dropdown.OptionData>();
        List<TMP_Dropdown.OptionData> optionsList2 = new List<TMP_Dropdown.OptionData>();

        optionsList1.Clear();

        switch (attributeType.value)
        {
            case 0: // Skill increase
                dropdown1.gameObject.SetActive(true);
                dropdown2.gameObject.SetActive(false);
                stringField1.gameObject.SetActive(false);
                stringField2.gameObject.SetActive(false);

                dropdown1.ClearOptions();
                optionsList1.Add(new TMP_Dropdown.OptionData("Any"));
                optionsList1.Add(new TMP_Dropdown.OptionData("Strength"));
                optionsList1.Add(new TMP_Dropdown.OptionData("Dexterity"));
                optionsList1.Add(new TMP_Dropdown.OptionData("Intelligence"));
                optionsList1.Add(new TMP_Dropdown.OptionData("Senses"));
                optionsList1.Add(new TMP_Dropdown.OptionData("Charisma"));
                dropdown1.AddOptions(optionsList1);
                dropdown1.value = 0;

                break;
            case 1: // App bonus
                dropdown1.gameObject.SetActive(true);
                dropdown2.gameObject.SetActive(false);
                stringField1.gameObject.SetActive(false);
                stringField2.gameObject.SetActive(false);

                dropdown1.ClearOptions();
                optionsList1.Add(new TMP_Dropdown.OptionData("Any"));
                optionsList1.Add(new TMP_Dropdown.OptionData("Strength"));
                optionsList1.Add(new TMP_Dropdown.OptionData("Dexterity"));
                optionsList1.Add(new TMP_Dropdown.OptionData("Intelligence"));
                optionsList1.Add(new TMP_Dropdown.OptionData("Senses"));
                optionsList1.Add(new TMP_Dropdown.OptionData("Charisma"));
                optionsList1.Add(new TMP_Dropdown.OptionData("Force"));
                optionsList1.Add(new TMP_Dropdown.OptionData("Smash"));
                optionsList1.Add(new TMP_Dropdown.OptionData("Throw"));
                optionsList1.Add(new TMP_Dropdown.OptionData("Wrestle"));
                optionsList1.Add(new TMP_Dropdown.OptionData("Draw"));
                optionsList1.Add(new TMP_Dropdown.OptionData("Jump"));
                optionsList1.Add(new TMP_Dropdown.OptionData("Run"));
                optionsList1.Add(new TMP_Dropdown.OptionData("Sneak"));
                optionsList1.Add(new TMP_Dropdown.OptionData("Analyze"));
                optionsList1.Add(new TMP_Dropdown.OptionData("Deduce"));
                optionsList1.Add(new TMP_Dropdown.OptionData("Know"));
                optionsList1.Add(new TMP_Dropdown.OptionData("Learn"));
                optionsList1.Add(new TMP_Dropdown.OptionData("Feel"));
                optionsList1.Add(new TMP_Dropdown.OptionData("Hear"));
                optionsList1.Add(new TMP_Dropdown.OptionData("React"));
                optionsList1.Add(new TMP_Dropdown.OptionData("See"));
                optionsList1.Add(new TMP_Dropdown.OptionData("Charm"));
                optionsList1.Add(new TMP_Dropdown.OptionData("Glean"));
                optionsList1.Add(new TMP_Dropdown.OptionData("Manipulate"));
                optionsList1.Add(new TMP_Dropdown.OptionData("Persuade"));
                dropdown1.AddOptions(optionsList1);
                dropdown1.value = 0;

                break;
            case 2: // Max comp increase
                dropdown1.gameObject.SetActive(true);
                dropdown2.gameObject.SetActive(true);
                stringField1.gameObject.SetActive(false);
                stringField2.gameObject.SetActive(false);

                dropdown1.ClearOptions();
                optionsList1.Add(new TMP_Dropdown.OptionData("d4"));
                optionsList1.Add(new TMP_Dropdown.OptionData("d6"));
                optionsList1.Add(new TMP_Dropdown.OptionData("d8"));
                optionsList1.Add(new TMP_Dropdown.OptionData("d10"));
                optionsList1.Add(new TMP_Dropdown.OptionData("d12"));
                optionsList1.Add(new TMP_Dropdown.OptionData("d20"));
                dropdown1.AddOptions(optionsList1);
                dropdown1.value = 0;

                dropdown2.ClearOptions();
                optionsList2.Add(new TMP_Dropdown.OptionData("1"));
                optionsList2.Add(new TMP_Dropdown.OptionData("2"));
                optionsList2.Add(new TMP_Dropdown.OptionData("3"));
                optionsList2.Add(new TMP_Dropdown.OptionData("4"));
                optionsList2.Add(new TMP_Dropdown.OptionData("5"));
                optionsList2.Add(new TMP_Dropdown.OptionData("6"));
                dropdown2.AddOptions(optionsList2);
                dropdown2.value = 0;

                break;
            case 3: // Speed inc
                dropdown1.gameObject.SetActive(true);
                dropdown2.gameObject.SetActive(false);
                stringField1.gameObject.SetActive(false);
                stringField2.gameObject.SetActive(false);

                dropdown1.ClearOptions();
                optionsList1.Add(new TMP_Dropdown.OptionData("1"));
                optionsList1.Add(new TMP_Dropdown.OptionData("2"));
                optionsList1.Add(new TMP_Dropdown.OptionData("3"));
                optionsList1.Add(new TMP_Dropdown.OptionData("4"));
                optionsList1.Add(new TMP_Dropdown.OptionData("5"));
                dropdown1.AddOptions(optionsList1);
                dropdown1.value = 0;

                break;
            case 4: // Thresh inc
                dropdown1.gameObject.SetActive(true);
                dropdown2.gameObject.SetActive(false);
                stringField1.gameObject.SetActive(false);
                stringField2.gameObject.SetActive(false);

                dropdown1.ClearOptions();
                optionsList1.Add(new TMP_Dropdown.OptionData("1"));
                optionsList1.Add(new TMP_Dropdown.OptionData("2"));
                optionsList1.Add(new TMP_Dropdown.OptionData("3"));
                optionsList1.Add(new TMP_Dropdown.OptionData("4"));
                optionsList1.Add(new TMP_Dropdown.OptionData("5"));
                dropdown1.AddOptions(optionsList1);
                dropdown1.value = 0;

                break;
            case 5: // Act inc
                dropdown1.gameObject.SetActive(false);
                dropdown2.gameObject.SetActive(false);
                stringField1.gameObject.SetActive(false);
                stringField2.gameObject.SetActive(false);
                break;
            case 6: // Defiance inc
                dropdown1.gameObject.SetActive(false);
                dropdown2.gameObject.SetActive(false);
                stringField1.gameObject.SetActive(false);
                stringField2.gameObject.SetActive(false);
                break;
            case 7: // Learn specific art
                dropdown1.gameObject.SetActive(false);
                dropdown2.gameObject.SetActive(false);
                stringField1.gameObject.SetActive(true);
                stringField2.gameObject.SetActive(false);

                stringField1.text = string.Empty;

                break;
            case 8: // Learn art from list
                dropdown1.gameObject.SetActive(false);
                dropdown2.gameObject.SetActive(false);
                stringField1.gameObject.SetActive(true);
                stringField2.gameObject.SetActive(true);

                stringField1.text = string.Empty;
                stringField2.text = string.Empty;

                break;
            case 9: // Learn art of type
                dropdown1.gameObject.SetActive(true);
                dropdown2.gameObject.SetActive(true);
                stringField1.gameObject.SetActive(false);
                stringField2.gameObject.SetActive(false);

                dropdown1.ClearOptions();
                optionsList1.Add(new TMP_Dropdown.OptionData("Basic"));
                optionsList1.Add(new TMP_Dropdown.OptionData("Advanced"));
                optionsList1.Add(new TMP_Dropdown.OptionData("Expert"));
                dropdown1.AddOptions(optionsList1);
                dropdown1.value = 0;

                dropdown2.ClearOptions();
                optionsList2.Add(new TMP_Dropdown.OptionData("Fundamental"));
                optionsList2.Add(new TMP_Dropdown.OptionData("Combat"));
                optionsList2.Add(new TMP_Dropdown.OptionData("Collaborative"));
                optionsList2.Add(new TMP_Dropdown.OptionData("Reactionary"));
                optionsList2.Add(new TMP_Dropdown.OptionData("Recovery"));
                optionsList2.Add(new TMP_Dropdown.OptionData("Stealth"));
                optionsList2.Add(new TMP_Dropdown.OptionData("Utility"));
                dropdown2.AddOptions(optionsList2);

                break;
            case 10: // Learn any art
                dropdown1.gameObject.SetActive(true);
                dropdown2.gameObject.SetActive(false);
                stringField1.gameObject.SetActive(false);
                stringField2.gameObject.SetActive(false);

                dropdown1.ClearOptions();
                optionsList1.Add(new TMP_Dropdown.OptionData("Basic"));
                optionsList1.Add(new TMP_Dropdown.OptionData("Advanced"));
                optionsList1.Add(new TMP_Dropdown.OptionData("Expert"));
                dropdown1.AddOptions(optionsList1);
                dropdown1.value = 0;

                break;
            case 11: // Unlock spec
                dropdown1.gameObject.SetActive(true);
                dropdown2.gameObject.SetActive(false);
                stringField1.gameObject.SetActive(false);
                stringField2.gameObject.SetActive(true);

                dropdown1.ClearOptions();
                optionsList1.Add(new TMP_Dropdown.OptionData("1-Star"));
                optionsList1.Add(new TMP_Dropdown.OptionData("2-Star"));
                optionsList1.Add(new TMP_Dropdown.OptionData("3-Star"));
                optionsList1.Add(new TMP_Dropdown.OptionData("4-Star"));
                optionsList1.Add(new TMP_Dropdown.OptionData("5-Star"));
                dropdown1.AddOptions(optionsList1);
                dropdown1.value = 0;

                stringField2.text = string.Empty;

                break;
            case 12: // Other
                dropdown1.gameObject.SetActive(false);
                dropdown2.gameObject.SetActive(false);
                stringField1.gameObject.SetActive(true);
                stringField2.gameObject.SetActive(false);

                stringField1.text = string.Empty;

                break;
            case 13: // Initial skill setup
                dropdown1.gameObject.SetActive(false);
                dropdown2.gameObject.SetActive(false);
                stringField1.gameObject.SetActive(false);
                stringField2.gameObject.SetActive(false);

                break;
        }

        if (updateData)
            SetDataToDisplay();

    } // END OnAttributeTypeChange


    // Override
    //----------------------------------------//
    public void OnAttributeTypeChange()
    //----------------------------------------//
    {
        OnAttributeTypeChange(false, true);

    } // END Override


    // On any value change, set data
    //----------------------------------------//
    public void OnValueChange()
    //----------------------------------------//
    {
        if (holdValUpdates)
        {
            return;
        }

        SetDataToDisplay();

    } // END OnValueChange


    #endregion


    #region ON REMOVE


    // Removes attribute
    //----------------------------------------//
    public void OnRemoveButton()
    //----------------------------------------//
    {
        owningLevel.OnRemoveAttribute(this);
        FindObjectOfType<NewSpecHandler>().RefreshScroll();
        GameObject.Destroy(this.gameObject);

    } // END OnRemoveButton


    #endregion


} // END AddSpecAttribute.cs
