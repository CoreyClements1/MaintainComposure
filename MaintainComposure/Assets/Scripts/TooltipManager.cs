using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TooltipManager : MonoBehaviour
{

    // TooltipManager manages the tooltip


    #region VARIABLES


    public static TooltipManager Instance { get; private set; }

    // Art
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_Text typeText;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text actCostText;
    [SerializeField] private TMP_Text rangeText;
    [SerializeField] private TMP_Text tagsText;
    [SerializeField] private TMP_InputField descriptionField;
    [SerializeField] private TMP_Text descriptionFieldText;
    [SerializeField] private Image[] brightImages;
    [SerializeField] private Image[] midImages;
    [SerializeField] private Image[] darkImages;


    #endregion


    #region SETUP


    // Sets up singleton
    //----------------------------------------//
    public void SetupSingleton()
    //----------------------------------------//
    {
        if (TooltipManager.Instance == null)
        {
            TooltipManager.Instance = this;
        }
        else
        {
            if (TooltipManager.Instance != this)
            {
                Destroy(this);
            }
        }

    } // END SetupSingleton


    #endregion


    #region MONOBEHAVIOUR


    // Update
    //----------------------------------------//
    private void Update()
    //----------------------------------------//
    {
        FollowMouse();

    } // END Update


    #endregion


    #region FOLLOW MOUSE


    // Follows mouse cursor
    //----------------------------------------//
    private void FollowMouse()
    //----------------------------------------//
    {
        transform.position = Mouse.current.position.ReadValue();

    } // END FollowMouse


    #endregion


    #region SHOW / HIDE TOOLTIP


    // Shows the tooltip
    //----------------------------------------//
    public void ShowTooltip(ArtData dataToShow)
    //----------------------------------------//
    {
        SetupFromData(dataToShow);

        canvasGroup.alpha = 1f;

    } // END ShowTooltip


    // Shows the tooltip
    //----------------------------------------//
    public void ShowTooltip(ItemData dataToShow)
    //----------------------------------------//
    {
        // TODO

        canvasGroup.alpha = 1f;

    } // END ShowTooltip


    // Hides the tooltip
    //----------------------------------------//
    public void HideTooltip()
    //----------------------------------------//
    {
        canvasGroup.alpha = 0f;

    } // END HideTooltip


    #endregion


    #region TEXT SETUP


    // Sets up the text and colors from art data
    //----------------------------------------//
    private void SetupFromData(ArtData dataToSetup)
    //----------------------------------------//
    {
        // TEXT
        string typeTextString = "";

        if (dataToSetup.artComplexity != ArtsManager.ArtComplexity.Basic)
        {
            typeTextString += dataToSetup.artComplexity.ToString() + " ";
        }

        typeTextString += dataToSetup.artType.ToString() + " ";
        typeTextString += "Art";

        if (dataToSetup.passive)
        {
            typeTextString += " (Passive)";
        }

        if (dataToSetup.specializationSpecific)
        {
            typeTextString += " (Nonversatile)";
        }

        typeText.text = typeTextString;
        nameText.text = dataToSetup.artName;

        if (dataToSetup.actCost > 0)
        {
            actCostText.text = "Cost: " + dataToSetup.actCost.ToString() + " Acts";
        }
        else
        {
            actCostText.text = "Cost: N/A";
        }

        rangeText.text = dataToSetup.artRange;
        tagsText.text = dataToSetup.artTags;
        descriptionField.text = dataToSetup.artDescription;

        // COLORS
        typeText.color = ArtsManager.artTextColors[(int)dataToSetup.artType];
        nameText.color = ArtsManager.artTextColors[(int)dataToSetup.artType];
        actCostText.color = ArtsManager.artTextColors[(int)dataToSetup.artType];
        rangeText.color = ArtsManager.artTextColors[(int)dataToSetup.artType];
        tagsText.color = ArtsManager.artTextColors[(int)dataToSetup.artType];
        descriptionFieldText.color = ArtsManager.artTextColors[(int)dataToSetup.artType];

        foreach (Image img in brightImages)
        {
            img.color = ArtsManager.artTextColors[(int)dataToSetup.artType];
        }

        foreach (Image img in midImages)
        {
            img.color = ArtsManager.artBgColors[(int)dataToSetup.artType];
        }

        foreach (Image img in darkImages)
        {
            img.color = Color.black;
        }

    } // END SetupFromData


    #endregion


} // END TooltipManager
