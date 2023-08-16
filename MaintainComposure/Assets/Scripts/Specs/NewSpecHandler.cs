using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewSpecHandler : MonoBehaviour
{

    // NewSpecHandler handles the new / edit specialization editor


    #region VARIABLES


    private SpecializationData specData;
    private bool editingExisting = false;
    private string oldName;

    // UI
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_InputField specNameField;
    [SerializeField] private TMP_Dropdown starLevelDropdown;
    [SerializeField] private TMP_Dropdown numLevelsDropdown;
    [SerializeField] private Transform contentParent;
    [SerializeField] private AddSpecLevel specLevelPrefab;
    [SerializeField] private GameObject blankPrefab;

    private bool disableValUpdates = false;
    private List<AddSpecLevel> specLevelList = new List<AddSpecLevel>();


    #endregion


    #region SETUP


    // Sets up data for new spec
    //----------------------------------------//
    public void SetupDataForNew()
    //----------------------------------------//
    {
        disableValUpdates = true;
        editingExisting = false;
        specData = new SpecializationData();
        numLevelsDropdown.value = 0;
        starLevelDropdown.value = 0;
        SetupFromLevelsDropdown();
        ShowPanel();
        disableValUpdates = false;

    } // END SetupDataForNew


    // Sets up data for editing existing
    //----------------------------------------//
    public void SetupDataForEdit(SpecializationData oldSpecData)
    //----------------------------------------//
    {
        disableValUpdates = true;
        editingExisting = true;
        oldName = oldSpecData.specName;
        specData = oldSpecData;
        SetupFromData();
        ShowPanel();
        disableValUpdates = false;

    } // END SetupDataForEdit


    // Resets spec area from levels dropdown
    //----------------------------------------//
    private void SetupFromLevelsDropdown()
    //----------------------------------------//
    {
        switch(numLevelsDropdown.value)
        {
            case 0:
                specData.numLevels = 5;
                break;
            case 1:
                specData.numLevels = 10;
                break;
            case 2:
                specData.numLevels = 20;
                break;
        }

        specLevelList.Clear();

        foreach (Transform child in contentParent)
        {
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < specData.numLevels; i++)
        {
            AddSpecLevel newSpecLevel = GameObject.Instantiate(specLevelPrefab);
            newSpecLevel.transform.SetParent(contentParent.transform);
            newSpecLevel.SetupLevelFromScratch(i);
            specLevelList.Add(newSpecLevel);
        }

    } // END SetupFromLevelsDropdown


    // Sets up spec area from data
    //----------------------------------------//
    private void SetupFromData()
    //----------------------------------------//
    {
        specNameField.text = specData.specName;
        starLevelDropdown.value = specData.starLevel;
        switch(specData.numLevels)
        {
            case 5: numLevelsDropdown.value = 0; break;
            case 10: numLevelsDropdown.value = 1; break;
            case 20: numLevelsDropdown.value = 2; break;
        }

        specLevelList.Clear();

        foreach (Transform child in contentParent)
        {
            GameObject.Destroy(child.gameObject);
        }

        for (int i = 0; i < specData.numLevels; i++)
        {
            AddSpecLevel newSpecLevel = GameObject.Instantiate(specLevelPrefab);
            newSpecLevel.transform.SetParent(contentParent.transform);
            newSpecLevel.SetupLevel(specData.levelUpDatas[i], i);
            specLevelList.Add(newSpecLevel);
        }

    } // END SetupFromData


    #endregion


    #region SHOW / HIDE


    // Shows the panel
    //----------------------------------------//
    public void ShowPanel()
    //----------------------------------------//
    {
        canvasGroup.alpha = 0f;
        canvasGroup.LeanAlpha(1f, .3f);

    } // END ShowPanel


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


    #region VAL UPDATE


    // On value changed, update data
    //----------------------------------------//
    public void OnValueChange()
    //----------------------------------------//
    {
        if (!disableValUpdates)
        {
            specData.specName = specNameField.text;
            specData.starLevel = starLevelDropdown.value;
        }

    } // END OnValueChange


    // On num level changed, spawn new levels
    //----------------------------------------//
    public void OnLevelDropdownChange()
    //----------------------------------------//
    {
        if (!disableValUpdates)
        {
            SetupFromLevelsDropdown();
            specData.numLevels = numLevelsDropdown.value;
        }

    } // END OnLevelDropdownChange


    #endregion


    #region END BUTTONS


    // OnConfirm, confirm and send back to specialization manager
    //----------------------------------------//
    public void OnConfirm()
    //----------------------------------------//
    {
        List<LevelUpData> levelUpDatasList = new List<LevelUpData>();

        foreach (AddSpecLevel specLevel in specLevelList)
        {
            levelUpDatasList.Add(specLevel.GetData());
        }

        specData.levelUpDatas = levelUpDatasList.ToArray();

        if (editingExisting)
        {
            SpecializeManager.Instance.EndEditSpec(oldName, specData);
        }
        else
        {
            SpecializeManager.Instance.EndCreateNewSpec(specData);
        }

    } // END OnConfirm


    // OnCancel, cancel and hide panel
    //----------------------------------------//
    public void OnCancel()
    //----------------------------------------//
    {
        HidePanel();

    } // END OnCancel


    #endregion


    #region REFRESH SCROLL


    // Refreshes the scroll by adding and removing a component
    //----------------------------------------//
    public void RefreshScroll()
    //----------------------------------------//
    {
        StartCoroutine(RefreshCoroutine());

    } // END RefreshScroll


    // Refresh coroutine
    //----------------------------------------//
    private IEnumerator RefreshCoroutine()
    //----------------------------------------//
    {
        GameObject newBlank = GameObject.Instantiate(blankPrefab);
        newBlank.transform.SetParent(contentParent);

        yield return null;

        GameObject.Destroy(newBlank);

    } // END RefreshCoroutine


    #endregion


} // END NewSpecHandler.cs
