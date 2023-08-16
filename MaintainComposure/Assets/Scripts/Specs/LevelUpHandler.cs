using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpHandler : MonoBehaviour
{

    // LevelUpHandler handles the levelling up of a specialization


    #region VARIABLES


    private SpecializationData specData;
    private int oldSpecLevel;

    private List<LevelUpHandlerAttribute> attributeList = new List<LevelUpHandlerAttribute>();

    // UI
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Transform contentParent;
    [SerializeField] private LevelUpHandlerAttribute attributeLevelUpPrefab;

    [SerializeField] private Transform chooseArtPanel;
    [SerializeField] private Transform chooseArtContentParent;

    [SerializeField] private TMP_Dropdown artTypeDropdown;
    [SerializeField] private TMP_Dropdown artSortDropdown;
    [SerializeField] private Toggle onlyShowHighestComplexityToggle;
    [SerializeField] private TMP_Text complexityToggleText;

    [SerializeField] private LevelUpChooseArt levelUpChooseArtPrefab;
    [SerializeField] private LevelUpChooseSpec levelUpChooseSpecPrefab;
    private LevelUpHandlerAttribute storedAttribute;

    private Vector3 chooseArtPanelStartPos;
    private Vector3 chooseArtPanelEndPos;


    #endregion


    #region SETUP


    // On start set art panel pos's
    //----------------------------------------//
    private void Start()
    //----------------------------------------//
    {
        chooseArtPanelStartPos = chooseArtPanel.transform.position;
        chooseArtPanelEndPos = chooseArtPanelStartPos + Vector3.right * 475;

    } // END Start


    // Sets up data for new spec
    //----------------------------------------//
    public void SetupLevelUp(SpecializationData _specData, int _oldSpecLevel)
    //----------------------------------------//
    {
        specData = _specData;
        oldSpecLevel = _oldSpecLevel;

        DisplayAttributes();

        ShowPanel();

    } // END SetupDataForNew


    #endregion


    #region DISPLAY


    // Displays level up attributes
    //----------------------------------------//
    public void DisplayAttributes()
    //----------------------------------------//
    {
        foreach(Transform child in contentParent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach(LevelUpAspect levelUpAspect in specData.levelUpDatas[oldSpecLevel + 1].levelUpAspects)
        {
            LevelUpHandlerAttribute newAttribute = GameObject.Instantiate(attributeLevelUpPrefab);
            newAttribute.SetupAttribute(levelUpAspect);
            newAttribute.transform.SetParent(contentParent);
            attributeList.Add(newAttribute);
        }

    } // END DisplayAttributes


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
        foreach(LevelUpHandlerAttribute attribute in attributeList)
        {
            attribute.ExcecuteLevelup();
        }

        StartCoroutine(HidePanelCoroutine());

    } // END HidePanel


    // Hides the panel
    //----------------------------------------//
    private IEnumerator HidePanelCoroutine()
    //----------------------------------------//
    {
        canvasGroup.LeanAlpha(0f, .3f);

        yield return new WaitForSeconds(.3f);

        TooltipManager.Instance.HideTooltip();
        gameObject.SetActive(false);

    } // END HidePanelCoroutine


    #endregion


    #region END BUTTONS


    // OnConfirm, confirm and send back to specialization manager
    //----------------------------------------//
    public void OnConfirm()
    //----------------------------------------//
    {
        foreach(LevelUpHandlerAttribute attribute in attributeList)
        {
            attribute.ExcecuteLevelup();
        }

        SpecializeManager.Instance.EndLevelup();
        HidePanel();

    } // END OnConfirm


    // OnCancel, cancel and hide panel
    //----------------------------------------//
    public void OnCancel()
    //----------------------------------------//
    {
        HidePanel();

    } // END OnCancel


    #endregion


    #region CHOOSE ART


    // Shows choose art panel
    //----------------------------------------//
    public void OpenChooseArtPanel(LevelUpHandlerAttribute handlerAttribute)
    //----------------------------------------//
    {
        chooseArtPanel.LeanMove(chooseArtPanelEndPos, .3f).setEaseOutExpo();

        if (handlerAttribute.levelUpAspect.levelUpType == LevelUpAspect.LevelUpType.UnlockSpec)
        {
            DisplayChoosableSpecs(handlerAttribute);
        }
        else
        {
            DisplayChooseableArts(handlerAttribute);
        }

    } // END ShowChooseArtPanel


    // Closes choose art panel
    //----------------------------------------//
    public void CloseChooseArtPanel()
    //----------------------------------------//
    {
        chooseArtPanel.LeanMove(chooseArtPanelStartPos, .3f).setEaseOutExpo();

    } // END CloseChooseArtPanel


    // Displays chooseable arts
    //----------------------------------------//
    public void DisplayChooseableArts(LevelUpHandlerAttribute handlerAttribute)
    //----------------------------------------//
    {
        storedAttribute = handlerAttribute;

        bool sortByType;
        bool onlyOneType;
        bool displayExactComplexity;
        ArtsManager.ArtType artTypeToDisplay = ArtsManager.ArtType.Fundamental;
        ArtsManager.ArtComplexity artComplexityToDisplay = (ArtsManager.ArtComplexity)Enum.ToObject(typeof(ArtsManager.ArtComplexity), handlerAttribute.levelUpAspect.assocIncNumber);

        onlyShowHighestComplexityToggle.gameObject.SetActive(true);
        complexityToggleText.gameObject.SetActive(true);

        if (onlyShowHighestComplexityToggle.isOn)
        {
            displayExactComplexity = true;
        }
        else
        {
            displayExactComplexity = false;
        }

        // Learn any art
        if (handlerAttribute.levelUpAspect.levelUpType == LevelUpAspect.LevelUpType.LearnAnyArt)
        {
            // Activate dropdowns
            artTypeDropdown.gameObject.SetActive(true);
            artSortDropdown.gameObject.SetActive(true);

            if (artTypeDropdown.value == 0)
            {
                onlyOneType = false;
            }
            else
            {
                onlyOneType = true;
                artTypeToDisplay = (ArtsManager.ArtType)Enum.ToObject(typeof(ArtsManager.ArtType), artTypeDropdown.value - 1);
            }

            if (artSortDropdown.value == 0)
            {
                sortByType = true;
            }
            else
            {
                sortByType = false;
            }

            ArtsManager.Instance.DisplayLearnableLevelUp(sortByType, onlyOneType, artTypeToDisplay, displayExactComplexity, artComplexityToDisplay, chooseArtContentParent, handlerAttribute, levelUpChooseArtPrefab);
        }
        // Learn art from list
        else if (handlerAttribute.levelUpAspect.levelUpType == LevelUpAspect.LevelUpType.LearnArtFromList)
        {
            onlyShowHighestComplexityToggle.gameObject.SetActive(false);
            complexityToggleText.gameObject.SetActive(false);
            artTypeDropdown.gameObject.SetActive(false);
            artSortDropdown.gameObject.SetActive(false);

            ArtsManager.Instance.DisplayLearnableLevelUpFromList(chooseArtContentParent, handlerAttribute, levelUpChooseArtPrefab);
        }
        // Learn art of type
        else
        {
            // Deactivate dropdowns
            artTypeDropdown.gameObject.SetActive(false);
            artSortDropdown.gameObject.SetActive(false);

            sortByType = true;
            onlyOneType = true;

            artTypeToDisplay = handlerAttribute.levelUpAspect.assocArtType;

            ArtsManager.Instance.DisplayLearnableLevelUp(sortByType, onlyOneType, artTypeToDisplay, displayExactComplexity, artComplexityToDisplay, chooseArtContentParent, handlerAttribute, levelUpChooseArtPrefab);
        }

    } // END DisplayChooseableArts


    // Displays choosable specializations
    //----------------------------------------//
    private void DisplayChoosableSpecs(LevelUpHandlerAttribute handlerAttribute)
    //----------------------------------------//
    {
        onlyShowHighestComplexityToggle.gameObject.SetActive(false);
        complexityToggleText.gameObject.SetActive(false);
        artTypeDropdown.gameObject.SetActive(false);
        artSortDropdown.gameObject.SetActive(false);

        SpecializeManager.Instance.DisplayChoosableSpecs(handlerAttribute, chooseArtContentParent, levelUpChooseSpecPrefab);

    } // END DisplayChoosableSpecs


    // On change, display
    //----------------------------------------//
    public void OnArtDisplayHeaderValChange()
    //----------------------------------------//
    {
        DisplayChooseableArts(storedAttribute);

    } // END OnArtDisplayHeaderValChange


    #endregion


} // END LevelUpHandler.cs
