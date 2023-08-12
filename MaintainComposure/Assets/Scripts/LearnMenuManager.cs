using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LearnMenuManager : MonoBehaviour
{

    // LearnMenuManager manages the learn menu (new arts and specializations)


    #region VARIABLES


    public static LearnMenuManager Instance { get; private set; }

    [SerializeField] private CanvasGroup darkener;
    [SerializeField] private RectTransform risingPanel;
    [SerializeField] private GameObject learnArtPanel;
    [SerializeField] private GameObject addItemPanel;
    [SerializeField] private GameObject specializationPanel;
    [SerializeField] private GameObject charSelectPanel;
    [SerializeField] private GameObject registryPanel;

    private readonly float animTime = .2f;
    private bool readyToOpenClose = true;

    private bool ableToClose = false;
    [SerializeField] private GameObject xButton;
    [SerializeField] private Color xButtonColorLight;
    [SerializeField] private Color xButtonColorDark;

    [SerializeField] private CanvasGroup newCharNamePopup;
    [SerializeField] private TMP_InputField newCharNameField;

    [SerializeField] private CanvasGroup confirmDeletePopup;
    private int pendingDeleteCharId;

    [SerializeField] private TMP_Dropdown registryTypeDropdown;
    [SerializeField] private GameObject artRegistryHeader;
    [SerializeField] private GameObject itemRegistryHeader;
    [SerializeField] private GameObject specRegistryHeader;
    public int currentRegistryType { get; private set; } = 0;


    #endregion


    #region SETUP


    // Sets up singleton
    //----------------------------------------//
    public void SetupSingleton()
    //----------------------------------------//
    {
        if (LearnMenuManager.Instance == null)
        {
            LearnMenuManager.Instance = this;
        }
        else
        {
            if (LearnMenuManager.Instance != this)
            {
                Destroy(this);
            }
        }

    } // END SetupSingleton


    #endregion


    #region SHOW / HIDE


    // Shows the new art panel
    //----------------------------------------//
    public void ShowNewArtPanel()
    //----------------------------------------//
    {
        if (readyToOpenClose)
        {
            // Show panel
            learnArtPanel.SetActive(true);
            addItemPanel.SetActive(false);
            specializationPanel.SetActive(false);
            charSelectPanel.SetActive(false);
            registryPanel.SetActive(false);

            xButton.GetComponent<Image>().color = xButtonColorDark;

            ArtsManager.Instance.DisplayLearnable();

            StartCoroutine(ShowPanelCoroutine());
        }

    } // END ShowNewArtPanel


    // Shows the specialization panel
    //----------------------------------------//
    public void ShowSpecializationPanel()
    //----------------------------------------//
    {
        if (readyToOpenClose)
        {
            // Show panel
            learnArtPanel.SetActive(false);
            addItemPanel.SetActive(false);
            specializationPanel.SetActive(true);
            charSelectPanel.SetActive(false);
            registryPanel.SetActive(false);

            xButton.GetComponent<Image>().color = xButtonColorDark;

            StartCoroutine(ShowPanelCoroutine());
        }

    } // END ShowSpecializationPanel


    // Shows the character selection panel
    //----------------------------------------//
    public void ShowCharacterSelectPanel()
    //----------------------------------------//
    {
        if (readyToOpenClose)
        {
            // Show panel
            learnArtPanel.SetActive(false);
            addItemPanel.SetActive(false);
            specializationPanel.SetActive(false);
            charSelectPanel.SetActive(true);
            registryPanel.SetActive(false);

            xButton.GetComponent<Image>().color = xButtonColorLight;

            StartCoroutine(ShowPanelCoroutine());
        }

    } // END ShowCharacterSelectPanel


    // Shows the registry panel
    //----------------------------------------//
    public void ShowRegistryPanel()
    //----------------------------------------//
    {
        if (readyToOpenClose)
        {
            // Show panel
            learnArtPanel.SetActive(false);
            addItemPanel.SetActive(false);
            specializationPanel.SetActive(false);
            charSelectPanel.SetActive(false);
            registryPanel.SetActive(true);

            xButton.GetComponent<Image>().color = xButtonColorDark;

            ArtsManager.Instance.RegistryInitialDisplay();

            StartCoroutine(ShowPanelCoroutine());
        }

    } // END ShowCharacterSelectPanel


    // Shows the add item panel
    //----------------------------------------//
    public void ShowAddItemPanel()
    //----------------------------------------//
    {
        if (readyToOpenClose)
        {
            // Show panel
            learnArtPanel.SetActive(false);
            addItemPanel.SetActive(true);
            specializationPanel.SetActive(false);
            charSelectPanel.SetActive(false);
            registryPanel.SetActive(false);

            xButton.GetComponent<Image>().color = xButtonColorDark;

            InvItemsManager.Instance.DisplayObtainable();

            StartCoroutine(ShowPanelCoroutine());
        }

    } // END ShowCharacterSelectPanel


    // Shows the panel
    //----------------------------------------//
    private IEnumerator ShowPanelCoroutine()
    //----------------------------------------//
    {
        readyToOpenClose = false;

        // Show dark background
        darkener.gameObject.SetActive(true);
        darkener.LeanAlpha(1f, animTime * 2f).setEaseOutQuad();            

        // Raise panel
        risingPanel.LeanMoveLocalY(-100, animTime * 2f).setEaseOutExpo();

        yield return new WaitForSeconds(animTime);

        readyToOpenClose = true;

    } // END ShowPanelCoroutine


    // Hides the panel
    //----------------------------------------//
    public void HidePanel()
    //----------------------------------------//
    {
        if (readyToOpenClose)
            StartCoroutine(HideCoroutine());

    } // END HidePanel


    // Hides the panel
    //----------------------------------------//
    private IEnumerator HideCoroutine()
    //----------------------------------------//
    {
        readyToOpenClose = false;

        // Hide dark background
        darkener.LeanAlpha(0f, animTime).setEaseInQuad();

        // Lower panel
        risingPanel.LeanMoveLocalY(-1100, animTime).setEaseInQuad();

        yield return new WaitForSeconds(animTime);

        darkener.gameObject.SetActive(false);
        readyToOpenClose = true;
        SetAbleToClose(true);

    } // END HideCoroutine


    #endregion


    #region BUTTONS


    // Exits the panel
    //----------------------------------------//
    public void OnExitPanel()
    //----------------------------------------//
    {
        if (ableToClose)
        {
            HidePanel();
        }

    } // END OnExitPanel


    #endregion


    #region ABLE TO CLOSE


    // Sets able to close
    //----------------------------------------//
    public void SetAbleToClose(bool _ableToClose)
    //----------------------------------------//
    {
        if (ableToClose != _ableToClose)
        {
            ableToClose = _ableToClose;

            if (ableToClose)
            {
                xButton.SetActive(true);
            }
            else
            {
                xButton.SetActive(false);
            }
        }

    } // END SetAbleToClose


    #endregion


    #region NEW CHAR POPUP


    // Opens new char popup
    //----------------------------------------//
    public void OpenNewCharPopup()
    //----------------------------------------//
    {
        newCharNamePopup.gameObject.SetActive(true);
        newCharNamePopup.alpha = 0f;
        newCharNameField.text = string.Empty;

        newCharNamePopup.LeanAlpha(1f, .3f);

    } // END OpenNewCharPopup


    // Closes new char popup
    //----------------------------------------//
    private IEnumerator CloseNewCharPopup()
    //----------------------------------------//
    {
        newCharNamePopup.LeanAlpha(0f, .3f);

        yield return new WaitForSeconds(.3f);

        newCharNamePopup.gameObject.SetActive(false);

    } // END CloseNewCharPopup


    // Create new character with name
    //----------------------------------------//
    public void OnConfirmNewCharacter()
    //----------------------------------------//
    {
        StartCoroutine(CloseNewCharPopup());

        HeaderAndSaveManager.Instance.OnNewCharacter(newCharNameField.text);

    } // END OnConfirmNewCharacter


    #endregion


    #region DELETE POPUP


    // Opens the confirm delete panel
    //----------------------------------------//
    public void OpenConfirmDeletePanel(int assocCharId)
    //----------------------------------------//
    {
        pendingDeleteCharId = assocCharId;

        confirmDeletePopup.gameObject.SetActive(true);
        confirmDeletePopup.alpha = 0f;

        confirmDeletePopup.LeanAlpha(1f, .3f);

    } // END OpenConfirmDeletePanel


    // Closes confirm delete panel
    //----------------------------------------//
    private IEnumerator CloseConfirmDeletePanel()
    //----------------------------------------//
    {
        confirmDeletePopup.LeanAlpha(0f, .3f);

        yield return new WaitForSeconds(.3f);

        confirmDeletePopup.gameObject.SetActive(false);

    } // END CloseConfirmDeletePanel


    // On confirm delete, delete character
    //----------------------------------------//
    public void OnConfirmDelete()
    //----------------------------------------//
    {
        StartCoroutine(CloseConfirmDeletePanel());

        HeaderAndSaveManager.Instance.DeleteCharacter(pendingDeleteCharId);

    } // END OnConfirmDelete


    // On cancel delete, go back
    //----------------------------------------//
    public void OnCancelDelete()
    //----------------------------------------//
    {
        StartCoroutine(CloseConfirmDeletePanel());

    } // END OnCancelDelete


    #endregion


    #region REGISTRY SWAP


    // Swap to different registry type
    //----------------------------------------//
    public void OnSwapRegistryType()
    //----------------------------------------//
    {
        if (registryTypeDropdown.value != currentRegistryType)
        {
            currentRegistryType = registryTypeDropdown.value;

            switch(currentRegistryType)
            {
                case 0:
                    artRegistryHeader.SetActive(true);
                    itemRegistryHeader.SetActive(false);
                    specRegistryHeader.SetActive(false);
                    ArtsManager.Instance.OnRegistryValueChange();
                    break;
                case 1:
                    artRegistryHeader.SetActive(false);
                    itemRegistryHeader.SetActive(true);
                    specRegistryHeader.SetActive(false);
                    InvItemsManager.Instance.OnRegistryHeaderValueChange();
                    break;
                case 2:
                    artRegistryHeader.SetActive(false);
                    itemRegistryHeader.SetActive(false);
                    specRegistryHeader.SetActive(true);
                    break;
            }
        }

    } // END OnSwapRegistryType


    #endregion


} // END LearnMenuManager
