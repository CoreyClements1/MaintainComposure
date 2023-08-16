using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ArtsManager;

public class SpecEditorPanel : MonoBehaviour
{

    // ArtEditorPanel handles the editing of an art


    #region VARIABLES


    private ArtData editingArtData;
    private string oldArtName;
    [SerializeField] private CanvasGroup canvasGroup;
    private bool creatingNew = false;

    [SerializeField] private TMP_InputField nameInpField;
    [SerializeField] private TMP_Dropdown typeDropdown;
    [SerializeField] private TMP_Dropdown complexityDropdown;
    [SerializeField] private TMP_InputField actCostField;
    [SerializeField] private TMP_InputField rangeField;
    [SerializeField] private Toggle passiveToggle;
    [SerializeField] private Toggle specializationSpecificToggle;
    [SerializeField] private TMP_InputField tagsField;
    [SerializeField] private TMP_InputField artDescriptionField;


    #endregion


    #region SHOW / HIDE


    // Shows the panel and enables editing
    //----------------------------------------//
    public void ShowPanel(ArtData dataToEdit)
    //----------------------------------------//
    {
        creatingNew = false;

        oldArtName = dataToEdit.artName;
        editingArtData = new ArtData(dataToEdit);

        SetUiToEditingArtData();

        canvasGroup.alpha = 0f;
        canvasGroup.LeanAlpha(1f, .3f);

    } // END ShowPanel


    // Shows the panel and enables creating new
    //----------------------------------------//
    public void ShowPanelForNewArt()
    //----------------------------------------//
    {
        creatingNew = true;

        editingArtData = new ArtData("New Art", ArtsManager.ArtType.Fundamental, ArtsManager.ArtComplexity.Basic, 1, "0 meters", false, false, false, null, "Art description");

        SetUiToEditingArtData();

        canvasGroup.alpha = 0f;
        canvasGroup.LeanAlpha(1f, .3f);

    } // END ShowPanelForNewArt


    // Hides the panel
    //----------------------------------------//
    public void HidePanel()
    //----------------------------------------//
    {
        StartCoroutine(HidePanelCoroutine());

    } // END HidePanel


    // Hides the panel
    //----------------------------------------//
    private IEnumerator HidePanelCoroutine()
    //----------------------------------------//
    {
        canvasGroup.LeanAlpha(0f, .3f);

        yield return new WaitForSeconds(.3f);

        gameObject.SetActive(false);

    } // END HidePanelCoroutine


    #endregion


    #region CONFIRM / CANCEL


    // OnConfirm, end editing
    //----------------------------------------//
    public void OnConfirm()
    //----------------------------------------//
    {
        GetEditingArtDataFromUi();

        if (creatingNew)
        {
            ArtsManager.Instance.EndCreateNewArt(editingArtData);
        }
        else
        {
            ArtsManager.Instance.EndEditingRegistryArt(oldArtName, editingArtData);
        }
        
        HidePanel();

    } // END OnConfirm


    // OnCancel, cancel editing
    //----------------------------------------//
    public void OnCancel()
    //----------------------------------------//
    {
        HidePanel();

    } // END OnCancel


    #endregion


    #region GET / SET UI


    // Sets the editing art data to match the UI
    //----------------------------------------//
    private void GetEditingArtDataFromUi()
    //----------------------------------------//
    {
        editingArtData.artName = nameInpField.text;
        editingArtData.artType = (ArtType)Enum.ToObject(typeof(ArtType), typeDropdown.value);
        editingArtData.artComplexity = (ArtComplexity)Enum.ToObject(typeof(ArtComplexity), complexityDropdown.value);

        int t;
        if (Int32.TryParse(actCostField.text, out t))
        {
            editingArtData.actCost = t;
        }

        editingArtData.artRange = rangeField.text;
        editingArtData.passive = passiveToggle.isOn;
        editingArtData.specializationSpecific = specializationSpecificToggle.isOn;
        editingArtData.artTags = tagsField.text;
        editingArtData.artDescription = artDescriptionField.text;

    } // END GetEditingArtDataFromUi


    // Sets the UI to match the art data we are editing
    //----------------------------------------//
    private void SetUiToEditingArtData()
    //----------------------------------------//
    {
        nameInpField.text = editingArtData.artName;
        typeDropdown.value = (int)editingArtData.artType;
        complexityDropdown.value = (int)editingArtData.artComplexity;
        actCostField.text = "" + editingArtData.actCost;
        rangeField.text = editingArtData.artRange;
        passiveToggle.isOn = editingArtData.passive;
        specializationSpecificToggle.isOn = editingArtData.specializationSpecific;
        tagsField.text = editingArtData.artTags;
        artDescriptionField.text = editingArtData.artDescription;

    } // END SetUiToEditingArtData


    #endregion


} // END ArtEditorPanel.cs
