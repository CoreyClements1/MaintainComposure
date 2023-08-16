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
    [SerializeField] private RectTransform rectTransform;
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
    private bool pivotLeft = true;


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
        if (Mouse.current.position.ReadValue().x + 500 > Screen.currentResolution.width)
        {
            if (pivotLeft)
            {
                rectTransform.pivot = new Vector2(1.1f, .5f);
                pivotLeft = false;
            }
        }
        else
        {
            if (!pivotLeft)
            {
                rectTransform.pivot = new Vector2(-0.1f, .5f);
                pivotLeft = true;
            }
        }

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
        SetupFromData(dataToShow);

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


    // Sets up the text and colors from art data (ART VERSION)
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

        if (dataToSetup.artRange != null && dataToSetup.artRange != "" && dataToSetup.artRange != string.Empty)
        {
            rangeText.text = "Range: " + dataToSetup.artRange;
        }
        else
        {
            rangeText.text = "Range: N/A";
        }

        if (dataToSetup.artTags != null && dataToSetup.artTags != "" && dataToSetup.artTags != string.Empty)
        {
            tagsText.text = "Tags: " + dataToSetup.artTags;
        }
        else
        {
            tagsText.text = "Tags: N/A";
        }
        
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


    // Sets up the text and colors from item data (ITEM VERSION)
    //----------------------------------------//
    private void SetupFromData(ItemData dataToSetup)
    //----------------------------------------//
    {
        // TEXT
        string typeTextString = "";

        typeTextString += dataToSetup.itemType.ToString();

        if (dataToSetup.passive)
        {
            typeTextString += " (Passive)";
        }

        typeText.text = typeTextString;
        nameText.text = dataToSetup.itemName;

        if (dataToSetup.itemType == InvItemsManager.ItemType.Weapon && dataToSetup.actCost > 0)
        {
            actCostText.text = "Cost: " + dataToSetup.actCost.ToString() + " Acts";
        }
        else
        {
            actCostText.text = "Cost: N/A";
        }

        if (dataToSetup.itemType == InvItemsManager.ItemType.Weapon && dataToSetup.range > 0)
        {
            rangeText.text = "Range: " + dataToSetup.range + " meters";
        }
        else
        {
            rangeText.text = "Range: N/A";
        }

        if (dataToSetup.itemTags != null && dataToSetup.itemTags != "" && dataToSetup.itemTags != string.Empty)
        {
            tagsText.text = "Tags: " + dataToSetup.itemTags;
        }
        else
        {
            tagsText.text = "Tags: N/A";
        }

        descriptionField.text = dataToSetup.itemDescription;

        // COLORS
        typeText.color = InvItemsManager.itemTextColors[(int)dataToSetup.itemType];
        nameText.color = InvItemsManager.itemTextColors[(int)dataToSetup.itemType];
        actCostText.color = InvItemsManager.itemTextColors[(int)dataToSetup.itemType];
        rangeText.color = InvItemsManager.itemTextColors[(int)dataToSetup.itemType];
        tagsText.color = InvItemsManager.itemTextColors[(int)dataToSetup.itemType];
        descriptionFieldText.color = InvItemsManager.itemTextColors[(int)dataToSetup.itemType];

        foreach (Image img in brightImages)
        {
            img.color = InvItemsManager.itemTextColors[(int)dataToSetup.itemType];
        }

        foreach (Image img in midImages)
        {
            img.color = InvItemsManager.itemBgColors[(int)dataToSetup.itemType];
        }

        foreach (Image img in darkImages)
        {
            img.color = Color.black;
        }

    } // END SetupFromData


    #endregion


} // END TooltipManager
