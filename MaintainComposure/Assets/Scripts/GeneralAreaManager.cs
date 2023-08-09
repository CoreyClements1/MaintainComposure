using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GeneralAreaManager : MonoBehaviour
{

    // GeneralAreaManager manages the aspects of the general area


    #region VARIABLES


    public static GeneralAreaManager Instance { get; private set; }

    // Composure
    public int currentComposure { get; private set; }
    public int maxComposure { get; private set; }
    public int tempComposure { get; private set; }

    [SerializeField] private TMP_InputField currentComposureField;
    [SerializeField] private TMP_Text maxComposureText;
    [SerializeField] private TMP_InputField tempComposureField;

    // Acts
    public int currentActs { get; private set; }
    public int maxActs { get; private set; }

    [SerializeField] private TMP_InputField currentActsField;
    [SerializeField] private TMP_Text maxActsField;

    // Speed
    public int moveSpeed { get; private set; }
    public int speedBonuses { get; private set; } = 0;
    public float speedMultipliers { get; private set; } = 1;
    public int storedRunScore { get; private set; } = 0;

    [SerializeField] private TMP_Text speedText;

    // Threshold
    public int composureThreshold { get; private set; }

    [SerializeField] private TMP_Text thresholdText;

    // Defiances
    public int currentDefiances { get; private set; }
    public int maxDefiances { get; private set; }


    #endregion


    #region MONOBEHAVIOUR AND SETUP


    // Sets up singleton
    //----------------------------------------//
    public void SetupSingleton()
    //----------------------------------------//
    {
        if (GeneralAreaManager.Instance == null)
        {
            GeneralAreaManager.Instance = this;
        }
        else
        {
            if (GeneralAreaManager.Instance != this)
            {
                Destroy(this);
            }
        }

    } // END SetupSingleton


    // Sets up attributes
    //----------------------------------------//
    public void SetupAttributes()
    //----------------------------------------//
    {
        maxActs = 1;
        maxActsField.text = "" + maxActs;
        currentActs = 1;
        currentActsField.text = "" + currentActs;

        maxComposure = 8;
        maxComposureText.text = "" + maxComposure;
        currentComposure = 8;
        currentComposureField.text = "" + currentComposure;
        tempComposure = 0;
        tempComposureField.text = "" + tempComposure;

        composureThreshold = 10;
        thresholdText.text = "" + composureThreshold;

        speedBonuses = 0;
        UpdateSpeedFromRun(0);

    } // END SetupSkills


    #endregion


    #region TEXT


    // Updates the text to match attributes
    //----------------------------------------//
    public void UpdateText()
    //----------------------------------------//
    {
        currentComposureField.text = "" + currentComposure;
        maxComposureText.text = "" + maxComposure;
        tempComposureField.text = "" + tempComposure;

        currentActsField.text = "" + currentActs;
        maxActsField.text = "" + maxActs;

    } // END UpdateText


    // On an attribute change, update attributes
    //----------------------------------------//
    public void OnAttributeChange()
    //----------------------------------------//
    {
        int parseNum = 0;

        if (Int32.TryParse(currentComposureField.text, out parseNum))
            currentComposure = parseNum;

        if (Int32.TryParse(tempComposureField.text, out parseNum))
            tempComposure = parseNum;

        if (Int32.TryParse(currentActsField.text, out parseNum))
            currentActs = parseNum;

    } // END OnAttributeChange


    #endregion


    #region COMPOSURE


    // Resets the current composure to max
    //----------------------------------------//
    public void ResetCurrentComposure()
    //----------------------------------------//
    {
        currentComposure = maxComposure;

        currentComposureField.text = "" + currentComposure;

    } // END ResetCurrentComposure


    // Adds an amount to current composure
    //----------------------------------------//
    public void AddToCurrentComposure(int amtToAdd)
    //----------------------------------------//
    {
        // Damage
        if (amtToAdd < 0)
        {
            // Take from temp
            if (tempComposure > 0)
            {
                tempComposure += amtToAdd;
                
                // Rollover into current composure
                if (tempComposure < 0)
                {
                    currentComposure += tempComposure;

                    tempComposure = 0;
                }
            }
            // Take from current
            else
            {
                currentComposure += amtToAdd;
            }

            // If 0, we dead
            if (currentComposure <= 0)
            {
                currentComposure = 0;

                // TODO: DIE
            }
        }
        // Heal
        else
        {
            currentComposure += amtToAdd;

            // Over max, reset to max
            if (currentComposure > maxComposure)
            {
                currentComposure = maxComposure;
            }
        }

        tempComposureField.text = "" + tempComposure;
        currentComposureField.text = "" + currentComposure;

    } // END AddToCurrentComposure


    // Adds temporary composure
    //----------------------------------------//
    public void AddTempComposure(int tempToAdd)
    //----------------------------------------//
    {
        tempComposure += tempToAdd;

        // Rollover into current
        if (tempComposure < 0)
        {
            int oldTemp = tempComposure;
            tempComposure = 0;
            // Damage current for rollover amount
            AddToCurrentComposure(oldTemp);
        }

        currentComposureField.text = "" + currentComposure;
        tempComposureField.text = "" + tempComposure;

    } // END AddTempComposure


    // Resets temporary composure
    //----------------------------------------//
    public void ResetTempComposure()
    //----------------------------------------//
    {
        tempComposure = 0;

        tempComposureField.text = "" + tempComposure;

    } // END ResetTempComposure


    // Adds to max composure
    //----------------------------------------//
    public void AddToMaxComposure(int numToAdd)
    //----------------------------------------//
    {
        maxComposure += numToAdd;

        if (maxComposure < 0)
        {
            maxComposure = 0;
        }

        maxComposureText.text = "" + maxComposure;

    } // END AddToMaxComposure


    #endregion


    #region ACTS


    // Adds to the current acts
    //----------------------------------------//
    public void AddToCurrentActs(int actsToAdd)
    //----------------------------------------//
    {
        currentActs += actsToAdd;

        if (currentActs > maxActs)
        {
            currentActs = maxActs;
        }

        if (currentActs < 0)
        {
            currentActs = 0;
        }

        currentActsField.text = "" + currentActs;

    } // END AddToCurrentActs


    // Resets acts
    //----------------------------------------//
    public void ResetActs()
    //----------------------------------------//
    {
        currentActs = maxActs;

        currentActsField.text = "" + currentActs;

    } // END ResetActs


    // Adds to max acts
    //----------------------------------------//
    public void AddToMaxActs(int numToAdd)
    //----------------------------------------//
    {
        maxActs += numToAdd;

        if (maxActs < 0)
        {
            maxActs = 0;
        }

        maxActsField.text = "" + maxActs;

    } // END AddToMaxActs


    #endregion


    #region SPEED


    // Updates movement speed from run application
    //----------------------------------------//
    public void UpdateSpeedFromRun(int runScore)
    //----------------------------------------//
    {
        storedRunScore = runScore;

        moveSpeed = Mathf.FloorToInt((10 + speedBonuses + runScore) * speedMultipliers);

        speedText.text = "" + moveSpeed;

    } // END UpdateSpeedFromRun


    // Updates movement speed using stored run
    //----------------------------------------//
    public void UpdateSpeed()
    //----------------------------------------//
    {
        moveSpeed = Mathf.FloorToInt((10 + speedBonuses + storedRunScore) * speedMultipliers);

        speedText.text = "" + moveSpeed;

    } // END UpdateSpeedFromRun


    // Adds to the speed bonus
    //----------------------------------------//
    public void AddToSpeedBonus(int speedBonusAddition)
    //----------------------------------------//
    {
        speedBonuses += speedBonusAddition;

        UpdateSpeed();

    } // END AddToSpeedBonus


    #endregion


    #region THRESHOLD


    // Adds to comp threshold
    //----------------------------------------//
    public void AddToThreshold(int numToAdd)
    //----------------------------------------//
    {
        composureThreshold += numToAdd;

        thresholdText.text = "" + composureThreshold;

    } // END AddToThreshold


    #endregion


    #region DEFIANCES


    // Adds to defiances
    //----------------------------------------//
    public void AddToDefiances(int numToAdd)
    //----------------------------------------//
    {
        // TODO

    } // END AddToDefiances


    #endregion


} // END GeneralAreaManager.cs
